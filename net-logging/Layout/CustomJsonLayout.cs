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

        private IAttributeLoader[] attributeLoaders;

        public CustomJsonLayout () : base ()
        {
            this.attributeLoaders = new IAttributeLoader[4];
            this.attributeLoaders[0] = new HostAttributeLoader();
            this.attributeLoaders[1] = new ContextAttributeLoader();
            this.attributeLoaders[2] = new EventAttributeLoader(this);
            this.attributeLoaders[3] = new StacktraceAttributeLoader(this);
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
            foreach (IAttributeLoader attributeLoader in this.attributeLoaders)
            {
                if (attributeLoader.Contains (key))
                {
                    return attributeLoader;
                }
            }
            return this.attributeLoaders[0];
        }
    }
}
