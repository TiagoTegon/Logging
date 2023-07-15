using System;
using System.Threading;

class Program
{
    static void Main(string[] args)
    {
        Logging logging = Logging.Instance;

        logging.Log("This is an initial log message");
        logging.Log("This is a second log message");

        // Simulate additional work
        for (int i = 0; i < 5; i++)
        {
            Console.WriteLine($"Testing additional work {i}");
            Thread.Sleep(500);
        }

        logging.Shutdown();
    }
}