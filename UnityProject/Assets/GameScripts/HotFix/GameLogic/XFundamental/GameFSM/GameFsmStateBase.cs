using System.Threading.Tasks;
using TEngine;

namespace GameLogic.GameFSM
{
    public class GameFsmStateBase 
    {
        protected object[] enterParams;
        
        /// <summary>
        /// 状态初始化时调用。
        /// </summary>
        /// <param name="procedureOwner">流程持有者。</param>
        public virtual void OnInit()
        {
            
        }

        /// <summary>
        /// 进入状态时调用。
        /// </summary>
        /// <param name="inEnterParams">流程持有者。</param>
        public virtual void OnEnter(params object[] inEnterParams)
        {
            enterParams = inEnterParams;
        }

        /// <summary>
        /// 状态轮询时调用。
        /// </summary>
        /// <param name="procedureOwner">流程持有者。</param>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
        /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
        public  virtual void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
        
        }
        
        public virtual async Task<bool> PreOnEnter(GameFsmStateBase lastGameState)
        {
            return true;
        }

        public virtual async Task<bool> PreOnLeave(GameFsmStateBase nextState)
        {
            return true;
        }

        /// <summary>
        /// 离开状态时调用。
        /// </summary>
        public  virtual void OnLeave()
        {
        
        }
    }
}