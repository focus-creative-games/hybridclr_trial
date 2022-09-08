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
        [InitializeOnLoadMethod]
        private static void Setup()
        {
            if (!SettingsUtil.Enable)
            {
                return;
            }

            var installerController = new Installer.InstallerController();
            if (!installerController.HasInstalledHybridCLR())
            {
                if (installerController.CheckValidIl2CppInstallDirectory(installerController.Il2CppBranch, installerController.Il2CppInstallDirectory) == Installer.InstallErrorCode.Ok)
                {
                    if (EditorUtility.DisplayDialog(
                          "初始化HybridCLR", // title
                          "检测到未初始化HybridCLR，是否自动安装", // description
                          "Yes",
                          "No"
                        ))
                    {
                        installerController.InitHybridCLR(installerController.Il2CppBranch, installerController.Il2CppInstallDirectory);
                    }
                }
                if (!installerController.HasInstalledHybridCLR())
                {
                    Debug.LogError($"未初始化HybridCLR,请在菜单 HybridCLR/Installer 中执行安装");
                }
            }
        }
    }
}
