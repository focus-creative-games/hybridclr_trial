using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace HybridCLR.Editor
{
    internal static class SetUpOnLoad
    {
#if !UNITY_2020_1_OR_NEWER || !UNITY_IOS
        //[InitializeOnLoadMethod]
        private static void Setup()
        {
            if (!SettingsUtil.Enable)
            {
                return;
            }

            ///
            /// unity允许使用UNITY_IL2CPP_PATH环境变量指定il2cpp的位置，因此我们不再直接修改安装位置的il2cpp，
            /// 而是在本地目录
            ///
            //var installerController = new Installer.InstallerController();
            //if (!installerController.HasInstalledHybridCLR())
            //{
            //    if (installerController.CheckValidIl2CppInstallDirectory(installerController.Il2CppBranch, installerController.Il2CppInstallDirectory) == Installer.InstallErrorCode.Ok)
            //    {
            //        if(EditorUtility.DisplayDialog(
            //              "初始化HybridCLR", // title
            //              "检测到未初始化HybridCLR，是否自动安装", // description
            //              "Yes",
            //              "No"
            //            ))
            //        {
            //            installerController.InitHybridCLR(installerController.Il2CppBranch, installerController.Il2CppInstallDirectory);
            //        }
            //    }
            //    if (!installerController.HasInstalledHybridCLR())
            //    {
            //        Debug.LogError($"未初始化HybridCLR,请在菜单 HybridCLR/Installer 中执行安装");
            //    }
            //}
        }
#endif
    }
}
