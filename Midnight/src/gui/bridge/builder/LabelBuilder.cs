namespace Midnight.GUI;

public class LabelBuilder : WidgetBuilder {
    private bool _usingCustomFont;

    public LabelBuilder(DesignBuilder designBuilder) : base(designBuilder) {
    }

    public string Text {
        get => GetStringDrawable().Value;
        set {
            StringDrawable drawable = GetStringDrawable();
            drawable.Value = value;
        }
    }

    public override Entity Build() {
        Entity frameEntity = Prototypes.Instantiate<LabelPrototype>();
        Widget widget = frameEntity.Get<Widget>();
        widget.Builder = this;
        return frameEntity;
    }

    public LabelBuilder With(string text) {
        Text = text;
        return this;
    }

    public LabelBuilder WithSize(float size) {
        StringDrawable drawable = GetStringDrawable();
        Font font = GetFont(drawable);
        font.Size = size;
        return this;
    }

    private StringDrawable GetStringDrawable() {
        Components components = Result.GetComponents();
        DrawableDisplayer displayer = components.Get<DrawableDisplayer>();
        return displayer.Drawable as StringDrawable;
    }

    private Font GetFont(StringDrawable drawable) {
        if (!_usingCustomFont) {
            drawable.Font = drawable.Font.Clone();
            _usingCustomFont = true;
        }

        return drawable.Font;
    }
}
