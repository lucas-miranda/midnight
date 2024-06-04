using System.Collections.Generic;
using XnaGraphics = Microsoft.Xna.Framework.Graphics;
using Midnight.Diagnostics;

namespace Midnight;

public class DrawBatcher<V> where V : struct, XnaGraphics.IVertexType {
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
        Shader shader
        //IShaderParameters shaderParameters,
        //float layerDepth = 1.0f
    ) {
        if (shader == null) {
            shader = DefaultShader;
        }

        int groupId = CalculateGroupId(texture, shader);

        if (!_groups.TryGetValue(groupId, out BatchGroup group)) {
            group = new();
            group.Texture = texture;
            _groups.Add(groupId, group);
        }

        group.Extend(vertexData);
    }

    public void Flush(RenderingServer r) {
        foreach (BatchGroup group in _groups.Values) {
            // TODO  REMOVE
            if (group.Texture != null && group.Texture is Texture2D tex) {
                if (DefaultShader is SpriteShader spriteShader) {
                    spriteShader.Texture = tex;
                }
            }

            var xnaGD = Program.Rendering.XnaGraphicsDevice;
            xnaGD.BlendState = XnaGraphics.BlendState.AlphaBlend;
            xnaGD.SamplerStates[0] = XnaGraphics.SamplerState.PointClamp;
            xnaGD.DepthStencilState = XnaGraphics.DepthStencilState.Default;
            xnaGD.RasterizerState = XnaGraphics.RasterizerState.CullNone;

            DefaultShader.Apply();
            group.Flush(r);
        }
    }

    private int CalculateGroupId(Texture texture, Shader shader) {
        Debug.AssertNotNull(texture);
        Debug.AssertNotNull(shader);
        int hashCode = 1861411795;

        unchecked {
            hashCode = hashCode * -1521134295 + texture.GetHashCode();
            hashCode = hashCode * -1521134295 + shader.GetHashCode();
        }

        return hashCode;
    }


    private class BatchGroup {
        private V[] _vertices = new V[4];
        private int _verticesIndex;

        public Texture Texture { get; set; }
        public V[] Vertices { get => _vertices; }

        public void Extend(IList<V> vertices) {
            EnsureCapacity(vertices.Count);
            vertices.CopyTo(_vertices, _verticesIndex);
            _verticesIndex += vertices.Count;
        }

        public void Flush(RenderingServer r) {
            if (_verticesIndex <= 0) {
                return;
            }

            System.Console.WriteLine($"Flushing {_verticesIndex} vertices");

            for (int i = 0; i < _verticesIndex; i++) {
                System.Console.WriteLine($"> {Vertices[i]}");
            }

            r.XnaGraphicsDevice.Textures[0] = Texture.Underlying;

            r.XnaGraphicsDevice.DrawUserPrimitives(
                XnaGraphics.PrimitiveType.TriangleList,
                _vertices,
                0,
                _verticesIndex / 3
            );

            _verticesIndex = 0;
        }

        private void EnsureCapacity(int additional) {
            System.Array.Resize(ref _vertices, Math.CeilPower2(_verticesIndex + additional));
        }
    }
}
