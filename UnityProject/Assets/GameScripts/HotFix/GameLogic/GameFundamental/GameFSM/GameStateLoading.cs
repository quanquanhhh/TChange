using TEngine;

namespace GameLogic.GameFSM
{
    public class GameStateLoading : GameFsmStateBase
    {
        public override void OnEnter(params object[] inEnterParams)
        {
            base.OnEnter(inEnterParams);
            
            GameEvent.Get<ILoginUI>().ShowLoginUI();
            GameModule.UI.ShowUIAsync<Lobby>();
        }
    }
}