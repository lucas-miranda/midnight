using Midnight.Diagnostics;

namespace Midnight.GUI;

public abstract class ContainerLayout {
    public void Execute(Container container) {
        Assert.NotNull(container);
        Executing(container);
    }

    protected abstract void Executing(Container container);
}
