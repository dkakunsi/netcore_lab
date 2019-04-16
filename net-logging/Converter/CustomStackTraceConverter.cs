using log4net.Core;
using log4net.Layout.Pattern;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;

namespace net_logging.Converter
{
    public class CustomStackTraceConverter : PatternLayoutConverter
    {
        protected override void Convert(TextWriter writer, LoggingEvent loggingEvent)
        {
            var format = JsonConvert.SerializeObject (GenerateStackTrace (loggingEvent.ExceptionObject, IsRecursive ()));
            writer.Write (format);
        }

        private bool IsRecursive ()
        {
            var isRecursive = this.Properties["recursive"];

            if (isRecursive == null)
            {
                return false;
            }

            if (!"true".Equals (isRecursive.ToString ().ToLower ()))
            {
                return false;
            }
            return true;
        }

        private List<Object> GenerateStackTrace (Exception ex, bool recursive)
        {
            if (ex == null) {
                return null;
            }

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
                    file = frame.GetFileName (),
                    method = frame.GetMethod ().Name,
                    line = frame.GetFileLineNumber ()
                });
            }

            return stack;
        }
    }
}