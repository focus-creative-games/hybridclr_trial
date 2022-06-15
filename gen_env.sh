#!/usr/bin/env sh
PROJECT_PATH=`pwd`

# 重复执行该脚本可以用于更新huatuo

# 想要使用自定义目录的，直接在此处cd过去即可
# cd ..

# 这里是unity Editor安装路径，需要修改为自己的安装路径
# 注意在win下 e: 盘符的路径应填写为 /e
UnityEditorPath=/e/Unity/2020.3.33f1c2/Editor/Data/
if [ ! -d "${UnityEditorPath}" ];then
	echo "Unity 安装目录不存在, 请修改脚本指定Unity安装目录"
	echo "当前使用路径：" ${UnityEditorPath}
	exit 1
fi

if [ ! -d "unity_il2cpp_with_huatuo" ];then
	mkdir -p unity_il2cpp_with_huatuo
fi

cd unity_il2cpp_with_huatuo

if [ ! -d "huatuo_git_cache" ];then
	mkdir huatuo_git_cache
	mkdir project_il2cpp
fi

cd huatuo_git_cache

echo "更新il2cpp_huatuo"
if [ ! -d "il2cpp_huatuo" ];then
	git clone https://github.com/pirunxi/il2cpp_huatuo.git
	cd il2cpp_huatuo
	git checkout 2020.3.33
	cd ..
else
	cd il2cpp_huatuo
	git pull
	cd ..
fi


echo "更新 huatuo"
if [ ! -d "huatuo" ];then
	git clone https://github.com/focus-creative-games/huatuo.git
else
	cd huatuo
	git pull
	cd ..
fi

cd ..
cd project_il2cpp

if [ ! -d "MonoBleedingEdge" ];then
	echo "拷贝 MonoBleedingEdge 目录"
	cp -r ${UnityEditorPath}/MonoBleedingEdge ./
fi
if [ ! -d "il2cpp" ];then
	echo "拷贝 il2cpp 目录"
	cp -r ${UnityEditorPath}/il2cpp ./
fi

echo "删除旧的libil2cpp目录"
rm -rf ./il2cpp/libil2cpp


echo "拷贝新的 libil2cpp 目录"
cp -r ../huatuo_git_cache/il2cpp_huatuo/libil2cpp ./il2cpp/libil2cpp
echo "拷贝新的 huatuo 目录"
cp -r ../huatuo_git_cache/huatuo/huatuo ./il2cpp/libil2cpp/huatuo

echo "删除缓存"
cd $PROJECT_PATH
cd Library
rm -rf Il2cppBuildCache
echo "环境构建完成"


