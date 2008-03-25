using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace ServerLight
{
    public interface IPlugin
    {
        void InitializePlugin(IServiceContainerHelper serviceContainerHelper);
    }
}
