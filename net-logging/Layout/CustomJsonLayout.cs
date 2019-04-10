using log4net;
using log4net.Core;
using log4net.Layout;
using Newtonsoft.Json;
using net_logging.AttributeLoader;
using System.Collections.Specialized;
using System.IO;

namespace net_logging.Layout
{
    public class CustomJsonLayout : PatternLayout
    {
        public bool IsRecursiveStackTrace { get; set; }

        public string Attributes { get; set; }

        public LoggingEvent Event { get; set; }

        private IAttributeLoader HostAttributeLoader;

        private IAttributeLoader ContextAttributeLoader;

        private EventAttributeLoader EventAttributeLoader;

        private StacktraceAttributeLoader StacktraceAttributeLoader;

        public CustomJsonLayout () : base ()
        {
            this.HostAttributeLoader = new HostAttributeLoader();
            this.ContextAttributeLoader = new ContextAttributeLoader();
            this.EventAttributeLoader = new EventAttributeLoader(this);
            this.StacktraceAttributeLoader = new StacktraceAttributeLoader(this);
            this.IgnoresException = false;
        }

        public override void Format (TextWriter writer, LoggingEvent loggingEvent)
        {
            this.Event = loggingEvent;

            var format = new OrderedDictionary ();
            foreach (string attribute in GetAttributes ())
            {
                format.Add (attribute, SelectLoader (attribute).Load (attribute));
            }

            writer.WriteLine (JsonConvert.SerializeObject (format));
        }

        private string[] GetAttributes ()
        {
            return this.Attributes.Split (",");
        }

        private IAttributeLoader SelectLoader (string key)
        {
            if (this.EventAttributeLoader.Contains (key))
            {
                return this.EventAttributeLoader;
            }
            else if (this.HostAttributeLoader.Contains (key))
            {
                return this.HostAttributeLoader;
            }
            else if (this.StacktraceAttributeLoader.Contains (key))
            {
                return this.StacktraceAttributeLoader;
            }
            else
            {
                return this.ContextAttributeLoader;
            }
        }
    }
}
