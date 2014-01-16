solution_name = "MyCouch-All"
solution_dir_path = "../src"
project_name = "MyCouch"
project_name_cloudant = "MyCouch.Cloudant"
builds_dir_path = "builds"
build_version = "0.20.0"
build_config = "Release"
build_name = "${project_name}-v${build_version}-${build_config}"
build_dir_path = "${builds_dir_path}/${build_name}"
testrunner = "xunit.console.clr4.exe"
nuget = "nuget.exe"

target default, (clean, compile, copy, test, zip, nuget_pack):
    pass
	
target copy, (copy_core, copy_cloudant):
	pass
	
target nuget_pack, (nuget_pack_core, nuget_pack_cloudant):
	pass

target clean:
    rm(build_dir_path)

target compile:
    msbuild(
        file: "${solution_dir_path}/${solution_name}.sln",
        targets: ("Clean", "Build"),
        configuration: build_config)

target copy_core:
    with FileList("${solution_dir_path}/Projects/${project_name}.Net40/bin/${build_config}"):
        .Include("${project_name}.*.{dll,xml}")
        .ForEach def(file):
            file.CopyToDirectory("${build_dir_path}/Net40")
    with FileList("${solution_dir_path}/Projects/${project_name}.Net45/bin/${build_config}"):
        .Include("${project_name}.*.{dll,xml}")
        .ForEach def(file):
            file.CopyToDirectory("${build_dir_path}/Net45")
    with FileList("${solution_dir_path}/Projects/${project_name}.NetCore451/bin/${build_config}"):
        .Include("${project_name}.*.{dll,xml}")
        .ForEach def(file):
            file.CopyToDirectory("${build_dir_path}/NetCore451")

target copy_cloudant:
    with FileList("${solution_dir_path}/Projects/${project_name_cloudant}.Net40/bin/${build_config}"):
        .Include("${project_name_cloudant}.*.{dll,xml}")
        .ForEach def(file):
            file.CopyToDirectory("${build_dir_path}/Net40")
    with FileList("${solution_dir_path}/Projects/${project_name_cloudant}.Net45/bin/${build_config}"):
        .Include("${project_name_cloudant}.*.{dll,xml}")
        .ForEach def(file):
            file.CopyToDirectory("${build_dir_path}/Net45")
    with FileList("${solution_dir_path}/Projects/${project_name_cloudant}.NetCore451/bin/${build_config}"):
        .Include("${project_name_cloudant}.*.{dll,xml}")
        .ForEach def(file):
            file.CopyToDirectory("${build_dir_path}/NetCore451")

target test, (test40, test45, testnetcore451):
    pass

target test40:
    exec(testrunner, "${solution_dir_path}/Tests/${project_name}.Net40.UnitTests/bin/${build_config}/${project_name}.Net40.UnitTests.dll")

target test45:
    exec(testrunner, "${solution_dir_path}/Tests/${project_name}.Net45.UnitTests/bin/${build_config}/${project_name}.Net45.UnitTests.dll")

target testnetcore451:
    exec(testrunner, "${solution_dir_path}/Tests/${project_name}.NetCore451.UnitTests/bin/${build_config}/${project_name}.NetCore451.UnitTests.dll")

target zip:
    zip(build_dir_path, "${builds_dir_path}/${build_name}.zip")

target nuget_pack_core:
    exec(nuget, "pack ${project_name}.nuspec -version ${build_version} -basepath ${build_dir_path} -outputdirectory ${builds_dir_path}")

target nuget_pack_cloudant:
    exec(nuget, "pack ${project_name_cloudant}.nuspec -version ${build_version} -basepath ${build_dir_path} -outputdirectory ${builds_dir_path}")