using TEngine;
using UnityEngine;
using UnityEngine.Events;

namespace GameLogic
{
    public class UpdateModule : Singleton<UpdateModule> ,IModule
    {
        
        private static UnityAction updateAction;
        private static UnityAction secondUpdateAction;
        private static UnityAction halfSecondUpdateAction;

        private static UnityAction lateUpdateAction;
        private static UnityAction fixedUpdateAction;

        private float halfSecondIntervalUpdaterNextActionTime;
        private float secondIntervalUpdaterNextActionTime;

        public void InitializeModule()
        {
            Utility.Unity.AddUpdateListener(Instance.Update);
            Utility.Unity.AddFixedUpdateListener(Instance.FixedUpdate);
            Utility.Unity.AddLateUpdateListener(Instance.LateUpdate);
            Utility.Unity.AddOnApplicationPauseListener(Instance.OnApplicationPause);
#if UNITY_ANDROID
            Utility.Unity.AddOnApplicationQuitListener(Instance.OnApplicationQuit);      
#endif
            //   Utility.Unity.AddDestroyListener(Instance.OnDestroy);
        }

        public void OnStartGameLogic()
        {
            
        }

        public void OnShutDown()
        {
            Utility.Unity.RemoveUpdateListener(Instance.Update);
            Utility.Unity.RemoveFixedUpdateListener(Instance.FixedUpdate);
            Utility.Unity.RemoveUpdateListener(Instance.LateUpdate);
            Utility.Unity.RemoveOnApplicationPauseListener(Instance.OnApplicationPause);
           
            updateAction = null;
            secondUpdateAction = null;
            halfSecondUpdateAction = null;
            lateUpdateAction = null;
            fixedUpdateAction = null;
             
          //  Utility.Unity.RemoveDestroyListener(Instance.OnDestroy);
        }
       
        public void Update()
        {
          
            if (updateAction != null)
            {
                updateAction.Invoke();
            }

            if (Time.time > secondIntervalUpdaterNextActionTime)
            {
                secondIntervalUpdaterNextActionTime = Time.time + 1.0f;

                if (secondUpdateAction != null)
                {
                    secondUpdateAction.Invoke();
                }
            }

            if (Time.time > halfSecondIntervalUpdaterNextActionTime)
            {
                halfSecondIntervalUpdaterNextActionTime = Time.time + 0.5f;

                if (halfSecondUpdateAction != null)
                {
                    halfSecondUpdateAction?.Invoke();
                }
            }
        }

        public void LateUpdate()
        {
            if (lateUpdateAction == null)
                return;
            lateUpdateAction.Invoke();
        }

        public void FixedUpdate()
        {
            if (fixedUpdateAction == null)
                return;
            fixedUpdateAction?.Invoke();
        }

        
        private void OnApplicationPause(bool isPause)
        {
            EventBus.Dispatch(new EventOnApplicationPause(isPause));
        }

        private void OnApplicationQuit()
        {
        }
        
        #region Hook Update

        public static void HookUpdate(UnityAction updateable)
        {
            updateAction += updateable;
        }

        public static void HookSecondUpdate(UnityAction updateable)
        {
            secondUpdateAction += updateable;
        }

        public static void HookHalfSecondUpdate(UnityAction updateable)
        {
            halfSecondUpdateAction += updateable;
        }

        public static void UnhookUpdate(UnityAction updateable)
        {
            updateAction -= updateable;
            secondUpdateAction -= updateable;
            halfSecondUpdateAction -= updateable;
        }

        public static void HookFixedUpdate(UnityAction fixedUpdateable)
        {
            fixedUpdateAction += fixedUpdateable;
        }

        public static void UnhookFixedUpdate(UnityAction fixedUpdateable)
        {
            fixedUpdateAction -= fixedUpdateable;
        }

        public static void HookLateUpdate(UnityAction lateUpdateable)
        {
            lateUpdateAction += lateUpdateable;
        }

        public static void UnhookLateUpdate(UnityAction lateUpdateable)
        {
            lateUpdateAction -= lateUpdateable;
        }

        #endregion
    }
}