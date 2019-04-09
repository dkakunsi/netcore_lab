namespace net_logging.AttributeLoader
{
    public class EnvironmentAttributeLoader : AttributeLoader
    {
        public bool contain(string key)
        {
            return true;
        }

        public object get(string key)
        {
            return null;
        }
    }
}