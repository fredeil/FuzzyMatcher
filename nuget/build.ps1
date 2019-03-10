$ErrorActionPreference = "Stop"

# Find MSBuild on this machine
if ($IsMacOS) {
    $msbuild = "msbuild"
} else {
    $vswhere = 'C:\Program Files (x86)\Microsoft Visual Studio\Installer\vswhere.exe'
    $msbuild = & $vswhere -latest -products * -requires Microsoft.Component.MSBuild -property installationPath
    $msbuild = join-path $msbuild 'MSBuild\15.0\Bin\MSBuild.exe'
}

Write-Output "Using MSBuild from: $msbuild"

# Build the project
& $msbuild "./" /restore /t:Build /p:Configuration=Release /p:Deterministic=false
if ($lastexitcode -ne 0) { exit $lastexitcode; }

# Create the stable NuGet package
& $msbuild "./" /t:Pack /p:Configuration=Release /p:Deterministic=false
if ($lastexitcode -ne 0) { exit $lastexitcode; }

# Copy everything into the output folder
Copy-Item "./bin/Release" "./Artifacts" -Recurse -Force

exit $lastexitcode;