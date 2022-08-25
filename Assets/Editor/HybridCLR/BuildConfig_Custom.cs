using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace HybridCLR.Editor
{
    public static partial class BuildConfig
    {

        /// <summary>
        /// 所有热更新dll列表。放到此列表中的dll在打包时OnFilterAssemblies回调中被过滤。
        /// </summary>
        public static List<string> HotUpdateAssemblies { get; } = new List<string>
        {
            "HotFix.dll",
            "HotFix2.dll",
        };

        public static List<string> AOTMetaAssemblies { get; } = new List<string>()
        {
            "mscorlib.dll",
            "System.dll",
            "System.Core.dll", // 如果使用了Linq，需要这个
        };

        public static List<string> AssetBundleFiles { get; } = new List<string>
        {
            "common",
        };
    }
}
