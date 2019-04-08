using System;
using System.IO;
using System.Reflection;

namespace net_logging
{
  class Program
  {
    static void Main(string[] args)
    {
      var logRepository = log4net.LogManager.GetRepository(Assembly.GetEntryAssembly());
      log4net.Config.XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
      log4net.ThreadContext.Properties["correlationId"] = "0xecdd43523666723600093";

      Runner runner = new Runner();
      runner.run();
    }
  }
}
