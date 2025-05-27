using System.IO;
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
        [CloudFunc]
        public async Task<ActivityResult> GetActivities()
        {

            var path = $"Assets/GameScripts/HotFix/GameLogic/XFundamental/Net/CloudService/ActivityInfos/Activities.json";
            string str = await File.ReadAllTextAsync(path);
            var activityresult = SerializeHelper.DeserializeFromJsonStr<ActivityResult>(str);
            return activityresult;
        }
    }
}
