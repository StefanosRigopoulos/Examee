using System.Reflection;
using System.Runtime.Loader;

namespace API.Helpers
{
    public class CustomAssemblyLoadContext : AssemblyLoadContext
    {
        private readonly string _dependencyPath;
        public CustomAssemblyLoadContext(string dependencyPath) : base(isCollectible: true)
        {
            _dependencyPath = dependencyPath;
            Resolving += ResolveDependency;
        }
        private Assembly ResolveDependency(AssemblyLoadContext context, AssemblyName assemblyName)
        {
            string dependencyDllPath = Path.Combine(_dependencyPath, assemblyName.Name + ".dll");
            if (File.Exists(dependencyDllPath))
            {
                return LoadFromAssemblyPath(dependencyDllPath);
            }
            return null!;
        }
        protected override Assembly Load(AssemblyName assemblyName)
        {
            string dependencyFilePath = Path.Combine(_dependencyPath, $"{assemblyName.Name}.dll");
            if (File.Exists(dependencyFilePath))
            {
                return LoadFromAssemblyPath(dependencyFilePath);
            }
            return null!;
        }
    }
}