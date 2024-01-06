# 发布日志

## 4.5.8

发布日期 2023.12.08.

### Runtime

- [new] 新增2019支持
- [fix] 修复优化 box; brtrue|brfalse序列时，当类型为class或nullable类型时，无条件转换为无条件branch语句的bug
- [fix] 修复 ClassFieldLayoutCalculator未释放 _classMap的每个key-value对中value对象，造成内存泄露的bug
- [fix] 修复计算 ExplicitLayout的struct的native_size的bug
- [fix] 修复当出现签名完全相同的虚函数与虚泛型函数时，计算override未考虑泛型签名，错误地返回了不匹配的函数，导致虚表错误的bug
- [fix][2021] 修复开启faster(smaller) build选项后某些情况下完全泛型共享AOT函数未使用补充元数据来设置函数指针，导致调用时出错的bug
- [fix] 修复 SuperSetAOTHomologousImage.h 在 android平台的编译错误
- [fix] 修复ConvertInvokeArgs有可能传递了非对齐args，导致CopyStackObject在armv7这种要求内存对齐的平台发生崩溃的bug
- [fix] 修复Image::ReadGenericClas中Il2CppGenericInst内存泄露的bug
- [fix] 修复 Image::ReadStandAloneSig内存泄露的bug
- [fix] 修复 SuperSetAOTHomologousImage::ReadTypeFromResolutionScope 某分支没有return的编译警告
- [opt] 使用MetadataPool池避免元数据重复分配及泄露
- [refactor][opt] 重构元数据模块，大幅优化元数据内存，补充元数据占用内存降为原来的33%，热更新程序集元数据内存降为原来的75%
- [refactor][opt] 将std::unordered_xxx容器换成il2cpp对应版本，提升性能
- [change] 重构dhao工作流

### Editor

- [new] 新增2019支持，同时2019 iOS支持源码方式打包
- [fix] 修复 DllEncryptor加密后TypeDef token和MethodDef token id扰乱的问题
- [fix] 修复计算类型implements的接口的virutal method时未正确设置implType导致explicte override计算出错的bug
- [fix] 修复IsDHEType的参数为多维数组时抛出异常的bug
- [change] 重构dhao工作流，移除`HybridCLR/CreateAOTSnapshot`、`HybridCLR/Generate/DHEAssemblyList`和`HybridCLR/Generate/DHEAssemblyOptionDatas`菜单

## 4.5.7

发布日期 2023.11.24.

### Runtime

- [fix] 修复bgt之类指令未取双重取反进行判断，导致当浮点数与Nan比较时由于不满足对称性执行了错误的分支的bug
- [opt] Il2CppGenericInst分配统一使用MetadataCache::GetGenericInst分配唯一池对象，优化内存分配
- [opt] 由于Interpreter部分Il2CppGenericInst统一使用MetadataCache::GetGenericInst，比较 Il2CppGenericContext时直接比较 class_inst和method_inst指针
- [opt] 优化SuperSet补充元数据占用内存，以mscorlib为例，InitRuntimeMetadatas占用内存由4395k降到576k

### Editor

- [fix] 修复裁剪aot dll中出现netstandard时，生成桥接函数异常的bug
- [fix] 修复当出现非常规字段名时生成的桥接函数代码文件有编译错误的bug
- [change] 删除不必要的Datas~/Templates目录，直接以原始文件为模板
- [refactor] 重构 AssemblyCache和 AssemblyReferenceDeepCollector，消除冗余代码

## 4.5.6

### Runtime

- [fix] 由于il2cpp自身原因，打印native堆栈时有时候会错误地映射到其他函数，当被映射函数正好是dhe函数，并且dhe程序集还未加载，便会导致崩溃。通过添加检查解决此问题
- [fix] 修复Class::FromGenericParameter错误地设置了thread_static_fields_size=-1，导致为其分配ThreadStatic内存的严重bug
- [fix] 修复struct计算actualSize和nativeSize时未使用StructLayout中指定的size的严重bug
- [fix] 修复Il2CppGenericContextCompare比较时仅仅对比inst指针的bug，造成热更新模块大量泛型函数重复
- [fix] 修复 initonly static变量未比较字段名，导致错误地将不同byte[]常量判定为等价的bug
- [fix] 修复 InterpreterImage::GetFieldOrParameterDefalutValueByRawIndex返回的data指针没有8字节对齐，导致IsRuntimeMetadataInitialized断言失败。同时当data指针为奇数时，还会导致多次初始化一个metadata指针的严重bug
- [remove] 删除PeepholeOptimization.cpp中不必要的断言

### Editor

- [new] 检查当前安装的libil2cpp版本是否与package版本匹配，避免升级package后未重新install的问题
- [new] Generate支持 netstandard
- [fix] 修复 ReversePInvokeWrap生成不必要地解析referenced dll，导致如果有aot dll引用了netstandard会出现解析错误的bug
- [fix] 修复BashUtil.RemoveDir在偶然情况下出现删除目录失败的问题。新增多次重试
- [fix] 修复桥接函数计算时未归结函数参数类型，导致出现多个同名签名的bug

## v4.5.5

发布日期 2023.10.13.

- [merge] 合并main分支的改动及修复。详见hybridclr_unity中RELEASELOG.MD。

## v4.5.4

- [merge] 合并main分支的改动和修复。详见hybridclr_unity中RELEASELOG.MD。

## v4.5.3

发布日期 2023.09.28。

- [fix][严重] 修复2022版本ExplicitLayout未设置layout.alignment，导致计算出size==0的bug
- [fix] 修复计算interface成员函数slot时未考虑到static之类函数的bug
- [fix] 修复Transform中未析构pendingFlows造成内存泄露的bug
- [fix] ldobj当T为byte之类size<4的类型时，未将数据展开为int的bug
- [fix] 修复FullGenericSharingReflectionInvokeInterpreter在调整了传参方式后的bug
- [fix] 合并主线修复的多个bug
- [fix][editor]修复错误地缓存了Field导致计算Field错误的bug
- [fix][Editor] 修复MetaUtil.ToShareTypeSig将Ptr和ByRef计算成IntPtr的bug，正确应该是UIntPtr
- [new] 重构桥接函数，彻底支持全平台
- [opt] TemporaryMemoryArena默认内存块大小由1M调整8K

## v4.5.2

发布日期 2023.09.12。

- [fix] 修复未将DHE类型映射为原始类型导致MetadataCache::GetUnresovledCallStubs查找失败的bug

## v4.5.1

发布日期 2023.09.08。从此版本起，使用版本号而不是日期标注版本。

- [new] 支持基础代码加固，打乱所有字节码值
- [fix][严重] 修复计算interpreter部分枚举类型签名的bug
- [fix] 修复Unity 2020 static_assert在vs 2019上的编译错误
- [fix] 修复Native2Managed分配的arguments栈空间未释放的bug
- [fix] 修复POF_LoadLdcBinOp和POF_LoadLdcBinOpStore优化的bug
- [new][editor] 新增 `HybridCLR/Generate/DHEAssemblyOptionDatas_NoChange` 菜单命令
- [fix][editor] 修复GetBuildPlayerOptions在某些未初始化环境抛出location数据invalid的bug
- [fix][editor] 修复MethodCompareCache.GetOrAddFieldIndexCache未将计算结果缓存的bug
- [fix][editor] 修复AssemblyOptionDataGenerator.GenerateNotAnyChangeData未填充dhaoOption的DllMD5字段的bug
- [fix][editor] 修复导出xcode工程时生成lump文件的bug
- [remove][editor] 移除无用的LZ4.dll文件

## 2023.09.01

- [fix][严重] 修复增量式GC的若干bug
- [fix] 修复RuntimeApi.Cpp中将Transform拼写成Tranform，导致iOS上打包失败的错误
- [fix][严重] 修复CalcClassNotStaticFields计算泛型类型的泛型父类时未inflate的bug
- [opt] 优化内存复制，将一些不可能重叠的复制操作由memmove改为memcpy
