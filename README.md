# HybridCLR 体验项目

一个示例热更新项目。

想了解更多，请加 QQ群: 

- HybridCLR c#热更新 开发交流群：651188171
- HybridCLR 新手群：428404198

你可以使用发布的包来体验HybridCLR热更新功能。

**示例项目使用 Unity 2020.3.33(任意后缀子版本如f1、f1c1、f1c2都可以) 版本**，2020.3.x系列都可以，但为了避免新手无谓的出错，尽量使用对应版本。

## 目录介绍

- Assets Unity项目目录
  - Main AOT主包模块
  - Hotfix 热更新模块
- HybridCLRData 包含HybridCLR的il2cpp本地安装目录

## 使用介绍

HybridCLR为c++实现，只有打包后才可使用。日常开发在编辑器下，无需打包。

如何打包出一个可热更新的包，请先参阅 [快速开始](https://focus-creative-games.github.io/hybridclr/start_up/)。

### 运行流程

本示例演示了如下几部分内容

- 将dll和资源打包成ab
- 多热更新dll，并且按依赖顺序加载它们
- 热更新脚本挂载到热更新资源中，并且正常运行
- 直接反射运行普通热更新函数App::Main

进入场景后，Main场景中的LoadDll会按顺序加载StreamingAssets目录下common AssetBundle里的HotFix.dll和HotFix2.dll，其中HotFix2.dll依赖HotFix.dll。
接着运行HotFix2.dll里的App::Main函数。

注意！多热更新dll不是必须的！大多数项目完全可以只有HotFix.dll这一个热更新模块。纯粹出于演示才故意设计了两个热更新模块。

### 体验热更新

#### 预备工作

根据你所使用的Unity年度版本，**还需要额外**安装2020.3.33或者2021.3.1版本，不限 f1、f1c1之类后缀。

**注意！**对于需要打iOS版本的开发者，由于HybridCLR需要裁减后的AOT dll，但Unity Editor未提供公开接口可以复制出target为iOS
时的AOT dll，故必须使用修改后的UnityEditor.CoreModule.dll覆盖Unity自带的相应文件。

具体操作为将 `HybridCLRData/ModifiedUnityAssemblies/2021.3.1/UnityEditor.CoreModule.dll` 覆盖 `{Editor安装目录}/Editor/Data/Managed/UnityEngine/UnityEditor.CoreModule`，具体覆盖目录有可能因为操作系统或者Unity版本而有不同。

这个 UnityEditor.CoreModule.dll 每个Unity小版本都不相同，我们目前暂时只提供了2021.3.1版本，如需其他版本请自己手动制作，详情请见 [修改的UnityEditor.CoreModule.dll](https://focus-creative-games.github.io/hybridclr/modify_unity_dll/)

以Win64为例，其他平台同理。

- 安装HybridCLR （安装HybridCLR的原理请看 [快速上手](https://focus-creative-games.github.io/hybridclr/start_up/)）
  - 需要已经安装 2020.3.33或2021.3.1（根据你的Unity大版本），不限 f1、f1c1之类后缀 
  - 点击菜单 `HybridCLR/Install`，弹出安装界面
  - 如果安装界面没有错误或者警告，则说明il2cpp路径设置正常，否则需要你手动选择正确的il2cpp目录
  - 点击 install 按钮完成安装
- 打包主工程
  - **请确保你已经掌握了常规的il2cpp为backend的打包过程**
  - **请确保你已经在你电脑上对于一个未使用HybrildCLR的项目成功打包出il2cpp为backend的相应包**，也就是打包环境是正常的！
  - 菜单 HybridCLR/Build/Win64 ，运行完成后，会在Release_Win64目录下生成程序
  - 运行Release_Win64/HybridCLRTrial.exe，会看到打出 hello, HybridCLR.prefab
- 更新ab包
  - 修改HotFix项目的PrintHello代码，比如改成打印 "hello,world"。
  - 运行菜单 HybridCLR/BuildBundles/Win64，重新生成ab
  - 将StreamingAssets下的ab包复制到Release_Win64\HybridCLRTrial_Data\StreamingAssets。
  - 再将运行，屏幕上会打印"hello,world"。

剩下的体验之旅，比如各种c#特性，自己体验吧。
