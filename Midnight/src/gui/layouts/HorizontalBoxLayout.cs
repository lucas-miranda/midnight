
namespace Midnight.GUI;

public class HorizontalBoxLayout : ContainerLayout {
    protected override void Executing(Container container) {
        Vector2 pos = container.Padding.TopLeft;
        float h = 0.0f;

        foreach (Object child in container) {
            pos += new Vector2(child.Margin.Left, 0.0f);
            child.Transform.Position = new(pos.X, pos.Y + child.Margin.Top);
            child.TryLayout();
            pos += new Vector2(child.Size.Width + child.Margin.Right, 0.0f);
            h = Math.Max(h, child.Margin.Vertical + child.Size.Height);
        }

        //Rectangle? childrenBounds = CalculateChildrenBounds(container);
        container.Size = new(
            pos.X + container.Padding.Right,
            h + container.Padding.Vertical
        );
    }

    private Rectangle? CalculateChildrenBounds(Container container) {
        if (container.IsEmpty()) {
            return null;
        }

        Rectangle? bounds = null;

        foreach (Object child in container) {
            if (!bounds.HasValue) {
                bounds = child.Bounds;
            } else {
                bounds = Rectangle.Enclose(bounds.Value, child.Bounds);
            }
        }

        return bounds;
    }
}
