namespace Core.Events
{
    public abstract class EventBase
    {
        // ������������ӹ�ͬ���¼�����
        public float TimeStamp { get; private set; }

        protected EventBase()
        {
            TimeStamp = UnityEngine.Time.time;
        }
    }
}