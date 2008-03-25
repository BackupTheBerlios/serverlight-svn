/*
 * Copyright (c) 2007 Maxence Dislaire
 *
 * This file is part of ServerLight.
 *
 * ServerLight is free open source software; you can redistribute it and/or
 * modify it.
 *
 */
using System;
using System.Diagnostics;
using System.ServiceModel;
using System.Threading;
using System.Windows.Forms;
using ServerLight;

namespace ServerLight
{
    public class ServerLighConsoletHost  : CommandLineOptions
    {
        private static readonly AutoResetEvent s_Event = new AutoResetEvent(false);
        private static ServiceHost m_serviceHost;
        private static ServerLight s_ServerLightInstance;
        private static readonly IServiceContainerHelper s_serviceContainerHelper = new ServiceContainerHelper();



        /// <summary>
        /// just to 
        /// </summary>
        [Option(Short = "secretboolparam")]
        public bool secretboolparam;


        public ServerLighConsoletHost(string[] args)  : base(args)
        {
            #region Setup Unhandled Exception Handler

            Application.ThreadException += ShowErrorBox;
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            AppDomain.CurrentDomain.UnhandledException += ShowErrorBox;

            #endregion
        }

        private static void ShowErrorBox(object sender, ThreadExceptionEventArgs e)
        {
            ShowErrorBox(e.Exception, null);
        }

        private static void ShowErrorBox(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = e.ExceptionObject as Exception;
            ShowErrorBox(ex, "Unhandled exception", e.IsTerminating);
        }

        private static void ShowErrorBox(Exception exception, string message)
        {
            ShowErrorBox(exception, message, false);
        }

        private static void ShowErrorBox(Exception exception, string message, bool mustTerminate)
        {
            try
            {
                using (ExceptionBox box = new ExceptionBox(exception, message, mustTerminate))
                {
                    try
                    {
                        box.ShowDialog(NotifyIconForm.Instance);
                    }
                    catch (InvalidOperationException)
                    {
                        box.ShowDialog();
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show(exception.ToString());
            }
        }

        internal static ServerLight ServerLightInstance
        {
            get
            {
                if (s_ServerLightInstance == null)
                {
                    throw new Exception("You should call ServerLight.Main(string[] args) before.");
                }
                return s_ServerLightInstance;
            }
        }

        [STAThread, LoaderOptimization(LoaderOptimization.MultiDomainHost)]
        public static void Main(string[] args)
        {
            new ServerLighConsoletHost(args).Go();
        }


        private void Go()
        {
#if DEBUG
            LaunchServerLight();
#else
            try
            {
                //if ServerLight is allready launch, just use the allready lanched webser to launchDefaultWebBrowser.
                if (ProcessHelper.IsRegister(GetUniqueName()))
                {
                    Uri basePipeRecorderServiceUri = new Uri(GetAppSettingbasePipeRecorderService());
                    IServerLight lightWebServer = ChannelFactory<IServerLight>.CreateChannel(new NetNamedPipeBinding(NetNamedPipeSecurityMode.None), new EndpointAddress(basePipeRecorderServiceUri));

                    lightWebServer.LaunchDefaultWebBrowser();
                }
                else
                {
                    //a special trick to know if serverlight is allready running or not.
                    if (!secretboolparam)
                    {

                        Process.Start(Process.GetCurrentProcess().MainModule.FileName.Replace(".vshost", String.Empty), "/secretboolparam");
                    }
                    else
                    {
                        ProcessHelper.Register(GetUniqueName());
                        try
                        {
                            LaunchServerLight();
                        }
                        finally
                        {
                            ProcessHelper.UnRegister(GetUniqueName());
                        }
                    }
                }
            }
            catch (Exception)
            {
                ProcessHelper.UnRegister(GetUniqueName());
                throw;
            }
#endif
        }

        private static string GetAppSettingbasePipeRecorderService()
        {
            return "net.pipe://localhost/ServerLight/HomeUrl/" + GetUniqueName();
        }

        private static string GetUniqueName()
        {
            return System.Text.RegularExpressions.Regex.Replace(Environment.CommandLine,@"\W*",string.Empty).Replace(".vshost",string.Empty);
        }

        private static void LaunchServerLight()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            s_ServerLightInstance = new ServerLight();
            s_ServerLightInstance.StartWebServer();

            //starting remotint API
            Uri baseAddress = new Uri(GetAppSettingbasePipeRecorderService());
            m_serviceHost = new ServiceHost(s_ServerLightInstance, baseAddress);
            m_serviceHost.AddServiceEndpoint(typeof(IServerLight), new NetNamedPipeBinding(NetNamedPipeSecurityMode.None), baseAddress);
            m_serviceHost.Open();


            VoidMethodDelegate v = delegate
                                       {
                                           ServerLightInstance.LaunchDefaultWebBrowser();
                                           NotifyIconForm.Instance.NotifyIcon.ShowBalloonTip(1000, "ServerLight", s_ServerLightInstance.WebServerUri.ToString(), ToolTipIcon.Info);
                                           
                                           s_serviceContainerHelper.AddService<IMenuService>(NotifyIconForm.Instance);
                                           s_serviceContainerHelper.AddService<IServerLight>(s_ServerLightInstance);
                                           
                                           PluginLoader pluginLoader = new PluginLoader(AppDomain.CurrentDomain.BaseDirectory);
                                           pluginLoader.LoadPluginAssemblies(s_serviceContainerHelper);  
                        
                                           Application.Run(NotifyIconForm.Instance);
                                       };

            v.BeginInvoke(delegate(IAsyncResult result)
                              {
                                  VoidMethodDelegate vv = (VoidMethodDelegate) result.AsyncState;
                                  vv.EndInvoke(result);
                                  s_Event.Set();
                              }, v);

            s_Event.WaitOne();
            s_Event.Close();

            s_ServerLightInstance.StopWebServer();
            if (m_serviceHost != null)
            {
                m_serviceHost.Close();
            }
        }
    }


    [ServiceContract]
    public interface IServerLight
    {
        [OperationContract]
        void LaunchDefaultWebBrowser();

        Uri WebServerUri { get; }
        Uri GetWebServerHomePageUri();
        string ServerPhysicalPath { get; }
    }

    public delegate void VoidMethodDelegate();
}