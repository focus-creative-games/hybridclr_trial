using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Diagnostics;

using Debug = UnityEngine.Debug;

namespace HybridCLR
{
    public enum InstallErrorCode
    {
        Ok,
        Il2CppInstallPathNotMatchIl2CppBranch,
        Il2CppInstallPathNotExists,
        NotIl2CppPath,
    }

    public partial class InstallController
    {
        private string m_Il2CppInstallDirectory;

        public string Il2CppInstallDirectory
        {
            get
            {
                return m_Il2CppInstallDirectory;
            }
            set
            {
                m_Il2CppInstallDirectory = value?.Replace('\\', '/');
                if (!string.IsNullOrEmpty(m_Il2CppInstallDirectory))
                {
                    EditorPrefs.SetString("UnityInstallDirectory", m_Il2CppInstallDirectory);
                }
            }
        }

        private string GetIl2CppPlusBranchByUnityVersion(string unityVersion)
        {
            if (unityVersion.Contains("2020."))
            {
                return "2020.3.33";
            }
            if (unityVersion.Contains("2021."))
            {
                return "2021.3.1";
            }
            return "not support";
        }

        public string Il2CppBranch => GetIl2CppPlusBranchByUnityVersion(Application.unityVersion);

        public string InitLocalIl2CppBatFile => Application.dataPath + "/../HybridCLRData/init_local_il2cpp_data.bat";

        public string InitLocalIl2CppBashFile => Application.dataPath + "/../HybridCLRData/init_local_il2cpp_data.sh";

        public InstallController()
        {
            PrepareIl2CppInstallPath();
        }

        void PrepareIl2CppInstallPath()
        {
            m_Il2CppInstallDirectory = EditorPrefs.GetString("Il2CppInstallDirectory");
            if (CheckValidIl2CppInstallDirectory(Il2CppBranch, m_Il2CppInstallDirectory) == InstallErrorCode.Ok)
            {
                return;
            }
            var il2cppBranch = Il2CppBranch;
            var curAppInstallPath = EditorApplication.applicationPath;
            if (curAppInstallPath.Contains(il2cppBranch))
            {
                Il2CppInstallDirectory = $"{Directory.GetParent(curAppInstallPath)}/Data/il2cpp";
                return;
            }
            string unityHubRootDir = Directory.GetParent(curAppInstallPath).Parent.Parent.ToString();
            Debug.Log("unity hub root dir:" + unityHubRootDir);
            foreach (var unityInstallDir in Directory.GetDirectories(unityHubRootDir, "*", SearchOption.TopDirectoryOnly))
            {
                Debug.Log("nity install dir:" + unityInstallDir);
                if (unityInstallDir.Contains(il2cppBranch))
                {
                    Il2CppInstallDirectory = $"{unityInstallDir}/Editor/Data/il2cpp";
                    return;
                }
            }

            Il2CppInstallDirectory = $"{Directory.GetParent(curAppInstallPath)}/Data/il2cpp";
        }

        public void InitHybridCLR(string il2cppBranch, string il2cppInstallPath)
        {
            if (CheckValidIl2CppInstallDirectory(il2cppBranch, il2cppInstallPath) != InstallErrorCode.Ok)
            {
                Debug.LogError($"请正确设置 il2cpp 安装目录");
                return;
            }

            if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                RunInitLocalIl2CppDataBat(il2cppBranch, il2cppInstallPath);
            }
            else
            {
                RunInitLocalIl2CppDataBash(il2cppBranch, il2cppInstallPath);
            }
        }

        public InstallErrorCode CheckValidIl2CppInstallDirectory(string il2cppBranch, string installDir)
        {
            installDir = installDir.Replace('\\', '/');
            if (!Directory.Exists(installDir))
            {
                return InstallErrorCode.Il2CppInstallPathNotExists;
            }

            if (!installDir.Contains(il2cppBranch))
            {
                return InstallErrorCode.Il2CppInstallPathNotMatchIl2CppBranch;
            }

            if (!installDir.EndsWith("/Editor/Data/il2cpp"))
            {
                return InstallErrorCode.NotIl2CppPath;
            }

            return InstallErrorCode.Ok;
        }

        private void RunInitLocalIl2CppDataBat(string il2cppBranch, string il2cppInstallPath)
        {
            using (Process p = new Process())
            {
                p.StartInfo.WorkingDirectory = Application.dataPath + "/../HybridCLRData";
                p.StartInfo.FileName = InitLocalIl2CppBatFile;
                p.StartInfo.UseShellExecute = true;
                p.StartInfo.Arguments = $"{il2cppBranch} \"{il2cppInstallPath}\"";
                p.Start();
                p.WaitForExit();
            }
        }

        private void RunInitLocalIl2CppDataBash(string il2cppBranch, string il2cppInstallPath)
        {
            using (Process p = new Process())
            {
                p.StartInfo.WorkingDirectory = Application.dataPath + "/../HybridCLRData";
                p.StartInfo.FileName = InitLocalIl2CppBashFile;
                p.StartInfo.UseShellExecute = true;
                p.StartInfo.Arguments = $"{il2cppBranch} '{il2cppInstallPath}'";
                p.Start();
                p.WaitForExit();
            }
        }
    }
}
