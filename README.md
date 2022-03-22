# huatuo 体验项目

huatuo将于2022.3.23(明天)开源，故此仓库不包含源码，只包含编译好的Release包，以及一个示例热更新项目。

你可以使用发布的包来体验huatuo热更新功能。

**示例项目使用 Unity 2022.3.7 版本**

## 目录介绍

- Release 发布包目录
- Assets Unity项目目录
  - Main AOT主包模块
  - Hotfix 热更新模块

## 使用介绍

### 工作原理

进入场景后，Main场景中的LoadDll会自动加载 StreamingAssets目录下的 HotFix.dll，并且运行 App::Main函数。

### 运行测试

运行Release/huatuo即可。你会看到屏幕上打印 "hello,huatuo" 。

### 体验热更新

- 打开 huatuo.sln
- 在HotFix项目的App::Main中修改代码，比如改成打印 "hello,world"。
- 切到unity，让它编译HotFix。然后运行copydll.bat，将编译好的dll复制到发布目录。**注意**要退出测试程序，不然dll被占用，无法复制。
- 再将运行，屏幕上会打印"hello,world"。

剩下的体验之旅，比如各种c#特性，自己体验吧。huatuo一定让你震惊。
