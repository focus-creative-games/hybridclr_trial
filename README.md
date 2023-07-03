# HybridCLR 体验项目

一个示例热更新项目。本示例演示了热更新脚本挂载到热更新资源中，并且正常运行。

想了解更多，请加 QQ群: 

- 官方1群：651188171（满）
- 新手1群：428404198（满）
- 新手2群：**680274677（推荐）**
- QQ悬赏互助群：762953653。悬赏提问，快速解决问题。
- 商业合作邮箱: business@code-philosophy.com

## 文档

- [文档](https://hybridclr.doc.code-philosophy.com/)
- [快速上手](https://hybridclr.doc.code-philosophy.com/docs/beginner/quickstart)

## 目录介绍

- Assets Unity项目目录
  - Main AOT主包模块，对应常规项目的主项目，资源更新模块
  - HotUpdate 热更新代码模块

### HybridCLR相关Editor菜单介绍

- `HybridCLR/Settings` 打开HybridCLR相关设置
- `HybridCLR/Build` 一键打包相关快捷命令
- 其他菜单介绍请参见 [hybridclr package]([https://focus-creative-games.github.io/hybridclr/hybridclr_unity/](https://hybridclr.doc.code-philosophy.com/#/basic/com.focus-creative-games.hybridclr_unity))


## 体验热更新

### 预备工作

- 安装适当的Unity版本，2019.4.x、2020.3.x、2021.3.x、2022.3.x系列都可以
- 打开`HybridCLR/Installer...`菜单，点击安装按钮完成安装。如有疑问，可参考 [快速上手](https://hybridclr.doc.code-philosophy.com/docs/beginner/quickstart)

### Editor中预览(可选)

**如果你不用在Editor预览，可以跳过本节内容，直接执行`打包`小节中操作。**

 在Editor中运行前必须执行以下操作，否则会出错。

- 运行菜单 `HybridCLR/Generate/All` 一键执行必要的生成操作
- 运行菜单 `HybridCLR/Build/BuildAssetsAndCopyToStreamingAssets` 复制热更新资源及dll到StreamingAssets目录

### 打包

如果是Win平台，已经提供提供了快捷的菜单命令：

- 菜单 HybridCLR/Build/Win64 ，运行完成后，会在Release_Win64目录下生成程序
- 运行Release_Win64/HybridCLRTrial.exe，会看到打出 hello, HybridCLR.prefab

注意，如果你使用最新版本的vs，有可能遇到 遇到 `xxxx\il2cpp\libil2cpp\utils\Il2CppHashMap.h(71): error C2039: 'hash_compare': is not a member of 'stdext'` 编译错误。这是.net 7发布后最新版本vs改动打破了一些向后兼容性引起。详细解决办法请查看[常见错误](https://focus-creative-games.github.io/hybridclr/common_errors/)。

如果是其他平台：
- 如果是WebGL平台，需要一些特殊操作，请先仔细看文档。由于流程比较复杂，请务必跑通其他平台后再尝试。
- 运行菜单 `HybridCLR/Generate/All` 一键执行必要的生成操作
- 运行菜单 `HybridCLR/Build/BuildAssetsAndCopyToStreamingAssets` 打包热更新资源及dll
- Build Settings里打包游戏
- 运行刚刚打包成功的游戏

### 热更新测试

- 修改`Assets/HotUpdate/HotUpdateMain.cs`的代码，比如添加打印 "hello,world"。
- 运行菜单 `HybridCLR/Build/BuildAssetsAndCopyToStreamingAssets` 重新打包热更新资源及dll
- 将`Assets/StreamingAssets`下的所有文件复制到你刚才打包的游戏的StreamingAssets目录
- 再将运行，屏幕上会打印"hello,world"。


