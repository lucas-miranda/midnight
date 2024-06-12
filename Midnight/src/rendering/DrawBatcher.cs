using System.Collections.Generic;
using System.Reflection;
using XnaGraphics = Microsoft.Xna.Framework.Graphics;
using Midnight.Diagnostics;

namespace Midnight;

public class DrawBatcher<V> where V : struct, XnaGraphics.IVertexType {
    private const int INITIAL_BUFFERS_SIZE = 300;

    private List<int> _groupsRemoval = new();
    private Dictionary<int, BatchGroup> _groups = new();
    private ShaderMaterial _defaultMaterial;
    private XnaGraphics.DynamicVertexBuffer _vertexBuffer;
    private XnaGraphics.DynamicIndexBuffer _indexBuffer;
    private XnaGraphics.VertexDeclaration _vertexDeclaration;

    public DrawBatcher() {
        // fetch VertexDeclaration from V
        XnaGraphics.IVertexType type = System.Activator.CreateInstance(typeof(V)) as XnaGraphics.IVertexType;
        _vertexDeclaration = type.VertexDeclaration;
    }

    public ShaderMaterial DefaultMaterial {
        get => _defaultMaterial;
        set {
            if (value == null) {
                // TODO  Load default Shader
                throw new System.ArgumentNullException(nameof(value), "Default ShaderMaterial can't be null.");
            }

            _defaultMaterial = value;
        }
    }

    internal XnaGraphics.DynamicVertexBuffer VertexBuffer { get => _vertexBuffer; }
    internal XnaGraphics.DynamicIndexBuffer IndexBuffer { get => _indexBuffer; }

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
        ShaderMaterial material,
        DrawSettings? settings
        //IShaderParameters shaderParameters,
        //float layerDepth = 1.0f
    ) {
        //System.Console.WriteLine("Push:");
        if (material == null) {
            material = DefaultMaterial;
            //System.Console.WriteLine($"- Uses DefaultMaterial (hash: {material.GetHashCode()})");
        } else {
            //System.Console.WriteLine($"- Uses Custom Material (hash: {material.GetHashCode()}");
        }

        if (settings == null) {
            settings = DrawSettings.Default;
            //System.Console.WriteLine($"- Uses DefaultSettings (hash: {settings.GetHashCode()}");
        } else {
            //System.Console.WriteLine($"- Uses Custom Settings (hash: {settings.GetHashCode()}");
        }

        int groupId;

        if (texture != null) {
            groupId = CalculateGroupId(texture, material, settings.Value);
        } else {
            groupId = CalculateGroupId(material, settings.Value);
        }

        //System.Console.WriteLine("Group Id: " + groupId);

        if (!_groups.TryGetValue(groupId, out BatchGroup group)) {
            group = new() {
                Texture = texture,
                Material = material.Duplicate(),
                Settings = settings.Value,
            };

            _groups.Add(groupId, group);

            System.Console.WriteLine($"Creating batch group (total: {_groups.Count})");
        }

        //System.Console.WriteLine("Push!");
        group.Extend(vertexData);
    }

    public void Flush(RenderingServer r) {
        //System.Console.WriteLine($"Flushing {_groups.Count} groups...");
        foreach (KeyValuePair<int, BatchGroup> group in _groups) {
            group.Value.Flush(r, this);

            if (!group.Value.IsValid) {
                _groupsRemoval.Add(group.Key);
            }
        }

        // remove invalid groups
        if (_groupsRemoval.Count > 0) {
            foreach (int id in _groupsRemoval) {
                _groups.Remove(id);
            }

            System.Console.WriteLine($"Removing {_groupsRemoval.Count} invalid batch group (total remaining: {_groups.Count})");
            _groupsRemoval.Clear();
        }
    }

    internal void LoadContent() {
        CreateBuffers(INITIAL_BUFFERS_SIZE);
    }

    private int CalculateGroupId(Texture texture, ShaderMaterial material, DrawSettings settings) {
        Debug.AssertNotNull(texture);
        Debug.AssertNotNull(material);
        Debug.AssertNotNull(settings);
        int hashCode = 1861411795;

        unchecked {
            hashCode = hashCode * 1610612741 + texture.GetHashCode();
            hashCode = hashCode * 1610612741 + material.GetHashCode();
            hashCode = hashCode * 1610612741 + settings.GetHashCode();
        }

        return hashCode;
    }

    private int CalculateGroupId(ShaderMaterial material, DrawSettings settings) {
        Debug.AssertNotNull(material);
        Debug.AssertNotNull(settings);
        int hashCode = 1861411795;

        unchecked {
            hashCode = hashCode * 1610612741 + material.GetHashCode();
            hashCode = hashCode * 1610612741 + settings.GetHashCode();
        }

        return hashCode;
    }

    private void CreateBuffers(int length) {
        if (_vertexBuffer != null) {
            _vertexBuffer.Dispose();
        }

        _vertexBuffer = new(
            Program.Rendering.XnaGraphicsDevice,
            _vertexDeclaration,
            length,
            XnaGraphics.BufferUsage.WriteOnly
        );

        if (_indexBuffer != null) {
            _indexBuffer.Dispose();
        }

        _indexBuffer = new(
            Program.Rendering.XnaGraphicsDevice,
            XnaGraphics.IndexElementSize.ThirtyTwoBits,
            _vertexBuffer.VertexCount,
            XnaGraphics.BufferUsage.WriteOnly
        );

        // pre-initialize index buffer
        System.Span<int> indices = stackalloc int[_indexBuffer.IndexCount];

        for (int i = 0; i < indices.Length; i++) {
            indices[i] = i;
        }

        _indexBuffer.SetData(
            0,
            indices.ToArray(),
            0,
            indices.Length
        );
    }

    private sealed class BatchGroup {
        /// <summary>
        /// After <see cref="Flush"/> being called this many times without any draw data, it'll be automatically invalidated.
        /// </summary>
        public const int UnusedTimesUntilPrune = 100;

        private V[] _vertices = new V[8];
        private int _verticesIndex;
        private ShaderMaterial _material;
        private int _unusedTimes;

#if DEBUG
        private System.Type _shaderTexType;
#endif

        public Texture Texture { get; set; }

        public ShaderMaterial Material {
            get => _material;
            set {
                _material = value;

#if DEBUG
                if (_material != null && _material is ITextureUniform texUniform) {
                    // cache shader expected texture type
                    // so we can verify it later on
                    _shaderTexType = texUniform.GetType()
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

        public void Flush(RenderingServer r, DrawBatcher<V> batcher) {
            if (!IsValid) {
                return;
            }

            if (Texture != null && Texture.IsDisposed) {
                // texture was disposed, group isn't valid anymore
                Invalidate();
                return;
            }

            if (_verticesIndex <= 0) {
                _unusedTimes += 1;

                if (_unusedTimes >= UnusedTimesUntilPrune) {
                    Invalidate();
                }

                return;
            }

            _unusedTimes = 0;

            // adjust texture, if needed
            if (Material is ITextureUniform texUniform && Texture != null) {
                r.XnaGraphicsDevice.Textures[0] = Texture.Underlying;
#if DEBUG
                Debug.Assert(Texture.GetType().IsAssignableTo(_shaderTexType));
#endif
                texUniform.Texture = Texture; // ?
            }

            // apply draw settings
            Settings.Apply();

            // load buffers
            LoadBuffers(r, batcher);

            // each pass must apply each pass while draw vertices
            int primitiveCount = Settings.Primitive.CalculateCount(_verticesIndex);
            foreach (ShaderPass pass in Material.Apply()) {
                //System.Console.WriteLine("Using technique: " + Material.BaseShader.CurrentTechnique.Name);
                pass.Apply();

                r.XnaGraphicsDevice.DrawIndexedPrimitives(
                    Settings.Primitive.ToXna(),
                    0,
                    0,
                    _verticesIndex,
                    0,
                    primitiveCount
                );
            }

            // reset
            _verticesIndex = 0;
        }

        public void Invalidate() {
            IsValid = false;
        }

        private void EnsureCapacity(int additional) {
            System.Array.Resize(ref _vertices, Math.CeilPower2(_verticesIndex + additional));
        }

        private void LoadBuffers(RenderingServer r, DrawBatcher<V> batcher) {
            // ensure vertices will fit at vertex buffer
            if (_verticesIndex > batcher.VertexBuffer.VertexCount) {
                // need resize buffers
                batcher.CreateBuffers(Math.CeilPower2(_verticesIndex) * 2);
            }

            // -> vertex buffer
            batcher.VertexBuffer.SetData(
                0,
                Vertices,
                0,
                _verticesIndex,
                batcher._vertexDeclaration.VertexStride,
                XnaGraphics.SetDataOptions.None
            );

            r.XnaGraphicsDevice.SetVertexBuffer(batcher.VertexBuffer);

            // -> index buffer
            r.XnaGraphicsDevice.Indices = batcher.IndexBuffer;
        }
    }
}
