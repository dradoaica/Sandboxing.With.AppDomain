using System.Collections.Generic;
using Plugin.Common;
using System;
using System.Reflection;
using System.IO;

namespace Plugin.Implementation
{
    public class PluginImpl : PluginBase
    {
        public PluginImpl()
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        }

        private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            // TODO: handle fail when loading assembly from another location than the application base
            return null;
        }

        public override void EntryPoint(ref Dictionary<string, object> context)
        {
            Console.WriteLine(string.Format("---[entry point in '{0}' domain]---", AppDomain.CurrentDomain.FriendlyName));
            Console.WriteLine("---populate context---");
            context.Add(Guid.NewGuid().ToString(), new Random().Next(int.MinValue, int.MaxValue));
            context.Add(Guid.NewGuid().ToString(), new Random().Next(int.MinValue, int.MaxValue));
            context.Add(Guid.NewGuid().ToString(), new Random().Next(int.MinValue, int.MaxValue));
            context.Add(Guid.NewGuid().ToString(), new Random().Next(int.MinValue, int.MaxValue));
            Console.WriteLine("---load assembly from another location than the application base---");
            Object obj = ReflectionHelper.CreateInstance("ExternalClassLibrary", "ExternalClassLibrary.DummyClass", Path.GetFullPath(@"..\..\..\ExternalClassLibrary\bin\Debug"));
            Console.WriteLine("---[display loaded assemblies]---");
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
                Console.WriteLine(assembly.FullName);
            Console.WriteLine("---[display loaded assemblies]---");
            Console.WriteLine(string.Format("---[/entry point in '{0}' domain]---", AppDomain.CurrentDomain.FriendlyName));
        }

        public override void Dispose()
        {
            base.Dispose();
            AppDomain.CurrentDomain.AssemblyResolve -= CurrentDomain_AssemblyResolve;
        }
    }
}
