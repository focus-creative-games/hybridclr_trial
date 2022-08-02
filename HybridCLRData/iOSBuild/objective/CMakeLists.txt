# file: CMakeList.txt
cmake_minimum_required(VERSION 3.14)

project(il2cpp)

set(IL2CPP_SOURCE_DIR ${HUATUO_IL2CPP_SOURCE_DIR})

message(STATUS "objective project, binary dir: " ${PROJECT_BINARY_DIR})
message(STATUS "objective project, il2cpp source dir: " ${PROJECT_SOURCE_DIR})


add_definitions(-DIL2CPP_TARGET_IOS=1)
add_definitions(-DBASELIB_DYNAMICLIBRARY=1)
add_definitions(-DBASELIB_INLINE_NAMESPACE=il2cpp_baselib)
add_definitions(-DAARCH64)
add_definitions(-D__aarch64__)
add_definitions(-DIL2CPP_SUPPORT_THREADS)
add_definitions(-DIL2CPP_THREADS_PTHREAD)
#add_definitions(-DIL2CPP_TARGET_DARWIN)
add_definitions(-DIL2CPP_PLATFORM_SUPPORTS_TIMEZONEINFO)

add_definitions(-MMD)
#add_definitions(-MT dependencies)


include_directories(${IL2CPP_SOURCE_DIR}/libil2cpp)
include_directories(${IL2CPP_SOURCE_DIR}/external)
include_directories(${IL2CPP_SOURCE_DIR}/external/baselib/Include/)
include_directories(${IL2CPP_SOURCE_DIR}/external/bdwgc/include/)
include_directories(${IL2CPP_SOURCE_DIR}/external/baselib/Platforms/OSX/Include/)


get_filename_component(PARENT_PATH ${PROJECT_BINARY_DIR} PATH)
aux_source_directory(${PARENT_PATH}/lump_mm LUMP_SOURCE_LIST)

#find_library(zlib ${PROJECT_SOURCE_DIR}/external)
add_library(objective STATIC ${LUMP_SOURCE_LIST})

