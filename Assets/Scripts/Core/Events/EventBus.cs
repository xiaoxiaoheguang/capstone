using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Events
{
    /// <summary>
    /// �򵥵�ȫ���¼����ߣ����̰߳�ȫ����
    /// - ʹ���¼�������Ϊ�����洢�������¼��Ķ������б�
    /// - �������� Action<EventBase> ��ʽ���棬����ʱ�Ὣ EventBase ǿ��ת���ؾ������͡�
    /// ע�⣺��ʵ�ּٶ������̣߳�Unity ���̣߳��е��ã������Ҫ���߳�ʹ�ã�������ͬ��/�����Ϊ�̰߳�ȫ�����ݽṹ��
    /// </summary>
    public class EventBus
    {
        // ����ʵ�����ӳٳ�ʼ����
        private static EventBus instance;
        /// <summary>
        /// ��ȡȫ��Ψһ EventBus ʵ�����ӳٴ�������
        /// </summary>
        public static EventBus Instance
        {
            get
            {
                // C# 8 д������ instance Ϊ null �򴴽���ʵ��
                instance ??= new EventBus();
                return instance;
            }
        }

        // �������ֵ䣺�¼����� -> �������¼��Ĵ������б�
        // �洢Ϊ Action<EventBase> ����ͳһ���ã�����ʱ�����ǿ������ת����
        private readonly Dictionary<Type, List<Action<EventBase>>> subscribers 
            = new Dictionary<Type, List<Action<EventBase>>>();

        // ˽�й��캯����ȷ��ֻ��ͨ�� Instance ��ȡ����
        private EventBus() { }

        /// <summary>
        /// ����ָ�����͵��¼���
        /// handler ��ǩ��ӦΪ (T evt) => { ... }������ T : EventBase
        /// �ڲ����Ѿ������͵� handler ��װΪ Action<EventBase> �����档
        /// </summary>
        public void Subscribe<T>(Action<T> handler) where T : EventBase
        {
            var eventType = typeof(T);
            if (!subscribers.ContainsKey(eventType))
            {
                // �״ζ��ĸ�����ʱ�����������б�
                subscribers[eventType] = new List<Action<EventBase>>();
            }
            
            // ���������͵� handler ��װΪͨ�õ� Action<EventBase>
            subscribers[eventType].Add((e) => handler((T)e));
        }

        /// <summary>
        /// ȡ������ָ�����͵��¼���
        /// ʹ�� Target �� Method �Ƚ���ƥ��ԭʼ handler����ʵ�������;�̬��������Ч����
        /// ����Ҳ�����Ӧ�Ķ�������Ĭ���ء�
        /// </summary>
        public void Unsubscribe<T>(Action<T> handler) where T : EventBase
        {
            var eventType = typeof(T);
            if (!subscribers.ContainsKey(eventType)) return;

            // ���� Target��ʵ�����󣩺� Method��������Ϣ��ƥ�䲢�Ƴ���ذ�װ��ί��
            subscribers[eventType].RemoveAll(
                existing => existing.Target == handler.Target && 
                           existing.Method == handler.Method);
        }

        /// <summary>
        /// �����¼������ж����ߡ�
        /// ��û�ж�������ֱ�ӷ��ء���ÿ�������߽��е��ò������쳣����֤������Ϊĳ���������쳣Ӱ�����������ߡ�
        /// </summary>
        public void Publish<T>(T eventData) where T : EventBase
        {
            var eventType = typeof(T);
            if (!subscribers.ContainsKey(eventType)) return;

            var handlers = subscribers[eventType];
            // ֱ�ӱ�����ǰ�б�������á�����Ҫ�ڴ��������������/ȡ�����İ�ȫ�޸ļ��ϣ�
            // �ɿ��Ǹ���һ�� handlers ����ʱ�б�handlers.ToArray()�����Ա���ö���ڼ��޸ļ������쳣��
            foreach (var handler in handlers)
            {
                try
                {
                    handler.Invoke(eventData);
                }
                catch (Exception e)
                {
                    // ��¼�����¼����͵Ĵ�����󣬲��ж�����������
                    Debug.LogError($"Error handling event {eventType.Name}: {e}");
                }
            }
        }

        /// <summary>
        /// ������ж����ߣ����糡���л�������ʱ�ɵ��ã���
        /// </summary>
        public void Clear()
        {
            subscribers.Clear();
        }
    }
}