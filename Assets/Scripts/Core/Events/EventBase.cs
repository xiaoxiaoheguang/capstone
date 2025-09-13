namespace Core.Events
{
    public abstract class EventBase
    {
        // 可以在这里添加共同的事件属性
        public float TimeStamp { get; private set; }

        protected EventBase()
        {
            TimeStamp = UnityEngine.Time.time;
        }
    }
}