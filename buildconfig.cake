public class BuildConfig
{
    private const string Version = "6.0.0";
    private const bool IsPreRelease = false;

    public readonly string SrcDir = "./source/";
    public readonly string OutDir = "./build/";    

    public string Target { get; private set; }
    public string SemVer { get; private set; }
    public string BuildVersion { get; private set; }
    public string BuildProfile { get; private set; }
    public bool IsTeamCityBuild { get; private set; }
    public string NuGetSource { get; private set; }
    public string NuGetApiKey { get; private set; }

    public static BuildConfig Create(
        ICakeContext context,
        BuildSystem buildSystem)
    {
        if (context == null)
            throw new ArgumentNullException("context");

        var buildRevision = context.Argument("buildrevision", "0");
        var nuGetSource = context.EnvironmentVariable("NuGetSource") ?? context.Argument("nugetsource", string.Empty);
        var nuGetApiKey = context.EnvironmentVariable("NuGetApiKey") ?? context.Argument("nugetapikey", string.Empty);

        return new BuildConfig
        {
            Target = context.Argument("target", "Default"),
            SemVer = Version + (IsPreRelease ? $"-pre{buildRevision}" : string.Empty),
            BuildVersion = Version + "." + buildRevision,
            BuildProfile = context.Argument("configuration", "Release"),
            IsTeamCityBuild = buildSystem.TeamCity.IsRunningOnTeamCity,
            NuGetSource = nuGetSource,
            NuGetApiKey = nuGetApiKey
        };
    }
}
