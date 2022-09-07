using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditorInternal;
using UnityEngine;

[CreateAssetMenu(fileName = "GlobalSettings", menuName = "HybridCLR/GlobalSettings")]
public class HybridCLRGlobalSettings : ScriptableObject
{
    public bool enable = true;

    public string BuildCacheDir = "HybridCLRBuildCache"; //

    public AssemblyDefinitionAsset[] hotfixAssemblyDefinitions;

    public string[] hotfixAssemblies;

    public string[] AOTMetadataDlls;

    public string hotfixDllOutputDir = "HotFixDlls";

    public string hybridCLRDataDir = "HybridCLRData";

    public string strippedAssemblyDir = "AssembliesPostIl2CppStrip";
}
