using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace ServerLight
{


    internal class PluginLoader
    {
        private static PluginLoader s_instance;
        private List<PluginAssembly> m_pluginAssemblies;
        private string m_BaseDirectory;


        public PluginLoader(string baseDirectory)
        {
            m_pluginAssemblies = new List<PluginAssembly>();
            m_BaseDirectory = baseDirectory;
        }

        /// <summary>
        /// Load pluggin assemblies from config file (plugins.xml)
        /// </summary>
        public void LoadPluginAssemblies(IServiceContainerHelper serviceContainerHelper)
        {
            SetupPluginListToLoadFromBaseDirectory();
            
            List<PluginAssembly> failed = new List<PluginAssembly>();
            foreach (PluginAssembly pluginAssembly in m_pluginAssemblies)
            {
                try
                {
                    pluginAssembly.LoadPluginsInstance();
                }
                catch (Exception e)
                {
                    failed.Add(pluginAssembly);
                }
            }
            m_pluginAssemblies.RemoveAll(failed.Contains);
            m_pluginAssemblies.ForEach(delegate(PluginAssembly pluginAssembly)
            {
                try
                {
                    pluginAssembly.InitializePlugins(serviceContainerHelper);
                }
                catch (Exception)
                {
                    failed.Add(pluginAssembly);
                }
            });
            m_pluginAssemblies.RemoveAll(failed.Contains);
        }


        private void SetupPluginListToLoadFromBaseDirectory()
        {
            DirectoryInfo pluginDir = new DirectoryInfo(m_BaseDirectory);
            try
            {
                m_pluginAssemblies.Add(new PluginAssembly(Process.GetCurrentProcess().MainModule.FileName));
            }
            catch (Exception)
            {
                return;
            }
            foreach (FileInfo fileInfo in pluginDir.GetFiles("*.dll", SearchOption.AllDirectories))
            {
                try
                {
                    m_pluginAssemblies.Add(new PluginAssembly(fileInfo.FullName));
                }
                catch (Exception)
                {
                    return;
                }
            }
        }
    }
    /// <summary>
    /// Assembly that contain a plugin.
    /// (One plugin per assembly for now)
    /// </summary>
    public class PluginAssembly
    {
        private readonly string m_assemblyFullFileName;
        private List<IPlugin> m_pluginInstance;

        internal PluginAssembly(string assemblyFullFileName)
        {
            m_assemblyFullFileName = assemblyFullFileName;
            m_pluginInstance = new List<IPlugin>();
        }

        public string AssemblyFullFileName
        {
            get
            {
                return m_assemblyFullFileName;
            }
        }

        /// <summary>
        /// The plugin instance. (only one plugin per assembly for now)
        /// </summary>
        internal List<IPlugin> PluginInstance
        {
            get { return m_pluginInstance; }
        }

        internal void LoadPluginsInstance()
        {
            Assembly pluginAssembly = Assembly.LoadFrom(this.AssemblyFullFileName);
            Type[] types = pluginAssembly.GetTypes();
            foreach (Type type in types)
            {
                if (typeof(IPlugin).IsAssignableFrom(type))
                {
                    IPlugin instance;
                    try
                    {
                        instance = (IPlugin)Activator.CreateInstance(type);
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.ToString());
                        continue;
                    }
                    if (instance != null)
                    {
                        m_pluginInstance.Add(instance);
                    }
                }
            }
        }


        public void InitializePlugins(IServiceContainerHelper serviceContainerHelper)
        {
            Debug.Assert(serviceContainerHelper != null);
            if (serviceContainerHelper == null)
            {
                throw new ArgumentNullException("serviceContainerHelper");
            }

            m_pluginInstance.ForEach(delegate(IPlugin obj)
                                         {
                                             obj.InitializePlugin(serviceContainerHelper);
                                         });
        }
    }
}