using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Events
{
    /// <summary>
    /// 简单的全局事件总线（非线程安全）。
    /// - 使用事件类型作为键，存储该类型事件的订阅者列表。
    /// - 订阅者以 Action<EventBase> 形式保存，发布时会将 EventBase 强制转换回具体类型。
    /// 注意：此实现假定在主线程（Unity 主线程）中调用；如果需要跨线程使用，需增加同步/锁或改为线程安全的数据结构。
    /// </summary>
    public class EventBus
    {
        // 单例实例（延迟初始化）
        private static EventBus instance;
        /// <summary>
        /// 获取全局唯一 EventBus 实例（延迟创建）。
        /// </summary>
        public static EventBus Instance
        {
            get
            {
                // C# 8 写法：若 instance 为 null 则创建新实例
                instance ??= new EventBus();
                return instance;
            }
        }

        // 订阅者字典：事件类型 -> 该类型事件的处理器列表
        // 存储为 Action<EventBase> 便于统一调用（发布时会进行强制类型转换）
        private readonly Dictionary<Type, List<Action<EventBase>>> subscribers 
            = new Dictionary<Type, List<Action<EventBase>>>();

        // 私有构造函数，确保只能通过 Instance 获取单例
        private EventBus() { }

        /// <summary>
        /// 订阅指定类型的事件。
        /// handler 的签名应为 (T evt) => { ... }，其中 T : EventBase
        /// 内部将把具体类型的 handler 包装为 Action<EventBase> 并保存。
        /// </summary>
        public void Subscribe<T>(Action<T> handler) where T : EventBase
        {
            var eventType = typeof(T);
            if (!subscribers.ContainsKey(eventType))
            {
                // 首次订阅该类型时创建处理器列表
                subscribers[eventType] = new List<Action<EventBase>>();
            }
            
            // 将具体类型的 handler 包装为通用的 Action<EventBase>
            subscribers[eventType].Add((e) => handler((T)e));
        }

        /// <summary>
        /// 取消订阅指定类型的事件。
        /// 使用 Target 和 Method 比较以匹配原始 handler（对实例方法和静态方法都有效）。
        /// 如果找不到对应的订阅项则静默返回。
        /// </summary>
        public void Unsubscribe<T>(Action<T> handler) where T : EventBase
        {
            var eventType = typeof(T);
            if (!subscribers.ContainsKey(eventType)) return;

            // 根据 Target（实例对象）和 Method（方法信息）匹配并移除相关包装的委托
            subscribers[eventType].RemoveAll(
                existing => existing.Target == handler.Target && 
                           existing.Method == handler.Method);
        }

        /// <summary>
        /// 发布事件到所有订阅者。
        /// 若没有订阅者则直接返回。对每个订阅者进行调用并捕获异常，保证不会因为某个订阅者异常影响其他订阅者。
        /// </summary>
        public void Publish<T>(T eventData) where T : EventBase
        {
            var eventType = typeof(T);
            if (!subscribers.ContainsKey(eventType)) return;

            var handlers = subscribers[eventType];
            // 直接遍历当前列表并逐个调用。若需要在处理过程中允许订阅/取消订阅安全修改集合，
            // 可考虑复制一份 handlers 到临时列表（handlers.ToArray()），以避免枚举期间修改集合抛异常。
            foreach (var handler in handlers)
            {
                try
                {
                    handler.Invoke(eventData);
                }
                catch (Exception e)
                {
                    // 记录具体事件类型的处理错误，不中断其他处理器
                    Debug.LogError($"Error handling event {eventType.Name}: {e}");
                }
            }
        }

        /// <summary>
        /// 清空所有订阅者（例如场景切换或重置时可调用）。
        /// </summary>
        public void Clear()
        {
            subscribers.Clear();
        }
    }
}