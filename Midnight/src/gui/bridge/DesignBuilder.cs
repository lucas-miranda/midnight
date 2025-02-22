using Midnight.Diagnostics;

namespace Midnight.GUI;

public class DesignBuilder : ContainerBuilder {
    private System.Action<DesignBuilder> _fn;

    public DesignBuilder() : base(null) {
        DesignBuilder = this;
    }

    public bool IsBuilding { get; private set; }
    public bool IsBuilded { get; private set; }

    public override GUI.Object Build() {
        Frame rootFrame = new Frame() {
            Padding = Spacing.Empty,
            Margin = Spacing.Empty,
        };

        rootFrame.Background.Opacity = 0.0f;
        rootFrame.Border.Opacity = 0.0f;

        return rootFrame;
    }

    public void Build(System.Action<DesignBuilder> fn) {
        if (IsBuilded) {
            return;
        }

        Assert.NotNull(fn);
        _fn = fn;

        // test

        Start();
        _fn.Invoke(this);
        End();
        IsBuilded = true;

        // test  print the whole tree
        Logger.Line($"Result:\n{Result.TreeToString()}");
    }

    public void Evaluate() {
        Assert.NotNull(_fn, "Design was not builded.");
        Start();
        _fn.Invoke(this);
        End();

        // test  print the whole tree
        Logger.Line($"Result:\n{Result.TreeToString()}");
    }

    public void Start() {
        /*
        if (IsBuilded) {
            Reset();
        }
        */

        Prepare();
        IsBuilding = true;
    }

    public void End() {
        IsBuilding = false;
    }

    /*
    public void Reset() {
        IsBuilding = false;
    }
    */
}
