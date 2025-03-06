namespace Midnight.GUI;

public abstract class WidgetBuilder {
    public WidgetBuilder(DesignBuilder designBuilder) {
        DesignBuilder = designBuilder;
    }

    public DesignBuilder DesignBuilder { get; protected set; }
    public virtual Entity Result { get; private set; }

    public abstract Entity Build();

    public void Prepare() {
        if (Result.IsDefined) {
            return;
        }

        Result = Build();
    }

    public virtual void Reset() {
        //Result = Build();
    }

    public virtual bool Run() {
        return false;
    }

    public void RequestEvaluate() {
        DesignBuilder.Evaluate();
    }

    public static implicit operator bool(WidgetBuilder b) {
        return b.Run();
    }
}
