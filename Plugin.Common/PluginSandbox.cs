using System;
using System.Collections.Generic;

namespace Plugin.Common
{
    public class PluginSandbox : IDisposable
    {
        private AppDomain _sandbox;
        private PluginBase _plugin;
        public PluginBase Plugin { get { return _plugin; } }
        private Dictionary<string, object> _context;
        public Dictionary<string, object> Context
        {
            get { return _context; }
            set { _context = value; }
        }
        private Exception _exception;
        public Exception Exception { get { return _exception; } }

        public PluginSandbox(PluginSandboxOptions options)
        {
            // default context
            _context = new Dictionary<string, object>();
            Init(options);
        }

        private void Init(PluginSandboxOptions options)
        {
            try
            {
                var info = new AppDomainSetup();
                info.ApplicationBase = options.ApplicationBase;
                if (options.PermissionSet == null)
                    _sandbox = AppDomain.CreateDomain(options.FriendlyName, options.Evidence, info);
                else
                    _sandbox = AppDomain.CreateDomain(options.FriendlyName, options.Evidence, info, options.PermissionSet);
                object obj = _sandbox.CreateInstanceAndUnwrap(options.AssemblyName, options.TypeName);
                _plugin = obj as PluginBase;
                if (_plugin == null)
                    throw new ApplicationException(string.Format("Type '{0}' must extend 'Plugin.Common.PluginBase'"));
            }
            catch (Exception e)
            {
                _exception = e;
            }
        }

        public bool CallEntryPoint()
        {
            if (_exception != null)
                return false;

            try
            {
                _plugin.EntryPoint(ref _context);

                return true;
            }
            catch (Exception e)
            {
                _exception = e;

                return false;
            }
        }

        public void Dispose()
        {
            if (_plugin != null)
            {
                _plugin.Dispose();
                _plugin = null;
            }
            if (_sandbox != null)
            {
                AppDomain.Unload(_sandbox);
                _sandbox = null;
            }
        }
    }
}
