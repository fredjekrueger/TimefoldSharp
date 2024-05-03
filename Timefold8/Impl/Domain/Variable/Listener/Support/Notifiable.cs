namespace TimefoldSharp.Core.Impl.Domain.Variable.Listener.Support
{
    public interface Notifiable
    {
        void ResetWorkingSolution();
        void TriggerAllNotifications();
        void CloseVariableListener();
    }
}
