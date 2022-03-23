# huatuo 体验项目

一个示例热更新项目。

想了解更多，请加 QQ群: 651188171（huatuo c#热更新）。

你可以使用发布的包来体验huatuo热更新功能。

**示例项目使用 Unity 2020.3.7 版本**，需要精确匹配。

## 目录介绍

- Debug 发布包目录
- Assets Unity项目目录
  - Main AOT主包模块
  - Hotfix 热更新模块

## 使用介绍

huatuo为c++实现，只有打包后才可使用。日常开发在编辑器下，无需打包。

如何打包出一个可热更新的包，请先参阅 [快速上手](https://github.com/focus-creative-games/huatuo/blob/main/docs/start_up.md)。

Debug目录已经包含一个build好的win 64版本。

Debug目录的版本非Release，请不要用它来做性能测试。

### 工作原理

进入场景后，Main场景中的LoadDll会自动加载 StreamingAssets目录下的 HotFix.dll，并且运行 App::Main函数。

### 运行测试

如果你使用我们已经build好的，请运行Debug/huatuo即可。你会看到屏幕上打印 "hello,huatuo" 。否则运行你们自己打包出的程序。

### 体验热更新

- 打开 huatuo.sln
- 在HotFix项目的App::Main中修改代码，比如改成打印 "hello,world"。
- 切到unity，让它编译HotFix。然后运行copydll.bat，将编译好的dll复制到发布目录。**注意**要退出测试程序，不然dll被占用，无法复制。
- 再将运行，屏幕上会打印"hello,world"。

剩下的体验之旅，比如各种c#特性，自己体验吧。
