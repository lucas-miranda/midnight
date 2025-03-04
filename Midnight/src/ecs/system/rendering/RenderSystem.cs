namespace Midnight;

public sealed class RenderSystem : EntitySystem {
    public override void Setup() {
        Subscribe<UpdateStepEvent, GraphicDisplayer>(Update);
        Subscribe<RenderStepEvent, GraphicDisplayer>(Render);
    }

    public void Update(UpdateStepEvent e, GraphicDisplayer displayer) {
        /*
        if (displayer is DrawableDisplayer drawableDisplayer) {
            drawableDisplayer.Drawable.Update(e.DeltaTime);
        }
        */
    }

    public void Render(RenderStepEvent e, GraphicDisplayer displayer) {
        if (displayer is DrawableDisplayer drawableDisplayer) {
            drawableDisplayer.Drawable.Draw(e.DeltaTime);
        }
    }
}
