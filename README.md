# 说明

本工程包含了旗舰版的试用版本(删减了代码)的com.code-philsophy.hybridclr包及相关工具。

由于是试用版本，并不包含hybridclr和il2cpp_plus仓库的cpp代码。无法像社区版本那样直接打包。

试用版本只支持构建iOS包。

## 支持的版本

只支持2020.3.x、2021.3.x、2022.3.x系列LTS版本

## 接入流程

- 将`Packages/com.code-philsophy.hybridclr` 复制到`{proj}/Packages`目录下
- 按照[快速上手](https://hybridclr.doc.code-philosophy.com/docs/business/ultimate/quickstart)文档完成接入流程。注意，在安装环节由于没有hybridclr和il2cpp_plus代码，直接点击Install按钮安装社区版本。
根据你们项目所用的版本，向我们索取相对应的Unity.Il2CPP.dll文件，替换安装文档中描述的本地Unity.IL2CPP.dll文件。Editor部分只需完成`BuildTools/BackupAOTDll`和`BuildTools/CreateManifestAtBackupDir`。

## 打包

- 运行`Generate/All`
- 导出xcode工程
- 按照快速上手文档说明，备份打包时生成的aot dll以及生成manifest清单文件。这些文件可以随包携带，也可以热更新下载
- 将目录`{proj}\HybridCLRData\LocalIl2CppData-WindowsEditor\il2cpp\libil2cpp\hybridclr\generated`提供给我们，用于生成旗舰版本libil2cpp.a文件
- 使用我们生成的libil2cpp.a替换xcode工程中的libil2cpp.a文件
- 构建 iOS app


## 首包测试

- 运行`{proj}/Packages/com.code-philsophy.hybridclr/Tools~/DhaoGenerator --aot-backup-dir {aot-backup-dir} --dhe-assembly-names XXX,YYY,ZZZ` 生成首包所需要的dhao文件。
`{aot-backup-dir}`参数为打包时备份的aot dll目录，`dhe-assembly-anmes`参数则是dhe程序集列表，以逗号分割，程序集名不含'.dll'
- 将打包时备份的dhe dll及上一步生成的dhao文件加入资源管理系统。自由选择随包携带或者动态下载
- 运行


## 热更新测试

- 运行 `HybridCLR/CompileDlls/activeBuildTarget`生成最新的热更新dll
- 运行`{proj}/Packages/com.code-philsophy.hybridclr/Tools~/DhaoGenerator --aot-backup-dir {aot-backup-dir} --hotupdate-dir {hotudpate-dir} --dhe-assembly-names XXX,YYY,ZZZ` 生成热更新所需要的dhao文件。
其中`hotupdate-dir`为热更新dll目录
- 将最新的热更新dhe dll及上一步生成的dhao文件加入资源管理系统
- 运行

