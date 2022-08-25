# file: CMakeList.txt
cmake_minimum_required(VERSION 3.14)



set (CMAKE_CXX_STANDARD 11)
set (CMAKE_C_STANDARD 11)

set(HUATUO_IL2CPP_SOURCE_DIR "../LocalIl2CppData/il2cpp " CACHE STRING "源代码路径")
set(OUTPUT_BIN_DIR "lib" CACHE STRING "输出路径")
set(OUTPUT_BIN_DIR_IMP ${CMAKE_BINARY_DIR}/${OUTPUT_BIN_DIR})


set(TMP ${HUATUO_IL2CPP_SOURCE_DIR})
if ( NOT TMP )
    message(FATAL_ERROR "需要设置环境变量: HUATUO_IL2CPP_SOURCE_DIR")
else()
    message(STATUS "unity il2cpp 路径为: ${TMP}")
endif()


project(il2cpp)
set(LUMP_DIR ${PROJECT_BINARY_DIR})
execute_process(COMMAND sh gen_lump.sh ${LUMP_DIR} WORKING_DIRECTORY ${CMAKE_SOURCE_DIR})

set(IL2CPP_SOURCE_DIR ${HUATUO_IL2CPP_SOURCE_DIR})

message(STATUS "il2cpp project, binary dir: " ${CMAKE_BINARY_DIR})
message(STATUS "il2cpp project, build dir: " ${PROJECT_BINARY_DIR})
message(STATUS "il2cpp project, il2cpp source dir: " ${IL2CPP_SOURCE_DIR})



add_definitions(-DIL2CPP_TARGET_IOS=1)
add_definitions(-DBASELIB_DYNAMICLIBRARY=1)
add_definitions(-DBASELIB_INLINE_NAMESPACE=il2cpp_baselib)
add_definitions(-DAARCH64)
add_definitions(-D__aarch64__)
add_definitions(-DIL2CPP_SUPPORT_THREADS)
add_definitions(-DIL2CPP_THREADS_PTHREAD)
add_definitions(-DIL2CPP_PLATFORM_SUPPORTS_TIMEZONEINFO)

include_directories(${IL2CPP_SOURCE_DIR}/libil2cpp/)
include_directories(${IL2CPP_SOURCE_DIR}/external/)
include_directories(${IL2CPP_SOURCE_DIR}/external/baselib/Include/)
include_directories(${IL2CPP_SOURCE_DIR}/external/bdwgc/include/)
include_directories(${IL2CPP_SOURCE_DIR}/external/baselib/Platforms/OSX/Include/)

include(${PROJECT_SOURCE_DIR}/CMake/ucm.cmake)
ucm_add_dirs(${IL2CPP_SOURCE_DIR}/libil2cpp/hybridclr TO hybridclr_src RECURSIVE ADDITIONAL_EXT mm m)
ucm_add_dirs(${IL2CPP_SOURCE_DIR}/libil2cpp/vm TO vm_src RECURSIVE ADDITIONAL_EXT mm m)
ucm_add_dirs(${IL2CPP_SOURCE_DIR}/libil2cpp/pch TO pch_src RECURSIVE ADDITIONAL_EXT mm m)
ucm_add_dirs(${IL2CPP_SOURCE_DIR}/libil2cpp/utils TO utils_src RECURSIVE ADDITIONAL_EXT mm m)
ucm_add_dirs(${IL2CPP_SOURCE_DIR}/libil2cpp/vm-utils TO vm_utils_src RECURSIVE ADDITIONAL_EXT mm m)
ucm_add_dirs(${IL2CPP_SOURCE_DIR}/libil2cpp/codegen TO codegen_src RECURSIVE ADDITIONAL_EXT mm m)
ucm_add_dirs(${IL2CPP_SOURCE_DIR}/libil2cpp/metadata TO metadata_src RECURSIVE ADDITIONAL_EXT mm m)
ucm_add_dirs(${IL2CPP_SOURCE_DIR}/libil2cpp/os TO os_src RECURSIVE ADDITIONAL_EXT mm m)
ucm_add_dirs(${IL2CPP_SOURCE_DIR}/libil2cpp/debugger TO debugger_src RECURSIVE ADDITIONAL_EXT mm m)
ucm_add_dirs(${IL2CPP_SOURCE_DIR}/libil2cpp/mono TO mono_src RECURSIVE ADDITIONAL_EXT mm m)
ucm_add_dirs(${IL2CPP_SOURCE_DIR}/libil2cpp/gc TO gc_src RECURSIVE ADDITIONAL_EXT mm m)
ucm_add_dirs(${IL2CPP_SOURCE_DIR}/libil2cpp/icalls TO icalls_src RECURSIVE ADDITIONAL_EXT mm m)

aux_source_directory(${IL2CPP_SOURCE_DIR}/libil2cpp LIBIL2CPP_SOURCE_LIST)
aux_source_directory(${IL2CPP_SOURCE_DIR}/external/zlib/ ZLIB_C_SOURCE_LIST)

set(src_file ${hybridclr_src}
	${vm_src} 
	${pch_src} 
	${utils_src}
	${vm_utils_src}
	${codegen_src}
	${metadata_src}
	${os_src}
	${debugger_src}
	${mono_src}
	${gc_src}
	${icalls_src}
	${ZLIB_C_SOURCE_LIST}
	${LIBIL2CPP_SOURCE_LIST})
	
SET_SOURCE_FILES_PROPERTIES(${src_file} PROPERTIES HEADER_FILE_ONLY TRUE)

# 一个假的工程 方便编辑 实际不参与生成
add_library(editOnly STATIC 
	${src_file}
	)
	




# 设置输出目录 不然xcode会生成在一个临时目录里 很难定位路径
if(CMAKE_GENERATOR STREQUAL Xcode)
    set(CMAKE_ARCHIVE_OUTPUT_DIRECTORY_DEBUG ${OUTPUT_BIN_DIR_IMP})
    set(CMAKE_ARCHIVE_OUTPUT_DIRECTORY_RELEASE ${OUTPUT_BIN_DIR_IMP})
    set(CMAKE_LIBRARY_OUTPUT_DIRECTORY_DEBUG ${OUTPUT_BIN_DIR_IMP})
    set(CMAKE_LIBRARY_OUTPUT_DIRECTORY_RELEASE ${OUTPUT_BIN_DIR_IMP})
    set(CMAKE_RUNTIME_OUTPUT_DIRECTORY_DEBUG ${OUTPUT_BIN_DIR_IMP})
    set(CMAKE_RUNTIME_OUTPUT_DIRECTORY_RELEASE ${OUTPUT_BIN_DIR_IMP})
endif()

add_subdirectory(original)
add_subdirectory(external)
add_subdirectory(objective)









