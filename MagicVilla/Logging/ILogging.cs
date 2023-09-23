using Serilog;

namespace MagicVilla.Logging;

public interface ILogging
{
  void Log(string message, LogType type);
}
