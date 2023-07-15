public class Logging
{
    private static Logging? instance;
    private static readonly object lockObject = new object();

    private Logging() { }

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
        Console.WriteLine($"[LOG] {message}");
    }
}