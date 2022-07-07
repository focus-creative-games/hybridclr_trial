
rem git clone https://github.com/focus-creative-games/hybridclr
git clone https://gitee.com/focus-creative-games/hybridclr huatuo_repo

rem git clone https://github.com/focus-creative-games/il2cpp_huatuo
git clone https://gitee.com/focus-creative-games/il2cpp_plus il2cpp_huatuo_repo


rem set default branch
set DEFAULT_VERSION=2020.3.33
cd il2cpp_huatuo_repo

git switch %DEFAULT_VERSION%

echo succ

pause

