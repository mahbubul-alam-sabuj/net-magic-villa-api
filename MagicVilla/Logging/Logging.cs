namespace MagicVilla.Logging;

public class Logging : ILogging
{
  public void Log(string message, LogType type)
  {
    switch(type)
    {
      case LogType.Info:
        Console.WriteLine($"Info: {message}");
        break;
      case LogType.Success:
        Console.WriteLine($"Success: {message}");
        break;
      case LogType.Error:
        Console.WriteLine($"Error: {message}");
        break;
    }
  }
}
