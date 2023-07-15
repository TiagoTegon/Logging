using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

public enum LogLevel
{
    NOTSET,
    DEBUG,
    INFO,
    WARNING,
    ERROR,
    CRITICAL
}

public class Logging
{
    private static Logging? instance;
    private static readonly object lockObject = new object();
    private readonly ConcurrentQueue<LogEntry> logQueue;
    private readonly CancellationTokenSource cancellationTokenSource;
    private readonly Task loggingTask;

    private Logging() 
    {
        logQueue = new ConcurrentQueue<LogEntry>();
        cancellationTokenSource = new CancellationTokenSource();
        loggingTask = Task.Run(LogMessagesAsync, cancellationTokenSource.Token);
    }

    public static Logging Instance
    {
        get
        {
            if (instance == null)
            {
                lock (lockObject)
                {
                    instance ??= new Logging();
                }
            }
            return instance;
        }
    }

    public void Log(string message, LogLevel level = LogLevel.NOTSET)
    {
        logQueue.Enqueue(new LogEntry(message, level));
    }

    public void Debug(string message)
    {
        Log(message, LogLevel.DEBUG);
    }

    public void Info(string message)
    {
        Log(message, LogLevel.INFO);
    }

    public void Warning(string message)
    {
        Log(message, LogLevel.WARNING);
    }

    public void Error(string message)
    {
        Log(message, LogLevel.ERROR);
    }

    public void Critical(string message)
    {
        Log(message, LogLevel.CRITICAL);
    }

    public void Shutdown()
    {
        cancellationTokenSource.Cancel();
        loggingTask.Wait();
    }

    private class LogEntry
    {
        public string Message { get; }
        public LogLevel Level { get; }

        public LogEntry(string message, LogLevel level)
        {
            Message = message;
            Level = level;
        }
    }

    private async Task LogMessagesAsync()
    {
        while(!cancellationTokenSource.Token.IsCancellationRequested)
        {
            if (logQueue.TryDequeue(out var logEntry)) 
            {
                string formatedLogMessage = FormatLogMessage(logEntry);
                WriteLogMessage(formatedLogMessage);
            }
            else
            {
                await Task.Delay(100);
            }
        }
    }

    private static string FormatLogMessage(LogEntry logEntry)
    {
        string timestamp = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
        return $"[{timestamp}] [{logEntry.Level}] {logEntry.Message}";
    }

    private static void WriteLogMessage(string logMessage)
    {
        Console.WriteLine(logMessage);
    }
}