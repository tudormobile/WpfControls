@echo off
del /s *.trx
dotnet test --logger trx;LogFilePrefix=output --collect "Code Coverage" --settings test.runsettings
if %errorlevel% neq 0 exit /b %errorlevel%
dotnet build -c Release
if %errorlevel% neq 0 exit /b %errorlevel%
dotnet tool update -g dotnet-Coverage
dotnet-coverage merge **\*.coverage --remove-input-files --output-format xml --output output.xml
dotnet tool update -g dotnet-reportgenerator-globaltool
reportgenerator -reports:"output.xml" -targetdir:"output" -reporttypes:"MarkdownSummaryGithub"
dotnet tool update -g docfx
if %errorlevel% neq 0 exit /b %errorlevel%
docfx docs/docfx.json
if %errorlevel% neq 0 exit /b %errorlevel%
del docs\_site\*.ico
dotnet build -c Release src/WpfControls/Tudormobile.Wpf.Behaviors.csproj
dotnet build -c Release src/WpfControls/Tudormobile.Wpf.Converters.csproj
powershell -command compress-archive -Path src\WpfControls\bin\release\*.nupkg -Destination package.zip -Force
set /p ver=<src\WpfControls\bin\release\ver.txt
echo version=%ver%

echo Test Results
set reg=executed=\"(\d+)\" passed=\"(\d+)\" failed=\"(\d+)\"
set test=":purple_circle: Tests: "
set pass=" :green_circle: Passed: "
set fail=" :red_circle: Failed: "
powershell -command "Get-ChildItem -Recurse -Filter *.trx | ForEach-Object {get-content $_.FullName | select-string '%reg%' | ForEach-Object { '%test%' + $_.Matches[0].Groups[1].Value + '%pass%' + $_.Matches[0].Groups[2].Value + '%fail%' + $_.Matches[0].Groups[3].Value}}"

echo Code Coverage
powershell -command "get-content output\SummaryGithub.md | select-string '<summary>(.+%)</summary>' | ForEach-Object {$_.Matches[0].Groups[1].Value}"
del output.xml
del package.zip
