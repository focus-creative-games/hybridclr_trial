using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace HybridCLR.Editor.BuildProcessors
{
    internal class CheckSettings : IPreprocessBuildWithReport
    {
        public int callbackOrder => 0;

        public void OnPreprocessBuild(BuildReport report)
        {
            if (!SettingsUtil.Enable)
            {

#if !UNITY_2020_1_OR_NEWER || !UNITY_IOS
                string oldIl2cppPath = Environment.GetEnvironmentVariable("UNITY_IL2CPP_PATH");
                if (!string.IsNullOrEmpty(oldIl2cppPath))
                {
                    Environment.SetEnvironmentVariable("UNITY_IL2CPP_PATH", "");
                    Debug.Log($"[BPCheckSettings] 清除 UNITY_IL2CPP_PATH, 旧值为:'{oldIl2cppPath}'");
                }
#endif
                return;
            }
            if (UnityEditor.PlayerSettings.gcIncremental)
            {
                Debug.LogError($"[BPCheckSettings] HybridCLR不支持增量式GC，已经自动将该选项关闭");
                UnityEditor.PlayerSettings.gcIncremental = false;
            }


#if !UNITY_2020_1_OR_NEWER || !UNITY_IOS
            string curIl2cppPath = Environment.GetEnvironmentVariable("UNITY_IL2CPP_PATH");
            if (curIl2cppPath != SettingsUtil.LocalIl2CppDir)
            {
                Environment.SetEnvironmentVariable("UNITY_IL2CPP_PATH", SettingsUtil.LocalIl2CppDir);
                Debug.Log($"[BPCheckSettings] UNITY_IL2CPP_PATH 当前值为:'{curIl2cppPath}'，更新为:'{SettingsUtil.LocalIl2CppDir}'");
            }
#endif
        }
    }
}
