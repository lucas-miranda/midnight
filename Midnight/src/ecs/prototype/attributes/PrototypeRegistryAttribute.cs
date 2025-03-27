using Midnight.Diagnostics;

namespace Midnight;

[System.AttributeUsage(System.AttributeTargets.Class)]
public class PrototypeRegistryAttribute : System.Attribute {
    public PrototypeRegistryAttribute() {
    }

    public PrototypeRegistryAttribute(System.Type builderType) {
        Assert.Is<GUI.WidgetBuilder>(builderType);
        BuilderType = builderType;
    }

    public PrototypeRegistryAttribute(System.Type componentType, System.Type builderType) {
        Assert.Is<Component>(componentType);
        Assert.Is<GUI.WidgetBuilder>(builderType);
        ComponentType = componentType;
        BuilderType = builderType;
    }

    public System.Type ComponentType { get; }
    public System.Type BuilderType { get; }
}
