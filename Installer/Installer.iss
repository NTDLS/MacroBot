;#define Debug
#define CompanyName         "NetworkDLS"
#define ApplicationName     "MacroBot"
#define ApplicationVersion  "2.0.0"

[Setup]
;-- Main Setup Information
 AppName                         = {#ApplicationName}
 AppVerName                      = {#ApplicationName} {#ApplicationVersion}
 AppCopyright                    = Copyright © 2000-2025 {#CompanyName}.
 DefaultDirName                  = {commonpf}\{#CompanyName}\{#ApplicationName}
 DefaultGroupName                = {#CompanyName}\{#ApplicationName}
 UninstallDisplayIcon            = {app}\MacroBot.exe
 SetupIconFile                    = "..\Art\Icon.ico"
 PrivilegesRequired              = admin
 Uninstallable                   = Yes
 Compression                     = ZIP/9
 MinVersion                      = 6.2
 ArchitecturesInstallIn64BitMode = x64os
 ArchitecturesAllowed            = x86 x64os
 OutputBaseFilename              = {#ApplicationName} {#ApplicationVersion}
 
;-- Windows 2000+ Support Dialog
 AppPublisher    = {#CompanyName}
 AppPublisherURL = http://www.NetworkDLS.com/
 AppUpdatesURL   = http://www.NetworkDLS.com/
 AppVersion      = {#ApplicationVersion}

[Files]
 Source: "Publish\*.*"; DestDir: "{app}"; Flags: IgnoreVersion recursesubdirs;
 
;---------------------------------------------------------------------

[Icons]
 Name: "{group}\{#ApplicationName}"; Filename: "{app}\MacroBot.exe";
 
;---------------------------------------------------------------------

[Run]
 Filename: "{app}\MacroBot"; Description: "Launch MacroBot."; Flags: postinstall nowait skipifsilent shellexec;

;---------------------------------------------------------------------
