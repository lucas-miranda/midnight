namespace Midnight.GUI;

public sealed class ExtentRenderSystem : EntitySystem {
    private RectangleDrawable _marginDebug, _paddingDebug;

    public override void Setup(Scene scene) {
        Subscribe<RenderStepEvent>()
            .With<Transform>()
            .With<Extent>()
            .Submit(Render);

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

    public void Render(RenderStepEvent e, Query<Transform> transform, Query<Extent> extent) {
        //if (Debug.DrawMargin) {
            // top
            _marginDebug.Transform.Position = new Vector2(-extent.Entry.Margin.Left, -extent.Entry.Margin.Top);
            _marginDebug.Size = new(extent.Entry.Size.Width + extent.Entry.Margin.Horizontal, extent.Entry.Margin.Top);

            _marginDebug.Draw(e.DeltaTime, new DrawParams { Transform = transform.Entry.Local });

            // right
            _marginDebug.Transform.Position = new Vector2(extent.Entry.Size.Width, 0.0f);
            _marginDebug.Size = new(extent.Entry.Margin.Right, extent.Entry.Size.Height);

            _marginDebug.Draw(e.DeltaTime, new DrawParams { Transform = transform.Entry.Local });

            // bottom
            _marginDebug.Transform.Position = new Vector2(-extent.Entry.Margin.Left, extent.Entry.Size.Height);
            _marginDebug.Size = new(extent.Entry.Size.Width + extent.Entry.Margin.Horizontal, extent.Entry.Margin.Bottom);

            _marginDebug.Draw(e.DeltaTime, new DrawParams { Transform = transform.Entry.Local });

            // left
            _marginDebug.Transform.Position = new Vector2(-extent.Entry.Margin.Left, 0.0f);
            _marginDebug.Size = new(extent.Entry.Margin.Left, extent.Entry.Size.Height);

            _marginDebug.Draw(e.DeltaTime, new DrawParams { Transform = transform.Entry.Local });
        //}

        //if (Debug.DrawPadding) {
            // top
            _paddingDebug.Transform.Position = Vector2.Zero;
            _paddingDebug.Size = new(extent.Entry.Size.Width, extent.Entry.Padding.Top);

            _paddingDebug.Draw(e.DeltaTime, new DrawParams { Transform = transform.Entry.Local });

            // right
            _paddingDebug.Transform.Position = new Vector2(extent.Entry.Size.Width - extent.Entry.Padding.Right, extent.Entry.Padding.Top);
            _paddingDebug.Size = new(extent.Entry.Padding.Right, extent.Entry.Size.Height - extent.Entry.Padding.Vertical);

            _paddingDebug.Draw(e.DeltaTime, new DrawParams { Transform = transform.Entry.Local });

            // bottom
            _paddingDebug.Transform.Position = new Vector2(0.0f, extent.Entry.Size.Height - extent.Entry.Padding.Bottom);
            _paddingDebug.Size = new(extent.Entry.Size.Width, extent.Entry.Padding.Bottom);

            _paddingDebug.Draw(e.DeltaTime, new DrawParams { Transform = transform.Entry.Local });

            // left
            _paddingDebug.Transform.Position = new Vector2(0.0f, extent.Entry.Padding.Top);
            _paddingDebug.Size = new(extent.Entry.Padding.Left, extent.Entry.Size.Height - extent.Entry.Padding.Vertical);

            _paddingDebug.Draw(e.DeltaTime, new DrawParams { Transform = transform.Entry.Local });
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
