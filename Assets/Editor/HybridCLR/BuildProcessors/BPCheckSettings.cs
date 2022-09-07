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
    internal class BPCheckSettings : IPreprocessBuildWithReport
    {
        public int callbackOrder => 0;

        public void OnPreprocessBuild(BuildReport report)
        {
            if (UnityEditor.PlayerSettings.gcIncremental)
            {
                Debug.LogError($"[BPCheckSettings] 不支持增量式GC，已经自动将该选项关闭");
                UnityEditor.PlayerSettings.gcIncremental = false;
            }
        }
    }
}
