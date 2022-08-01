using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace HybridCLR
{
    public static partial class BuildConfig
    {

        /// <summary>
        /// 需要在Prefab上挂脚本的热更dll名称列表，不需要挂到Prefab上的脚本可以不放在这里
        /// 但放在这里的dll即使勾选了 AnyPlatform 也会在打包过程中被排除
        /// 
        /// 另外请务必注意： 需要挂脚本的dll的名字最好别改，因为这个列表无法热更（上线后删除或添加某些非挂脚本dll没问题）。
        /// 
        /// 注意：多热更新dll不是必须的！大多数项目完全可以只有HotFix.dll这一个热更新模块,纯粹出于演示才故意设计了两个热更新模块。
        /// 另外，是否热更新跟dll名毫无关系，凡是不打包到主工程的，都可以是热更新dll。
        /// </summary>
        public static List<string> MonoHotUpdateDllNames { get; } = new List<string>()
        {
            "HotFix.dll",
        };

        /// <summary>
        /// 所有热更新dll列表。放到此列表中的dll在打包时OnFilterAssemblies回调中被过滤。
        /// </summary>
        public static List<string> AllHotUpdateDllNames { get; } = MonoHotUpdateDllNames.Concat(new List<string>
        {
            // 这里放除了s_monoHotUpdateDllNames以外的脚本不需要挂到资源上的dll列表
            "HotFix2.dll",
        }).ToList();

        public static List<string> AOTMetaDlls { get; } = new List<string>()
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
