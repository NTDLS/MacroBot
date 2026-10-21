@echo off
set path=C:\Program Files (x86)\Inno Setup 6\;%path%

rd Publish /q /s
rd Output /q /s

md Publish
md Output

REM Compile service for Windows.x64.
dotnet publish ..\MacroBot -c Release -o publish --runtime win-x64 --self-contained false
del Publish\*.pdb
iscc ./Installer.Iss

rd publish /q /s

pause
