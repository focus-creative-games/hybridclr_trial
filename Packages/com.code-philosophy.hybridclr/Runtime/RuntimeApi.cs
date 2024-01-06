using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using HybridCLR.Runtime;

namespace HybridCLR
{
    public static class RuntimeApi
    {
#if UNITY_EDITOR
        private static int s_interpreterThreadObjectStackSize = 128 * 1024;
        private static int s_interpreterThreadFrameStackSize = 2 * 1024;
        private static bool s_enableTransformOptimization = true;
#endif

        /// <summary>
        /// 加载补充元数据assembly
        /// </summary>
        /// <param name="dllBytes"></param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
#if UNITY_EDITOR
        public static unsafe LoadImageErrorCode LoadMetadataForAOTAssembly(byte[] dllBytes, HomologousImageMode mode)
        {
            return LoadImageErrorCode.OK;
        }
#else
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern LoadImageErrorCode LoadMetadataForAOTAssembly(byte[] dllBytes, HomologousImageMode mode);
#endif

        /// <summary>
        /// 获取解释器线程栈的最大StackObject个数(size*8 为最终占用的内存大小)
        /// </summary>
        /// <returns></returns>
#if UNITY_EDITOR
        public static int GetInterpreterThreadObjectStackSize()
        {
            return s_interpreterThreadObjectStackSize;
        }
#else
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern int GetInterpreterThreadObjectStackSize();
#endif

        /// <summary>
        /// 设置解释器线程栈的最大StackObject个数(size*8 为最终占用的内存大小)
        /// </summary>
        /// <param name="size"></param>
#if UNITY_EDITOR
        public static void SetInterpreterThreadObjectStackSize(int size)
        {
            s_interpreterThreadObjectStackSize = size;
        }
#else
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern void SetInterpreterThreadObjectStackSize(int size);
#endif
        /// <summary>
        /// 获取解释器线程函数帧数量(sizeof(InterpreterFrame)*size 为最终占用的内存大小)
        /// </summary>
        /// <returns></returns>
#if UNITY_EDITOR
        public static int GetInterpreterThreadFrameStackSize()
        {
            return s_interpreterThreadFrameStackSize;
        }
#else
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern int GetInterpreterThreadFrameStackSize();
#endif
        
        /// <summary>
        /// 设置解释器线程函数帧数量(sizeof(InterpreterFrame)*size 为最终占用的内存大小)
        /// </summary>
        /// <param name="size"></param>
#if UNITY_EDITOR
        public static void SetInterpreterThreadFrameStackSize(int size)
        {
            s_interpreterThreadFrameStackSize = size;
        }
#else
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern void SetInterpreterThreadFrameStackSize(int size);
#endif

        /// <summary>
        /// 是否开启了完全泛型共享.
        /// 对于 Unity 2020及更早版本, 返回false
        /// 对于 Unity 2021版本，如果打包时使用 faster(smaller)选项，则为true，否则为false
        /// 对于 Unity 2022及更高版本，返回true
        /// </summary>
        /// <param name="size"></param>
#if UNITY_EDITOR || !UNITY_2021
        public static bool IsFullGenericSharingEnabled()
        {
#if UNITY_2022_1_OR_NEWER
            return true;
#else
            return false;
#endif
        }
#else
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern bool IsFullGenericSharingEnabled();
#endif

#if UNITY_EDITOR
        public static bool IsTransformOptimizationEnabled()
        {
            return s_enableTransformOptimization;
        }
#else
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern bool IsTransformOptimizationEnabled();
#endif

#if UNITY_EDITOR
        public static void EnableTransformOptimization(bool enable)
        {
            s_enableTransformOptimization = enable;
        }
#else
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern void EnableTransformOptimization(bool enable);
#endif

        /// <summary>
        /// 加载差分混合执行 assembly
        /// </summary>
        /// <param name="currentDllBytes">热更新dhe dll的文件内容</param>
        /// <param name="optionBytes">dhao数据。如果为null，表示完全没有变化</param>
        /// <param name="originalDllMd5">用于生成dhao的原始aot dll的md5值，格式为32个字符的"AABB..." </param>
        /// <param name="currentDllMd5">用于生成dhao的热更新aot dll（文件内容即为参数currentDllBytes）的md5值，格式为32个字符的"AABB..." </param>
        /// <returns></returns>
        public static unsafe LoadImageErrorCode LoadDifferentialHybridAssembly(byte[] currentDllBytes, byte[] optionBytes, string originalDllMd5, string currentDllMd5)
        {
            var option = new DifferentialHybridAssemblyOptions();
            option.Unmarshal(optionBytes);
            if (option.OriginalDllMD5 != originalDllMd5)
            {
                throw new ArgumentException($"originalDllMd5 not match. from argument:{originalDllMd5} from option.OriginalDllMd5:{currentDllMd5}");
            }
            if (option.CurrentDllMD5 != currentDllMd5)
            {
                throw new ArgumentException($"originalDllMd5 not match. from argument:{originalDllMd5} from option.OriginalDllMd5:{currentDllMd5}");
            }
            return LoadDifferentialHybridAssembly(currentDllBytes, optionBytes);
        }

#if UNITY_EDITOR
        private static LoadImageErrorCode LoadDifferentialHybridAssembly(byte[] dllBytes, byte[] optionBytes)
        {
            return LoadImageErrorCode.OK;
        }
#else
        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern unsafe LoadImageErrorCode LoadDifferentialHybridAssembly(byte[] dllBytes, byte[] optionBytes);
#endif
    }
}
