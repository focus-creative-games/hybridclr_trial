

rem 请修改为你所用Unity的il2cpp目录
rem 一般像这样 C:\Program Files\Unity\Hub\Editor\2020.3.33f1c2\Editor\Data\il2cpp

set IL2CPP_PATH=C:\Program Files\Unity\Hub\Editor\2020.3.33f1c2\Editor\Data\il2cpp

if not exist "%IL2CPP_PATH%" (
    echo "你未指定正确的il2cpp路径"
    goto EXIT
)

set LOCAL_IL2CPP_DATA=LocalIl2CppData

if not exist %LOCAL_IL2CPP_DATA% (
    mkdir %LOCAL_IL2CPP_DATA%
)


rem Unity 打包时允许使用环境变量UNITY_IL2CPP_PATH自定义%IL2CPP_PATH%的位置
rem 但同时又要求同级目录包含MonoBleedingEdge，因此需要拷贝这两个目录

rem 拷贝 MonoBleedingEdge 目录
set MBE=%LOCAL_IL2CPP_DATA%\MonoBleedingEdge
if not exist %MBE% (
    xcopy /q /i /e "%IL2CPP_PATH%\..\MonoBleedingEdge" %MBE%
)


rem 拷贝il2cpp目录
set IL2CPP=%LOCAL_IL2CPP_DATA%\il2cpp
if not exist %IL2CPP% (
    xcopy /q /i /e "%IL2CPP_PATH%" %IL2CPP%
)

rem 接下来替换 il2cpp目录下的libil2cpp为 huatuo修改后的版本
rem 需要使用 {https://gitee.com/juvenior/il2cpp_huatuo}/libil2cpp 替换 il2cpp/libil2cpp目录
rem 需要使用 {https://gitee.com/focus-creative-games/huatuo}/huatuo 添加到 il2cpp/libil2cpp目录下，即il2cpp/libil2cpp/huatuo

set HUATUO_REPO=huatuo_repo

if not exist %HUATUO_REPO% (
    echo 未安装huatuo https://gitee.com/focus-creative-games/huatuo,请运行 init_huatuo_repos.bat or init_huatuo_repos.sh
    goto EXIT 
)

set IL2CPP_HUATUO_REPO=il2cpp_huatuo_repo
if not exist %IL2CPP_HUATUO_REPO% (
    echo 未安装il2cpp_huatuo https://gitee.com/juvenior/il2cpp_huatuo ,请运行 init_huatuo_repos.bat or init_huatuo_repos.sh
    goto EXIT 
)

set LIBIL2CPP_PATH=%LOCAL_IL2CPP_DATA%\il2cpp\libil2cpp
rd /s /q %LIBIL2CPP_PATH%

xcopy /q /i /e %IL2CPP_HUATUO_REPO%\libil2cpp %LIBIL2CPP_PATH%
xcopy /q /i /e %HUATUO_REPO%\huatuo %LIBIL2CPP_PATH%\huatuo

rem 务必清除缓存，不然build仍然使用旧版本。
rem 只影响直接build的情况，不影响导出工程的情形。
echo 清除 Library\Il2cppBuildCache 缓存目录
rd /s /q ..\Library\Il2cppBuildCache

echo 初始化成功

:EXIT

PAUSE