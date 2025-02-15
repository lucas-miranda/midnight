
namespace Midnight.Logging;

public class LogCategory {
    public static readonly LogCategory Info = new("info"),
                                       Error = new("err"),
                                       Warning = new("warn"),
                                       Success = new("ok");

    public LogCategory(string name) {
        Name = name;
    }

    public string Name { get; }

    public override string ToString() {
        return Name;
    }
}
