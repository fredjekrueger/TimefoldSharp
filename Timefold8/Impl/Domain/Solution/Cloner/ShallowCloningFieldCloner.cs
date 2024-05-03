using System.Reflection;

namespace TimefoldSharp.Core.Impl.Domain.Solution.Cloner
{
    public sealed class ShallowCloningFieldCloner
    {
        public static ShallowCloningFieldCloner Of(PropertyInfo field)
        {
            Type fieldType = field.GetType();
            if (fieldType == typeof(bool))
            {
                return new ShallowCloningFieldCloner(field, FieldCloningUtils.CopyField<bool>);
            }
            else if (fieldType == typeof(byte))
            {
                return new ShallowCloningFieldCloner(field, FieldCloningUtils.CopyField<byte>);
            }
            else if (fieldType == typeof(char))
            {
                return new ShallowCloningFieldCloner(field, FieldCloningUtils.CopyField<char>);
            }
            else if (fieldType == typeof(short))
            {
                return new ShallowCloningFieldCloner(field, FieldCloningUtils.CopyField<short>);
            }
            else if (fieldType == typeof(int))
            {
                return new ShallowCloningFieldCloner(field, FieldCloningUtils.CopyField<int>);
            }
            else if (fieldType == typeof(long))
            {
                return new ShallowCloningFieldCloner(field, FieldCloningUtils.CopyField<long>);
            }
            else if (fieldType == typeof(float))
            {
                return new ShallowCloningFieldCloner(field, FieldCloningUtils.CopyField<float>);
            }
            else if (fieldType == typeof(double))
            {
                return new ShallowCloningFieldCloner(field, FieldCloningUtils.CopyField<double>);
            }
            else
            {
                return new ShallowCloningFieldCloner(field, FieldCloningUtils.CopyField<object>);
            }
        }

        internal void Clone<C>(C original, C clone)
        {
            throw new NotImplementedException();
        }

        public ShallowCloningFieldCloner(PropertyInfo field, Action<PropertyInfo, object, object> copyField)
        {
            this.field = field;
        }


        private readonly PropertyInfo field;


    }
}
