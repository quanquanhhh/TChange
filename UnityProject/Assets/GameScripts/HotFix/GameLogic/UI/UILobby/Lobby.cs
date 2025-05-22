using GameLogic;
using GameLogic.GameFSM;
using GameScripts.Net;
using UnityEngine;
using UnityEngine.UI;

namespace GameLogic
{
    [Window(UILayer.Top, "UILobby", true)]
    public class Lobby : UIWindow
    {
        [UIBinder("GameBtn")]
        private Button GameBtn;
        [UIBinder("DecoBtn")]
        private Button DecoBtn;
        [UIBinder("PlayerInfo")]
        private GameObject playerInfo;

        [UIBinder("DebugPlayWin")]
        private Button DebugLevelWin;
        

        protected override void OnCreate()
        {
            base.OnCreate();
            
            CreateWidget<UIPlayerInfo>(playerInfo);
            DebugLevelWin.gameObject.SetActive(false);
        }

        public override void AddBtnListener()
        {
            DecoBtn.onClick.AddListener(EnterDeco);
            GameBtn.onClick.AddListener(EnterGame);
            DebugLevelWin.onClick.AddListener(OnDebugLevelWin);
            
        }
        
        private async void OnDebugLevelWin()
        {
            await NetworkManager.Instance.LevelUp();
        }
        private void EnterGame()
        {
            GameApp.Get<GameFsm>().ToState<GameStatePlaying>();
            DebugLevelWin.gameObject.SetActive(true);
        }

        private void EnterDeco()
        {
            GameApp.Get<GameFsm>().ToState<GameStateDeco>();
            DebugLevelWin.gameObject.SetActive(false);
        }
    }
}