import MyPhantom

solution_name = "MyCouch-All"
solution_dir_path = "../src"
project_name = "MyCouch"
builds_dir_path = "builds"
build_version = "0.9.0"
build_config = "Release"
build_name = "${project_name}-v${build_version}-${build_config}"
build_dir_path = "${builds_dir_path}/${build_name}"

target default, (clean, compile, copy, test, zip, nuget_pack):
  pass

target clean:
    rm(build_dir_path)

target compile:
    msbuild(
        file: "${solution_dir_path}/${solution_name}.sln",
        targets: ("Clean", "Build"),
        configuration: build_config)
        
target copy:
    with FileList("${solution_dir_path}/Projects/${project_name}.Net40/bin/${build_config}"):
        .Include("${project_name}.*.{dll,xml}")
        .ForEach def(file):
            file.CopyToDirectory("${build_dir_path}/Net40")
    with FileList("${solution_dir_path}/Projects/${project_name}.Net45/bin/${build_config}"):
        .Include("${project_name}.*.{dll,xml}")
        .ForEach def(file):
            file.CopyToDirectory("${build_dir_path}/Net45")
    with FileList("${solution_dir_path}/Projects/${project_name}.WinRT/bin/${build_config}"):
        .Include("${project_name}.*.{dll,xml}")
        .ForEach def(file):
            file.CopyToDirectory("${build_dir_path}/WinRT")

target test:
    mynunit(
        assemblies: MyFileList("${solution_dir_path}/Tests/*.UnitTests/bin/${build_config}", "*.UnitTests.{dll}"),
        options: "/framework=v4.0.30319 /xml=${build_dir_path}/NUnit-Report-${build_name}-UnitTests.xml")

target zip:
    zip(build_dir_path, "${builds_dir_path}/${build_name}.zip")
    
target nuget_pack:
    mynuget(
        options: "pack ${solution_name}.nuspec -version ${build_version} -basepath ${builds_dir_path} -outputdirectory ${builds_dir_path}")
    mynuget(
        options: "pack ${solution_name}.Source.nuspec -version ${build_version} -basepath ${solution_dir_path} -outputdirectory ${builds_dir_path}")