using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace ServerLight.SilverlightSpyPlugin
{
    public class MultipleIEPlugin : IPlugin
    {
        #region IPlugin Members

        public void InitializePlugin(IServiceContainerHelper serviceContainerHelper)
        {
            IMenuService service = serviceContainerHelper.GetService<IMenuService>();
            IServerLight serverlight = serviceContainerHelper.GetService<IServerLight>();
            if (service != null)
            {
                if (Directory.Exists(@"C:\Program Files\MultipleIEs"))
                {
                    ToolStripMenuItem mltipleIEsMenuItem = new ToolStripMenuItem();
                    mltipleIEsMenuItem.Name = "MultipleIEsPluginToolStripMenuItem";
                    mltipleIEsMenuItem.Size = new System.Drawing.Size(184, 22);
                    mltipleIEsMenuItem.Text = "MultipleIEs";
                    service.AddToolStripMenuItem(mltipleIEsMenuItem);

                    #region IE3
                    if (File.Exists(@"C:\Program Files\MultipleIEs\IE3\iexplore.exe"))
                    {
                        ToolStripMenuItem ie3 = new ToolStripMenuItem();
                        ie3.Name = "IE3sPluginToolStripMenuItem";
                        ie3.Size = new System.Drawing.Size(184, 22);
                        ie3.Text = "IE3";
                        ie3.Click += delegate
                        {
                            MethodInvoker v = delegate
                            {
                                Uri templatesdir = new Uri(serverlight.WebServerUri, serverlight.GetWebServerHomePageUri());
                                Process.Start(@"C:\Program Files\MultipleIEs\IE3\iexplore.exe", templatesdir.ToString());
                            };
                            v.BeginInvoke(null, null);
                        };
                        mltipleIEsMenuItem.DropDownItems.Add(ie3);
                    }
                    #endregion IE3

                    #region IE401
                    if (File.Exists(@"C:\Program Files\MultipleIEs\IE401\iexplore.exe"))
                    {
                        ToolStripMenuItem ie3 = new ToolStripMenuItem();
                        ie3.Name = "IE3sPluginToolStripMenuItem";
                        ie3.Size = new System.Drawing.Size(184, 22);
                        ie3.Text = "IE401";
                        ie3.Click += delegate
                        {
                            MethodInvoker v = delegate
                            {
                                Uri templatesdir = new Uri(serverlight.WebServerUri, serverlight.GetWebServerHomePageUri());
                                Process.Start(@"C:\Program Files\MultipleIEs\IE401\iexplore.exe", templatesdir.ToString());
                            };
                            v.BeginInvoke(null, null);
                        };
                        mltipleIEsMenuItem.DropDownItems.Add(ie3);
                    }
                    #endregion IE401

                    #region IE501
                    if (File.Exists(@"C:\Program Files\MultipleIEs\IE501\iexplore.exe"))
                    {
                        ToolStripMenuItem ie3 = new ToolStripMenuItem();
                        ie3.Name = "IE3sPluginToolStripMenuItem";
                        ie3.Size = new System.Drawing.Size(184, 22);
                        ie3.Text = "IE501";
                        ie3.Click += delegate
                        {
                            MethodInvoker v = delegate
                            {
                                Uri templatesdir = new Uri(serverlight.WebServerUri, serverlight.GetWebServerHomePageUri());
                                Process.Start(@"C:\Program Files\MultipleIEs\IE501\iexplore.exe", templatesdir.ToString());
                            };
                            v.BeginInvoke(null, null);
                        };
                        mltipleIEsMenuItem.DropDownItems.Add(ie3);
                    }
                    #endregion IE501

                    #region IE55
                    if (File.Exists(@"C:\Program Files\MultipleIEs\IE55\iexplore.exe"))
                    {
                        ToolStripMenuItem ie3 = new ToolStripMenuItem();
                        ie3.Name = "IE3sPluginToolStripMenuItem";
                        ie3.Size = new System.Drawing.Size(184, 22);
                        ie3.Text = "IE55";
                        ie3.Click += delegate
                        {
                            MethodInvoker v = delegate
                            {
                                Uri templatesdir = new Uri(serverlight.WebServerUri, serverlight.GetWebServerHomePageUri());
                                Process.Start(@"C:\Program Files\MultipleIEs\IE55\iexplore.exe", templatesdir.ToString());
                            };
                            v.BeginInvoke(null, null);
                        };
                        mltipleIEsMenuItem.DropDownItems.Add(ie3);
                    }
                    #endregion IE55

                    #region IE6
                    if (File.Exists(@"C:\Program Files\MultipleIEs\IE6\iexplore.exe"))
                    {
                        ToolStripMenuItem ie3 = new ToolStripMenuItem();
                        ie3.Name = "IE3sPluginToolStripMenuItem";
                        ie3.Size = new System.Drawing.Size(184, 22);
                        ie3.Text = "IE6";
                        ie3.Click += delegate
                        {
                            MethodInvoker v = delegate
                            {
                                Uri templatesdir = new Uri(serverlight.WebServerUri, serverlight.GetWebServerHomePageUri());
                                Process.Start(@"C:\Program Files\MultipleIEs\IE6\iexplore.exe", templatesdir.ToString());
                            };
                            v.BeginInvoke(null, null);
                        };
                        mltipleIEsMenuItem.DropDownItems.Add(ie3);
                    }
                    #endregion IE6
                }
            }

            /// <summary>
            /// Ensure that the specified file existe on the system disk. If not, write it form the ResourceStream that is embbed in this Assemblby.
            /// </summary>
            /// <param name="ressourceFileName"></param>
            //private static void EnsureFile(string ressourceFileName)
            //{
            //    string storageDirectoryFullName = GetStorageDirectoryFullName();
            //    if (!File.Exists(storageDirectoryFullName + "\\" + ressourceFileName))
            //    {
            //        if (!Directory.Exists(storageDirectoryFullName))
            //        {
            //            Directory.CreateDirectory(storageDirectoryFullName);
            //        }
            //        Stream stream = typeof(EncodersHelpers).Assembly.GetManifestResourceStream(typeof(EncodersHelpers).Namespace + "." + ressourceFileName);
            //        byte[] buf = new byte[stream.Length];
            //        stream.Read(buf, 0, (Int32)stream.Length);
            //        try
            //        {
            //            File.WriteAllBytes(storageDirectoryFullName + "\\" + ressourceFileName, buf);
            //        }
            //        catch (Exception e)
            //        {
            //            throw new EncodeEngineException(ressourceFileName + " not found and/or cant write it to disk", e);
            //        }
            //    }
            //}

            //private static string GetStorageDirectoryFullName()
            //{
            //    return AppDomain.CurrentDomain.
            //}
        }

        #endregion
    }
}
