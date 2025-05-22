using GameScripts;
using TMPro;

namespace GameLogic
{
    public class UIPlayerInfo : UIWidget
    {
        [UIBinder("UserID")]
        private TextMeshProUGUI userID;
        [UIBinder("Diamond")]
        private TextMeshProUGUI diamonCnt;
        [UIBinder("Coin")]
        private TextMeshProUGUI coinCnt;
        [UIBinder("Level")]
        private TextMeshProUGUI lv;

        protected override void OnCreate()
        {
            base.OnCreate();
            SetAllInfo();
        }

        protected override void RegisterEvent()
        {
            base.RegisterEvent();
            EventBus.Subscribe<UpdateAccountInfo>(OnUpdateAccountInfo);
        }

        private void OnUpdateAccountInfo(UpdateAccountInfo obj)
        {
            SetAllInfo();
        }

        public void SetAllInfo()
        {
            userID.SetText($"name : {UserInfo.user.Nickname}");
            diamonCnt.SetText($"diamonds {UserInfo.user.Diamonds}");
            userID.SetText($"coins {UserInfo.user.Coins}");
            lv.SetText($"lv. {UserInfo.user.Lv}");
            
        }
    }
}