using log4net;
using log4net.Core;

namespace net_logging.AttributeLoader
{
    public class EventAttributeLoader : AttributeLoader
    {
        private LoggingEvent Event;

        public bool contain(string key)
        {
            return true;
        }

        public object get(string key)
        {
            return null;
        }
    }

    public class ContextAttributeLoader : EventAttributeLoader
    {

    }
}