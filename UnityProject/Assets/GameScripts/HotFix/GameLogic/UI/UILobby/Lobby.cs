using GameLogic;
using UnityEngine;
using UnityEngine.UI;

namespace GameLogic
{
    [Window(UILayer.Top, "UILobby", true)]
    public class Lobby : UIWindow
    {
        [UIBinder("StartBtn")]
        private Button StartBtn;

        protected override void OnCreate()
        {
            base.OnCreate();
            Debug.Log(" has StartBtn = " + (StartBtn != null));
        }
    }
}