using System;

namespace MyCouch
{
    public interface IViewQuery
    {
        string DesignDocument { get; }
        string ViewName { get; }
        IViewQueryOptions Options { get; }
        IViewQuery Configure(Action<IViewQueryConfigurator> configurator);
    }
}