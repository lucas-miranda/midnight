using System.IO;
using System.Text;

namespace Midnight.Logging;

public class FileLoggerListener : TextLoggerListener {
    private TextWriter _writer;
    private StringBuilder _builder;
    private uint _elapsedTimeSinceLastAutoFlush;

    public FileLoggerListener(PathBuf filepath)
        : base(new StreamWriter(filepath, false, Encoding.UTF8))
    {
        _builder = new();
    }

    public bool CanAutoFlush { get; set; } = true;
    public System.TimeSpan AutoFlushDelay { get; set; } = new(0, 0, 1);

    public override void Update(DeltaTime deltaTime) {
        base.Update(deltaTime);

        if (!CanAutoFlush || _builder.Length == 0) {
            return;
        }

        _elapsedTimeSinceLastAutoFlush += (uint) deltaTime.Milli;

        if (_elapsedTimeSinceLastAutoFlush >= AutoFlushDelay.TotalMilliseconds) {
            Flush();
        }
    }

    public override void Created() {
        WriteLine(string.Format("Created at {0}", System.DateTime.Now.ToString("G")));
        WriteLine();
        Flush();
    }

    public override void Send(LogMessage message) {
        // HH:mm:ss  channel  cat D message

        Write(message.DateTime.ToString("HH:mm:ss"));

        if (message.IsCustomChannel) {
            Write("  ");
            Write(message.Channel);
        }

        Write("  ");
        Write(message.Category.ToString());

        if (message.IsDebug) {
            Write(" D ");
        } else {
            Write("   ");
        }

        Write(message.Text);

        if (message.AppendNewLine) {
            WriteLine();
        }
    }

    public override void Flush() {
        _elapsedTimeSinceLastAutoFlush = 0;
        Writer.Write(_builder.ToString());
        Writer.Flush();
        _builder.Clear();
    }

    protected override void Write(string message, LogSectionStyle? style = null) {
        _builder.Append(message);
    }

    protected override void WriteLine(string message = null, LogSectionStyle? style = null) {
        _builder.AppendLine(message);
    }
}
