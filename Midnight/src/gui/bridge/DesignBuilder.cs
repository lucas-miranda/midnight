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
        return new Frame();
    }

    public void Build(System.Action<DesignBuilder> fn) {
        if (IsBuilded) {
            return;
        }

        Debug.AssertNotNull(fn);
        _fn = fn;

        // test

        Start();
        _fn.Invoke(this);
        End();
        IsBuilded = true;

        // test  print the whole tree
        System.Console.WriteLine($"Result:\n{Result.TreeToString()}");
    }

    public void Evaluate() {
        Debug.AssertNotNull(_fn, "Design was not builded.");
        Start();
        _fn.Invoke(this);
        End();

        // test  print the whole tree
        System.Console.WriteLine($"Result:\n{Result.TreeToString()}");
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
