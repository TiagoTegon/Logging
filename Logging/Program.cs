using System;
using System.Threading;

class Program
{
    static void Main(string[] args)
    {
        Logging logging = Logging.Instance;

        logging.Log("This is a notset log message");
        logging.Debug("This is a debug log message");
        logging.Info("This is an info log message");
        logging.Warning("This is a warning log message");
        logging.Error("This is an error log message");
        logging.Critical("This is a critical log message");

        // Simulate additional work
        for (int i = 0; i < 5; i++)
        {
            Console.WriteLine($"Testing additional work {i}");
            Thread.Sleep(500);
        }

        logging.Shutdown();
    }
}