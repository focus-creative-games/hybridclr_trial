using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditorInternal;
using UnityEngine;

[CreateAssetMenu(fileName = "GlobalSettings", menuName = "HybridCLR/GlobalSettings")]
public class HybridCLRGlobalSettings : ScriptableObject
{
    public bool enable = true;

    public bool cloneFromGitee = true; // false 则从github上拉取

    public string BuildCacheDir = "HybridCLRBuildCache"; //

    public AssemblyDefinitionAsset[] hotfixAssemblyDefinitions;

    public string[] hotfixAssemblies;

    public string[] AOTMetadataDlls;

    public string hotfixDllOutputDir = "HotFixDlls";

    public string hybridCLRDataDir = "HybridCLRData";

    public string strippedAssemblyDir = "AssembliesPostIl2CppStrip";

    public string outputLinkFile = "LinkGenerator/link.xml";

    public int pinvokeReverseWrapperCount = 10;
}
