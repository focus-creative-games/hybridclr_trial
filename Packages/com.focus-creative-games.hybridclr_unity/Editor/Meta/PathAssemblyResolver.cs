using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HybridCLR.Editor.Meta
{
    public class PathAssemblyResolver : IAssemblyResolver
    {
        private readonly string[] _searchPaths;
        public PathAssemblyResolver(params string[] searchPaths)
        {
            _searchPaths = searchPaths;
        }

        public string ResolveAssembly(string assemblyName)
        {
            foreach(var path in _searchPaths)
            {
                string assPath = Path.Combine(path, assemblyName, ".dll");
                if (File.Exists(assPath))
                {
                    return assPath;
                }
            }
            throw new Exception($"Not find assembly:{assemblyName}");
        }
    }
}
