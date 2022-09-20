using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HotUpdateAssemblyManifest", menuName = "HybridCLR/HotUpdateAssemblyManifest")]
public class HotUpdateAssemblyManifest : ScriptableObject
{
    [Header("AOT 补充元数据dll列表")]
    public string[] AOTMetadataDlls = new string[] {"mscorlib", "System", "System.Core" };
}
