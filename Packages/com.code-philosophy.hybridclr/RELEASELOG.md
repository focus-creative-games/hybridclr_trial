# 发布日志

# 改动日志

## v4.4.11

### Runtime

- [new] 新增2019支持* 
- [fix] 修复 Image::ReadStandAloneSig内存泄露的bug
- [fix] 修复 SuperSetAOTHomologousImage::ReadTypeFromResolutionScope 某分支没有return的编译警告
- [fix] 修复优化 box; brtrue|brfalse序列时，当类型为class或nullable类型时，无条件转换为无条件branch语句的bug
- [fix] 修复 ClassFieldLayoutCalculator未释放 _classMap的每个key-value对中value对象，造成内存泄露的bug
- [fix] 修复计算 ExplicitLayout的struct的native_size的bug
- [fix] 修复当出现签名完全相同的虚函数与虚泛型函数时，计算override未考虑泛型签名，错误地返回了不匹配的函数，导致虚表错误的bug
- [fix] 修复 SuperSetAOTHomologousImage.h 在 android平台的编译错误
- [fix] 修复ConvertInvokeArgs有可能传递了非对齐args，导致CopyStackObject在armv7这种要求内存对齐的平台发生崩溃的bug
- [fix] 修复Image::ReadGenericClas中Il2CppGenericInst内存泄露的bug
- [fix] 修复 Image::ReadStandAloneSig内存泄露的bug
- [fix] 修复 SuperSetAOTHomologousImage::ReadTypeFromResolutionScope 某分支没有return的编译警告
- [opt] 使用MetadataPool池避免元数据重复分配及泄露
- [refactor][opt] 重构元数据模块，大幅优化元数据内存，补充元数据占用内存降为原来的33%，热更新程序集元数据内存降为原来的75%
- [refactor][opt] 将std::unordered_xxx容器换成il2cpp对应版本，提升性能
- [fix] 修复通过StructLayout指定size时，计算ClassFieldLayout的严重bug
- [fix] 修复bgt之类指令未取双重取反进行判断，导致当浮点数与Nan比较时由于不满足对称性执行了错误的分支的bug
- [fix] 修复Class::FromGenericParameter错误地设置了thread_static_fields_size=-1，导致为其分配ThreadStatic内存的严重bug
- [opt] Il2CppGenericInst分配统一使用MetadataCache::GetGenericInst分配唯一池对象，优化内存分配
- [opt] 由于Interpreter部分Il2CppGenericInst统一使用MetadataCache::GetGenericInst，比较 Il2CppGenericContext时直接比较 class_inst和method_inst指针


### Editor

- [new] 新增2019支持，同时2019 iOS支持源码方式打包
- [fix] 修复 DllEncryptor加密后TypeDef token和MethodDef token id扰乱的问题
- [fix] 修复裁剪aot dll中出现netstandard时，生成桥接函数异常的bug
- [fix] 修复当出现非常规字段名时生成的桥接函数代码文件有编译错误的bug
- [change] 删除不必要的Datas~/Templates目录，直接以原始文件为模板
- [refactor] 重构 AssemblyCache和 AssemblyReferenceDeepCollector，消除冗余代码

## v4.4.10

### Runtime

- [fix] 修复 MetadataGarbage未收集MethodBody的LocalVars，导致被回收的bug
- [fix] 修复bgt之类指令未取双重取反进行判断，导致当浮点数与Nan比较时由于不满足对称性执行了错误的分支的bug
- [fix] 修复 ClassFieldLayoutCalculator未释放 _classMap的每个key-value对中value对象的bug
- [opt] 移除MetadataCache::GetGenericInst中不必要的加锁


## v4.4.9

发布日期 2023.11.23。

### Runtime

- [fix] 修复未walk Reflection::s_TypeMap，导致key被回收的bug
- [fix] 修复有些Il2CppType的type为IL2CPP_TYPE_CLASS,但data.typeHandle == nullptr时，walk出现断言失败的bug

## v4.4.8

发布日期 2023.11.15。

### Runtime

- [fix] 修复泛型元数据泄露的bug，100%回收内存

## v4.4.7

发布日期 2023.11.13。

### Runtime

- [fix] 修复未扫描泛型Il2CppClass元数据，导致部分元数据被意外卸载的bug

## v4.4.6

发布日期 2023.11.10。

### Runtime

- [fix] 修复卸载程序集时，未标记剩余热更新程序集元数据，导致这些元数据被卸载的bug

## v4.4.5

### Runtime

- [fix] 修复MetadataPool::Free释放Il2CppGenericInst的bug
- [fix] 修复gc相关bug

## v4.4.4

### Runtime

- [fix] 修复Class::FromGenericParameter错误地设置了thread_static_fields_size=-1，导致为其分配ThreadStatic内存的严重bug
- [fix] 修复struct计算actualSize和nativeSize时未使用StructLayout中指定的size的严重bug
- [fix] 修复Il2CppGenericContextCompare比较时仅仅对比inst指针的bug，造成热更新模块大量泛型函数重复
- [fix] 修复2022版本分配MethodInfo没有通过MetadataPool，导致大量泄露的bug
- [fix] 修复了Image::ReadMethodRefInfoFromToken中意外地修改了从MetadataPool获取的Il2CppGenericInst的严重bug
- [change] 释放 ThreadStatic数据
- [change] RuntimeApi::UnloadAssembly改为InternalCall
- [remove] 删除PeepholeOptimization.cpp中不必要的断言

### Editor

- [new] 检查当前安装的libil2cpp版本是否与package版本匹配，避免升级package后未重新install的问题
- [new] Generate支持 netstandard
- [fix] 修复 ReversePInvokeWrap生成不必要地解析referenced dll，导致如果有aot dll引用了netstandard会出现解析错误的bug
- [fix] 修复BashUtil.RemoveDir在偶然情况下出现删除目录失败的问题。新增多次重试
- [fix] 修复桥接函数计算时未归结函数参数类型，导致出现多个同名签名的bug

## v4.4.3

- 支持2021.3.31f1及2022.3.11f1版本
- 继续优化和修复热重载的一些问题

## v4.4.2

发布日期 2023.09.14.

- [fix] 修复 InterpMethodInfo析构内存泄露的bug
- [fix] 修复Transform中未析构pendingFlows造成内存泄露的bug
- [opt] TemporaryMemoryArena默认内存块大小由1M调整8K
- [change] 复用泛型Il2CppClass
- [change] 将std::vector换成il2cpp::utils::dynamic_array，将std::unordered_map换成Il2CppHashMap

## v4.4.1

发布日期 2023.09.11。从此版本起，使用版本号而不是日期标注版本。

- [new] 支持Il2CppClass复用，明显降低卸载程序集时不可回收的内存量
- [new] 支持基础代码加固，打乱所有字节码值
- [fix][严重] 修复计算interpreter部分枚举类型签名的bug
- [fix] 修复Unity 2020 static_assert在vs 2019上的编译错误
- [fix] 修复Native2Managed分配的arguments栈空间未释放的bug
- [fix] 修复POF_LoadLdcBinOp和POF_LoadLdcBinOpStore优化的bug
- [fix][editor] 修复GetBuildPlayerOptions在某些未初始化环境抛出location数据invalid的bug
- [fix][editor] 修复导出xcode工程时生成lump文件的bug
- [remove][editor] 移除无用的LZ4.dll文件

## 2023.09.01

- [new] 合并指令优化相关代码
- [merge] 合并main分支v4.0.0之前的改动
- [fix][严重] 修复增量式GC的若干bug
- [fix] 修复RuntimeApi.Cpp中将Transform拼写成Tranform，导致iOS上打包失败的错误
- [fix][严重] 修复CalcClassNotStaticFields计算泛型类型的泛型父类时未inflate的bug
- [opt] 优化内存复制，将一些不可能重叠的复制操作由memmove改为memcpy