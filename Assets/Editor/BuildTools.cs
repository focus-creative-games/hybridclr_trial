using HybridCLR.Editor;
using HybridCLR.Editor.DHE;
using HybridCLR.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;

public static class BuildTools
{
    public const string BackupAOTDllDir = "HybridCLRData/BackupAOT";

    public const string EncrypedDllDir = "HybridCLRData/EncryptedDll";

    public const string DhaoDir = "HybridCLRData/Dhao";

    public const string ManifestFile = "manifest.txt";


    /// <summary>
    /// 备份构建主包时生成的裁剪AOT dll
    /// </summary>
    [MenuItem("BuildTools/BackupAOTDll")]
    public static void BackupAOTDllFromAssemblyPostStrippedDir()
    {
        BuildTarget target = EditorUserBuildSettings.activeBuildTarget;
        var backupDir = $"{BackupAOTDllDir}/{target}";
        System.IO.Directory.CreateDirectory(backupDir);
        var dlls = System.IO.Directory.GetFiles(SettingsUtil.GetAssembliesPostIl2CppStripDir(target));
        foreach (var dll in dlls)
        {
            var fileName = System.IO.Path.GetFileName(dll);
            string dstFile = $"{BackupAOTDllDir}/{target}/{fileName}";
            System.IO.File.Copy(dll, dstFile, true);
            Debug.Log($"BackupAOTDllFromAssemblyPostStrippedDir: {dll} -> {dstFile}");
        }
    }

    /// <summary>
    /// 创建dhe manifest文件，格式为每行一个 'dll名，原始dll的md5'
    /// </summary>
    /// <param name="outputDir"></param>
    [MenuItem("BuildTools/CreateManifestAtBackupDir")]
    public static void CreateManifest()
    {
        BuildTarget target = EditorUserBuildSettings.activeBuildTarget;
        string backupDir = $"{BackupAOTDllDir}/{target}";
        CreateManifest(backupDir);
    }

    public static string CreateMD5Hash(byte[] bytes)
    {
        return BitConverter.ToString(new MD5CryptoServiceProvider().ComputeHash(bytes)).Replace("-", "").ToUpperInvariant();
    }

    public static void CreateManifest(string outputDir)
    {
        Directory.CreateDirectory(outputDir);
        var lines = new List<string>();
        BuildTarget target = EditorUserBuildSettings.activeBuildTarget;
        string backupDir = $"{BackupAOTDllDir}/{target}";
        foreach (string dheDll in SettingsUtil.DifferentialHybridAssemblyNames)
        {
            string originalDll = $"{backupDir}/{dheDll}.dll";
            string originalDllMd5 = CreateMD5Hash(File.ReadAllBytes(originalDll));
            lines.Add($"{dheDll},{originalDllMd5}");
        }
        string manifestFile = $"{outputDir}/{ManifestFile}";
        File.WriteAllBytes(manifestFile, System.Text.Encoding.UTF8.GetBytes(string.Join("\n", lines)));
        Debug.Log($"CreateManifest: {manifestFile}");
    }
}
