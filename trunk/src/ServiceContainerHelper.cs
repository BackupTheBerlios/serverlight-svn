using System.ComponentModel.Design;
using System.Diagnostics;

namespace ServerLight
{
    /// <summary>
    /// This class is a generic wrapper around the .net ServiceContainer
    /// </summary>
    [DebuggerStepThrough()]
    public class ServiceContainerHelper : IServiceContainerHelper
    {
        private ServiceContainer m_serviceContainer;

        public ServiceContainerHelper()
        {
            m_serviceContainer = new ServiceContainer();
        }

        public TService AddService<TService>(TService serviceInstance) where TService : class
        {
            if (m_serviceContainer.GetService(typeof(TService)) == null)
            {
                m_serviceContainer.AddService(typeof(TService), serviceInstance);
            }
            return serviceInstance;
        }

        public TService GetService<TService>() where TService : class
        {
            return (TService)m_serviceContainer.GetService(typeof(TService));
        }

        public void RemoveService<TService>(TService serviceInstance) where TService : class
        {
            m_serviceContainer.RemoveService(serviceInstance.GetType());
        }
    }
}