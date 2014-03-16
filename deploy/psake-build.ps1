Framework "4.5.1"

Properties {
    $solution_name = "MyCouch-All"
    $solution_dir_path = "..\src"
    $solution_path = "$solution_dir_path\$solution_name.sln"
    $project_name = "MyCouch"
    $project_name_cloudant = "MyCouch.Cloudant"
    $builds_dir_path = "builds"
    $build_version = "0.22.0"
    $build_config = "Release"
    $build_name = "${project_name}-v${build_version}-${build_config}"
    $build_dir_path = "${builds_dir_path}\${build_name}"
    $testrunner = "xunit.console.clr4.exe"
    $nuget = "nuget.exe"
}

task default -depends Clean, Build, Copy

task Clean {
    Clean-Directory("$build_dir_path")
}

task Build {
    Exec { msbuild "$solution_path" /t:Clean /v:quiet }
    Exec { msbuild "$solution_path" /t:Build /p:Configuration=$build_config /v:quiet }
}

task Copy {
    CopyTo-Build("$project_name.Net40")
    CopyTo-Build("$project_name.Net45")
    CopyTo-Build("$project_name.NetCore451")
    
    CopyTo-Build("$project_name_cloudant.Net40")
    CopyTo-Build("$project_name_cloudant.Net45")
    CopyTo-Build("$project_name_cloudant.NetCore451")
}

task UnitTest {
    UnitTest-ProjecT("Net40")
    UnitTest-ProjecT("Net45")
    UnitTest-ProjecT("NetCore451")
}

task NuGet-Pack {
    NuGet-Pack-Project($project_name)
    NuGet-Pack-Project($project_name_cloudant)
}

Function UnitTest-Project($t) {
    & $testrunner "$solution_dir_path\Tests\$project_name.$t.UnitTests\bin\$build_config\$project_name.$t.UnitTests.dll"
}

Function NuGet-Pack-Project($t) {
    & $nuget pack "$t.nuspec" -version $build_version -basepath $build_dir_path -outputdirectory $builds_dir_path
}

Function EnsureClean-Directory($dir) {
    Clean-Directory($dir)
    Create-Directory($dir)
}

Function Clean-Directory($dir){
	if (Test-Path -path $dir) {
        rmdir $dir -recurse -force
    }
}

Function Create-Directory($dir){
	if (!(Test-Path -path $dir)) {
        new-item $dir -force -type directory
    }
}

Function CopyTo-Build($t) {
    $trg = "$build_dir_path\$t"
    EnsureClean-Directory($trg)
    
    CopyTo-Directory "$solution_dir_path\Projects\$t\bin\$build_config\$t.*" $trg
}

Function CopyTo-Directory($src, $trg) {
    Copy-Item -Path $src -Include *.dll,*.xml -Destination $trg -Recurse -Container
}