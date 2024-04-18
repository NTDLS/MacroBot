;#define Debug
#define CompanyName         "NetworkDLS"
#define ApplicationName     "MacroBot"
#define ApplicationVersion  "1.0.3"

[Setup]
;-- Main Setup Information
 AppName                         = {#ApplicationName}
 AppVerName                      = {#ApplicationName} {#ApplicationVersion}
 AppCopyright                    = Copyright © 2000-2024 {#CompanyName}.
 DefaultDirName                  = {commonpf}\{#CompanyName}\{#ApplicationName}
 DefaultGroupName                = {#CompanyName}\{#ApplicationName}
 UninstallDisplayIcon            = {app}\MacroBot.exe
 SetupIconFile                    = "..\Art\Icon.ico"
 PrivilegesRequired              = admin
 Uninstallable                   = Yes
 Compression                     = ZIP/9
 MinVersion                      = 6.2
 ArchitecturesInstallIn64BitMode = x64
 ArchitecturesAllowed            = x86 x64
 OutputBaseFilename              = {#ApplicationName} {#ApplicationVersion}
 
;-- Windows 2000+ Support Dialog
 AppPublisher    = {#CompanyName}
 AppPublisherURL = http://www.NetworkDLS.com/
 AppUpdatesURL   = http://www.NetworkDLS.com/
 AppVersion      = {#ApplicationVersion}

[Files]
 Source: "..\bin\Release\net8.0-windows\*.json"; Excludes: "*vshost*"; DestDir: "{app}";              Flags: IgnoreVersion;
 Source: "..\bin\Release\net8.0-windows\*.exe";  Excludes: "*vshost*"; DestDir: "{app}";              Flags: IgnoreVersion;
 Source: "..\bin\Release\net8.0-windows\*.dll";  Excludes: "*vshost*"; DestDir: "{app}";              Flags: IgnoreVersion;

;---------------------------------------------------------------------

[Icons]
 Name: "{group}\{#ApplicationName}"; Filename: "{app}\MacroBot.exe";
 
;---------------------------------------------------------------------

[Run]
 Filename: "{app}\MacroBot"; Description: "Launch MacroBot."; Flags: postinstall nowait skipifsilent shellexec;

;---------------------------------------------------------------------
