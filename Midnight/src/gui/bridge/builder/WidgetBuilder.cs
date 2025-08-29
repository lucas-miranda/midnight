namespace Midnight.GUI;

public abstract class WidgetBuilder {
    public WidgetBuilder(DesignBuilder designBuilder) {
        DesignBuilder = designBuilder;
    }

    /// <summary>
    /// <see cref="DesignBuilder"/> which originates this.
    /// </summary>
    public DesignBuilder DesignBuilder { get; protected set; }

    /// <summary>
    /// Entity which has the build result.
    /// </summary>
    public virtual Entity Result { get; private set; }

    /// <summary>
    /// Construct everything which makes this widget into an <see cref="Entity"/>.
    /// </summary>
    public abstract Entity Build();

    public bool IsActive { get; private set; }

    /// <summary>
    /// Prepare, by building, `Result` only if it isn't already done.
    /// </summary>
    public void Prepare() {
        if (Result.IsDefined) {
            Scene.Current.Entities.RegisterOnce(Result);
        } else {
            Result = Build();
        }

        Activate();
    }

    public virtual void Reset() {
        IsActive = false;
    }

    /// <summary>
    /// Runs a query and returns if it was successful or not.
    /// It must be implemented by a child class, otherwise it'll always return false.
    /// </summary>
    public virtual bool Run() {
        return false;
    }

    public virtual void Ended() {
        if (!IsActive && Result.IsDefined) {
            Transform transform = Result.Get<Transform>();
            transform.Parent = null;

            Scene.Current.Entities.Remove(Result);
        }
    }

    /// <summary>
    /// Request
    /// </summary>
    public void RequestEvaluate() {
        DesignBuilder.Evaluate();
    }

    public void Activate() {
        IsActive = true;
    }

    /// <summary>
    /// An implicit conversion to bool, so we can use a widget in a more concise way.
    ///
    /// Example how this can be useful:
    /// <code>
    /// if (builder.PushButton("Test Button")) {
    ///     // do something when button is pushed
    /// }
    /// </code>
    /// </summary>
    public static implicit operator bool(WidgetBuilder b) {
        return b.Run();
    }
}
