using System.Threading.Tasks;
using GameScripts.Model;
using Unity.UOS.CloudSave;
using Unity.UOS.CloudSave.Model.Files;
using Unity.UOS.Func.Stateless.Core.Attributes;
using UnityEngine;

namespace GameScripts.Net
{
    [CloudService]
    public class LoginService
    {
        [CloudFunc]
        public async Task<LoginResult> Login(string username)
        {
            // 用户使用时候要替换成自己的 Auth
            // 检查用户是否有存档，
            // 如果没有就是新用户第一次登陆
            // 创建一个存档，把密码存到属性里 (hash一下)
            // 如果存在就检查密码对不对
            Debug.Log(" Login 1");
            var saveItems = await CloudSaveSDK.Instance.Files.ListAllAsync();
            Debug.Log(" Login 2");
            if (saveItems.Count == 0)
            {
                // 新用户-创建
                // 创建存档
                var newUser = new User
                {
                    Username = username,
                    Nickname = username + Helper.GetRandomEmoji(),
                    Diamonds = 500,
                    Coins = 0,
                };

                var data = SerializeHelper.SerializeToByteArray(newUser);
                var options = new CreateOptions
                {
                    ProgressType = ProgressType.Linear
                };
                Debug.Log(" Login 3");
                var saveId = await CloudSaveSDK.Instance.Files.CreateAsync(username, data, options);
                return new LoginResult
                {
                    SaveId = saveId,
                    User = newUser
                };
            }
            Debug.Log(" Login 4");

            if (saveItems.Count > 1)
            {
                return new LoginResult
                {
                    Message = "Internal Server Error"
                };
            }

            Debug.Log(" Login 5");
            // 验证密码
            var saveItem = saveItems[0];
            // 读取存档
            var saveData = await CloudSaveSDK.Instance.Files.LoadBytesAsync(saveItem.SaveId);
            var user = SerializeHelper.DeserializeFromByteArray<User>(saveData);
            Debug.Log(" Login 6");
            return new LoginResult
            {
                SaveId = saveItem.SaveId,
                User = user
            };
        }
    }
}