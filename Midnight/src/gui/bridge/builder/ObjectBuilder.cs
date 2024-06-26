namespace Midnight.GUI;

public abstract class ObjectBuilder {
    public ObjectBuilder(DesignBuilder designBuilder) {
        DesignBuilder = designBuilder;
    }

    public DesignBuilder DesignBuilder { get; protected set; }
    public virtual GUI.Object Result { get; private set; }

    public abstract GUI.Object Build();

    public void Prepare() {
        if (Result != null) {
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

    public static implicit operator bool(ObjectBuilder b) {
        return b.Run();
    }
}
