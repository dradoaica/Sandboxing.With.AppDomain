using System;
using System.Collections.Generic;

namespace Plugin.Common
{
    public abstract class PluginBase : MarshalByRefObject, IDisposable
    {
        public abstract void EntryPoint(ref Dictionary<string, object> context);
        public virtual void Dispose() { }
    }
}
