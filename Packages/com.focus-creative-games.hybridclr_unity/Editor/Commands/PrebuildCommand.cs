using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;

namespace HybridCLR.Editor.Commands
{
    public static class PrebuildCommand
    {
        /// <summary>
        /// 按照必要的顺序，执行所有生成操作，适合打包前操作
        /// </summary>
        [MenuItem("HybridCLR/Generate_Link_AOT_MethodBridge_ReversePInvoke", priority = 30)]
        public static void GenerateAll()
        {
            ReversePInvokeWrapperGeneratorCommand.GenerateReversePInvokeWrapper();
            MethodBridgeGeneratorCommand.GenerateMethodBridge();
            LinkGeneratorCommand.GenerateLinkXml();
        }
    }
}
