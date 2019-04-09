namespace net_logging.AttributeLoader
{
    public interface AttributeLoader
    {
        bool contain(string key);

        object get(string key);
    }
}