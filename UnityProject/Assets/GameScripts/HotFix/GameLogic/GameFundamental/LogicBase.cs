using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameLogic
{
    public class LogicBase
    {
        #region Events
        
        protected List<EventBus.Listener> listeners;
        protected bool updateEnabled = false;
        protected bool lateUpdateEnabled = false;
         /// <summary>
        /// 订阅游戏内事件
        /// </summary>
        /// <param name="handleAction"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public bool SubscribeEvent<T>(Action<T> handleAction) where T:IEvent
        {
            if (listeners == null)
            {
                listeners = new List<EventBus.Listener>();
            }
            
            var listener = EventBus.Subscribe<T>(handleAction);
            
            if (listener != null)
            {
                listeners.Add(listener);
                return true;
            }
            return false;
        }
         
        /// <summary>
        /// 订阅游戏内有先后优先级的事件
        /// </summary>
        /// <param name="handleAction"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public bool SubscribeEvent<T>(Action<Action, T, IEventHandlerScheduler> handleAction, int priority) where T : IEvent
        {
            if (listeners == null)
            {
                listeners = new List<EventBus.Listener>();
            }
            
            var listener = EventBus.Subscribe<T>(handleAction, priority);
            
            if (listener != null)
            {
                listeners.Add(listener);
                return true;
            }
            return false;
        }
        
        /// <summary>
        /// 取消订阅事件
        /// </summary>
        /// <param name="handleAction"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public bool UnsubscribeEvent<T>(Action<Action, T, IEventHandlerScheduler> handleAction) where T : IEvent
        {
            if (listeners == null)
                return false;

            for (var i = 0; i < listeners.Count; i++)
            {
                if (listeners[i].eventHandler == (Delegate) handleAction)
                {
                    EventBus.UnSubscribe(listeners[i]);
                    listeners.RemoveAt(i);
                    return true;
                }
            }
            
            return false;
        }
        
        /// <summary>
        /// 取消事件订阅
        /// </summary>
        /// <param name="handleAction"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public bool UnsubscribeEvent<T>(Action<T> handleAction) where T:IEvent
        {
            if (listeners == null)
                return false;

            for (var i = 0; i < listeners.Count; i++)
            {
                if (listeners[i].eventHandler == (Delegate) handleAction)
                {
                    EventBus.UnSubscribe(listeners[i]);
                    listeners.RemoveAt(i);
                    return true;
                }
            }
            
            return false;
        }
        
        public void CleanAllSubscribedEvents()
        {
            if (listeners == null)
                return;
            for (var i = 0; i < listeners.Count; i++)
            {
                EventBus.UnSubscribe(listeners[i]);
            }
            
            listeners.Clear();
        }
        
        #endregion
        
        
        public virtual void InitializeOnCreate()
        {
            SubscribeEvents();
        }

        public virtual void OnStart()
        {
            
        }
        
        public virtual void OnLogicStart()
        {
            
        }
        
        public virtual void OnCoreAssetsReady()
        {
            
        }

        public virtual void OnConfigLoadReady()
        {
            
        }

        public virtual void OnLoginSuccess()
        {
            
        }

        public virtual void SubscribeEvents()
        {
            
        }
        
        /// <summary>
        /// 0:0.9f ,1:0.5f, 2:every frame
        /// </summary>
        /// <param name="updateInterval"></param>
        public void EnableUpdate(int updateInterval = 0)
        {
            if (updateEnabled)
            {
                DisableUpdate();
            } 
            if (updateInterval == 0)
                UpdateModule.HookSecondUpdate(Update);
            else if (updateInterval == 1)
                UpdateModule.HookHalfSecondUpdate(Update);
            else
                UpdateModule.HookUpdate(Update);
            updateEnabled = true;
        }
        
        public void DisableUpdate()
        {
            if (updateEnabled)
            {
                UpdateModule.UnhookUpdate(Update);
                updateEnabled = false;
            }
        }

        public virtual void Update()
        {
            
        }
        
        public void EnableLateUpdate()
        {
            if (lateUpdateEnabled)
            {
                DisableLateUpdate();
            } 
            UpdateModule.HookLateUpdate(LateUpdate);
            lateUpdateEnabled = true;
        }
        
        public void DisableLateUpdate()
        {
            if (lateUpdateEnabled)
            {
                UpdateModule.UnhookLateUpdate(LateUpdate);
                lateUpdateEnabled = false;
            }
        }

        public virtual void LateUpdate()
        {
            
        }

        public virtual void OnShutDown()
        {
            DisableUpdate();
            CleanAllSubscribedEvents();
        }
        public virtual int GetLevelDifficulty(int levelId)
        {
            return 1;
        }
        public virtual int GetMainLevel()
        {
            return 0;
        }
    }
}