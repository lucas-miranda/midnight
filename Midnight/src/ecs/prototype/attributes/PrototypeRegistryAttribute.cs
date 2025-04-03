using Midnight.Diagnostics;

namespace Midnight;

[System.AttributeUsage(System.AttributeTargets.Class)]
public class PrototypeRegistryAttribute : System.Attribute {
    public PrototypeRegistryAttribute() {
    }

    public PrototypeRegistryAttribute(System.Type componentType) {
        Assert.Is<Component>(componentType);
        ComponentType = componentType;
    }

    public System.Type ComponentType { get; }
}
