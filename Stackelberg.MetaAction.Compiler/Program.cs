using CommandLine;
using PDDLSharp.CodeGenerators;
using PDDLSharp.CodeGenerators.PDDL;
using PDDLSharp.ErrorListeners;
using PDDLSharp.Models.PDDL;
using PDDLSharp.Models.PDDL.Domain;
using PDDLSharp.Models.PDDL.Problem;
using PDDLSharp.Parsers;
using PDDLSharp.Parsers.PDDL;
using Stackelberg.MetaAction.Compiler.Compilers;
using Stackelberg.MetaAction.Compiler.Helpers;
using System.Diagnostics;

namespace Stackelberg.MetaAction.Compiler
{
    internal class Program : BaseCLI
    {
        static int Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
              .WithParsed(Run)
              .WithNotParsed(HandleParseError);
            return 0;
        }

        public static void Run(Options opts)
        {
            Stopwatch watch = new Stopwatch();

            opts.DomainFilePath = PathHelper.RootPath(opts.DomainFilePath);
            opts.ProblemFilePath = PathHelper.RootPath(opts.ProblemFilePath);
            opts.MetaActionFile = PathHelper.RootPath(opts.MetaActionFile);
            opts.OutputPath = PathHelper.RootPath(opts.OutputPath);

            ConsoleHelper.WriteLineColor("Verifying paths...");
            watch.Start();
            if (!Directory.Exists(opts.OutputPath))
                Directory.CreateDirectory(opts.OutputPath);
            if (!File.Exists(opts.DomainFilePath))
                throw new FileNotFoundException($"Domain file not found: {opts.DomainFilePath}");
            if (!File.Exists(opts.ProblemFilePath))
                throw new FileNotFoundException($"Problem file not found: {opts.ProblemFilePath}");
            if (!File.Exists(opts.MetaActionFile))
                throw new FileNotFoundException($"Meta action file not found: {opts.MetaActionFile}");
            watch.Stop();
            ConsoleHelper.WriteLineColor($"Done! [{watch.ElapsedMilliseconds}ms]", ConsoleColor.Green);

            ConsoleHelper.WriteLineColor("Parsing files...");
            watch.Restart();
            IErrorListener listener = new ErrorListener();
            IParser<INode> parser = new PDDLParser(listener);

            var domain = parser.ParseAs<DomainDecl>(new FileInfo(opts.DomainFilePath));
            var problem = parser.ParseAs<ProblemDecl>(new FileInfo(opts.ProblemFilePath));
            var metaAction = parser.ParseAs<ActionDecl>(new FileInfo(opts.MetaActionFile));
            watch.Stop();
            ConsoleHelper.WriteLineColor($"Done! [{watch.ElapsedMilliseconds}ms]", ConsoleColor.Green);

            if (opts.Compiler == Options.TargetCompiler.Conditional)
            {
                var conditionals = GenerateConditionalDecl(domain, problem, metaAction);
                OutputFiles(conditionals, opts.OutputPath);
            } 
            else if (opts.Compiler == Options.TargetCompiler.Simplified)
            {
                var conditionals = GenerateConditionalDecl(domain, problem, metaAction);
                var simplifiedConditionalDec = GenerateSimplifiedDecl(conditionals);
                OutputFiles(simplifiedConditionalDec, opts.OutputPath);
            }
        }

        private static PDDLDecl GenerateConditionalDecl(DomainDecl domain, ProblemDecl problem, ActionDecl metaAction)
        {
            ConsoleHelper.WriteLineColor("Generating conditional domain/problem...");
            ConditionalEffectCompiler compiler = new ConditionalEffectCompiler();
            var conditionalDecl = compiler.GenerateConditionalEffects(domain, problem, metaAction);
            ConsoleHelper.WriteLineColor($"Done!", ConsoleColor.Green);
            return conditionalDecl;
        }

        private static PDDLDecl GenerateSimplifiedDecl(PDDLDecl conditionalDecl)
        {
            ConsoleHelper.WriteLineColor("Generating simplified domain/problem...");
            ConditionalEffectSimplifyer abstractor = new ConditionalEffectSimplifyer();
            var simplifiedConditionalDec = abstractor.SimplifyConditionalEffects(conditionalDecl.Domain, conditionalDecl.Problem);
            ConsoleHelper.WriteLineColor($"Done!", ConsoleColor.Green);
            return simplifiedConditionalDec;
        }

        private static void OutputFiles(PDDLDecl decl, string outputPath)
        {
            ConsoleHelper.WriteLineColor("Outputting files...");
            ICodeGenerator<INode> generator = new PDDLCodeGenerator(new ErrorListener());
            generator.Readable = true;

            generator.Generate(decl.Domain, Path.Combine(outputPath, "simplified_domain.pddl"));
            generator.Generate(decl.Problem, Path.Combine(outputPath, "simplified_problem.pddl"));
            ConsoleHelper.WriteLineColor($"Done!", ConsoleColor.Green);
        }
    }
}