
namespace Midnight.Diagnostics;

public class Debug {
    private const string DiagnosticsFormat = "FPS: {0}";
    private Entity _diagnosticsEntity;
    private TextDisplayer _diagnosticsTextDisplayer;

    public Canvas Canvas {
#if DEBUG
        get; private set;
#else
        get { return null; }
#endif
    }

    [System.Diagnostics.Conditional("DEBUG")]
    public void Initialize() {
    }

    [System.Diagnostics.Conditional("DEBUG")]
    internal void LoadContent() {
        Canvas = Canvas.FromBackBuffer(DepthFormat.Depth24Stencil8);
        Program.Rendering.Layers.Register(100, Canvas);

        _diagnosticsEntity = new();

        Transform2D diagnosticsTrans = _diagnosticsEntity.Components.Create<Transform2D>();
        diagnosticsTrans.Position = new();

        _diagnosticsTextDisplayer = new() {
            //Font = ...,
            Value = string.Format(DiagnosticsFormat, "-"),
        };
    }

    [System.Diagnostics.Conditional("DEBUG")]
    internal void UnloadContent() {
        Canvas.Dispose();
    }

    [System.Diagnostics.Conditional("DEBUG")]
	internal void Render(DeltaTime dt, RenderingServer r) {
	    r.Target.Push(Canvas);
	    _diagnosticsTextDisplayer.Render(dt, r);
	    r.Target.Pop();
    }
}
