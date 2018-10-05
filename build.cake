#tool nuget:?package=xunit.runner.visualstudio&version=2.4.0

///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

///////////////////////////////////////////////////////////////////////////////
// FOLDERS
///////////////////////////////////////////////////////////////////////////////

var testDirectory = Directory("./CakeCoverageTest.Test/");
var testLocation = testDirectory.Path + $"/bin/{configuration}/*/*.Test.dll";

///////////////////////////////////////////////////////////////////////////////
// TASKS
///////////////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
{
    CleanDirectories("**/bin/" + configuration);
    CleanDirectories("**/obj/" + configuration);
    CleanDirectories("TestResults");
});

Task("Restore")
    .IsDependentOn("Clean")
    .Does(() =>
{
    DotNetCoreRestore();
});

Task("Build")
    .IsDependentOn("Restore")
    .Does(() =>
{
    MSBuild("./CakeCoverageTest.sln", new MSBuildSettings() {
        Configuration = configuration
    });
});

// the VSTest task has a bug where it doesn't look in the right location for vstest.console.exe, see https://github.com/cake-build/cake/issues/1522#issuecomment-341612194
VSTestSettings FixToolPath(VSTestSettings settings)
{
    settings.ToolPath = VSWhereLatest(new VSWhereLatestSettings { 
        Requires = "Microsoft.VisualStudio.PackageGroup.TestTools.Core" 
    }).CombineWithFilePath(File(@"Common7\IDE\CommonExtensions\Microsoft\TestWindow\vstest.console.exe"));
    return settings;
}

Task("Test-VSTest")
    .Description("Runs unit tests through VSTest with code coverage.  This is typically for VSTS, but could be run locally.")
    .IsDependentOn("Build")
    .Does(() => 
{
    Information("Looking for tests in: " + testLocation);
    VSTest(testLocation, FixToolPath(new VSTestSettings
    {
        EnableCodeCoverage = true,
        InIsolation = true, // run in a separate process to get coverage data accurately
        Logger = "trx", // VSTS only speaks trx
        TestAdapterPath = "tools/xunit.runner.visualstudio.2.4.0/build/_common" // run VSTest with the xunit adapter
    }));
});

Task("Default")
.Does(() => {
   Information("Hello Cake!");
});

RunTarget(target);