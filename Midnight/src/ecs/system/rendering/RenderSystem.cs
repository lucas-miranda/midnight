namespace Midnight;

public sealed class RenderSystem : EntitySystem {
    public override void Setup() {
        Subscribe<UpdateStepEvent>()
            .WithMultiple<GraphicDisplayer>()
            .Submit(Update);

        Subscribe<RenderStepEvent>()
            .WithMultiple<GraphicDisplayer>()
            .Submit(Render);
    }

    public void Update(UpdateStepEvent e, MultiQuery<GraphicDisplayer> displayers) {
        /*
        if (displayer is DrawableDisplayer drawableDisplayer) {
            drawableDisplayer.Drawable.Update(e.DeltaTime);
        }
        */
    }

    public void Render(RenderStepEvent e, MultiQuery<GraphicDisplayer> displayers) {
        foreach (GraphicDisplayer displayer in displayers) {
            if (displayer is DrawableDisplayer drawableDisplayer) {
                drawableDisplayer.Drawable.Draw(e.DeltaTime);
            }
        }
    }
}
