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
        }

        protected override Assembly Load(AssemblyName assemblyName)
        {
            string dependencyFilePath = Path.Combine(_dependencyPath, $"{assemblyName.Name}.dll");
            if (File.Exists(dependencyFilePath))
            {
                return LoadFromAssemblyPath(dependencyFilePath);
            }
            return null;
        }
    }
}