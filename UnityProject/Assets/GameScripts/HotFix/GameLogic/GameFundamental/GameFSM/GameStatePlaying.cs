using GameScripts;
using GameScripts.Net;
using TEngine;
using UnityEngine;

namespace GameLogic.GameFSM
{
    public class GameStatePlaying : GameFsmStateBase
    {
        public override async void OnEnter(params object[] inEnterParams)
        {
            Debug.Log(" GameState - enter Playing");
            base.OnEnter(inEnterParams);
            
            Debug.Log(" GameState - AddPlayLevel = " + UserInfo.user.Lv);
        }

        public override void OnLeave()
        {
            base.OnLeave();
            Debug.Log(" GameState - Leave Playing");
        }
    }
}