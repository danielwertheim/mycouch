#load "./buildconfig.cake"

var config = BuildConfig.Create(Context, BuildSystem);

Information($"SrcDir: '{config.SrcDir}'");
Information($"OutDir: '{config.OutDir}'");
Information($"SemVer: '{config.SemVer}'");
Information($"BuildVersion: '{config.BuildVersion}'");
Information($"BuildProfile: '{config.BuildProfile}'");
Information($"IsTeamCityBuild: '{config.IsTeamCityBuild}'");

Task("Default")
    .IsDependentOn("InitOutDir")
    .IsDependentOn("Restore")
    .IsDependentOn("Build")
    .IsDependentOn("UnitTests");

Task("CI")
    .IsDependentOn("Default")
    .IsDependentOn("IntegrationTests")
    .IsDependentOn("Pack");
/********************************************/
Task("InitOutDir").Does(() => {
    EnsureDirectoryExists(config.OutDir);
    CleanDirectory(config.OutDir);
});

Task("Restore").Does(() => {
    foreach(var sln in GetFiles($"{config.SrcDir}*.sln")) {
        MSBuild(sln, settings =>
            settings
                .SetConfiguration(config.BuildProfile)
                .SetVerbosity(Verbosity.Minimal)
                .WithTarget("Restore")
                .WithProperty("TreatWarningsAsErrors", "true")
                .WithProperty("Version", config.SemVer));
    }
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

Task("UnitTests").Does(() => {
    var settings = new DotNetCoreTestSettings {
        Configuration = config.BuildProfile,
        NoBuild = true
    };
    foreach(var testProj in GetFiles($"{config.SrcDir}tests/**/*.UnitTests.csproj")) {
        DotNetCoreTest(testProj.FullPath, settings);
    }
});

Task("IntegrationTests").Does(() => {
    var settings = new DotNetCoreTestSettings {
        Configuration = config.BuildProfile,
        NoBuild = true
    };
    foreach(var testProj in GetFiles($"{config.SrcDir}tests/**/*.IntegrationTests.csproj")) {
        DotNetCoreTest(testProj.FullPath, settings);
    }
});

Task("Pack").Does(() => {
    DeleteFiles($"{config.SrcDir}projects/**/*.nupkg");

    foreach(var proj in GetFiles($"{config.SrcDir}projects/**/*.csproj")) {
        MSBuild(proj, settings =>
            settings
                .SetConfiguration(config.BuildProfile)
                .SetVerbosity(Verbosity.Minimal)
                .WithTarget("Pack")
                .WithProperty("TreatWarningsAsErrors", "true")
                .WithProperty("NoRestore", "true")
                .WithProperty("NoBuild", "true")
                .WithProperty("Version", config.SemVer));
    }

    CopyFiles(
        GetFiles($"{config.SrcDir}projects/**/*.nupkg"),
        config.OutDir);
});

Task("Upload").Does(() => {
    foreach(var nupkg in GetFiles($"{config.OutDir}*.nupkg")) {
        NuGetPush(nupkg, new NuGetPushSettings {
            Source = config.NuGetSource,
            ApiKey = config.NuGetApiKey
        });
    }
});

RunTarget(config.Target);