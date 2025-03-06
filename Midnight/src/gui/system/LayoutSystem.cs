namespace Midnight.GUI;

public sealed class LayoutSystem : EntitySystem {
    public override void Setup() {
        Subscribe<UpdateStepEvent>()
            .With<Transform>()
            .With<Extent>()
            .Submit(Layout);
    }

    public void Layout(UpdateStepEvent e, Query<Transform> transform, Query<Extent> extent) {
        //Logger.DebugLine($"LayoutSystem -> Layout for {transform.Entry.Entity}");
        SolveLayout(transform, extent);
        //Logger.DebugLine($"Complete: LayoutSystem -> Layout for {transform.Entry.Entity}");
    }

    private void SolveLayout(Transform transform, Extent extent) {
        //Logger.DebugLine($"Solving layout for entity: {transform.Entity} ({transform.Entity.GetComponents()}) (child count: {transform.ChildCount})");
        Vector2 pos = extent.Padding.TopLeft;
        float h = 0.0f;

        foreach (Transform childTransform in transform) {
            //Logger.DebugLine($"- child: {childTransform.Entity} (child count: {childTransform.ChildCount})");
            Extent childExtent = Query<Extent>(childTransform.Entity);
            pos += new Vector2(childExtent.Margin.Left, 0.0f);
            childTransform.Local.Position = new(pos.X, pos.Y + childExtent.Margin.Top);

            SolveLayout(childTransform, childExtent);
            //child.TryLayout();

            pos += new Vector2(childExtent.Size.Width + childExtent.Margin.Right, 0.0f);
            h = Math.Max(h, childExtent.Margin.Vertical + childExtent.Size.Height);
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
}
