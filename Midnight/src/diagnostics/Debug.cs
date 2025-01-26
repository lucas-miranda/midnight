using System.Collections.Generic;
using System.IO;

namespace Midnight.Diagnostics;

public class Debug {
    private const string DiagnosticsFormat = "FPS: {0}";
    private Entity _diagnosticsEntity;
    private TextDisplayer _diagnosticsTextDisplayer;

    private List<DebugIndicator> _indicators = new();

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
    internal void GraphicsReady() {
        Canvas = Canvas.FromBackBuffer(DepthFormat.Depth24Stencil8);
        Program.Rendering.Layers.Register(100, Canvas);

        _diagnosticsEntity = new();

        Transform2D diagnosticsTrans = _diagnosticsEntity.Components.Create<Transform2D>();
        Texture2D fontTexture = Texture2D.Load(Midnight.Embedded.Resources.Fonts.AccidentalPresident.Texture);

        using (MemoryStream dataStream = new(Midnight.Embedded.Resources.Fonts.AccidentalPresident.Data, false)) {
            _diagnosticsTextDisplayer = new() {
                Font = MTSDF.LoadFont(fontTexture, dataStream),
                Value = string.Format(DiagnosticsFormat, "0"),
            };

            _diagnosticsEntity.Components.Add(_diagnosticsTextDisplayer);
        }
    }

    [System.Diagnostics.Conditional("DEBUG")]
    internal void UnloadContent() {
        Canvas.Dispose();
    }

    [System.Diagnostics.Conditional("DEBUG")]
	internal void Render(DeltaTime dt, RenderingServer r) {
	    r.Target.Push(Canvas);
	    r.Clear(Color.Transparent);

        // update text displayer
        _diagnosticsTextDisplayer.Value = string.Format(DiagnosticsFormat, Program.Current.FPS.Current);

        // adjust position
        var trans = _diagnosticsEntity.Components.Get<Transform2D>();
        trans.Position = new Vector2(Canvas.Size.Width - _diagnosticsTextDisplayer.Size.Width - 20, 5);

        // render!
	    _diagnosticsTextDisplayer.Render(dt, r);

	    r.Flush();
	    r.Target.Pop();
    }
}
