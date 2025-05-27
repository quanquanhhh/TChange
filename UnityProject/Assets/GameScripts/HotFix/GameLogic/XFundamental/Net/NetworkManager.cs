using System;
using System.Threading.Tasks;
using GameScripts.Model;
using UnityEngine;
using UnityEngine.Events;


namespace GameScripts.Net
{
    
    public class NetworkManager : MonoBehaviour
    {
        private UnityEvent<Account> onLogin = new UnityEvent<Account>();
        private UnityEvent<QueryResult> onLevelWin = new UnityEvent<QueryResult>();
        public UnityEvent<ActivityResult> onGetActivity = new UnityEvent<ActivityResult>();        
        public UnityEvent<string, int> onError = new UnityEvent<string, int>();
        
        public static NetworkManager Instance { get; private set; }
        private string _saveId;
        private User _user;
        private ActionService _as;

        // Start is called before the first frame update
        private void Start()
        {
            if (Instance != null)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
            onLevelWin.AddListener(x=>UserInfo.SetCurLevel(x.Level));
            
            onLogin.AddListener(x=>UserInfo.SetUserInfo(x));
            
            onGetActivity.AddListener(x=>ActivityInfo.SetActivityStr(x));
        }

        public async Task Login(string username)
        {
            Debug.Log(" Login Fun");
            try
            {
                var loginService = new LoginService();
                Debug.Log(" Login Fun1" + username);
                var loginResult = await loginService.Login(username);
                Debug.Log(" Login Fun2" + loginResult.Message);
                if (!string.IsNullOrEmpty(loginResult.Message))
                {
                    throw new Exception(loginResult.Message);
                }
                _saveId = loginResult.SaveId;
                _user = loginResult.User;
                _as = new ActionService();

                Debug.Log($"loginResult: {loginResult.User.Nickname}, {loginResult.User.Coins}, {loginResult.User.Diamonds}");
                var newData = new Account
                {
                    Nickname = loginResult.User.Nickname,
                    Coins = loginResult.User.Coins,
                    Diamonds = loginResult.User.Diamonds,
                    Lv = loginResult.User.Level
                };
                
                Debug.Log($"{newData}");
                
                onLogin.Invoke(newData);
            }
            catch (Exception e)
            {
                Debug.Log(" login fail " + e.Message);
                onError.Invoke(e.Message, 3);
            }
        }

        // public async Task GetProperty()
        // {
        //     try
        //     {
        //         // TODO: add local cache
        //         // var queryResult = await _as.QueryInfo(_saveId);
        //         // Debug.Log($"getProperty: " +
        //         //           $"diamonds={queryResult.Diamonds}, " +
        //         //           $"coins={queryResult.Coins}, " +
        //         //           $"luckLevel={queryResult.LuckLevel}");
        //         //
        //         // // invoke ui update
        //         // onProperty.Invoke(new Property
        //         // {
        //         //     Diamonds = queryResult.Diamonds,
        //         //     Coins = queryResult.Coins,
        //         //     LuckLevel = queryResult.LuckLevel
        //         // });
        //         onProperty.Invoke(new Property
        //         {
        //             Diamonds = _user.Diamonds,
        //             Coins = _user.Coins,
        //             LuckLevel = CalcLuckLevel(_user.LuckPoints, _user.DrawCounts)
        //         });
        //     }
        //     catch (Exception e)
        //     {
        //         onError.Invoke(e.Message, 3);
        //     }
        // }

        // public async Task GetInventory(InventoryType typ)
        // {
        //     try
        //     {
        //         // var result = typ switch
        //         // {
        //         //     InventoryType.HeroInventory => await _as.GetHeroes(_saveId),
        //         //     InventoryType.PropInventory => await _as.GetProps(_saveId),
        //         //     _ => throw new ArgumentOutOfRangeException(nameof(typ), typ, null)
        //         // };
        //         //
        //         // var listResult = new List<Item>(result.Items.Count);
        //         // listResult.AddRange(result.Items.Select(it => new Item
        //         // {
        //         //     Type = it.Value.Type switch
        //         //     {
        //         //         0 => ItemType.Hero,
        //         //         1 => ItemType.Prop,
        //         //         _ => ItemType.Other
        //         //     },
        //         //     Name = it.Value.Name,
        //         //     Count = it.Value.Count,
        //         //     Level = it.Value.Level
        //         // }));
        //         //
        //         // if (typ == InventoryType.HeroInventory)
        //         //     onHeroes.Invoke(listResult);
        //         // else
        //         //     onInventory.Invoke(listResult);
        //
        //         var items = typ switch
        //         {
        //             InventoryType.HeroInventory => _user.Heroes,
        //             InventoryType.PropInventory => _user.Props,
        //             _ => throw new ArgumentOutOfRangeException(nameof(typ), typ, null)
        //         };
        //
        //
        //         var listResult = new List<Progress.Item>(items.Count);
        //         listResult.AddRange(items.Select(it => new Progress.Item
        //         {
        //             Type = it.Value.Type switch
        //             {
        //                 0 => ItemType.Hero,
        //                 1 => ItemType.Prop,
        //                 _ => ItemType.Other
        //             },
        //             Name = it.Value.Name,
        //             Count = it.Value.Count,
        //             Level = it.Value.Level
        //         }));
        //
        //         if (typ == InventoryType.HeroInventory)
        //             onHeroes.Invoke(listResult);
        //         else
        //             onInventory.Invoke(listResult);
        //
        //     }
        //     catch (Exception e)
        //     {
        //         onError.Invoke(e.Message, 3);
        //     }
        // }

        public async Task LevelUp(int count = 1)
        {
            try
            {
                var LevelResult = await _as.LevelUp(_saveId, count);
                onLevelWin.Invoke(LevelResult);
            }
            catch (Exception e)
            {
                onError.Invoke(e.Message, 3);
            }
        }

        public async Task TestActivityJson()
        {
            try
            {
                var result = await _as.GetActivities();
            }
            catch (Exception e)
            {
                Debug.Log(e);
                throw;
            }
        }
        private static int CalcLuckLevel(int points, int counts)
        {
            // 5 levels
            // level 4: 100/10 = 10
            // level 3: 100/30 = 3.3
            // level 2: 100/50 = 2
            // level 1: 100/70 = 1.4
            // level 0

            // default level 2
            if (counts == 0) return 2;
            var luckPt = (double)points / counts;
            return luckPt switch
            {
                >= 11 => 4,
                >= 3.6 => 3,
                >= 2.2 => 2,
                >= 1.5 => 1,
                _ => 0
            };
        }

    }
}