using XnaGraphics = Microsoft.Xna.Framework.Graphics;

namespace Midnight;

/// <summary>
/// Defines how vertex data is ordered.
/// </summary>
public enum PrimitiveType {
    /// <summary>
    /// Renders the specified vertices as a sequence of isolated triangles.
    /// Each group of three vertices defines a separate triangle.
    /// Back-face culling is affected by the current winding-order render state.
    /// </summary>
    TriangleList = XnaGraphics.PrimitiveType.TriangleList,

    /// <summary>
    /// Renders the vertices as a triangle strip.
    /// The back-face culling flag is flipped automatically on even-numbered triangles.
    /// </summary>
    TriangleStrip = XnaGraphics.PrimitiveType.TriangleStrip,

    /// <summary>
    /// Renders the vertices as a list of isolated straight line segments; the count may be any positive integer.
    /// </summary>
    LineList = XnaGraphics.PrimitiveType.LineList,

    /// <summary>
    /// Renders the vertices as a single polyline; the count may be any positive integer.
    /// </summary>
    LineStrip = XnaGraphics.PrimitiveType.LineStrip,

    /// <summary>
    /// Treats each vertex as a single point. Vertex n defines point n. N points are drawn.
    /// </summary>
    PointList = XnaGraphics.PrimitiveType.PointListEXT,
}

internal static class PrimitiveTypeExtensions {
    public static int CalculateCount(this PrimitiveType type, int verticesCount) {
        switch (type) {
            case PrimitiveType.TriangleList:
                return verticesCount / 3;
            case PrimitiveType.TriangleStrip:
                return verticesCount - 2;
            case PrimitiveType.LineList:
                return verticesCount / 2;
            case PrimitiveType.LineStrip:
                return verticesCount - 1;
            case PrimitiveType.PointList:
                return verticesCount;
            default:
                return 0;
        }
    }

    public static XnaGraphics.PrimitiveType ToXna(this PrimitiveType type) {
        return (XnaGraphics.PrimitiveType) type;
    }
}
