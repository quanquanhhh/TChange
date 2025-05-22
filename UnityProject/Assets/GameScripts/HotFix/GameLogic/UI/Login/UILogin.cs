using GameLogic;
using GameLogic.GameFSM;
using GameScripts.Net;
using Unity.Passport.Runtime;
using UnityEngine;
using UnityEngine.UI;

namespace GameScripts.UI.Login
{
    [Window(UILayer.Top, "UILogin", true)]
    public class UILogin : UIWindow
    {

        [UIBinder("LoginBtn")]
        private Button LoginBtn;

        protected override void OnCreate()
        {
            base.OnCreate();
            Debug.Log(" loginbtn == null ?" + (LoginBtn == null));
        }

        public override void AddBtnListener()
        {
            base.AddBtnListener();
            LoginBtn.onClick.AddListener(ToLoading);
        }

        private async void ToLoading()
        {
            var userId = PassportSDK.CurrentPersona.UserID;
            Debug.Log($"currentUserId, use as save name: {userId}");
            await NetworkManager.Instance.Login(userId);
            Debug.Log($"ToLoading: {userId}");
            GameApp.Get<GameFsm>().ToState<GameStateLoading>();
            Close();
        }
    }
}