using System;
using System.Collections.Generic;
using System.Linq;
using TEngine;

namespace GameLogic
{
    
    //类型安全
    //方便自动解除
    public interface IEvent
    {
    }
    
    public struct PriorityEventHandleEnd:IEvent{}

    //游戏中事件派发器
    //类型安全
    //订阅事件返回Listener 便于统一取消订阅，避免手动去逐个订阅造成意外漏调错调
    //支持事件订阅者按照优先级链式处理事件内容
    public static class EventBus
    {
        public class Listener
        {
            public readonly Type eventType;
            public readonly Delegate eventHandler;

            public Listener(Type inEventType, Delegate inEventHandler)
            {
                eventType = inEventType;
                eventHandler = inEventHandler;
            }
        }

        private static Dictionary<Type, Delegate> handlerMap;
        private static Dictionary<Type, IEventHandlerScheduler> priorityHandlerMap;

        static EventBus()
        {
            handlerMap = new Dictionary<Type, Delegate>();
            priorityHandlerMap = new Dictionary<Type, IEventHandlerScheduler>();
        }

        public static Listener Subscribe<T>(Action<T> action) where T : IEvent
        {
            Type eventType = typeof(T);
            if (handlerMap.ContainsKey(eventType))
            {
                var handle = handlerMap[eventType] as Action<T>;
                if (handle != null && handle.GetInvocationList().Contains(action))
                {
                    return null;
                }
                 
                handle += action;
                handlerMap[eventType] = handle;
            }
            else
            {
                handlerMap.Add(typeof(T), action);
            }

            var listener = new Listener(eventType, action);

            return listener;
        }
        
        public static Listener Subscribe<T>(Action<Action,T, IEventHandlerScheduler> action, int priority) where T : IEvent
        {
            Type eventType = typeof(T);
            if (priorityHandlerMap.ContainsKey(eventType))
            {
                var scheduler = priorityHandlerMap[eventType] as EventHandlerScheduler<T>;
                if (scheduler == null)
                {
                    return null;
                }
             
                scheduler.AddingHandler(action, priority);
                
              //  handle += action;
            }
            else
            {
                var scheduler = new EventHandlerScheduler<T>();
                priorityHandlerMap.Add(typeof(T), scheduler);
                scheduler.AddingHandler(action,priority);
                
            }

            var listener = new Listener(eventType, action);

            return listener;
        }

        public static void UnSubscribe(Listener listener)
        {
            if(listener == null)
                return;
            
            if (handlerMap.ContainsKey(listener.eventType))
            {
                var handle = handlerMap[listener.eventType];
                if (handle.GetInvocationList().Contains(listener.eventHandler))
                {
                    handle = Delegate.Remove(handle, listener.eventHandler);
                    
                    if (handle == null)
                    {
                        handlerMap.Remove(listener.eventType);
                        return;
                    }
                    
                    handlerMap[listener.eventType] = handle;
                }
            }

            if (priorityHandlerMap.ContainsKey(listener.eventType))
            {
                var scheduler = priorityHandlerMap[listener.eventType];
                scheduler.RemoveHandler(listener.eventHandler);
            }
        }

        public static void Dispatch<T>(T eventData = default) where T : IEvent
        {
            if (handlerMap.ContainsKey(eventData.GetType()))
            {
                var handle = handlerMap[eventData.GetType()] as Action<T>;
                Log.Info(eventData.GetType().Name);
                handle?.Invoke(eventData);
            }
        }

        public static void Dispatch<T>(T eventData, Action handleEndAction, bool fireToNormal = false) where T : IEvent
        {
            //同时向不带优先级的handler也发送Event
            if (fireToNormal && handlerMap.ContainsKey(eventData.GetType()))
            {
                var handle = handlerMap[eventData.GetType()] as Action<T>;
                Log.Info(eventData.GetType().Name);
                handle?.Invoke(eventData);
            }
            
            if (priorityHandlerMap.ContainsKey(eventData.GetType()))
            {
                Log.Info(eventData.GetType().Name);
                var scheduler = priorityHandlerMap[eventData.GetType()] as EventHandlerScheduler<T>;
                scheduler?.Schedule(eventData, handleEndAction);
            }
            else
            {
                handleEndAction?.Invoke();
            }
        }

        public static bool IsHandlePriorityEvent(Type iEventType)
        {
            if (priorityHandlerMap != null)
            {
                foreach (var kv in priorityHandlerMap)
                {
                    var eventType =  kv.Value.GetType().GetGenericArguments()[0];
                    if (eventType == iEventType)
                    {
                        return kv.Value.IsHandleEvent();
                    }
                }
            }
            
            return false;
        }

        
        public static bool IsHandlePriorityEvent()
        {
            if (priorityHandlerMap != null)
            {
                foreach (var kv in priorityHandlerMap)
                {
                    var eventType =  kv.Value.GetType().GetGenericArguments()[0];
                    if (kv.Value.IsHandleEvent())
                    {
                        return true;
                    }
                }
            }
            
            return false;
        }
        
        public static void Reset()
        {
            handlerMap = new Dictionary<Type, Delegate>();
            priorityHandlerMap = new Dictionary<Type, IEventHandlerScheduler>();
        }
    }
}