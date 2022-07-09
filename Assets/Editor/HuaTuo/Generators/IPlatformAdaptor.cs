using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Huatuo.Generators
{
    public interface IPlatformAdaptor
    {
        bool IsArch32 { get; }

        TypeInfo CreateTypeInfo(Type type, bool returnValue);

        IEnumerable<MethodBridgeSig> GetPreserveMethods();

        void Generate(List<MethodBridgeSig> methods, List<string> outputLines);
    }
}
