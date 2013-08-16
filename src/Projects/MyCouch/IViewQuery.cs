using System;

namespace MyCouch
{
    public interface IViewQuery
    {
        IViewIdentity View { get; }
        IViewQueryOptions Options { get; }
        IViewQuery Configure(Action<IViewQueryConfigurator> configurator);
    }
}