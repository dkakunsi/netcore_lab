using log4net;
using log4net.Core;
using net_logging.Layout;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace net_logging.AttributeLoader
{
    public class EventAttributeLoader : IAttributeLoader
    {
        private const string TIMESTAMP = "timestamp";

        private const string THREAD = "thread";

        private const string CATEGORY = "category";

        private const string LEVEL = "level";

        private const string MESSAGE = "message";

        private readonly List<string> SUPPORTED_ATTRIBUTES = new List<string>(new string[] { TIMESTAMP, THREAD, CATEGORY, LEVEL, MESSAGE });

        protected CustomJsonLayout Layout;

        public EventAttributeLoader (CustomJsonLayout layout)
        {
            this.Layout = layout;
        }

        public virtual bool Contains (string key)
        {
            return SUPPORTED_ATTRIBUTES.Contains (key);
        }

        public virtual object Load (string key)
        {
            switch (key) {
                case TIMESTAMP:
                    return Layout.Event.TimeStampUtc;
                case THREAD:
                    return Layout.Event.ThreadName;
                case CATEGORY:
                    return Layout.Event.LoggerName;
                case LEVEL:
                    return Layout.Event.Level.DisplayName;
                case MESSAGE:
                    return Layout.Event.MessageObject;
                default:
                    return null;
            }
        }
    }

    public class StacktraceAttributeLoader : EventAttributeLoader
    {

        private const string STACKTRACE = "stacktrace";

        public StacktraceAttributeLoader (CustomJsonLayout layout) : base (layout)
        {
        }

        public override bool Contains (string key)
        {
            return STACKTRACE.Equals (key);
        }

        public override object Load (string key)
        {
            if (!Contains (key))
            {
                return null;
            }
            return Layout.Event.ExceptionObject != null ? GenerateStackTrace (Layout.Event.ExceptionObject, Layout.IsRecursiveStackTrace) : null;
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
                    file = frame.GetFileName (),
                    method = frame.GetMethod ().Name,
                    line = frame.GetFileLineNumber ()
                });
            }

            return stack;
        }
    }
}
