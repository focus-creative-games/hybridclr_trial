using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UnityEditor;
using UnityEditor.Callbacks;

#if UNITY_IOS || UNITY_STANDALONE_OSX || true
using UnityEditor.iOS.Xcode;

namespace HybridCLR.Editor.BuildProcessors
{

    internal static class GenerateXcodeWorkspace
    {
        [PostProcessBuild]
        private static void OnPostprocessBuild(BuildTarget target, string path)
        {
            // 先只支持ios， mac以后再说
            switch (target)
            {
                case BuildTarget.iOS:
                    OnPostprocessBuildIOS(path);
                    break;
                default:
                    break;

            }
        }

        private static void OnPostprocessBuildIOS(string path)
        {
            string projectPath = PBXProject.GetPBXProjectPath(path);
            string il2cppProjPath = Path.Combine(BuildConfig.HybridCLRDataDir, "iOSBuild", "build", "il2cpp.xcodeproj");


            PBXProject project = new PBXProject();
            project.ReadFromString(File.ReadAllText(projectPath));
            string targetGUID = project.GetUnityFrameworkTargetGuid();

            // 移除旧的libil2cpp.a
            var libil2cppGUID = project.FindFileGuidByProjectPath("Libraries/libil2cpp.a");
            project.RemoveFileFromBuild(targetGUID, libil2cppGUID);
            project.RemoveFile(libil2cppGUID);
            File.Delete(Path.Combine(path, "Libraries", "libil2cpp.a"));

            #region 使用新的libil2cpp.a
            var libDir = Path.Combine(BuildConfig.HybridCLRDataDir, "iOSBuild", "build", "lib");

            // 添加文件引用并且链接
            var libPath = Path.Combine(libDir, "libil2cpp_original.a");
            var guid = project.AddFile(libPath, "Libraries/libil2cpp_original.a", PBXSourceTree.Sdk);
            project.AddFileToBuild(targetGUID, guid);

            libPath = Path.Combine(libDir, "libexternal.a");
            guid = project.AddFile(libPath, "Libraries/libexternal.a", PBXSourceTree.Sdk);
            project.AddFileToBuild(targetGUID, guid);

            libPath = Path.Combine(libDir, "libobjective.a");
            guid = project.AddFile(libPath, "Libraries/libobjective.a", PBXSourceTree.Sdk);
            project.AddFileToBuild(targetGUID, guid);

            project.AddBuildProperty(targetGUID, "LIBRARY_SEARCH_PATHS", libDir);

            #endregion

            project.WriteToFile(projectPath);

            #region 创建Workspace
            XmlDocument doc = new XmlDocument();
            var root = doc.CreateElement("Workspace"/*, doc.DocumentElement.NamespaceURI*/);

            var fileRef = doc.CreateElement("FileRef"/*, doc.DocumentElement.NamespaceURI*/);
            fileRef.SetAttribute("location", $"group:{Path.GetDirectoryName(projectPath)}");
            root.AppendChild(fileRef);

            fileRef = doc.CreateElement("FileRef"/*, doc.DocumentElement.NamespaceURI*/);

            fileRef.SetAttribute("location", $"group:{il2cppProjPath}");
            root.AppendChild(fileRef);

            doc.AppendChild(root);

            var workspacePath = Path.Combine(path, "Unity-iPhone.xcworkspace");
            if (!Directory.Exists(workspacePath))
                Directory.CreateDirectory(workspacePath);

            using (StringWriter sw = new StringWriter())
            {
                using (XmlTextWriter tx = new XmlTextWriter(sw))
                {
                    tx.Formatting = Formatting.Indented;
                    doc.WriteTo(tx);
                    tx.Flush();

                    File.WriteAllText(Path.Combine(workspacePath, "contents.xcworkspacedata"), sw.GetStringBuilder().ToString());
                }
            }
            #endregion
        }
    }

}
#endif
