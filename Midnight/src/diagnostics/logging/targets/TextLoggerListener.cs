using System.IO;
using Midnight.Diagnostics;

namespace Midnight.Logging;

public abstract class TextLoggerListener : LoggerListener {
    public TextLoggerListener(TextWriter writer) {
        Assert.NotNull(writer);
        Writer = writer;
    }

    protected TextWriter Writer { get; private set; }

    public override void Flush() {
        Writer.Flush();
    }

    protected override void Write(string message, LogSectionStyle? style = null) {
        Writer.Write(message);
    }

    protected override void WriteLine(string message, LogSectionStyle? style = null) {
        Writer.WriteLine(message);
    }
}
