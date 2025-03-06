namespace Midnight;

public class Prototype {
    private System.Action<EntityBuilder> _builderFn;

    public Prototype() {
    }

    public Prototype(System.Action<EntityBuilder> builderFn) {
        _builderFn = builderFn;
    }

    public Entity Instantiate() {
        /*
        // create button prototype
        Prototype buttonProto = new();
        buttonProto.Add<Transform>();

        GUI.Extent extent = buttonProto.Add<GUI.Extent>();
        extent.Margin = new(15, 10);
        extent.Padding = new(5);

        DrawableDisplayer background = buttonProto.Add<DrawableDisplayer>();
        background.Drawable = new RectangleDrawable() {
            Color = 0x57606FFF,
        };

        // instantiate button from prototype
        Entity buttonEntity = buttonProto.Instantiate();
        */

        EntityBuilder builder = Scene.Current.Entities.Create();

        _builderFn?.Invoke(builder);
        Build(builder);

        return builder.Submit(this);
    }

    protected virtual void Build(EntityBuilder builder) {
    }
}
