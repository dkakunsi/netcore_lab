using System;
using log4net.Appender;
using log4net.Core;

namespace asp_webapi.Services
{
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
        new ExceptionThrower ().throwException ();
      }
      catch (Exception ex)
      {
        LOG.Debug ("This is DEBUG log", ex);
      }
    }
  }

  class ExceptionThrower
  {
    public void throwException ()
    {
      throw new Exception ("Hi, I'm exception.");
    }
  }
}
