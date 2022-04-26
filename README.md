# huatuo 体验项目

一个示例热更新项目。

想了解更多，请加 QQ群: 651188171（huatuo c#热更新）。

你可以使用发布的包来体验huatuo热更新功能。

**示例项目使用 Unity 2020.3.33f1c1(f1版本也可以) 版本**，需要精确匹配。

## 目录介绍

- Assets Unity项目目录
  - Main AOT主包模块
  - Hotfix 热更新模块

## 使用介绍

huatuo为c++实现，只有打包后才可使用。日常开发在编辑器下，无需打包。

如何打包出一个可热更新的包，请先参阅 [快速上手](https://github.com/focus-creative-games/huatuo/blob/main/docs/start_up.md)。

### 工作原理

进入场景后，Main场景中的LoadDll会自动加载 StreamingAssets目录下的 HotFix.dll，并且运行 App::Main函数。

### 体验热更新

以Win64为例，其他平台同理。

- 打包主工程
  - 点击菜单 Huatuo/BuildBundles/Win64，生成Win64目标的AssetBundle，生成的AssetBundle文件会自动复制到StreamingAssets目录下
  - Build打包 Win64平台的目录
  - 运行，会看到打出 hello, huatuo.prefab
- 更新ab包
  - 修复HotFix项目的PrintHello代码，比如改成打印 "hello,world"。
  - 运行菜单 Huatuo/BuildBundles/Win64，重新生成ab
  - 将StreamingAssets下的ab包同步到打包主工程时Build目标的StreamingAsset目录，在{BuildDir}\build\bin\huatuo_Data\StreamingAssets
- 再将运行，屏幕上会打印"hello,world"。

剩下的体验之旅，比如各种c#特性，自己体验吧。
