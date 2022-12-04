using System.Security;
using System.Security.Policy;

namespace Plugin.Common
{
    public class PluginSandboxOptions
    {
        public string FriendlyName { get; set; }
        public string ApplicationBase { get; set; }
        public string AssemblyName { get; set; }
        public string TypeName { get; set; }
        public Evidence Evidence { get; set; }
        public PermissionSet PermissionSet { get; set; }
    }
}
