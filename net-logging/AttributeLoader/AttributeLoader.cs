using net_logging.Layout;

namespace net_logging.AttributeLoader
{
    public interface IAttributeLoader
    {
        bool Contains(string key);

        object Load(string key);
    }
}