//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using UnityEditor;
//using UnityEngine;

//namespace HuaTuo.Generators
//{


//    public static class GeneratorEditorCommand
//    {

//        [MenuItem("HuaTuo/Generate/MethodBridge_X64")]
//        public static void MethodBridge_X86()
//        {
//            //var target = EditorUserBuildSettings.activeBuildTarget;

//            var g = new MethodBridgeGenerator(new MethodBridgeGeneratorOptions()
//            {
//                CallConvention = CallConventionType.X64,
//                Assemblies = AppDomain.CurrentDomain.GetAssemblies().ToList(),
//                OutputFile = $"{Application.dataPath}/../Library/Huatuo/MethodBridge_x64.cpp",
//            });

//            g.PrepareMethods();
//            g.Generate();

//        }

//        [MenuItem("HuaTuo/Generate/MethodBridge_Arm64")]
//        public static void MethodBridge_Arm64()
//        {
//            var g = new MethodBridgeGenerator(new MethodBridgeGeneratorOptions()
//            {
//                CallConvention = CallConventionType.Arm64,
//                Assemblies = AppDomain.CurrentDomain.GetAssemblies().ToList(),
//                OutputFile = $"{Application.dataPath}/../Library/Huatuo/MethodBridge_arm64.cpp",
//            });

//            g.PrepareMethods();
//            g.Generate();
//        }
//    }
//}
