using System.Collections.ObjectModel;
using TimefoldSharp.Core.API.Score;
using TimefoldSharp.Core.API.Score.Stream;

namespace TimefoldSharp.Core.Constraints.Streams.Common.Inliner
{
    public interface ConstraintMatchSupplier
    {

    }

    public static class ConstraintMatchSupplierHelper
    {
        public static ConstraintMatchSupplier Of<A>(
           Func<A, Score, ConstraintJustification> justificationMapping,
           Func<A, Collection<Object>> indictedObjectsMapping,
           A a)
        {
            throw new NotImplementedException();
        }

        public static ConstraintMatchSupplier Of<A, B>(
            Func<A, B, Score, ConstraintJustification> justificationMapping,
            Func<A, B, Collection<object>> indictedObjectsMapping,
            A a, B b)
        {
            throw new NotImplementedException();
        }

        public static ConstraintMatchSupplier Of<A, B, C>(
                Func<A, B, C, Score, ConstraintJustification> justificationMapping,
                Func<A, B, C, Collection<object>> indictedObjectsMapping,
                A a, B b, C c)
        {
            throw new NotImplementedException();
        }

        public static ConstraintMatchSupplier Of<A, B, C, D>(
                Func<A, B, C, D, Score, ConstraintJustification> justificationMapping,
                Func<A, B, C, D, Collection<object>> indictedObjectsMapping,
                A a, B b, C c, D d)
        {
            throw new NotImplementedException();
        }
    }
}
