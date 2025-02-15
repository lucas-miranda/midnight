
namespace Midnight.Logging;

public abstract class LoggerListener {
    public virtual void Update(DeltaTime deltaTime) {
    }

    public virtual void Created() {
    }

    public abstract void Send(LogMessage message);
    public abstract void Flush();

    protected abstract void Write(string message, LogSectionStyle? style = null);
    protected abstract void WriteLine(string message, LogSectionStyle? style = null);
}
