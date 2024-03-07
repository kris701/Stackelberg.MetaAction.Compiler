using PDDLSharp.Models.PDDL.Domain;
using Stackelberg.MetaAction.Compiler.Compilers;

namespace Stackelberg.MetaAction.Compiler.Compilers.Tests
{
    [TestClass]
    public class StaticPredicateDetectorTests
    {
        // Note, the actual static predicate detection is covered by PDDLSharp. We only add the = as well.
        [TestMethod]
        public void Can_AddEqualityToStaticPredicates()
        {
            // ARRANGE
            StaticPredicateDetector.Clear();
            Assert.IsFalse(StaticPredicateDetector.StaticPredicates.Contains("="));
            var domain = new DomainDecl();

            // ACT
            StaticPredicateDetector.GenerateStaticPredicates(domain);

            // ASSERT
            Assert.IsTrue(StaticPredicateDetector.StaticPredicates.Contains("="));
        }
    }
}
