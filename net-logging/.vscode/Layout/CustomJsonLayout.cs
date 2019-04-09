using log4net;
using log4net.Core;
using log4net.Layout;
using Newtonsoft.Json;
using net-logging.AttributeLoader;
using System.Collections.Specialized;

namespace net_logging..vscode.Layout
{
    public class CustomJsonLayout : PatternLayout
    {
        public bool IsRecursiveStackTrace { get; set; }

        public string Timezone { get; set; }

        public string DateFormat { get; set; }

        public string Attributes { get; set; }

        private AttributeLoader EnvironmentAttributeLoader;

        private EventAttributeLoader EventAttributeLoader;

        private EventAttributeLoader ContextAttributeLoader;

        public CustomJsonLayout()
        {
            base();
            this.EnvironmentAttributeLoader = new EnvironmentAttributeLoader();
            this.EventAttributeLoader = new EventAttributeLoader();
            this.ContextAttributeLoader = new ContextAttributeLoader();
        }

        public override void Format (TextWriter writer, LoggingEvent loggingEvent)
        {
            var format = new OrderedDictionary();
            foreach (string attribute in GetAttributes())
            {
                format.Add("attribute", SelectLoader(attribute).get(attribute));
            }

            writer.WriteLine (JsonConvert.SerializeObject(format));
        }

        private string[] GetAttributes() {
            return this.Attributes.split(",");
        }

        private AttributeLoader SelectLoader(string key) {
            if (this.EventAttributeLoader.contain(key))
            {
                return this.EventAttributeLoader;
            }
            else if (this.EnvironmentAttributeLoader.contain(key))
            {
                return this.EnvironmentAttributeLoader;
            }
            else 
            {
                return this.ContextAttributeLoader;
            }
        }
    }
}