# Project definition
cmake_minimum_required(VERSION 4.0...4.2)
project(OpenGLStarter)

#include(${CMAKE_BINARY_DIR}/conanbuildinfo.cmake)
#conan_basic_setup()

set(CMAKE_MODULE_PATH ${CMAKE_MODULE_PATH} "${CMAKE_SOURCE_DIR}/cmake/modules")
option(HIGHRES OFF)

# Source files
set(SRC_DIR "${CMAKE_CURRENT_SOURCE_DIR}/src")
set(INCLUDE_DIR "${CMAKE_CURRENT_SOURCE_DIR}/include")
set(LIB_DIR "${CMAKE_CURRENT_SOURCE_DIR}/libs")

set(SOURCES "${SRC_DIR}/main.cpp" "${INCLUDE_DIR}/Camera.h" "${INCLUDE_DIR}/Shader.h" "${INCLUDE_DIR}/stb_image.h" "${SRC_DIR}/helpers.cpp" "${SRC_DIR}/glad.c")

# Executable definition and properties
add_executable(${PROJECT_NAME} ${SOURCES})
target_include_directories(${PROJECT_NAME} PRIVATE "${SRC_DIR}" ${ASSIMP_INCLUDE_DIRS})

set_property(TARGET ${PROJECT_NAME} PROPERTY CXX_STANDARD 11)

# GLFW
set(GLFW_DIR "${LIB_DIR}/glfw")
set(GLFW_BUILD_EXAMPLES OFF CACHE INTERNAL "Build the GLFW example programs")
set(GLFW_BUILD_TESTS OFF CACHE INTERNAL "Build the GLFW test programs")
set(GLFW_BUILD_DOCS OFF CACHE INTERNAL "Build the GLFW documentation")
set(GLFW_INSTALL OFF CACHE INTERNAL "Generate installation target")
add_subdirectory("${GLFW_DIR}")
target_link_libraries(${PROJECT_NAME} "glfw" "${GLFW_LIBRARIES}")
target_include_directories(${PROJECT_NAME} PUBLIC "${GLFW_DIR}/include")
target_compile_definitions(${PROJECT_NAME} PUBLIC "GLFW_INCLUDE_NONE")

target_include_directories(${PROJECT_NAME} PUBLIC "${INCLUDE_DIR}")
# glad
set(GLAD_DIR "${LIB_DIR}/glad")
target_include_directories(${PROJECT_NAME} PUBLIC "${GLAD_DIR}/include")
#target_include_directories(${PROJECT_NAME} PRIVATE "${GLAD_DIR}/include")

if (HIGHRES)
target_compile_definitions(${PROJECT_NAME} PRIVATE __HIGHRES__=1)
MESSAGE("activated high resolution Mode")
else(HIGHRES)
target_compile_definitions(${PROJECT_NAME} PRIVATE __HIGHRES__=0)
MESSAGE("deactivated high resolution Mode")
endif()

# glm
#find_package(glm REQUIRED)
set(GLM_DIR "${LIB_DIR}/glm")
add_subdirectory("${GLM_DIR}")
target_include_directories(${PROJECT_NAME} PUBLIC "${GLM_DIR}")

