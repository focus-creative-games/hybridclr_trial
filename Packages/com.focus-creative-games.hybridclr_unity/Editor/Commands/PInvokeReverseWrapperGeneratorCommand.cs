using HybridCLR.Editor.LinkGenerator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace HybridCLR.Editor.Commands
{

    public static class PInvokeReverseWrapperGeneratorCommand
    {

        [MenuItem("HybridCLR/GeneratePInvokeReverseWrapper", priority = 20)]
        public static void GeneratePInvokeReverseWrapper()
        {
            string pInvokeReverseWrapperStubFile = $"{SettingsUtil.LocalIl2CppDir}/libil2cpp/hybridclr/metadata/ReversePInvokeMethodStub.cpp";
            string wrapperTemplateStr = Resources.Load<TextAsset>($"Templates/ReversePInvokeMethodStub.cpp").text;
            int wrapperCount = SettingsUtil.GlobalSettings.pinvokeReverseWrapperCount;
            var generator = new PInvokeReverseWrap.PInvokeReverseWrapGenerator();
            generator.Generate(wrapperTemplateStr, wrapperCount,pInvokeReverseWrapperStubFile);
            Debug.Log($"GeneratePInvokeReverseWrapper. wraperCount:{wrapperCount} output:{pInvokeReverseWrapperStubFile}");
            MethodBridgeCommand.CleanIl2CppBuildCache();
        }
    }
}
