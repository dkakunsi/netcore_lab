using System;
using System.IO;
using System.Reflection;
using log4net.Appender;
using log4net;
using log4net.Core;

namespace net_logging
{
  class Program
  {
    static void Main(string[] args)
    {
      var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
      log4net.Config.XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
      ThreadContext.Properties["correlationId"] = "0xecdd43523666723600093";
      ThreadContext.Properties["service"] = "Main";

      Runner runner = new Runner();
      runner.run();
    }
  }

  class Runner
  {
    private static log4net.ILog LOG = log4net.LogManager.GetLogger (typeof (Runner));

    public void run ()
    {
      Console.WriteLine ("Hello Worlds!");
      LOG.Error ("This is ERROR log.");
      LOG.Warn ("This is WARN log.");
      LOG.Info ("This is INFO log.");

      try
      {
        new ExceptionCatcher ().throwException ();
      }
      catch (Exception ex)
      {
        LOG.Debug ("This is DEBUG log", ex);
      }
    }
  }

  class ExceptionThrower
  {
    public virtual void throwException ()
    {
      throw new ArithmeticException ("Hi, I'm exception.");
    }
  }

  class ExceptionCatcher : ExceptionThrower
  {

    public override void throwException()
    {
      try
      {
          new ExceptionThrower().throwException();
      }
      catch (Exception ex)
      {
          throw new Exception("Exception catched.", ex);
      }
    }
  }
}
