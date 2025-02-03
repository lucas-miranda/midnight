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
#if DEBUG
    internal int DrawCallsCount { get; private set; }
#endif

    public void Push(
        Texture texture,
        System.Span<V> vertexData,
        int _minVertexIndex,
        int _verticesLength,
        int[] _indices,
        int _minIndex,
        int _primitivesCount,
        ShaderMaterial material,
        DrawSettings? settings
    ) {
        //System.Console.WriteLine("Push:");
        if (material == null) {
            material = DefaultMaterial;
            //System.Console.WriteLine($"- Uses DefaultMaterial (hash: {material.GetHashCode()})");
        } else {
            //System.Console.WriteLine($"- Uses Custom Material (hash: {material.GetHashCode()}");
        }

        if (!settings.HasValue) {
            settings = DrawSettings.Default;
            //System.Console.WriteLine($"- Uses DefaultSettings (hash: {settings.GetHashCode()}");
        } else {
            //System.Console.WriteLine($"- Uses Custom Settings (hash: {settings.GetHashCode()}");
        }

        if (settings.Value.Immediate) {
            // break current batch
            Flush(Program.Rendering);

            // create a temporary batch group
            BatchGroup tempGroup = new() {
                Texture = texture,
                Material = material.Duplicate(),
                Settings = settings.Value,
            };

            tempGroup.Extend(vertexData);

            // draw immediatelly
            FlushGroup(Program.Rendering, tempGroup);
            tempGroup.Invalidate();

            return;
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

            //System.Console.WriteLine($"Creating batch group (total: {_groups.Count})");
        }

        group.Extend(vertexData);
        //System.Console.WriteLine($"Push! (pushing {vertexData.Length} vertices, total: {group.VertexCount})");
    }

    public void Flush(RenderingServer r) {
        foreach (KeyValuePair<int, BatchGroup> group in _groups) {
            //System.Console.WriteLine("Flushing Group: " + group.Key);
            FlushGroup(r, group.Value);

            if (!group.Value.IsValid) {
                // add to removal
                //System.Console.WriteLine("- Group will be removed");
                _groupsRemoval.Add(group.Key);
            }
        }

        // remove invalid groups
        if (_groupsRemoval.Count > 0) {
            foreach (int id in _groupsRemoval) {
                _groups.Remove(id);
            }

            //System.Console.WriteLine($"Removing {_groupsRemoval.Count} invalid batch group (total remaining: {_groups.Count})");
            _groupsRemoval.Clear();
        }
    }

    internal void LoadContent() {
        CreateBuffers(INITIAL_BUFFERS_SIZE);
    }

    [System.Diagnostics.Conditional("DEBUG")]
    internal void ResetStats() {
        DrawCallsCount = 0;
    }

    private void FlushGroup(RenderingServer r, BatchGroup group) {
        // TODO  remove me
        // TODO  iterate through active Shaders and place WVP, extracted from MainCamera
        if (group.Material.BaseShader is IWVPShader wvpShader && r.MainCamera != null) {
            wvpShader.WorldViewProjection = r.MainCamera.ViewProjection;
        }

        group.Flush(r, this);
    }

    private int CalculateGroupId(Texture texture, ShaderMaterial material, DrawSettings settings) {
        Assert.NotNull(texture);
        Assert.NotNull(material);
        Assert.NotNull(settings);
        int hashCode = 1861411795;

        unchecked {
            hashCode = hashCode * 1610612741 + texture.GetHashCode();
            hashCode = hashCode * 1610612741 + material.GetHashCode();
            hashCode = hashCode * 1610612741 + settings.GetHashCode();
        }

        return hashCode;
    }

    private int CalculateGroupId(ShaderMaterial material, DrawSettings settings) {
        Assert.NotNull(material);
        Assert.NotNull(settings);
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
        public const int UnusedTimesUntilPrune = 5;

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
                if (_material != null) {
                    if (_material is ITextureUniform texUniform) {
                        // cache shader expected texture type
                        // so we can verify it later on
                        _shaderTexType = texUniform.GetType()
                                                   .GetProperty(
                                                        "Texture",
                                                        BindingFlags.Public | BindingFlags.Instance
                                                    )
                                                   .PropertyType;

                        return;
                    } else if (_material.BaseShader != null && _material.BaseShader is ITextureShader texShader) {
                        // cache shader expected texture type
                        // so we can verify it later on
                        _shaderTexType = texShader.GetType()
                                                  .GetProperty(
                                                       "Texture",
                                                       BindingFlags.Public | BindingFlags.Instance
                                                   )
                                                  .PropertyType;

                        return;
                    }
                }

                _shaderTexType = null;
#endif
            }
        }

        public DrawSettings Settings { get; set; }
        public V[] Vertices { get => _vertices; }
        public int VertexCount => _verticesIndex;
        public bool IsValid { get; private set; } = true;

        public void Extend(IList<V> vertices) {
            EnsureCapacity(vertices.Count);
            vertices.CopyTo(_vertices, _verticesIndex);
            _verticesIndex += vertices.Count;
        }

        public void Extend(System.Span<V> vertices) {
            EnsureCapacity(vertices.Length);

            for (int i = 0; i < vertices.Length; i++) {
                _vertices[i + _verticesIndex] = vertices[i];
            }

            _verticesIndex += vertices.Length;
        }

        public void Flush(RenderingServer r, DrawBatcher<V> batcher) {
            if (!IsValid) {
                return;
            }

            if (Texture != null && Texture.IsReleased) {
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
            bool usingTexture = false;

            if (Texture != null) {
                usingTexture = true;
                r.XnaGraphicsDevice.Textures[0] = Texture.Underlying;
#if DEBUG
                Assert.True(
                    Texture.GetType().IsAssignableTo(_shaderTexType),
                    $"Shader expects texture to be: {_shaderTexType?.Name ?? "-"}\nBut batch is using texture with type instead: {Texture.GetType().Name}"
                );
#endif

                if (Material is ITextureUniform texUniform) {
                    // by using material
                    texUniform.Texture = Texture;
                } else if (Material.BaseShader is ITextureShader texShader) {
                    // by using shader directly
                    texShader.Texture = Texture;
                }
            }

            // apply draw settings
            Settings.Apply();

            // load buffers
            LoadBuffers(r, batcher);

            // each pass must apply each pass while draw vertices
            int primitiveCount = Settings.Primitive.CalculateCount(_verticesIndex);

            foreach (ShaderPass pass in Material.Apply()) {
                //System.Console.WriteLine("Using technique: " + Material.BaseShader.CurrentTechnique.Name + $" Drawing {primitiveCount} {Settings.Primitive} ({_verticesIndex}/{Vertices.Length} vertices)");
                pass.Apply();

                r.XnaGraphicsDevice.DrawIndexedPrimitives(
                    Settings.Primitive.ToXna(),
                    0,
                    0,
                    _verticesIndex,
                    0,
                    primitiveCount
                );

                batcher.DrawCallsCount += 1;
            }

            // reset
            _verticesIndex = 0;

            if (usingTexture) {
                r.XnaGraphicsDevice.Textures[0] = null;
            }
        }

        public void Invalidate() {
            //System.Console.WriteLine("Invalidating BatchGroup");
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
