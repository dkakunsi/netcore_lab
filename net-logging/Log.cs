using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using log4net;
using log4net.Core;
using log4net.Layout;
using Newtonsoft.Json;

namespace net_logging
{
  class StandardizedLayout : PatternLayout
  {
    public override void Format (TextWriter writer, LoggingEvent loggingEvent)
    {
      var format = JsonConvert.SerializeObject (new
      {
        timestamp = loggingEvent.TimeStampUtc,
        correlationId = GetProperty ("correlationId"),
        tid = GetProperty ("tid"),
        principal = GetProperty ("principal"),
        host = Dns.GetHostName (),
        service = GetProperty ("service"),
        instance = GetProperty ("instance"),
        version = GetProperty ("version"),
        thread = loggingEvent.ThreadName,
        category = loggingEvent.LoggerName,
        level = loggingEvent.Level.DisplayName,
        message = loggingEvent.MessageObject,
        fault = GetProperty ("fault"),
        stacktrace = loggingEvent.ExceptionObject != null ? GenerateStackTrace (loggingEvent.ExceptionObject, true) : null,
        payload = GetProperty ("payload")
      });

      writer.WriteLine (format);
    }

    private Object GetProperty (String key)
    {
      return ThreadContext.Properties[key] != null ? ThreadContext.Properties[key] : GlobalContext.Properties[key];
    }

    private List<Object> GenerateStackTrace (Exception ex, bool recursive)
    {
      var stack = new List<Object> ();

      if (recursive)
      {
        // All exception stack will be printed.

        if (ex.InnerException != null)
        {
          stack.AddRange (GenerateStackTrace (ex.InnerException, recursive));
        }
      }
      else
      {
        ex = ExtractInnerException(ex);
      }

      stack.Add (new 
      {
        exception = ex.GetType ().FullName,
        message = ex.Message,
        stack = GenerateStackTrace (new StackTrace (ex, true))
      });

      return stack;
    }

    private Exception ExtractInnerException(Exception ex) {
        while (ex.InnerException != null)
        {
          ex = ex.InnerException;
        }

        return ex;
    }

    private List<Object> GenerateStackTrace (StackTrace trace)
    {
      var stack = new List<Object> ();

      foreach (StackFrame frame in trace.GetFrames ())
      {
        stack.Add (new 
        {
          clazz = frame.GetFileName (),
          method = frame.GetMethod ().Name,
          line = frame.GetFileLineNumber ()
        });
      }

      return stack;
    }
  }
}