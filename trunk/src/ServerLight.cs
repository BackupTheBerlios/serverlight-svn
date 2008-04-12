using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Reflection;
//using System.ServiceModel;
using System.Windows.Forms;
using Microsoft.VisualStudio.WebHost;

namespace ServerLight
{
    //[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    internal class ServerLight : IServerLight
    {
        private readonly string m_ServerPhysicalPath;
        private readonly int m_ServerPort;
        private readonly string m_ServerVirtualPath;
        private Uri m_webServerUri;
        private Server webServer;

        public ServerLight()
        {
            string serverPhysicalPathFromSetting = ConfigurationManager.AppSettings["ServerPhysicalPath"];
            if (!string.IsNullOrEmpty(serverPhysicalPathFromSetting))
            {
                m_ServerPhysicalPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, PathHelper.FromRelativePath(new DirectoryInfo(AppDomain.CurrentDomain.SetupInformation.ApplicationBase), serverPhysicalPathFromSetting));
            }
            else
            {
                m_ServerPhysicalPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, new DirectoryInfo(AppDomain.CurrentDomain.SetupInformation.ApplicationBase).FullName);
            }
            if (!Int32.TryParse(ConfigurationManager.AppSettings["ServerPort"], out m_ServerPort))
            {
                long ticks = DateTime.Now.Ticks;
                m_ServerPort = new Random((int)((ticks << 32) >> 32)).Next(22000, 22500);
            }
            m_ServerVirtualPath = "/";

            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(
                delegate(object sender, ResolveEventArgs args)
                    {
                        if (!args.Name.Equals("WebDev.WebHost, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", StringComparison.Ordinal))
                        {
                            return null;
                        }

                        Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ServerLight.WebDev.WebHost.dll");
                        byte[] buf = new byte[stream.Length];
                        stream.Read(buf, 0, (Int32) stream.Length);

                        //NOTE: WebDev.WebHost is going to load itself AGAIN into another AppDomain,
                        // and will be getting it's Assembliesfrom the BIN, including another copy of itself!
                        // Therefore we need to do this step FIRST because I've removed Cassini from the GAC
                        //Copy our assemblies down into the web server's BIN folder

                        string webSiteBinPath = Path.Combine(m_ServerPhysicalPath, "bin");
                        EnsureFile("WebDev.WebHost.dll", webSiteBinPath);

                        return Assembly.Load((byte[]) buf);
                    });
        }

        public Uri WebServerUri
        {
            get
            {
                if (m_webServerUri == null)
                {
                    throw new Exception("m_webServerUri was null. Start the WebServer before.");
                }
                return m_webServerUri;
            }
        }

        public Uri GetWebServerHomePageUri()
        {
            string homeUrl = GetHomePageFileName(m_ServerPhysicalPath);
            if (string.IsNullOrEmpty(homeUrl))
            {
               MessageBox.Show(string.Format("Cant find any aspx, html or htm in {0}.", m_ServerPhysicalPath), "ServerLiht", MessageBoxButtons.OK, MessageBoxIcon.Information);
               return null;
            }
            Uri homePageUri = new Uri(m_webServerUri, homeUrl);
            return homePageUri;
        }

        public string ServerPhysicalPath
        {
            get
            {

                return m_ServerPhysicalPath;
            }
        }

        #region IServerLight Members

        
        public void LaunchDefaultWebBrowser()
        {
            VoidMethodDelegate v = delegate
                                       {
                                           Uri homePageUri = GetWebServerHomePageUri();
                                           if (homePageUri == null)
                                           {
                                               return;
                                           }                                           
                                           Process.Start(homePageUri.ToString());
                                       };
            v.BeginInvoke(null, null);
        }

        public void OpenRootDirectory()
        {
            VoidMethodDelegate v = delegate
                                       {
                                           Process.Start(@"explorer.exe", m_ServerPhysicalPath);
                                       };
            v.BeginInvoke(null, null);
        }

        #endregion

        public void StartWebServer()
        {
            webServer = new Server(m_ServerPort, m_ServerVirtualPath, m_ServerPhysicalPath);
            string webServerUrl = String.Format("http://127.0.0.1:{0}{1}", m_ServerPort, m_ServerVirtualPath);
            m_webServerUri = new Uri(webServerUrl);

            webServer.Start();
            //Debug.WriteLine(String.Format("Web Server started on port {0} with VDir {1} in physical directory {2}", m_ServerPort, m_ServerVirtualPath, AppDomain.CurrentDomain.BaseDirectory));
        }

        //private static string ExtractResource(string filename, string directory)
        //{
        //    Assembly a = Assembly.GetExecutingAssembly();
        //    string filePath = null;
        //    using (Stream stream = a.GetManifestResourceStream("ServerLight." + filename))
        //    {
        //        if (stream != null)
        //        {
        //            filePath = Path.Combine(directory, filename);
        //            using (StreamWriter outfile = File.Create(filePath))
        //            {
        //                using (StreamReader infile = new StreamReader(stream))
        //                {
        //                    outfile.Write(infile.ReadToEnd());
        //                }
        //            }
        //        }
        //    }
        //    return filePath;
        //}

        private static void EnsureFile(string ressourceFileName, string directory)
        {
            string filePath = Path.Combine(directory, ressourceFileName);
            if (!File.Exists(filePath))
            {
                Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ServerLight." + ressourceFileName);
                byte[] buf = new byte[stream.Length];
                stream.Read(buf, 0, (Int32)stream.Length);
                try
                {
                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }
                    File.WriteAllBytes(filePath, buf);
                }
                catch (Exception e)
                {
                    throw new ApplicationException(ressourceFileName + " not found and cant write it to disk", e);
                }
                stream.Dispose();
            }
        }

        public void StopWebServer()
        {
            webServer.Stop();
        }

        private static string GetHomePageFileName(string serverPhysicalPath)
        {
            string homeUrlFromAppSettins = ConfigurationManager.AppSettings["HomeUrl"];
            if (!string.IsNullOrEmpty(homeUrlFromAppSettins))
            {
                if (!File.Exists(Path.Combine(serverPhysicalPath, homeUrlFromAppSettins)))
                {
                    MessageBox.Show("Specified HomeUrl in config file does not exist.", "ServerLight", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Process.GetCurrentProcess().Kill();
                }
                return homeUrlFromAppSettins;
            }
            if (File.Exists(Path.Combine(serverPhysicalPath, "default.aspx")))
            {
                return "default.aspx";
            }
            if (File.Exists(Path.Combine(serverPhysicalPath, "Default.aspx")))
            {
                return "Default.aspx";
            }
            if (File.Exists(Path.Combine(serverPhysicalPath, "Default.html")))
            {
                return "Default.html";
            }
            if (File.Exists(Path.Combine(serverPhysicalPath, "default.html")))
            {
                return "default.html";
            }
            if (File.Exists(Path.Combine(serverPhysicalPath, "Index.html")))
            {
                return "Index.html";
            }
            if (File.Exists(Path.Combine(serverPhysicalPath, "index.html")))
            {
                return "index.html";
            }
            string[] aspxFiles = Directory.GetFiles(serverPhysicalPath, "*.aspx");
            if (aspxFiles.Length > 0)
            {
                return Path.GetFileName(aspxFiles[0]);
            }
            string[] htmFiles = Directory.GetFiles(serverPhysicalPath, "*.htm");
            if (htmFiles.Length > 0)
            {
                return Path.GetFileName(htmFiles[0]);
            }
            return null;
        }
    }
}