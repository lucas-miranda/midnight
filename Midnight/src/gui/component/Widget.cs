namespace Midnight.GUI;

public class Widget : Component {
    public WidgetBuilder Builder { get; set; }
    public DesignBuilder DesignBuilder => Builder.DesignBuilder;
    public Layout Layout { get; set; }
}
