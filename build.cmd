@echo off

:: Examples
:: build debug
:: build debug /t:RunTests
:: build debug "/p:IsEnableCodeAnalysis=true"

set config=%1
if "%config%" == "" (
    set config=Debug
) else (
   SHIFT
)

msbuild build\Build.proj /p:Configuration="%config%" /m /v:m /fl /flp:LogFile=build.log;Verbosity=Normal /bl:build.binlog /nr:false %1 %2 %3 %4 %5 %6 %7 %8 %9
