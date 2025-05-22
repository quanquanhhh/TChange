using System.Threading.Tasks;
using GameScripts.Model;
using Unity.UOS.CloudSave;
// using Unity.UOS.CloudSave.Model.Files;
using Unity.UOS.Func.Stateless.Core.Attributes;

namespace GameScripts
{
    [CloudService]
    public class ActionService
    {
        [CloudFunc]
        public async Task<QueryResult> QueryInfo(string saveId)
        {
            // 读取存档
            var saveData = await CloudSaveSDK.Instance.Files.LoadBytesAsync(saveId);
            var user = SerializeHelper.DeserializeFromByteArray<User>(saveData);
            
            return new QueryResult
            {
                Level = user.Level
            };
        }

        [CloudFunc]
        public async Task<QueryResult> AddDiamonds(string saveId, int diff)
        {
            // read
            var saveData = await CloudSaveSDK.Instance.Files.LoadBytesAsync(saveId);
            var user = SerializeHelper.DeserializeFromByteArray<User>(saveData);
            user.Diamonds += diff;

            // write
            var data = SerializeHelper.SerializeToByteArray(user);
            var options = new Unity.UOS.CloudSave.Model.Files.UpdateOptions
            {
                File = new Unity.UOS.CloudSave.Model.Files.FileOptions
                {
                    FileBytes = data
                }
            };
            await CloudSaveSDK.Instance.Files.UpdateAsync(saveId, options);

            return new QueryResult
            {
                Level = user.Level
            };
        }

        [CloudFunc]
        public async Task<QueryResult> AddCoins(string saveId, int diff)
        {
            // read
            var saveData = await CloudSaveSDK.Instance.Files.LoadBytesAsync(saveId);
            var user = SerializeHelper.DeserializeFromByteArray<User>(saveData);
            user.Coins += diff;

            // write
            var data = SerializeHelper.SerializeToByteArray(user);
            var options = new Unity.UOS.CloudSave.Model.Files.UpdateOptions
            {
                File = new Unity.UOS.CloudSave.Model.Files.FileOptions
                {
                    FileBytes = data
                }
            };
            await CloudSaveSDK.Instance.Files.UpdateAsync(saveId, options);

            return new QueryResult
            {
                Level = user.Level
            };
        }

        // [CloudFunc]
        // public async Task<GetItemsResult> GetHeroes(string saveId)
        // {
        //     Debug.Log("call to get heroes");
        //
        //     // read
        //     var saveData = await CloudSaveSDK.Instance.Files.LoadBytesAsync(saveId);
        //     var user = SerializeHelper.DeserializeFromByteArray<User>(saveData);
        //
        //     return new GetItemsResult
        //     {
        //         Ok = true,
        //         Items = user.Heroes
        //     };
        // }

        // [CloudFunc]
        // public async Task<GetItemsResult> GetProps(string saveId)
        // {
        //     Debug.Log("call to get props");
        //
        //     // read
        //     var saveData = await CloudSaveSDK.Instance.Files.LoadBytesAsync(saveId);
        //     var user = SerializeHelper.DeserializeFromByteArray<User>(saveData);
        //
        //     return new GetItemsResult
        //     {
        //         Ok = true,
        //         Items = user.Props
        //     };
        // }

        [CloudFunc]
        public async Task<QueryResult> LevelUp(string saveId, int count = 1)
        {
            var saveData = await CloudSaveSDK.Instance.Files.LoadBytesAsync(saveId);
            var user = SerializeHelper.DeserializeFromByteArray<User>(saveData);
            user.Level += count;
            var data = SerializeHelper.SerializeToByteArray(user);
            var options = new Unity.UOS.CloudSave.Model.Files.UpdateOptions
            {
                File = new Unity.UOS.CloudSave.Model.Files.FileOptions
                {
                    UpdateFileWay = Unity.UOS.CloudSave.Model.Files.UpdateFileWay.ByFileBytes,
                    FileBytes = data,
                }
            };
            await CloudSaveSDK.Instance.Files.UpdateAsync(saveId, options);
            return new QueryResult()
            {
                Level = user.Level
            };
        }

        // [CloudFunc]
        // public async Task<DrawResult> Draw(string saveId, int count)
        // {
        //     Debug.Log("call to draw");
        //     // read
        //     var saveData = await CloudSaveSDK.Instance.Files.LoadBytesAsync(saveId);
        //     var user = SerializeHelper.DeserializeFromByteArray<User>(saveData);
        //
        //     if (count > user.Diamonds)
        //     {
        //         return new DrawResult
        //         {
        //             Ok = false,
        //             Message = "钻石不够"
        //         };
        //     }
        //
        //     // start to draw
        //     var res = new List<Item>(count);
        //     var pool = user.DrawPool;
        //     if (pool == null || pool.Count == 0)
        //     {
        //         pool = Helper.NewDrawPool();
        //     }
        //
        //     if (pool.Count < count)
        //     {
        //         var n = pool.Count;
        //         var needN = count - n;
        //         res.AddRange(pool);
        //         pool = Helper.NewDrawPool();
        //
        //         var lastN = pool.Skip(pool.Count - needN).Take(needN).ToList();
        //         pool.RemoveRange(pool.Count - needN, needN);
        //         res.AddRange(lastN);
        //     }
        //     else
        //     {
        //         var lastCount = pool.Skip(pool.Count - count).Take(count).ToList();
        //         pool.RemoveRange(pool.Count - count, count);
        //         res.AddRange(lastCount);
        //         if (res.Any(e => e.Type == 0))
        //         {
        //             pool = Helper.NewDrawPool();
        //         }
        //     }
        //
        //     // update user's heroes, props, and other properties
        //     var heroes = user.Heroes;
        //     var props = user.Props;
        //     var coins = 0;
        //     var diamonds = 0;
        //     var luckPoints = 0;
        //
        //     foreach (var e in res)
        //     {
        //         switch (e.Type)
        //         {
        //             case 0:
        //                 if (!heroes.TryAdd(e.Name, e))
        //                     heroes[e.Name].Count += e.Count;
        //                 luckPoints += 100;
        //                 break;
        //             case 1:
        //                 if (!props.TryAdd(e.Name, e))
        //                     props[e.Name].Count += e.Count;
        //                 luckPoints += 1;
        //                 break;
        //             case 2:
        //                 switch (e.Name)
        //                 {
        //                     case "diamonds":
        //                         diamonds += e.Count;
        //                         luckPoints += e.Count;
        //                         break;
        //                     case "coins":
        //                         coins += e.Count;
        //                         luckPoints += (e.Count + 3000) / 10000;
        //                         break;
        //                 }
        //                 break;
        //         }
        //     }
        //
        //     // write
        //     user.Diamonds += diamonds - count;
        //     user.Coins += coins;
        //     user.LuckPoints += luckPoints;
        //     user.DrawCounts += count;
        //     user.DrawPool = pool;
        //     user.Heroes = heroes;
        //     user.Props = props;
        //     var data = SerializeHelper.SerializeToByteArray(user);
        //     var options = new UpdateOptions
        //     {
        //         File = new FileOptions
        //         {
        //             UpdateFileWay = UpdateFileWay.ByFileBytes,
        //             FileBytes = data,
        //         }
        //     };
        //     Debug.Log(saveId);
        //     await CloudSaveSDK.Instance.Files.UpdateAsync(saveId, options);
        //     return new DrawResult
        //     {
        //         Ok = true,
        //         Items = res,
        //         User = user
        //     };
        // }
    }
}
