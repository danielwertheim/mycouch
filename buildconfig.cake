public class BuildConfig
{
    private const string Version = "5.0.2";

    public readonly string SrcDir = "./source/";
    public readonly string OutDir = "./build/";    
    
    public string Target { get; private set; }
    public string SemVer { get; private set; }
    public string BuildVersion { get; private set; }
    public string BuildProfile { get; private set; }
    public bool IsTeamCityBuild { get; private set; }
    
    public static BuildConfig Create(
        ICakeContext context,
        BuildSystem buildSystem)
    {
        if (context == null)
            throw new ArgumentNullException("context");

        var branch = context.Argument("branch", string.Empty).ToLower();
        var buildRevision = context.Argument("buildrevision", "0");
        var isRelease = branch == "master";

        return new BuildConfig
        {
            Target = context.Argument("target", "Default"),
            SemVer = Version + (isRelease
                ? string.Empty
                : "-pre" + buildRevision),
            BuildVersion = Version + "." + buildRevision,
            BuildProfile = context.Argument("configuration", "Release"),
            IsTeamCityBuild = buildSystem.TeamCity.IsRunningOnTeamCity
        };
    }
}
