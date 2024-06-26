using System.Collections;
using System.Collections.Concurrent;
using System.Reflection;
using TimefoldSharp.Core.API.Domain.Solution;
using TimefoldSharp.Core.API.Score;
using TimefoldSharp.Core.Impl.Domain.Solution.Descriptor;

namespace TimefoldSharp.Core.Impl.Domain.Solution.Cloner
{
    public sealed class FieldAccessingSolutionCloner : SolutionCloner
    {
        private readonly SolutionDescriptor solutionDescriptor;
        private readonly ConcurrentDictionary<Type, ClassMetadata> classMetadataMemoization = new ConcurrentDictionary<Type, ClassMetadata>();
        private readonly ConcurrentDictionary<Type, ConstructorInfo> constructorMemoization = new ConcurrentDictionary<Type, ConstructorInfo>();



        public ISolution CloneSolution(ISolution originalSolution)
        {
            int entityCount = solutionDescriptor.GetEntityCount(originalSolution);
            Dictionary<object, object> originalToCloneMap = new Dictionary<object, object>(entityCount + 1);
            Queue<Unprocessed> unprocessedQueue = new Queue<Unprocessed>(entityCount + 1);
            ISolution cloneSolution = Clone(originalSolution, originalToCloneMap, unprocessedQueue, RetrieveClassMetadata(originalSolution.GetType()));
            while (unprocessedQueue.Count > 0)
            {
                Unprocessed unprocessed = unprocessedQueue.Dequeue();
                object cloneValue = Process(unprocessed, originalToCloneMap, unprocessedQueue);
                FieldCloningUtils.SetObjectFieldValue(unprocessed.Bean, unprocessed.Property, cloneValue);
            }
            return cloneSolution;
        }

        private ClassMetadata RetrieveClassMetadata(Type declaringClass)
        {
            return classMetadataMemoization.GetOrAdd(declaringClass, new ClassMetadata(declaringClass, solutionDescriptor));
        }

        private object Process(Unprocessed unprocessed, Dictionary<object, object> originalToCloneMap, Queue<Unprocessed> unprocessedQueue)
        {
            Object originalValue = unprocessed.OriginalValue;
            PropertyInfo field = unprocessed.Property;
            Type fieldType = field.GetType();
            if (originalValue is IList collection)
            {
                return CloneCollection(fieldType, collection, originalToCloneMap, unprocessedQueue);
            }/* else if (originalValue is Dictionary map) 
            {
                return CloneMap(fieldType, map, originalToCloneMap, unprocessedQueue);
            } else if (originalValue.GetType().IsArray)
            {
                return CloneArray(fieldType, originalValue, originalToCloneMap, unprocessedQueue);
            }*/
            else
            {
                return Clone(originalValue, originalToCloneMap, unprocessedQueue,
                        RetrieveClassMetadata(originalValue.GetType()));
            }
        }

        private object CloneCollection(Type expectedType, IList originalCollection, Dictionary<object, object> originalToCloneMap, Queue<Unprocessed> unprocessedQueue)
        {
            var listType = typeof(List<>);
            var type = originalCollection.GetType();
            if (!type.IsGenericType)
                throw new Exception("JDEF");

            var genType = type.GenericTypeArguments[0];
            var constructedListType = listType.MakeGenericType(genType);
            var instance = (IList)Activator.CreateInstance(constructedListType);

            foreach (var originalElement in originalCollection)
            {
                var cloneElement = CloneCollectionsElementIfNeeded(originalElement, originalToCloneMap, unprocessedQueue);
                instance.Add(cloneElement);
            }
            return instance;
        }

        private object CloneCollectionsElementIfNeeded(object original, Dictionary<Object, Object> originalToCloneMap, Queue<Unprocessed> unprocessedQueue)
        {
            if (original == null)
            {
                return null;
            }
            // Because an element which is itself a Collection or Map might hold an entity, we clone it too
            // Also, the List<Long> in Map<String, List<Long>> needs to be cloned
            // if the List<Long> is a shadow, despite that Long never needs to be cloned (because it's immutable).
            if (original is IList collection)
            {
                return CloneCollection(typeof(List<>), collection, originalToCloneMap, unprocessedQueue);

            }
            else if (original.GetType().IsArray)
            {
                return CloneArray(original.GetType(), (Array)original, originalToCloneMap, unprocessedQueue);
            }
            ClassMetadata classMetadata = RetrieveClassMetadata(original.GetType());
            if (classMetadata.IsDeepCloned)
            {
                return Clone(original, originalToCloneMap, unprocessedQueue, classMetadata);
            }
            else
            {
                return original;
            }
        }

        private object CloneArray(Type expectedType, Array originalArray, Dictionary<object, object> originalToCloneMap, Queue<Unprocessed> unprocessedQueue)
        {
            int arrayLength = originalArray.Length;
            var cloneArray = Array.CreateInstance(originalArray.GetType().GetElementType(), arrayLength);

            for (int i = 0; i < arrayLength; i++)
            {
                object cloneElement = CloneCollectionsElementIfNeeded(originalArray.GetValue(i), originalToCloneMap, unprocessedQueue);
                cloneArray.SetValue(cloneElement, i);
            }
            return cloneArray;
        }

        private C Clone<C>(C original, Dictionary<object, object> originalToCloneMap, Queue<Unprocessed> unprocessedQueue, ClassMetadata declaringClassMetadata)
            where C : class
        {
            if (original == null)
            {
                return null;
            }
            object existingClone;
            originalToCloneMap.TryGetValue(original, out existingClone);

            if (existingClone != null)
            {
                return (C)existingClone;
            }
            Type declaringClass = original.GetType();
            C clone = ConstructClone<C>(declaringClass);
            originalToCloneMap.Add(original, clone);
            CopyFields(declaringClass, original, clone, unprocessedQueue, declaringClassMetadata);
            return clone;
        }

        private C ConstructClone<C>(Type clazz)
        {
            try
            {
                return (C)constructorMemoization.GetOrAdd(clazz, key =>
                {
                    try
                    {
                        ConstructorInfo constructor = key.GetConstructor(Type.EmptyTypes);
                        return constructor;
                    }
                    catch (Exception e)
                    {
                        throw new Exception("The class (" + key
                                + ") should have a no-arg constructor to create a planning clone.", e);
                    }
                }).Invoke(new object[] { });
            }
            catch (Exception e)
            {
                throw new Exception("The class (" + clazz
                        + ") should have a no-arg constructor to create a planning clone.", e);
            }
        }



        private void CopyFields<C>(Type clazz, C original, C clone, Queue<Unprocessed> unprocessedQueue, ClassMetadata declaringClassMetadata)
        {
            foreach (var fieldCloner in declaringClassMetadata.GetCopiedFieldArray())
            {
                fieldCloner.Clone(original, clone);
            }
            foreach (var fieldCloner in declaringClassMetadata.GetClonedFieldArray())
            {
                object unprocessedValue = fieldCloner.Clone(solutionDescriptor, original, clone);
                if (unprocessedValue != null)
                {
                    unprocessedQueue.Enqueue(new Unprocessed(clone, fieldCloner.GetField(), unprocessedValue));
                }
            }
            Type superclass = clazz.BaseType;
            if (superclass != null && superclass != typeof(object))
            {
                CopyFields(superclass, original, clone, unprocessedQueue, RetrieveClassMetadata(superclass));
            }
        }


        public FieldAccessingSolutionCloner(SolutionDescriptor solutionDescriptor)
        {
            this.solutionDescriptor = solutionDescriptor;
        }

        public sealed class Unprocessed
        {

            public object Bean { get; set; }
            public PropertyInfo Property { get; set; }
            public Object OriginalValue { get; set; }

            public Unprocessed(Object bean, PropertyInfo property, Object originalValue)
            {
                this.Bean = bean;
                this.Property = property;
                this.OriginalValue = originalValue;
            }

        }

        protected sealed class ClassMetadata
        {
            public readonly Type DeclaringClass;
            public readonly bool IsDeepCloned;
            SolutionDescriptor solutionDescriptor;

            private ShallowCloningFieldCloner[] copiedFieldArray;
            private DeepCloningFieldCloner[] clonedFieldArray;

            public ClassMetadata(Type declaringClass, SolutionDescriptor solutionDescriptor)
            {
                this.DeclaringClass = declaringClass;
                this.solutionDescriptor = solutionDescriptor;
                this.IsDeepCloned = DeepCloningUtils.IsClassDeepCloned(solutionDescriptor, declaringClass);

            }

            public ShallowCloningFieldCloner[] GetCopiedFieldArray()
            {
                if (copiedFieldArray == null)
                { // Lazy-loaded; some types (such as String) will never get here.
                    copiedFieldArray = DeclaringClass.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                        .Where(field => DeepCloningUtils.IsImmutable(field.GetType()))
                        .Select(f => ShallowCloningFieldCloner.Of(f))
                        .ToArray();
                }
                return copiedFieldArray;
            }

            internal DeepCloningFieldCloner[] GetClonedFieldArray()
            {
                if (clonedFieldArray == null)
                { // Lazy-loaded; some types (such as String) will never get here.
                    clonedFieldArray = DeclaringClass.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                            .Where(field => !DeepCloningUtils.IsImmutable(field.GetType()))
                            .Select(f => new DeepCloningFieldCloner(f))
                            .ToArray();
                }
                return clonedFieldArray;
            }
        }

    }
}
