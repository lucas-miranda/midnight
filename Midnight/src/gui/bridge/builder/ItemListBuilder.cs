namespace Midnight.GUI;

public class ItemListBuilder : ContainerBuilder {
    public ItemListBuilder(DesignBuilder designBuilder) : base(designBuilder) {
    }

    public override Entity Build() {
        Entity buttonEntity = Prototypes.Instantiate<ItemList>();
        Widget widget = buttonEntity.Get<Widget>();
        widget.Builder = this;
        return buttonEntity;
    }
}
