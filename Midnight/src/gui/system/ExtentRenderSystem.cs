namespace Midnight.GUI;

public sealed class ExtentRenderSystem : EntitySystem {
    private RectangleDrawable _marginDebug, _paddingDebug;

    public override void Setup() {
        Subscribe<RenderStepEvent, Transform, Extent>(Render);

        const float debugOpacity = .2f;
        _marginDebug = new() {
            Color = 0xFF4757FF,
            Opacity = debugOpacity,
            Filled = true,
        };

        _paddingDebug = new() {
            Color = 0x2ED573FF,
            Opacity = debugOpacity,
            Filled = true,
        };
    }

    public void Render(RenderStepEvent e, Transform transform, Extent extent) {
        //if (Debug.DrawMargin) {
            // top
            _marginDebug.Transform.Position = new Vector2(-extent.Margin.Left, -extent.Margin.Top);
            _marginDebug.Size = new(extent.Size.Width + extent.Margin.Horizontal, extent.Margin.Top);

            _marginDebug.Draw(e.DeltaTime, new DrawParams { Transform = transform.Local });

            // right
            _marginDebug.Transform.Position = new Vector2(extent.Size.Width, 0.0f);
            _marginDebug.Size = new(extent.Margin.Right, extent.Size.Height);

            _marginDebug.Draw(e.DeltaTime, new DrawParams { Transform = transform.Local });

            // bottom
            _marginDebug.Transform.Position = new Vector2(-extent.Margin.Left, extent.Size.Height);
            _marginDebug.Size = new(extent.Size.Width + extent.Margin.Horizontal, extent.Margin.Bottom);

            _marginDebug.Draw(e.DeltaTime, new DrawParams { Transform = transform.Local });

            // left
            _marginDebug.Transform.Position = new Vector2(-extent.Margin.Left, 0.0f);
            _marginDebug.Size = new(extent.Margin.Left, extent.Size.Height);

            _marginDebug.Draw(e.DeltaTime, new DrawParams { Transform = transform.Local });
        //}

        //if (Debug.DrawPadding) {
            // top
            _paddingDebug.Transform.Position = Vector2.Zero;
            _paddingDebug.Size = new(extent.Size.Width, extent.Padding.Top);

            _paddingDebug.Draw(e.DeltaTime, new DrawParams { Transform = transform.Local });

            // right
            _paddingDebug.Transform.Position = new Vector2(extent.Size.Width - extent.Padding.Right, extent.Padding.Top);
            _paddingDebug.Size = new(extent.Padding.Right, extent.Size.Height - extent.Padding.Vertical);

            _paddingDebug.Draw(e.DeltaTime, new DrawParams { Transform = transform.Local });

            // bottom
            _paddingDebug.Transform.Position = new Vector2(0.0f, extent.Size.Height - extent.Padding.Bottom);
            _paddingDebug.Size = new(extent.Size.Width, extent.Padding.Bottom);

            _paddingDebug.Draw(e.DeltaTime, new DrawParams { Transform = transform.Local });

            // left
            _paddingDebug.Transform.Position = new Vector2(0.0f, extent.Padding.Top);
            _paddingDebug.Size = new(extent.Padding.Left, extent.Size.Height - extent.Padding.Vertical);

            _paddingDebug.Draw(e.DeltaTime, new DrawParams { Transform = transform.Local });
        //}
    }

    private void SolveLayout(Transform transform, Extent extent) {
        Vector2 pos = extent.Padding.TopLeft;
        float h = 0.0f;

        foreach (Transform childTransform in transform) {
            (Transform Transform, Extent Extent) child = Query<Transform, Extent>(childTransform.Entity);
            pos += new Vector2(child.Extent.Margin.Left, 0.0f);
            child.Transform.Local.Position = new(pos.X, pos.Y + child.Extent.Margin.Top);

            SolveLayout(child.Transform, child.Extent);
            //child.TryLayout();

            pos += new Vector2(child.Extent.Size.Width + child.Extent.Margin.Right, 0.0f);
            h = Math.Max(h, child.Extent.Margin.Vertical + child.Extent.Size.Height);
        }

        //Rectangle? childrenBounds = CalculateChildrenBounds(container);
        extent.Size = new(
            pos.X + extent.Padding.Right,
            h + extent.Padding.Vertical
        );

        //displayer.Design.Root.Input(e);
    }
}
