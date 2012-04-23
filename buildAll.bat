@echo off

msbuild /t:Clean /p:Configuration=Release

del build
mkdir build

.nuget\nuget.exe pack -Build Tocsoft.Common.Helpers\Tocsoft.Common.Helpers.csproj -Prop Configuration=Release -OutputDirectory build
.nuget\nuget.exe pack -Build Tocsoft.Common.Helpers.Web\Tocsoft.Common.Helpers.Web.csproj -Prop Configuration=Release -OutputDirectory build
.nuget\nuget.exe pack -Build Tocsoft.Common.Helpers.Json\Tocsoft.Common.Helpers.Json.csproj -Prop Configuration=Release -OutputDirectory build
.nuget\nuget.exe pack -Build Tocsoft.Common.Database\Tocsoft.Common.Database.csproj -Prop Configuration=Release -OutputDirectory build
.nuget\nuget.exe pack -Build Tocsoft.Common.Database.SqlCE\Tocsoft.Common.SqlCeDatabase.csproj -OutputDirectory build
.nuget\nuget.exe pack -Build Tocsoft.Common.Umbraco\Tocsoft.Common.Umbraco.csproj -Prop Configuration=Release -OutputDirectory build

if %0 == push (

.nuget\nuget push build\*.nupkg

)