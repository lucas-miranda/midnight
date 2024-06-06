using System.Collections.Generic;
using System.Reflection;
using XnaGraphics = Microsoft.Xna.Framework.Graphics;
using Midnight.Diagnostics;

namespace Midnight;

public class DrawBatcher<V> where V : struct, XnaGraphics.IVertexType {
    private List<int> _groupsRemoval = new();
    private Dictionary<int, BatchGroup> _groups = new();
    private Shader _defaultShader;

    public DrawBatcher() {
    }

    public Shader DefaultShader {
        get => _defaultShader;
        set {
            if (value == null) {
                // TODO  Load default Shader
                throw new System.ArgumentNullException(nameof(value), "Default Shader can't be null.");
            }

            _defaultShader = value;
        }
    }

    public void Push(
        Texture texture,
        V[] vertexData,
        int _minVertexIndex,
        int _verticesLength,
        int[] _indices,
        int _minIndex,
        int _primitivesCount,
        //bool isHollow,
        //Vector2 position,
        //float rotation,
        //Vector2 scale,
        //Color color,
        //Vector2 origin,
        //Vector2 scroll,
        Shader shader,
        DrawSettings? settings
        //IShaderParameters shaderParameters,
        //float layerDepth = 1.0f
    ) {
        if (shader == null) {
            shader = DefaultShader;
        }

        if (settings == null) {
            settings = DrawSettings.Default;
        }

        int groupId = CalculateGroupId(texture, shader, settings.Value);

        if (!_groups.TryGetValue(groupId, out BatchGroup group)) {
            group = new() {
                Texture = texture,
                Shader = shader,
                Settings = settings.Value,
            };

            _groups.Add(groupId, group);
        }

        group.Extend(vertexData);
    }

    public void Flush(RenderingServer r) {
        foreach (KeyValuePair<int, BatchGroup> group in _groups) {
            group.Value.Flush(r);

            if (!group.Value.IsValid) {
                _groupsRemoval.Add(group.Key);
            }
        }

        // remove invalid groups
        if (_groupsRemoval.Count > 0) {
            foreach (int id in _groupsRemoval) {
                _groups.Remove(id);
            }

            _groupsRemoval.Clear();
        }
    }

    private int CalculateGroupId(Texture texture, Shader shader, DrawSettings settings) {
        Debug.AssertNotNull(texture);
        Debug.AssertNotNull(shader);
        Debug.AssertNotNull(settings);
        int hashCode = 1861411795;

        unchecked {
            hashCode = hashCode * 1610612741 + texture.GetHashCode();
            hashCode = hashCode * 1610612741 + shader.GetHashCode();
            hashCode = hashCode * 1610612741 + settings.GetHashCode();
        }

        return hashCode;
    }


    private class BatchGroup {
        private V[] _vertices = new V[4];
        private int _verticesIndex;
        private Shader _shader;

#if DEBUG
        private System.Type _shaderTexType;
#endif

        public Texture Texture { get; set; }

        public Shader Shader {
            get => _shader;
            set {
                _shader = value;

#if DEBUG
                if (_shader != null && _shader is ITextureShader texShader) {
                    // cache shader expected texture type
                    // so we can verify it later on
                    _shaderTexType = texShader.GetType()
                                              .GetProperty(
                                                   "Texture",
                                                   BindingFlags.Public | BindingFlags.Instance
                                               )
                                              .PropertyType;
                } else {
                    _shaderTexType = null;
                }
#endif
            }
        }

        public DrawSettings Settings { get; set; }
        public V[] Vertices { get => _vertices; }
        public bool IsValid { get; private set; } = true;

        public void Extend(IList<V> vertices) {
            EnsureCapacity(vertices.Count);
            vertices.CopyTo(_vertices, _verticesIndex);
            _verticesIndex += vertices.Count;
        }

        public void Flush(RenderingServer r) {
            if (!IsValid) {
                return;
            }

            if (Texture != null && Texture.IsDisposed) {
                // texture was disposed, group isn't valid anymore
                Invalidate();
                return;
            }

            if (_verticesIndex <= 0) {
                return;
            }

            // adjust texture, if needed
            if (Shader is ITextureShader texShader && Texture != null) {
                r.XnaGraphicsDevice.Textures[0] = Texture.Underlying;
#if DEBUG
                Debug.Assert(Texture.GetType().IsAssignableTo(_shaderTexType));
#endif
                texShader.Texture = Texture;
            }

            Settings.Apply();

            // each pass must reapply and render
            int primitiveCount = Settings.Primitive.CalculateCount(_verticesIndex);

            foreach (ShaderPass pass in Shader.Apply()) {
                pass.Apply();

                r.XnaGraphicsDevice.DrawUserPrimitives(
                    Settings.Primitive.ToXna(),
                    _vertices,
                    0,
                    primitiveCount
                );
            }

            _verticesIndex = 0;
        }

        public void Invalidate() {
            IsValid = false;
        }

        private void EnsureCapacity(int additional) {
            System.Array.Resize(ref _vertices, Math.CeilPower2(_verticesIndex + additional));
        }
    }
}
