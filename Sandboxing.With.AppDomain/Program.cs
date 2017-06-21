using Plugin.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Sandboxing.With.AppDomain
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(string.Format("--[entry point in '{0}' domain]--", System.AppDomain.CurrentDomain.FriendlyName));

            Console.WriteLine("--configure sandbox (with new application domain)--");
            var options = new PluginSandboxOptions();
            options.FrendlyName = "Plugin";
            options.ApplicationBase = Path.GetFullPath(@"..\..\..\Plugin.Implementation\bin\Debug");
            options.AssemblyName = "Plugin.Implementation";
            options.TypeName = "Plugin.Implementation.PluginImpl";

            var pluginSandbox = new PluginSandbox(options);
            Console.WriteLine("--call entry point--");
            pluginSandbox.CallEntryPoint();

            Console.WriteLine("--[display context content]--");
            foreach (KeyValuePair<string, object> pair in pluginSandbox.Context)
                Console.WriteLine(string.Format("key: '{0}'; value: '{1}'", pair.Key, Convert.ToString(pair.Value)));
            Console.WriteLine("--[/display context content]--");

            Console.WriteLine("--dispose sandbox--");
            pluginSandbox.Dispose();

            Console.WriteLine("---[display loaded assemblies]---");
            foreach (Assembly assembly in System.AppDomain.CurrentDomain.GetAssemblies())
                Console.WriteLine(assembly.FullName);
            Console.WriteLine("---[display loaded assemblies]---");

            Console.WriteLine(string.Format("--[/entry point in '{0}' domain]--", System.AppDomain.CurrentDomain.FriendlyName));
            Console.ReadKey();
        }
    }
}
