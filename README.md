# HybridCLR 体验项目

一个示例热更新项目。

想了解更多，请加 QQ群: 

- HybridCLR c#热更新 开发交流群：651188171
- HybridCLR 新手群：428404198

你可以使用发布的包来体验HybridCLR热更新功能。

**示例项目使用 Unity 2020.3.33(任意后缀子版本如f1、f1c1、f1c2都可以) 版本**，2019.4.x、2020.3.x、2021.3.x系列都可以，但为了避免新手无谓的出错，尽量使用2020.3.33版本来体验。

## 目录介绍

- Assets Unity项目目录
  - Main AOT主包模块，对应常规项目的主项目，资源更新模块
  - 剩下代码在默认的全局Assembly-Csharp.dll中，均为热更新脚本
- HybridCLRData 包含HybridCLR的il2cpp本地安装目录
- Packages/com.focus-creative-games/hybridclr_unity 为HybridCLR for Unity包。暂时先作为local package，成熟后做成独立package。

## 使用介绍

HybridCLR为c++实现，只有打包后才可使用。日常开发在编辑器下，无需打包。

如何打包出一个可热更新的包，请先参阅 [快速开始](https://focus-creative-games.github.io/hybridclr/start_up/)。


## 运行流程

本示例演示了如下几部分内容

- 将dll和资源打包成ab
- 热更新脚本挂载到热更新资源中，并且正常运行
- 直接反射运行普通热更新函数App::Main

进入场景后，Main场景中的LoadDll会按顺序加载StreamingAssets目录下common AssetBundle里的Assembly-Csharp.dll。接着运行App::Main函数。

## 体验热更新

### 通用预备工作

**===> 安装必须的Unity版本**

根据你所使用的Unity年度版本，**还需要额外**安装2019.4.40、2020.3.33或者2021.3.1版本（必须包含il2cpp模块），不限 f1、f1c1之类后缀。

**再次提醒** 当前Unity版本必须安装了 il2cpp 组件。如果未安装，请自行在UnityHub中安装。新手自行google或百度。

**===> 安装 visual studio**

要求必须安装 `使用c++的游戏开发` 这个组件

**===> 安装git**

### Unity版本相关特殊操作

**===> Unity 2021**

对于使用Unity 2021版本（2019、2020不需要）打包iOS平台的开发者，由于HybridCLR需要裁减后的AOT dll，但Unity Editor未提供公开接口可以复制出target为iOS时的AOT dll，故必须使用修改后的UnityEditor.CoreModule.dll覆盖Unity自带的相应文件。

具体操作为将 `HybridCLRData/ModifiedUnityAssemblies/2021.3.1/UnityEditor.CoreModule-{Win,Mac}.dll` 覆盖 `{Editor安装目录}/Editor/Data/Managed/UnityEngine/UnityEditor.CoreModule`，具体覆盖目录有可能因为操作系统或者Unity版本而有不同。

**由于权限问题，该操作无法自动完成，需要你手动执行。**

这个 UnityEditor.CoreModule.dll 每个Unity小版本都不相同，我们目前暂时只提供了2021.3.1版本，如需其他版本请自己手动制作，详情请见 [修改UnityEditor.CoreModule.dll](https://focus-creative-games.github.io/hybridclr/modify_unity_dll/)

**===> Unity 2019**

为了支持2019，需要修改il2cpp生成的源码，因此我们修改了2019版本的il2cpp.exe工具。故Installer的安装过程多了一个额外步骤：将 `HybridCLRData/ModifiedUnityAssemblies/2019.4.40/Unity.IL2CPP.dll` 复制到 `HybridCLRData/LocalIl2CppData/il2cpp/build/deploy/net471/Unity.IL2CPP.dll`

**注意，该操作自动完成，不需要手动操作。**

### 配置

目前需要几个配置文件

**===> HybridCLRGlobalSettings.asset**

HybridCLR全局配置，单例。 trial项目已经创建。新项目请在 Unity Editor的 Project 窗口右键 `HybridCLR/GlobalSettings` 创建。

关键参数介绍：

- Enable。 是否开启HybridCLR。
- HotFixAssmblyDefinitions。 以Assembly Definition形式存在的热更新模块。
- Hotfix Dlls。 以dll形式存在的热更新模块

**===> HotUpdateAssemblyManifest.asset**

包含了需要补充元数据的AOT assembly列表。这个字段原来在 HybridCLRGlobalSettings 中。

### 安装及打包及热更新测试

以Win64为例，其他平台同理。

- 安装HybridCLR (安装HybridCLR的原理请看 [快速上手](https://focus-creative-games.github.io/hybridclr/start_up/) )
    - 点击菜单 `HybridCLR/Installer...`，弹出安装界面。
    - 如果安装界面没有错误或者警告，则说明il2cpp路径设置正常，否则需要你手动选择正确的il2cpp目录
    - 点击 install 按钮完成安装
- 打包主工程
  
  **请确保你已经掌握了常规的il2cpp为backend的打包过程**

  **请确保你已经在你电脑上对于一个未使用HybrildCLR的项目成功打包出il2cpp为backend的相应包**，也就是打包环境是正常的！

打包前需要先在 Player Settings里作如下设置：
- script backend 必须选择 il2cpp
- Api Compatibility Level 选择 .NET 4.x（unity 2021 及之后版本这里显示为 .NET framework）
- 关闭 增量式gc 选项 (incremental gc)

打包：
- 菜单 HybridCLR/Build/Win64 ，运行完成后，会在Release_Win64目录下生成程序
- 运行Release_Win64/HybridCLRTrial.exe，会看到打出 hello, HybridCLR.prefab

更新ab包：
  - 修改HotFix项目的PrintHello代码，比如改成打印 "hello,world"。
  - 运行菜单 HybridCLR/BuildBundles/Win64，重新生成ab
  - 将StreamingAssets下的ab包复制到Release_Win64\HybridCLRTrial_Data\StreamingAssets。
  - 再将运行，屏幕上会打印"hello,world"。

### HybridCLR相关Editor菜单介绍

- `Installer...` 打开 安装器
- `GenerateLinkXml` 自动生成热更新代码所需的link.xml。
- `GenerateMethodBridge` 生成桥接函数
- `GenerateReversePInvokeWrapper` 生成 MonoPInvokeCallbackAttribute的预留桩函数
- `CompileDll` 编译热更新dll
- `BuildBundles` 构建用于热更资源和代码的ab包
- `Build` 一键打包相关快捷命令

### 其他

剩下的体验之旅，比如各种c#特性，自己体验吧。

