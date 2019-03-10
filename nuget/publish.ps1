Write-Output "Publishing Artifacts to nuget .."

$artifact = Get-ChildItem .\Artifacts\*.nupkg

dotnet nuget push $artifact