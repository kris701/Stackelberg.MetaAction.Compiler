using PDDLSharp.Models.PDDL.Domain;
using PDDLSharp.Models.PDDL.Expressions;

namespace Stackelberg.MetaAction.Compiler.Compilers
{
    public static class LegalActionChecker
    {
        public static ActionDecl EnsureLegalPreconditions(ActionDecl act)
        {
            var copy = act.Copy();

            var illigals = new List<IlligalParameters>();

            if (copy.Effects is AndExp effAnd)
            {
                var trues = new List<PredicateExp>();
                var falses = new List<PredicateExp>();
                foreach (var child in effAnd.Children)
                {
                    if (child is PredicateExp pred)
                        trues.Add(pred);
                    if (child is NotExp not && not.Child is PredicateExp pred2)
                        falses.Add(pred2);
                }

                foreach(var child in trues)
                {
                    var first = falses.FirstOrDefault(x => x.Name == child.Name);
                    if (first != null)
                    {
                        for(int i = 0; i < child.Arguments.Count; i++)
                        {
                            if (child.Arguments[i].Name != first.Arguments[i].Name)
                                illigals.Add(new IlligalParameters(child.Arguments[i].Name, first.Arguments[i].Name));
                        }
                    }
                }
            }

            if (copy.Preconditions is AndExp preAnd)
            {
                illigals = illigals.Distinct().ToList();
                foreach (var illigal in illigals)
                {
                    var newNot = new NotExp(preAnd, new PredicateExp("=", new List<NameExp>() { new NameExp(illigal.Parameter1), new NameExp(illigal.Parameter2) }));
                    newNot.Child.Parent = newNot;
                    preAnd.Add(newNot);
                }
            }
             
            return copy;
        }

        private class IlligalParameters
        {
            public string Parameter1 { get; set; }
            public string Parameter2 { get; set; }

            public IlligalParameters(string parameter1, string parameter2)
            {
                Parameter1 = parameter1;
                Parameter2 = parameter2;
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(Parameter1, Parameter2);
            }

            public override bool Equals(object? obj)
            {
                if (obj is IlligalParameters other)
                {
                    if (other.Parameter1 == Parameter2 && other.Parameter2 == Parameter1) return true;
                    if (other.Parameter1 != Parameter1) return false;
                    if (other.Parameter2 != Parameter2) return false;
                    return true;
                }
                return false;
            }
        }
    }
}
