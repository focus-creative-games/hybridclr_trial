
rem clone huatuo仓库,国内推荐用 gitee
rem git clone https://github.com/focus-creative-games/huatuo
git clone https://gitee.com/focus-creative-games/huatuo huatuo_repo

rem git clone https://github.com/pirunxi/il2cpp_huatuo
git clone https://gitee.com/juvenior/il2cpp_huatuo il2cpp_huatuo_repo


rem 设置默认分支为2020.3.33，避免很多人忘了切分支
set DEFAULT_VERSION=2020.3.33
cd il2cpp_huatuo_repo

git switch %DEFAULT_VERSION%

pause

