# file: CMakeList.txt
cmake_minimum_required(VERSION 3.14)

set (CMAKE_CXX_STANDARD 11)
set (CMAKE_C_STANDARD 11)

set(HUATUO_IL2CPP_SOURCE_DIR "../LocalIl2CppData/il2cpp " CACHE STRING "源代码路径")
set(OUTPUT_BIN_DIR ${CMAKE_BINARY_DIR}/lib/iOS CACHE STRING "输出路径")

set(TMP ${HUATUO_IL2CPP_SOURCE_DIR})
if ( NOT TMP )
    message(FATAL_ERROR "需要设置环境变量: HUATUO_IL2CPP_SOURCE_DIR")
else()
    message(STATUS "unity il2cpp 路径为: ${TMP}")
endif()


project(il2cpp)
execute_process(COMMAND sh gen_lump.sh ${PROJECT_BINARY_DIR} WORKING_DIRECTORY ${CMAKE_SOURCE_DIR})

add_subdirectory(external)
add_subdirectory(objective)


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

add_definitions(-MMD)
#add_definitions(-MT dependencies)

 
include_directories(${IL2CPP_SOURCE_DIR}/libil2cpp/)
include_directories(${IL2CPP_SOURCE_DIR}/external/)
include_directories(${IL2CPP_SOURCE_DIR}/external/baselib/Include/)
include_directories(${IL2CPP_SOURCE_DIR}/external/bdwgc/include/)
include_directories(${IL2CPP_SOURCE_DIR}/external/baselib/Platforms/OSX/Include/)


aux_source_directory(${PROJECT_BINARY_DIR}/lump_cpp LUMP_SOURCE_LIST)
aux_source_directory(${IL2CPP_SOURCE_DIR}/libil2cpp LIBIL2CPP_SOURCE_LIST)


#find_library(zlib ${PROJECT_SOURCE_DIR}/external)
add_library(il2cpp_original STATIC 
	${LIBIL2CPP_SOURCE_LIST}
	${LUMP_SOURCE_LIST} 
	)
	
add_dependencies(il2cpp_original external objective)

add_custom_command(TARGET il2cpp_original 
    POST_BUILD
	COMMAND ${CMAKE_COMMAND} -E make_directory ${OUTPUT_BIN_DIR}
    COMMAND xcrun -r libtool -static -o ${OUTPUT_BIN_DIR}/libil2cpp.a $<TARGET_FILE:il2cpp_original> $<TARGET_FILE:external> $<TARGET_FILE:objective>
    COMMENT "post build this is command combine libil2cpp_original.a, libojjective.a and libzlib.a into libil2cpp"
    )


