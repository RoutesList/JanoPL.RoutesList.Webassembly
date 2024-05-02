using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;
using Serilog;

namespace JanoPL.RoutesList.Build;

class Build : NukeBuild
{
    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Solution] readonly Solution Solution;

    [Parameter] readonly AbsolutePath TestResultDirectory = RootDirectory + "/.nuke/Artifacts/Test-Results/";

    Target LogInformation =>
        _ =>
            _.Executes(() =>
            {
                Log.Information($"Solution path : {Solution}");
                Log.Information($"Solution directory : {Solution.Directory}");
                Log.Information($"Configuration : {Configuration}");
                Log.Information($"TestResultDirectory : {TestResultDirectory}");
                Log.Information($"Solution path : {Solution}");
            });

    Target Preparation =>
        _ => _.DependsOn(LogInformation)
            .Executes(() =>
            {
                TestResultDirectory.CreateOrCleanDirectory();
                Log.Information($"Directory {TestResultDirectory.Name} : create or clean");
            });

    Target Restore =>
        _ =>
            _.DependsOn(Preparation)
                .Executes(
                    () =>
                    {
                        DotNetTasks.DotNetRestore();
                    });

    Target Clean => _ => _
        .Before(Restore)
        .Executes(() =>
        {
            DotNetTasks.DotNetClean();
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            DotNetTasks
                .DotNetBuild(
                    build => build.SetProjectFile(Solution)
                        .SetConfiguration(Configuration)
                );
        });

    // Target Tests => _ => _
    //     .DependsOn(Compile)
    //     .Executes(() =>
    //     {
    //
    //     });

    /// Support plugins are available for:
    /// - JetBrains ReSharper        https://nuke.build/resharper
    /// - JetBrains Rider            https://nuke.build/rider
    /// - Microsoft VisualStudio     https://nuke.build/visualstudio
    /// - Microsoft VSCode           https://nuke.build/vscode
    public static int Main() => Execute<Build>(x => x.Compile);
}