using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Unity.Collections.LowLevel.Unsafe;

namespace HybridCLR
{
    public static class RuntimeApi
    {
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
        public static int GetInterpreterThreadObjectStackSize()
        {
            return GetRuntimeOption(RuntimeOptionId.InterpreterThreadObjectStackSize);
        }

        /// <summary>
        /// 设置解释器线程栈的最大StackObject个数(size*8 为最终占用的内存大小)
        /// </summary>
        /// <param name="size"></param>
        public static void SetInterpreterThreadObjectStackSize(int size)
        {
            SetRuntimeOption(RuntimeOptionId.InterpreterThreadObjectStackSize, size);
        }
        

        /// <summary>
        /// 获取解释器线程函数帧数量(sizeof(InterpreterFrame)*size 为最终占用的内存大小)
        /// </summary>
        /// <returns></returns>
        public static int GetInterpreterThreadFrameStackSize()
        {
            return GetRuntimeOption(RuntimeOptionId.InterpreterThreadFrameStackSize);
        }
        
        /// <summary>
        /// 设置解释器线程函数帧数量(sizeof(InterpreterFrame)*size 为最终占用的内存大小)
        /// </summary>
        /// <param name="size"></param>
        public static void SetInterpreterThreadFrameStackSize(int size)
        {
            SetRuntimeOption(RuntimeOptionId.InterpreterThreadFrameStackSize, size);
        }

        public static bool IsTransformOptimizationEnabled()
        {
            return GetRuntimeOption(RuntimeOptionId.TransformOptimization) != 0;
        }

        public static void EnableTransformOptimization(bool enable)
        {
            SetRuntimeOption(RuntimeOptionId.TransformOptimization, enable ? 1 : 0);
        }

        public static bool IsLiveObjectValidationEnabled()
        {
            return GetRuntimeOption(RuntimeOptionId.LiveObjectValidation) != 0;
        }

        public static void EnableLiveObjectValidation(bool enable)
        {
            SetRuntimeOption(RuntimeOptionId.LiveObjectValidation, enable ? 1 : 0);
        }


#if UNITY_EDITOR

        private static readonly Dictionary<RuntimeOptionId, int> s_runtimeOptions = new Dictionary<RuntimeOptionId, int>();

        public static void SetRuntimeOption(RuntimeOptionId optionId, int value)
        {
            s_runtimeOptions[optionId] = value;
        }
#else
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern void SetRuntimeOption(RuntimeOptionId optionId, int value);
#endif

#if UNITY_EDITOR
        public static int GetRuntimeOption(RuntimeOptionId optionId)
        {
            if (s_runtimeOptions.TryGetValue(optionId, out var value))
            {
                return value;
            }
            return 0;
        }
#else
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern int GetRuntimeOption(RuntimeOptionId optionId);
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
        public static void UnloadAssembly(Assembly assembly)
        {

        }
#else
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern void UnloadAssembly(Assembly assembly);
#endif

        /// <summary>
        /// 加载访问策略
        /// </summary>
        /// <param name="accessPolicyData"></param>
#if UNITY_EDITOR
        public static void LoadAccessPolicy(byte[] accessPolicyData)
        {

        }
#else
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern void LoadAccessPolicy(byte[] accessPolicyData);
#endif
    }
}
