using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using XnaGraphics = Microsoft.Xna.Framework.Graphics;

namespace Midnight;

[System.Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct VertexPositionColorTexture : XnaGraphics.IVertexType, System.IEquatable<VertexPositionColorTexture> {
    internal static readonly XnaGraphics.VertexDeclaration Declaration;

    public Vector3 Position;
    public Color Color;
    public Vector2 TextureCoordinate;

    static VertexPositionColorTexture() {
        Declaration = new(new XnaGraphics.VertexElement[] {
            new(
                0,
                XnaGraphics.VertexElementFormat.Vector3,
                XnaGraphics.VertexElementUsage.Position,
                0
            ),
            new(
                12, //3 * sizeof(float),
                XnaGraphics.VertexElementFormat.Color,
                XnaGraphics.VertexElementUsage.Color,
                0
            ),
            new(
                16, //4 * sizeof(float),
                XnaGraphics.VertexElementFormat.Vector2,
                XnaGraphics.VertexElementUsage.TextureCoordinate,
                0
            ),
        });
    }

    public VertexPositionColorTexture(Vector3 position, Color color, Vector2 textureCoordinate) {
        Position = position;
        Color = color;
        TextureCoordinate = textureCoordinate;
    }

    public XnaGraphics.VertexDeclaration VertexDeclaration { get => Declaration; }

    public override string ToString() {
        return $"[{Position}; {Color}; {TextureCoordinate}]";
    }

    public bool Equals(VertexPositionColorTexture v) {
        return !(Position != v.Position || Color != v.Color || TextureCoordinate != v.TextureCoordinate);
    }

    public override bool Equals([NotNullWhen(true)] object obj) {
        return obj is VertexPositionColorTexture v && Equals(v);
    }

    public override int GetHashCode() {
        int hashCode = 486187739;

        unchecked {
            hashCode = hashCode * 1610612741 + Position.GetHashCode();
            hashCode = hashCode * 1610612741 + Color.GetHashCode();
            hashCode = hashCode * 1610612741 + TextureCoordinate.GetHashCode();
        }

        return hashCode;
    }

    public static bool operator ==(VertexPositionColorTexture a, VertexPositionColorTexture b) {
        return !(a.Position != b.Position || a.Color != b.Color || a.TextureCoordinate != b.TextureCoordinate);
    }

    public static bool operator !=(VertexPositionColorTexture a, VertexPositionColorTexture b) {
        return a.Position != b.Position || a.Color != b.Color || a.TextureCoordinate != b.TextureCoordinate;
    }
}
