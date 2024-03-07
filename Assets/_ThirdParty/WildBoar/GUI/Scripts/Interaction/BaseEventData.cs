using UnityEngine;

namespace WildBoar.GUIModule
{
    public abstract class AbstractEventData
    {
        protected bool m_Used;

        public virtual void Reset()
        {
            m_Used = false;
        }

        public virtual void Use()
        {
            m_Used = true;
        }

        public virtual bool used
        {
            get { return m_Used; }
        }
    }

    public class BaseEventData : AbstractEventData
    {
        private readonly EventSystem m_EventSystem;
        public BaseEventData(EventSystem eventSystem)
        {
            m_EventSystem = eventSystem;
        }

        public GameObject selectedObject
        {
            //TODO turn static filed into instance
            get { return EventSystem.currentSelectedGameObject; }
            set { EventSystem.currentSelectedGameObject = value; }
        }
    }
}
