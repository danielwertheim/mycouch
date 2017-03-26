#load "./buildconfig.cake"

var config = BuildConfig.Create(Context, BuildSystem);

Information("SrcDir: " + config.SrcDir);
Information("OutDir: " + config.OutDir);
Information("SemVer: " + config.SemVer);
Information("BuildProfile: " + config.BuildProfile);
Information("IsTeamCityBuild: " + config.IsTeamCityBuild);

Task("Default")
    .IsDependentOn("InitOutDir")
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

Task("Build").Does(() => {
    foreach(var sln in GetFiles(config.SrcDir + "*.sln")) {
        DotNetBuild(sln, settings => 
            settings.SetConfiguration(config.BuildProfile)
                .SetVerbosity(Verbosity.Minimal)
                .WithTarget("Rebuild")
                .WithProperty("TreatWarningsAsErrors", "true")
                .WithProperty("Version", config.SemVer)
                .WithProperty("AssemblyVersion", config.SemVer)
                .WithProperty("FileVersion", config.SemVer));
    }    
});

Task("UnitTests").Does(() => {
    var settings = new DotNetCoreTestSettings {
        Configuration = config.BuildProfile,
        NoBuild = true
    };
    foreach(var testProj in GetFiles(config.SrcDir + "tests/**/*.UnitTests.csproj")) {
        DotNetCoreTest(testProj.FullPath, settings);
    }
});

Task("IntegrationTests").Does(() => {
    var settings = new DotNetCoreTestSettings {
        Configuration = config.BuildProfile,
        NoBuild = true
    };
    foreach(var testProj in GetFiles(config.SrcDir + "tests/**/*.IntegrationTests.csproj")) {
        DotNetCoreTest(testProj.FullPath, settings);
    }
});

Task("Pack").Does(() => {
    foreach(var sln in GetFiles(config.SrcDir + "projects/**/*.csproj")) {
        DotNetBuild(sln, settings =>
            settings.SetConfiguration(config.BuildProfile)
                .SetVerbosity(Verbosity.Minimal)
                .WithTarget("Pack")
                .WithProperty("TreatWarningsAsErrors", "true")
                .WithProperty("Version", config.SemVer));
    }

    CopyFiles(
        GetFiles(config.SrcDir + "projects/**/*.nupkg"),
        config.OutDir);
});

RunTarget(config.Target);