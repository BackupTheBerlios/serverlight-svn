using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace ServerLight
{
    public delegate TReturn FunctionInvoker<TReturn>();

    public static class ProcessHelper
    {
        private static readonly Dictionary<string,Mutex> s_mutexDictionary = new Dictionary<string, Mutex>();

        public static void Register(string name)
        {
            Mutex mutex = new Mutex(true, name);

            if (s_mutexDictionary.ContainsKey(name))
            {
                s_mutexDictionary.Remove(name);
            }
            s_mutexDictionary.Add(name, mutex);
        }

        public static void UnRegister(string name)
        {
            if (s_mutexDictionary.ContainsKey(name))
            {
                Mutex mutex = s_mutexDictionary[name];
                mutex.Close();
                s_mutexDictionary.Remove(name);
            }
        }

        public static bool IsRegister(string name)
        {
            bool createdNew;
            Mutex mutex = new Mutex(true, name, out createdNew);
            if (createdNew)
            {
                mutex.Close();
            }
            return !createdNew;
        }
    }
}
