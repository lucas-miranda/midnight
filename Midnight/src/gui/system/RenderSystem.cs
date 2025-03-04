using Midnight.ECS;

namespace Midnight.GUI;

public sealed class RenderSystem : EntitySystem {
    public override void Setup() {
        //Subscribe<UpdateStepEvent, GraphicDisplayer>(Update);

        //Subscribe<RenderStepEvent, GraphicDisplayer, Transform>(Render);

        //Subscribe<RenderStepEvent>(Render);
        new SystemSubscribeContractBuilder<UpdateStepEvent, MultiComponentQuery<GraphicDisplayer>, SingleComponentQuery<Transform>>()
            .Submit(Update);
    }

    public void Update(UpdateStepEvent e, MultiComponentQuery<GraphicDisplayer> displayer, SingleComponentQuery<Transform> transform) {
    }

    public void Update(UpdateStepEvent e, GraphicDisplayer displayer, Transform transform) {
        /*
        if (displayer is DrawableDisplayer drawableDisplayer) {
            drawableDisplayer.Drawable.Update(e.DeltaTime);
        }
        */
    }

    public void Render(RenderStepEvent e, GraphicDisplayer displayer, Transform transform) {
        if (displayer is DrawableDisplayer drawableDisplayer) {
            drawableDisplayer.Drawable.Draw(
                e.DeltaTime,
                new DrawParams {
                    Transform = transform.Global
                }
            );
        }
    }
}
