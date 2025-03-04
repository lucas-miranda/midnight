using System.Collections.Generic;
using Midnight.Diagnostics;

namespace Midnight.GUI;

public class DesignBuilder : ContainerBuilder {
    private System.Action<DesignBuilder> _fn;

    public DesignBuilder() : base(null) {
        DesignBuilder = this;
    }

    public bool IsBuilding { get; private set; }
    public bool IsBuilded { get; private set; }

    public override Entity Build() {
        Entity frame = Prototypes.Instantiate<FramePrototype>();
        Components frameComponents = frame.GetComponents();

        var extent = frameComponents.Get<Extent>();
        extent.Padding = Spacing.Empty;
        extent.Margin = Spacing.Empty;

        var backgroundBorder = frameComponents.Get<BackgroundBorder>();
        backgroundBorder.Background.Drawable.Opacity = 0.0f;
        backgroundBorder.Border.Drawable.Opacity = 0.0f;

        return frame;
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
        Logger.DebugLine("Result:");
        Entity root = Result;
        Stack<(Transform, int)> stack = new();
        stack.Push((root.Get<Transform>(), 0));

        while (!stack.IsEmpty()) {
            (Transform transform, int level) = stack.Pop();
            string tab = new string(' ', level * 2);
            Logger.DebugLine($"{tab}- {transform.Entity.GetComponents().ToString()} ({transform.ChildCount})");

            foreach (Transform child in transform) {
                stack.Push((child, level + 1));
            }
        }

        //Logger.Line($"Result:\n{Result.TreeToString()}");
    }

    public void Evaluate() {
        Assert.NotNull(_fn, "Design was not builded.");
        Start();
        _fn.Invoke(this);
        End();

        // test  print the whole tree
        //Logger.Line($"Result:\n{Result.TreeToString()}");
    }

    public void Start() {
        if (IsBuilded) {
            Reset();
        }

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
