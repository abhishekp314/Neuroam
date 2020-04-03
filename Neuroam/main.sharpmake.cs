using System.IO;
using Sharpmake;

[module: Reference("Sharpmake.CommonPlatforms.dll")]

namespace Neuroam
{
    [Generate]
    public class Neuroam : Project
    {
        public Neuroam()
        {
            Name = "Neuroam";
            SourceRootPath = Path.Combine(@"[project.SharpmakeCsPath]", @"source");

            AddTargets(new Target(
                Platform.win64,
                DevEnv.vs2019,
                Optimization.Debug | Optimization.Release
            ));
        }

        [Configure()]
        public void ConfigureAll(Project.Configuration conf, Target target)
        {
            conf.ProjectPath = Path.Combine(@"[project.SharpmakeCsPath]", "projects");

            // Compiler Options
            conf.Options.Add(Options.Vc.General.PlatformToolset.LLVM);
            conf.Options.Add(Options.Vc.General.WindowsTargetPlatformVersion.v10_0_18362_0);
            conf.Options.Add(Options.Vc.Compiler.CppLanguageStandard.CPP17);

            // Linker Options
            conf.Options.Add(Options.Vc.Linker.GenerateMapFile.Disable);

            // Include Paths
            conf.IncludePaths.Add(Path.Combine(@"[project.SharpmakeCsPath]", @"source"));
            conf.IncludePaths.Add(Path.Combine(@"[project.SharpmakeCsPath]", @"external"));

            // PCH
            conf.PrecompHeader = "PrecompiledHeader.h";
            conf.PrecompSource = "PrecompiledHeader.cpp";

            // Libs
            conf.LibraryPaths.Add(Path.Combine(@"[project.SharpmakeCsPath]", @"external/Judy/lib"));
            conf.LibraryFiles.Add("judy.lib");
            conf.Defines.Add("JU_WIN");
        }
    }

    [Generate]
    public class NeuroamSolution : Solution
    {
        public NeuroamSolution()
        {
            Name = "Neuroam";
            AddTargets(new Target(
                Platform.win64,
                DevEnv.vs2019,
                Optimization.Debug | Optimization.Release
            ));
        }

        [Configure()]
        public void ConfigureAll(Solution.Configuration conf, Target target)
        {
            conf.SolutionPath = Path.Combine(@"[solution.SharpmakeCsPath]");
            conf.AddProject<Neuroam>(target);
        }

        [Main]
        public static void SharpmakeMain(Arguments sharpmakeArgs)
        {
            //ClangForWindows.Settings.LLVMInstallDir = @"C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\VC\Tools\Llvm";
            KitsRootPaths.SetUseKitsRootForDevEnv(DevEnv.vs2019, KitsRootEnum.KitsRoot10, Options.Vc.General.WindowsTargetPlatformVersion.v10_0_18362_0);
            sharpmakeArgs.Generate<NeuroamSolution>();
        }
    }
}