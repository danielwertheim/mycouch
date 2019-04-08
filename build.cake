#load "./buildconfig.cake"

var config = BuildConfig.Create(Context, BuildSystem);
var verbosity = DotNetCoreVerbosity.Minimal;

Information($"SrcDir: '{config.SrcDir}'");
Information($"ArtifactsDir: '{config.ArtifactsDir}'");
Information($"SemVer: '{config.SemVer}'");
Information($"BuildVersion: '{config.BuildVersion}'");
Information($"BuildProfile: '{config.BuildProfile}'");

Task("Default")
    .IsDependentOn("Clean")
    .IsDependentOn("Build")
    .IsDependentOn("UnitTests");

Task("CI")
    .IsDependentOn("Default")
    .IsDependentOn("IntegrationTests")
    .IsDependentOn("Pack");
/********************************************/
Task("Clean").Does(() => {
    EnsureDirectoryExists(config.ArtifactsDir);
    CleanDirectory(config.ArtifactsDir);
});

Task("Build").Does(() => {
    foreach(var sln in GetFiles($"{config.SrcDir}*.sln")) {
        MSBuild(sln, settings =>
            settings
                .SetConfiguration(config.BuildProfile)
                .SetVerbosity(Verbosity.Minimal)
                .WithTarget("Rebuild")
                .WithProperty("TreatWarningsAsErrors", "true")
                .WithProperty("NoRestore", "true")
                .WithProperty("Version", config.SemVer)
                .WithProperty("AssemblyVersion", config.BuildVersion)
                .WithProperty("FileVersion", config.BuildVersion));
    }
});

Task("Build").Does(() => {
    var settings = new DotNetCoreBuildSettings {
        Configuration = config.BuildProfile,
        NoIncremental = true,
        NoRestore = false,
        Verbosity = verbosity,
        MSBuildSettings = new DotNetCoreMSBuildSettings()
            .WithProperty("TreatWarningsAsErrors", "true")
            .WithProperty("Version", config.SemVer)
            .WithProperty("AssemblyVersion", config.BuildVersion)
            .WithProperty("FileVersion", config.BuildVersion)
            .WithProperty("InformationalVersion", config.BuildVersion)
    };
    
    foreach(var sln in GetFiles($"{config.SrcDir}*.sln")) {
        DotNetCoreBuild(sln.FullPath, settings);
    }
});

Task("UnitTests").Does(() => {
    var settings = new DotNetCoreTestSettings {
        Configuration = config.BuildProfile,
        NoBuild = true,
        NoRestore = true,
        Logger = "trx",
        ResultsDirectory = config.TestResultsDir,
        Verbosity = verbosity
    };
    foreach(var testProj in GetFiles($"{config.SrcDir}tests/**/UnitTests.csproj")) {
        DotNetCoreTest(testProj.FullPath, settings);
    }
});

Task("IntegrationTests").Does(() => {
    var settings = new DotNetCoreTestSettings {
        Configuration = config.BuildProfile,
        NoBuild = true,
        NoRestore = true,
        Logger = "trx",
        ResultsDirectory = config.TestResultsDir,
        Verbosity = verbosity
    };
    foreach(var testProj in GetFiles($"{config.SrcDir}tests/**/IntegrationTests.csproj")) {
        DotNetCoreTest(testProj.FullPath, settings);
    }
});

Task("Pack").Does(() => {
    var settings = new DotNetCorePackSettings
    {
        Configuration = config.BuildProfile,
        OutputDirectory = config.ArtifactsDir,
        NoRestore = true,
        NoBuild = true,
        Verbosity = verbosity,
        MSBuildSettings = new DotNetCoreMSBuildSettings()
            .WithProperty("Version", config.SemVer)
            .WithProperty("AssemblyVersion", config.BuildVersion)
            .WithProperty("FileVersion", config.BuildVersion)
            .WithProperty("InformationalVersion", config.BuildVersion)
    };

    foreach(var proj in GetFiles($"{config.SrcDir}projects/**/*.csproj")) {
        DotNetCorePack(proj.FullPath, settings);
    }
});

RunTarget(config.Target);