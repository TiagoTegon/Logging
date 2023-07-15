using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

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

    public void Log(string message)
    {
        logQueue.Enqueue(new LogEntry(message));
    }

    public void Shutdown()
    {
        cancellationTokenSource.Cancel();
        loggingTask.Wait();
    }

    private class LogEntry
    {
        public string Message { get; }

        public LogEntry(string message)
        {
            Message = message;
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
        return $"[{timestamp}] {logEntry.Message}";
    }

    private static void WriteLogMessage(string logMessage)
    {
        Console.WriteLine(logMessage);
    }
}