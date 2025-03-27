namespace Midnight.GUI;

[SystemRegistry]
public sealed class LayoutSystem : EntitySystem {
    public override void Setup(Scene scene) {
        Subscribe<UpdateStepEvent>()
            .With<Widget>()
            .With<Transform>()
            .With<Extent>()
            .Submit(Layout);
    }

    public void Layout(UpdateStepEvent e, Query<Widget> widget, Query<Transform> transform, Query<Extent> extent) {
        //Logger.DebugLine($"LayoutSystem -> Layout for {transform.Entry.Entity}");
        SolveLayout(widget, transform, extent);
        //Logger.DebugLine($"Complete: LayoutSystem -> Layout for {transform.Entry.Entity}");
    }

    private void SolveLayout(Widget widget, Transform transform, Extent extent) {
        switch (widget.Layout) {
            case Midnight.Layout.BoxHorizontal:
                HBoxLayout(widget, transform, extent);
                break;
            case Midnight.Layout.BoxVertical:
                VBoxLayout(widget, transform, extent);
                break;
            default:
                break;
        }
    }

    private Size2 SolveContentSize(Transform transform) {
        Size2 size = default;
        ContentGraphics contents = transform.Entity.Get<ContentGraphics>();
        //Logger.DebugLine($"Solving content size for entity '{transform.Entity}' using {contents.Entries.Count} entries");

        foreach (GraphicDisplayer displayer in contents.Entries) {
            //Logger.DebugLine($"- {displayer.GetType()}");
            size = Size2.MaxComponents(size, displayer.Size);
        }

        //Logger.DebugLine($"{transform.Entity} content size is: {size}");
        return size;
    }

    private void HBoxLayout(Widget widget, Transform transform, Extent extent) {
        //Logger.DebugLine($"Solving layout for entity: {transform.Entity} ({transform.Entity.GetComponents()}) (child count: {transform.ChildCount})");
        Vector2 pos = extent.Padding.TopLeft;
        float h = 0.0f;

        foreach (Transform childTransform in transform) {
            //Logger.DebugLine($"- child: {childTransform.Entity} (child count: {childTransform.ChildCount})");
            (Widget Widget, Extent Extent) child = Query<Widget, Extent>(childTransform.Entity);

            pos += new Vector2(child.Extent.Margin.Left, 0.0f);
            childTransform.Local.Position = new(pos.X, pos.Y + child.Extent.Margin.Top);

            SolveLayout(child.Widget, childTransform, child.Extent);
            //child.TryLayout();

            pos += new Vector2(child.Extent.Size.Width + child.Extent.Margin.Right, 0.0f);
            h = Math.Max(h, child.Extent.Margin.Vertical + child.Extent.Size.Height);
        }

        //Rectangle? childrenBounds = CalculateChildrenBounds(container);
        extent.Size = new(
            pos.X + extent.Padding.Right,
            h + extent.Padding.Vertical
        );

        Size2 contentSize = SolveContentSize(transform);
        extent.Size = Size2.MaxComponents(extent.Size, contentSize);

        //Logger.DebugLine($"{transform.Entity} size is: {extent.Size}");
        Emit(new ResizeEvent(extent.Size), extent.Entity);
        //displayer.Design.Root.Input(e);
    }

    private void VBoxLayout(Widget widget, Transform transform, Extent extent) {
        Vector2 pos = extent.Padding.TopLeft;
        float w = 0.0f;

        foreach (Transform childTransform in transform) {
            (Widget Widget, Extent Extent) child = Query<Widget, Extent>(childTransform.Entity);
            pos += new Vector2(0.0f, child.Extent.Margin.Top);
            childTransform.Local.Position = new(pos.X + child.Extent.Margin.Left, pos.Y);

            SolveLayout(child.Widget, childTransform, child.Extent);

            pos += new Vector2(0.0f, child.Extent.Size.Height + child.Extent.Margin.Bottom);
            w = Math.Max(w, child.Extent.Margin.Horizontal + child.Extent.Size.Width);
        }

        extent.Size = new(
            w + extent.Padding.Horizontal,
            pos.Y + extent.Padding.Bottom
        );

        Size2 contentSize = SolveContentSize(transform);
        extent.Size = Size2.MaxComponents(extent.Size, contentSize);
        Emit(new ResizeEvent(extent.Size), extent.Entity);
    }
}
