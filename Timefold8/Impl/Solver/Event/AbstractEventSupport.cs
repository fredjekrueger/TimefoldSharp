using TimefoldSharp.Core.Helpers;

namespace TimefoldSharp.Core.Impl.Solver.Event
{
    public class AbstractEventSupport<E> where E : class, EventListenerTF
    {
        private readonly List<E> eventListenerList = new List<E>();

        protected List<E> GetEventListeners()
        {
            return eventListenerList;
        }

        public void AddEventListener(E eventListener)
        {
            foreach (var addedEventListener in eventListenerList)
            {
                if (addedEventListener == eventListener)
                {
                    throw new Exception(
                            "Event listener (" + eventListener + ") already found in list (" + eventListenerList + ").");
                }
            }
            eventListenerList.Add(eventListener);
        }

        public void RemoveEventListener(E eventListener)
        {
            if (!eventListenerList.Remove(eventListener))
            {
                throw new Exception(
                        "Event listener (" + eventListener + ") not found in list (" + eventListenerList + ").");
            }
        }
    }
}
