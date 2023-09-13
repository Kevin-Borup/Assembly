cmake_minimum_required(VERSION 3.26)
project(AssemblyProject C)

set(CMAKE_C_STANDARD 99)

add_executable(AssemblyProject main.c
        FileHandler.c
        FileHandler.h)
