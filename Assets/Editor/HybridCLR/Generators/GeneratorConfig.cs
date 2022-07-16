using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HybridCLR.Generators
{
    internal class GeneratorConfig
    {


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
        public static List<string> PrepareCustomMethodSignatures64()
        {
            return new List<string>
            {
                "vi8i8",
            };
        }

        /// <summary>
        /// 如果提示缺失桥接函数，将提示缺失的签名加入到下列列表是简单的做法。
        /// 这里添加32位App缺失的桥接函数签名
        /// </summary>
        /// <returns></returns>
        public static List<string> PrepareCustomMethodSignatures32()
        {
            return new List<string>
            {
                "vi4i4",
            };
        }
    }
}
