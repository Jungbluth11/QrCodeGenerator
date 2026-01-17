#define MyAppName "QR Code Generator"
#define MyAppPublisher "Jungbluth"
#define MyAppExeName "QrCodeGenerator.exe"
#define AppVersion VERSION

[Setup]
AppId={{B7E7DD88-13E8-45F5-A4EA-64FA916F5CA4}
AppName={#MyAppName}
AppVersion={#AppVersion}
AppPublisher={#MyAppPublisher}
DefaultDirName={autopf}\{#MyAppName}
ArchitecturesAllowed=x64compatible
ArchitecturesInstallIn64BitMode=x64compatible
DisableProgramGroupPage=yes
Compression=lzma
SolidCompression=yes
WizardStyle=modern
OutputBaseFilename=QrCodeGeneratorInstaller_{#AppVersion}

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"
Name: "german"; MessagesFile: "compiler:Languages\German.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: ".\windows\*"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\windows\Assets\*"; DestDir: "{app}\Assets\"; Flags: ignoreversion
Source: ".\windows\CommunityToolkit.WinUI.UI\buildTransitive\*"; DestDir: "{app}\CommunityToolkit.WinUI.UI\buildTransitive"; Flags: ignoreversion
Source: ".\windows\Microsoft.WindowsDesktop.App\*"; DestDir: "{app}\Microsoft.WindowsDesktop.App"; Flags: ignoreversion
Source: ".\windows\Uno.Fonts.Fluent\Fonts\*"; DestDir: "{app}\Uno.Fonts.Fluent\Fonts"; Flags: ignoreversion
Source: ".\windows\Uno.Fonts.OpenSans\Fonts\*"; DestDir: "{app}\Uno.Fonts.OpenSans\Fonts"; Flags: ignoreversion


[Icons]
Name: "{autoprograms}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon
Name: "{group}\{cm:UninstallProgram,{#MyAppName}}"; Filename: "{uninstallexe}"

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent

