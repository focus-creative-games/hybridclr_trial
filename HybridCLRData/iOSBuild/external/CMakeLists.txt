# file: external/CMakeList.txt
cmake_minimum_required(VERSION 3.14)

set(IL2CPP_SOURCE_DIR ${HUATUO_IL2CPP_SOURCE_DIR})
set(SDK_VERSION $ENV{IPHONESIMULATOR_VERSION})

message(STATUS "external project, external source dir: " ${IL2CPP_SOURCE_DIR})


add_definitions(-DIL2CPP_TARGET_IOS=1)
add_definitions(-DBASELIB_DYNAMICLIBRARY=1)
add_definitions(-DBASELIB_INLINE_NAMESPACE=il2cpp_baselib)
add_definitions(-DAARCH64)
add_definitions(-D__aarch64__)
#add_definitions(-D__arm64__)
add_definitions(-DIL2CPP_SUPPORT_THREADS)
add_definitions(-DIL2CPP_THREADS_PTHREAD)
#add_definitions(-DIL2CPP_TARGET_DARWIN)
add_definitions(-DIL2CPP_PLATFORM_SUPPORTS_TIMEZONEINFO=0)

#add_definitions(-MMD)
#add_definitions(-MT dependencies)

#add_compile_options($<$<COMPILE_LANGUAGE:CXX>:-MMD>)
#add_compile_options($<$<COMPILE_LANGUAGE:CXX>:-MT dependencies>)

include_directories(${IL2CPP_SOURCE_DIR}/external/)
include_directories(${IL2CPP_SOURCE_DIR}/external/baselib/Include/)
include_directories(${IL2CPP_SOURCE_DIR}/external/bdwgc/include/)
include_directories(${IL2CPP_SOURCE_DIR}/external/baselib/Platforms/OSX/Include/)

aux_source_directory(${IL2CPP_SOURCE_DIR}/external/zlib/ ZLIB_C_SOURCE_LIST)
add_library(external STATIC ${ZLIB_C_SOURCE_LIST})




