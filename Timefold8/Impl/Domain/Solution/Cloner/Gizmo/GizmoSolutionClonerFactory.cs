using TimefoldSharp.Core.API.Domain.Solution;
using TimefoldSharp.Core.Impl.Domain.Common.Accessor.Gizmo;
using TimefoldSharp.Core.Impl.Domain.Solution.Descriptor;

namespace TimefoldSharp.Core.Impl.Domain.Solution.Cloner.Gizmo
{
    public sealed class GizmoSolutionClonerFactory
    {
        public static string GetGeneratedClassName(SolutionDescriptor solutionDescriptor)
        {
            return solutionDescriptor.SolutionClass.Name + "$Timefold$SolutionCloner";
        }

        public static SolutionCloner Build(SolutionDescriptor solutionDescriptor, GizmoClassLoader gizmoClassLoader)
        {
            return GizmoSolutionClonerImplementor.CreateClonerFor(solutionDescriptor, gizmoClassLoader);
        }
    }
}
