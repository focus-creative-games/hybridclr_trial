#!/bin/bash

export HUATUO_IL2CPP_SOURCE_DIR=$(pushd ../LocalIl2CppData/il2cpp > /dev/null && pwd && popd > /dev/null)

rm -rf build

mkdir build
cd build

# 设置Bitcode -DENABLE_BIECODE_INT=1
# 模拟器平台 -DPLATFORM=SIMULATOR64
# 设置部署版本 -DDEPLOYMENT_TARGET=10.10
# 不设置CMAKE_TOOLCHAIN_FILE 导出的mac版本 不过CMakeLists.txt没有兼容mac版本

cmake .. -G Xcode -DCMAKE_TOOLCHAIN_FILE=../CMake/ios.toolchain.cmake -DPLATFORM=OS64 -DHUATUO_IL2CPP_SOURCE_DIR=${HUATUO_IL2CPP_SOURCE_DIR} -DOUTPUT_BIN_DIR=lib
cmake --build . --config Release

if [ -f "lib/libil2cpp.a" ]
then
	echo 'build succ'
else
    echo "build fail"
    exit 1
fi
