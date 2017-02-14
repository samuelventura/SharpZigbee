TITLE NuGet Packager
@ECHO ON

CD %~dp0
CD ..
nuget pack SharpZigbee\SharpZigbee\package.nuspec
PAUSE

