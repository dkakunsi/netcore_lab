using System.Net;
using log4net;
using log4net.Core;

namespace net_logging.AttributeLoader
{
    public class HostAttributeLoader : IAttributeLoader
    {
        private const string HOST = "host";

        private const string HOSTNAME = "hostname";

        public bool Contains (string key)
        {
            return HOST.Equals (key) || HOSTNAME.Equals (key);
        }

        public object Load (string key)
        {
            if (!Contains (key))
            {
                return null;
            }
            return Dns.GetHostName ();
        }
    }

    public class ContextAttributeLoader : IAttributeLoader
    {

        public bool Contains (string key)
        {
            return GetProperty (key) != null;
        }

        public object Load (string key)
        {
            if (!Contains (key))
            {
                return null;
            }
            return GetProperty (key);
        }

        private object GetProperty (string key)
        {
            return ThreadContext.Properties[key] != null ? ThreadContext.Properties[key] : GlobalContext.Properties[key];
        }
    }
}
