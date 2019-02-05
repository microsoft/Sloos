@echo off
msbuild build\Build.proj /p:Configuration="%config%" /m /v:m /fl /flp:LogFile=clean.log;Verbosity=Normal /nr:false /t:clean
