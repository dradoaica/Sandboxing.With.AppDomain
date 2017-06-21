using System;
using System.IO;
using System.Reflection;

public class ReflectionHelper
{
    public static Assembly LoadAssemblyFromFile(string name, AppDomain appDomain, string dir)
    {
        Assembly ret = null;

        var assemblyName = new AssemblyName(name);

        string[] assemblyPaths = Directory.GetFiles(dir, assemblyName.Name + ".dll", SearchOption.TopDirectoryOnly);
        if (assemblyPaths.Length > 0)
            ret = appDomain.Load(AssemblyName.GetAssemblyName(assemblyPaths[0]));

        if (ret == null)
        {
            assemblyPaths = Directory.GetFiles(dir, assemblyName.Name + ".exe", SearchOption.TopDirectoryOnly);
            if (assemblyPaths.Length > 0)
                ret = appDomain.Load(AssemblyName.GetAssemblyName(assemblyPaths[0]));
        }

        return ret;
    }

    public static Assembly GetAssembly(string name, string dir)
    {
        if (name == null) throw new ArgumentNullException("name");
        Assembly ret;

        try
        {
            ret = LoadAssemblyFromFile(name, AppDomain.CurrentDomain, dir);
        }
        catch (Exception e)
        {
            throw new ApplicationException(string.Format("Error occurred retrieving assembly '{0}' in path '{1}'", name, Directory.GetCurrentDirectory()), e);
        }

        if (ret == null)
            throw new ApplicationException(string.Format("Cannot find assembly '{0}' in path '{1}'", name, Directory.GetCurrentDirectory()));
        return ret;
    }

    public static Type GetClass(string name, string assemblyName, string dir)
    {
        if (name == null) throw new ArgumentNullException("name");
        if (assemblyName == null) throw new ArgumentNullException("assemblyName");

        var assembly = GetAssembly(assemblyName, dir);
        Type ret;
        try
        {
            ret = assembly.GetType(name);
        }
        catch (Exception e)
        {
            throw new ApplicationException(string.Format("Error occured retrieving type '{0}' from assembly '{1}'", name, assemblyName), e);
        }

        if (ret == null)
            throw new ApplicationException(string.Format("Cannot find type '{0}' in assembly '{1}'", name, assemblyName));
        return ret;
    }

    public static object CreateInstance(string assemblyName, string className, string dir)
    {
        if (assemblyName == null) throw new ArgumentNullException("assemblyName");
        if (className == null) throw new ArgumentNullException("className");

        var type = GetClass(className, assemblyName, dir);

        try
        {
            var ci = type.GetConstructor(Type.EmptyTypes);
            if (ci == null)
                throw new ApplicationException("Cannot find default ctor");
            var ret = ci.Invoke(null);
            return ret;
        }
        catch (Exception e)
        {
            throw new ApplicationException(string.Format("Error instantiating type '{0}' using the default constructor", type), e);
        }
    }
}
