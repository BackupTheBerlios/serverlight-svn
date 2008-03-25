namespace ServerLight
{
    public interface IServiceContainerHelper
    {
        TService AddService<TService>(TService serviceInstance) where TService : class;

        TService GetService<TService>() where TService : class;

        void RemoveService<TService>(TService serviceInstance) where TService : class;
    }
}