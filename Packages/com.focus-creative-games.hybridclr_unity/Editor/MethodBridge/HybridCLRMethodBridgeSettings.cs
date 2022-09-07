using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HybridCLR.Editor
{
    [CreateAssetMenu(fileName = "MethodBridgeSettings", menuName = "HybridCLR/MethodBridgeSettings")]
    public class HybridCLRMethodBridgeSettings : ScriptableObject
    {
        ///// <summary>
        ///// 目前已经根据热更新dll的依赖自动计算需要扫描哪些dll来收集桥接函数。
        ///// 只要你的热更新以assembly def形式放到项目中，是不需要改这个的
        ///// </summary>
        ///// <returns></returns>
        //public string[] extraAssembiles;

        /// <summary>
        /// 暂时没有仔细扫描泛型，如果运行时发现有生成缺失，先手动在此添加类
        /// </summary>
        /// <returns></returns>
        public static List<Type> PrepareCustomGenericTypes()
        {
            return new List<Type>
            {
                typeof(Action<int, string, Vector3>),
            };
        }

        /// <summary>
        /// 如果提示缺失桥接函数，将提示缺失的签名加入到下列列表是简单的做法。
        /// 这里添加64位App缺失的桥接函数签名
        /// </summary>
        /// <returns></returns>
        public string[] customMethodSignatures64;

        /// <summary>
        /// 如果提示缺失桥接函数，将提示缺失的签名加入到下列列表是简单的做法。
        /// 这里添加32位App缺失的桥接函数签名
        /// </summary>
        /// <returns></returns>
        public string[] customMethodSignatures32;
    }
}
