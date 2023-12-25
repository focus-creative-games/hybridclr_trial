# Release log

# Change log

## v4.4.6

Rlease at 2013/11/10.

### Runtime

- [fix] Fixed the bug that when uninstalling an assembly, the remaining hot update assembly metadata was not marked, causing these metadata to be uninstalled.

## v4.4.5

### Runtime

- [fix] Fix the bug of MetadataPool::Free releasing Il2CppGenericInst
- [fix] Fix gc related bugs

## v4.4.4

### Runtime

- [fix] Fixed Class::FromGenericParameter incorrectly setting thread_static_fields_size=-1, resulting in a serious bug in allocating ThreadStatic memory for it
- [fix] Fix the serious bug that the size specified in StructLayout is not used when calculating actualSize and nativeSize in struct
- [fix] Fixed the bug where Il2CppGenericContextCompare only compares inst pointers when comparing, causing a large number of generic functions to be repeated in the hot update module
- [fix] Fixed the bug that the MethodInfo allocated in the 2022 version did not pass the MetadataPool, resulting in a large number of leaks
- [fix] Fixed a serious bug in Image::ReadMethodRefInfoFromToken that accidentally modified the Il2CppGenericInst obtained from MetadataPool
- [change] Release ThreadStatic data
- [change] RuntimeApi::UnloadAssembly changed to InternalCall
- [remove] Remove unnecessary assertions in PeepholeOptimization.cpp

### Editor

- [new] Check whether the currently installed libil2cpp version matches the package version to avoid the problem of not reinstalling after upgrading the package.
- [new] Generate supports netstandard
- [fix] Fixed the bug where ReversePInvokeWrap generates unnecessary parsing of referenced dll, resulting in a parsing error if an aot dll references netstandard.
- [fix] Fixed the issue where BashUtil.RemoveDir failed to delete the directory occasionally. Added multiple retries
- [fix] Fixed the bug that function parameter types were not resolved during bridge function calculation, resulting in multiple signatures with the same name.

## v4.4.3

- Supports 2021.3.31f1 and 2022.3.11f1 versions
- Continue to optimize and fix some issues with hot reloading

## v4.4.2

Release date 2020.09.14.

- [fix] Fix the bug of memory leak in InterpMethodInfo destruction
- [fix] Fixed the bug of memory leak caused by pendingFlows not being destructed in Transform
- [opt] TemporaryMemoryArena default memory block size is adjusted from 1M to 8K
- [change] Reuse generic Il2CppClass
- [change] Replace std::vector with il2cpp::utils::dynamic_array, and replace std::unordered_map with Il2CppHashMap

## v4.4.1

Release date 2023.09.11. From this version forward, version numbers are used instead of dates.

- [new] Support Il2CppClass reuse, significantly reducing the amount of unrecyclable memory when uninstalling an assembly
- [new] Support basic code reinforcement and disrupt all bytecode values
- [fix][Severe] Fixed a bug in calculating the signature of some enumeration types in the interpreter
- [fix] Fix the compilation error of Unity 2020 static_assert on vs 2019
- [fix] Fix the bug that the arguments stack space allocated by Native2Managed is not released
- [fix] Fix the optimization bug of POF_LoadLdcBinOp and POF_LoadLdcBinOpStore
- [fix][editor] Fixed the bug of GetBuildPlayerOptions throwing invalid location data in some uninitialized environments
- [fix][editor] Fixed the bug of generating lump file when exporting xcode project
- [remove][editor] Remove useless LZ4.dll file

## 2023.09.01

- [new] Merge instruction optimization related code
- [merge] Merge changes before main branch v4.0.0
- [fix][Severe] Fixed several bugs in incremental GC
- [fix] Fixed the error of spelling Transform as Tranform in RuntimeApi.Cpp, causing packaging failure on iOS
- [fix][Severe] Fixed the bug that CalcClassNotStaticFields did not inflate when calculating the generic parent class of a generic type
- [opt] Optimize memory copy, change some copy operations that cannot overlap from memmove to memcpy