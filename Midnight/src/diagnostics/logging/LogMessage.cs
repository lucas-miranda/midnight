
namespace Midnight.Logging;

public struct LogMessage {
    public string Text;
    public System.DateTime DateTime;
    public LogCategory Category;
    public string Channel;
    public bool AppendNewLine;
    public bool IsDebug;

    public LogMessage() {
        Category = LogCategory.Info;
        DateTime = System.DateTime.Now;
        AppendNewLine = true;
    }

    public bool IsCustomChannel => !string.IsNullOrEmpty(Channel) && Channel != "default";
}
