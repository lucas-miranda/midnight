using System.Collections.Generic;
using System.IO;
using Midnight.Diagnostics;
using Midnight.Logging;

namespace Midnight;

public class Logger {
    private static Logger _instance;
    private const string LogFilename = "output";
    private const string OutputLogFilename = $"{LogFilename}.txt";
    private const string LogFilenameFormat = $"{LogFilename}-{{0}}.txt";

    private List<LoggerListener> _listeners = new();

    static Logger() {
#if DEBUG
        CanAutoRegisterConsoleListener = true;
#else
        CanAutoRegisterConsoleListener = false;
#endif
    }

    public Logger() {
    }

    public static bool CanAutoRegisterConsoleListener { get; set; }
    public static int MaxLogHistory { get; set; } = 5;

    public static void Line(string contents, string channel = "default") {
        _instance.Send(new() {
            Text = contents,
            Channel = channel,
        });
    }

    [System.Diagnostics.Conditional("DEBUG")]
    public static void DebugLine(string contents, string channel = "default") {
        _instance.Send(new() {
            Text = contents,
            Channel = channel,
            IsDebug = true,
        });
    }

    public static void RegisterListener<T>() where T : LoggerListener, new() {
        T listener = new T();
        _instance._listeners.Add(listener);
        listener.Created();
    }

    public static void RegisterListener<T>(T listener) where T : LoggerListener {
        Assert.NotNull(listener);
        _instance._listeners.Add(listener);
        listener.Created();
    }

    public static bool RemoveListener<T>() where T : LoggerListener {
        for (int i = 0; i < _instance._listeners.Count; i++) {
            if (_instance._listeners[i] is T) {
                _instance._listeners.RemoveAt(i);
                return true;
            }
        }

        return false;
    }

    public static bool RemoveListener<T>(T listener) where T : LoggerListener {
        return _instance._listeners.Remove(listener);
    }

    public static void Flush() {
        foreach (LoggerListener listener in _instance._listeners) {
            listener.Flush();
        }
    }

    public static void ClearListeners() {
        _instance._listeners.Clear();
    }

    internal static void Initialize() {
        if (_instance != null) {
            return;
        }

        _instance = new();
        _instance.Initialized();
    }

    internal static void Update(DeltaTime deltaTime) {
        foreach (LoggerListener listener in _instance._listeners) {
            listener.Update(deltaTime);
        }
    }

    internal static void LateInitialize() {
        // log cycling
        // only move logs if output file (log 0) exists, otherwise nothing should be done
        ReadOnlyPathBuf logDir = ProjectDirs.Data;
        PathBuf outputLogFullpath = logDir + OutputLogFilename;

        Line($"Log files are stored at '{logDir}'");

        if (File.Exists(outputLogFullpath)) {
            /*
               Log Filenames:

               log 0: output.txt
               log 1: output-2.txt
               log 2: output-3.txt
               ...
            */

            // cycle log files, by increasing they index
            // we start at last index - 1
            for (int i = MaxLogHistory - 2; i >= 0; i--) {
                string filename = logDir + GetLogFilename(i),
                       nextFilename = logDir + GetLogFilename(i + 1);

                if (File.Exists(filename)) {
                    File.Move(filename, nextFilename, true);
                }
            }
        }

        RegisterListener(new FileLoggerListener(outputLogFullpath));
    }

    private void Initialized() {
        if (CanAutoRegisterConsoleListener) {
            RegisterListener<ConsoleLoggerListener>();
        }
    }

    private void Send(LogMessage message) {
        foreach (LoggerListener listener in _listeners) {
            listener.Send(message);
        }
    }

    private string NextLogFilename(out int foundFiles) {
        foundFiles = 0;

        for (int i = 0; i < MaxLogHistory; i++) {
            string filename = GetLogFilename(i);

            if (!File.Exists(filename)) {
                return filename;
            }

            foundFiles += 1;
        }

        return GetLogFilename(0);
    }

    private static string GetLogFilename(int index) {
        if (index <= 0) {
            return OutputLogFilename;
        }

        return string.Format(LogFilenameFormat, index + 1);
    }
}
