using TimefoldSharp.Core.API.Score.Stream.Uni;
using TimefoldSharp.Core.Config.Solver;
using TimefoldSharp.Core.Constraints.Streams.Bavet.Common;
using TimefoldSharp.Core.Constraints.Streams.Bavet.Uni;
using TimefoldSharp.Core.Constraints.Streams.Common;
using TimefoldSharp.Core.Impl.Domain.ConstraintWeight.Descriptor;
using TimefoldSharp.Core.Impl.Domain.Entity.Descriptor;
using TimefoldSharp.Core.Impl.Domain.Solution.Descriptor;

namespace TimefoldSharp.Core.Constraints.Streams.Bavet
{
    public sealed class BavetConstraintFactory : InnerConstraintFactory<BavetConstraint>
    {
        private readonly SolutionDescriptor solutionDescriptor;
        private readonly EnvironmentMode environmentMode;
        private readonly string defaultConstraintPackage;

        private readonly Dictionary<BavetAbstractConstraintStream, BavetAbstractConstraintStream> sharingStreamMap = new Dictionary<BavetAbstractConstraintStream, BavetAbstractConstraintStream>();


        public BavetConstraintFactory(SolutionDescriptor solutionDescriptor, EnvironmentMode environmentMode)
        {

            //throw new NotImplementedException();
            this.solutionDescriptor = solutionDescriptor;
            this.environmentMode = environmentMode;
            ConstraintConfigurationDescriptor configurationDescriptor = solutionDescriptor.GetConstraintConfigurationDescriptor();
            if (configurationDescriptor == null)
            {
                var pack = solutionDescriptor.SolutionClass.Namespace;
                this.defaultConstraintPackage = pack == null ? "" : pack;
            }
            else
            {
                throw new NotImplementedException();
                //this.defaultConstraintPackage = configurationDescriptor.getConstraintPackage();
            }

        }

        public override UniConstraintStream<A> ForEach<A>(Type sourceClass)
        {
            Func<A, bool> nullityFilter = GetNullityFilter<A>(sourceClass);
            return Share(new BavetForEachUniConstraintStream<A>(this, sourceClass, nullityFilter, RetrievalSemantics.STANDARD));
        }

        public Stream_ Share<Stream_>(Stream_ stream) where Stream_ : BavetAbstractConstraintStream
        {
            return Share(stream, t => { });
        }

        public Stream_ Share<Stream_>(Stream_ stream, Action<Stream_> consumer) where Stream_ : BavetAbstractConstraintStream
        {
            return (Stream_)ComputeIfAbsent<BavetAbstractConstraintStream, BavetAbstractConstraintStream>(sharingStreamMap, stream, k =>
            {
                consumer(stream);
                return stream;
            });
        }

        V ComputeIfAbsent<K, V>(Dictionary<K, V> dict, K key, Func<K, V> generator)
        {
            bool exists = dict.TryGetValue(key, out var value);
            if (exists)
            {
                return value;
            }
            var generated = generator(key);
            dict.Add(key, generated);
            return generated;
        }

        private Func<A, bool> GetNullityFilter<A>(Type fromClass)
        {
            EntityDescriptor entityDescriptor = GetSolutionDescriptor().FindEntityDescriptor(fromClass);
            if (entityDescriptor != null && entityDescriptor.HasAnyGenuineVariables())
            {
                return (Func<A, bool>)entityDescriptor.GetHasNoNullVariables<A>();
            }
            return null;
        }

        public string GetDefaultConstraintPackage()
        {
            return defaultConstraintPackage;
        }


        public override SolutionDescriptor GetSolutionDescriptor()
        {
            return solutionDescriptor;
        }
    }
}
