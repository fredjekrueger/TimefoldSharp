using TimefoldSharp.Core.API.Domain.Common;
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
            try
            {
                // Check if Gizmo on the classpath by verifying we can access one of its classes
                //Class.forName("io.quarkus.gizmo.ClassCreator", false, Thread.currentThread().getContextClassLoader());
            }
            catch (Exception)
            {
                throw new Exception("When using the domainAccessType (" +
                        DomainAccessType.GIZMO +
                        ") the classpath or modulepath must contain io.quarkus.gizmo:gizmo.\n" +
                        "Maybe add a dependency to io.quarkus.gizmo:gizmo.");
            }
            return GizmoSolutionClonerImplementor.CreateClonerFor(solutionDescriptor, gizmoClassLoader);
        }
    }
}
