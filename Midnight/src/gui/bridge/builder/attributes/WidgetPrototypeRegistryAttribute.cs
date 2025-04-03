using Midnight.Diagnostics;

namespace Midnight;

[System.AttributeUsage(System.AttributeTargets.Class)]
public class WidgetPrototypeRegistryAttribute : System.Attribute {
    public WidgetPrototypeRegistryAttribute() {
    }

    public WidgetPrototypeRegistryAttribute(System.Type builderType) {
        Assert.Is<GUI.WidgetBuilder>(builderType);
        BuilderType = builderType;
    }

    /// <summary>
    ///
    /// </summary>
    public System.Type BuilderType { get; }
}
