namespace MyCouch
{
    public interface IViewQueryConfigurator
    {
        IViewQueryConfigurator StartKey(string value);
        IViewQueryConfigurator EndKey(string value);
        IViewQueryConfigurator Limit(int value);
        IViewQueryConfigurator Skip(int value);
        IViewQueryConfigurator Reduce(bool value);
    }
}