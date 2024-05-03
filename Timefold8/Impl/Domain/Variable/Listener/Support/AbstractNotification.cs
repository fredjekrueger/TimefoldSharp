namespace TimefoldSharp.Core.Impl.Domain.Variable.Listener.Support
{
    public abstract class AbstractNotification
    {
        protected readonly object entity;

        protected AbstractNotification(object entity)
        {
            this.entity = entity;
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            throw new NotImplementedException();
        }
    }
}
