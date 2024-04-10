using CommandLine;

namespace Stackelberg.MetaAction.Compiler
{
    public class Options
    {
        [Flags]
        public enum TargetCompiler
        {
            Conditional = 0,
            Simplified = 1
        }

        [Option("domain", Required = true, HelpText = "Path to the domain file to use")]
        public string DomainFilePath { get; set; } = "";
        [Option("problem", Required = true, HelpText = "Path to the problem file to use")]
        public string ProblemFilePath { get; set; } = "";
        [Option("meta-action", Required = true, HelpText = "Path to the meta action file")]
        public string MetaActionFile { get; set; } = "";
        [Option("compiler", Required = true, HelpText = "What compiler to use.")]
        public TargetCompiler Compiler { get; set; } = TargetCompiler.Conditional;
        [Option("output", Required = false, HelpText = "Path to where to output the generated files.")]
        public string OutputPath { get; set; } = "";
    }
}
