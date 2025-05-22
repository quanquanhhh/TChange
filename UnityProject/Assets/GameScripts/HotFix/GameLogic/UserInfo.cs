using GameLogic;
using GameScripts.Model;
using UnityEngine;

namespace GameScripts
{
    public static class UserInfo
    {
        public static Account user;
        public static string PlayerId;

        public static int CurLevel;

        public static void SetPlayId(string playerId)
        {
            PlayerId = playerId;
        }

        public static void SetCurLevel(int level)
        {
            user.Lv = level;
            EventBus.Dispatch(new  UpdateAccountInfo());
        }

        public static void SetUserInfo(Account data)
        {
            user = data;
        }
    }
}