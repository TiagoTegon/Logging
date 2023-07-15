public class Logging
{
    private static Logging? instance;

    private Logging() { }

    public static Logging Instance
    {
        get
        {
            instance ??= new Logging();
            return instance;
        }
    }

    public void Log(string message)
    {
        Console.WriteLine($"[LOG] {message}");
    }
}