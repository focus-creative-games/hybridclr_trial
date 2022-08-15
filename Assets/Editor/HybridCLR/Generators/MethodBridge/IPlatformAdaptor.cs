using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HybridCLR.Generators.MethodBridge
{
    public interface IPlatformAdaptor
    {
        bool IsArch32 { get; }

        TypeInfo CreateTypeInfo(Type type, bool returnValue);

        void GenerateNormalMethod(MethodBridgeSig method, List<string> outputLines);

        void GenerateNormalStub(List<MethodBridgeSig> methods, List<string> lines);

        void GenerateAdjustThunkMethod(MethodBridgeSig method, List<string> outputLines);

        void GenerateAdjustThunkStub(List<MethodBridgeSig> methods, List<string> lines);
    }
}
