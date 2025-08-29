namespace Midnight.GUI;

[SystemRegistry]
public sealed class RenderSystem : EntitySystem {
    public override void Setup(Scene scene) {
        Subscribe<RenderStepEvent>()
            .WithMultiple<Drawable>()
            .With<Transform>()
            .Submit(Render);
    }

        /*
    public void Update(UpdateStepEvent e, MultiQuery<GraphicDisplayer> displayerQuery, Query<Transform> transformQuery) {
        if (displayer is DrawableDisplayer drawableDisplayer) {
            drawableDisplayer.Drawable.Update(e.DeltaTime);
        }
    }
        */

    public void Render(RenderStepEvent e, MultiQuery<Drawable> displayers, Query<Transform> transform) {
        foreach (Drawable drawable in displayers) {
            drawable.Draw(
                e.DeltaTime,
                new DrawParams {
                    Transform = transform.Entry.Global
                }
            );
        }
    }
}
