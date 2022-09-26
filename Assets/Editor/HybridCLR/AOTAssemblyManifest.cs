using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AOTAssemblyManifest", menuName = "HybridCLR/AOTAssemblyManifest")]
public class AOTAssemblyManifest : ScriptableObject
{
    [Header("AOT 补充元数据dll列表")]
    public string[] AOTMetadataDlls = new string[] {"mscorlib", "System", "System.Core" };
}
