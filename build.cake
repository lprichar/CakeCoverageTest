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
    var settings = new DotNetCoreBuildSettings { Configuration = configuration };
    DotNetCoreBuild("./CakeCoverageTest.sln", settings);
});

Task("Test")
    .Description("Runs unit tests.")
    .IsDependentOn("Build")
    .Does(() => 
{
    var testLocation = File("./CakeCoverageTest.Test/CakeCoverageTest.Test.csproj");
    var settings = new DotNetCoreTestSettings {
        Configuration = configuration,
        NoBuild = true,
        ArgumentCustomization = args => args
            .Append("--collect").AppendQuoted("Code Coverage")
            .Append("--logger").Append("trx")
    };
    DotNetCoreTest(testLocation, settings);
});

Task("Default")
.Does(() => {
   Information("Hello Cake!");
});

RunTarget(target);