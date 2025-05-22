using TEngine;
using UnityEngine;

namespace GameLogic.GameFSM
{
    public class GameStateDeco : GameFsmStateBase
    {
        public override void OnEnter(params object[] inEnterParams)
        {
            base.OnEnter(inEnterParams);
            Debug.Log(" GameState - enter GameStateDeco");
        }

        public override void OnLeave()
        {
            base.OnLeave();
            Debug.Log(" GameState - Leave GameStateDeco");
        }
    }
}