using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Unity.UOS.Auth;
using Unity.UOS.CloudSave;
using Unity.UOS.CloudSave.Exception;
using Unity.UOS.CloudSave.Model.Files;
using Unity.UOS.Encrypt;
using UnityEngine;
using FileOptions = Unity.UOS.CloudSave.Model.Files.FileOptions;


namespace GameScripts.Core
{
    public class Save : MonoBehaviour
    {
        
        // public string appId;
        // public string appSecret;
        public ulong userId = 1;
        private string _personaId = "test-persona-id";

        private async void Awake()
        {
            
            // 初始化 sdk instance
            try
            {
                // 如果安装了Uos Launcher 使用Uos Launcher关联的Uos App初始化SDK
                await CloudSaveSDK.InitializeAsync();
                // 如果需要使用其他 UOS APP，可传入一对AppId, AppSecret初始化SDK
                // await CloudSaveSDK.InitializeAsync(appId, appSecret);
            }
            catch (CloudSaveClientException e)
            {
                Debug.LogErrorFormat("failed to initialize sdk, clientEx: {0}", e);
                throw;
            }
            catch (CloudSaveServerException e)
            {
                Debug.LogErrorFormat("failed to initialize sdk, serverEx: {0}", e);
                throw;
            }

            // 初始化AuthTokenManager中的user token, personaId为选填项
            await AuthTokenManager.ExternalLogin(userId.ToString(), _personaId);
            
            var storages = new List<SaveBase>() {new SaveCommon()};
            SaveManager.Instance.Init(storages, false);
            await SampleLinearSave();
        }

        public async Task SampleLinearSave()
        {
            // 单存档使用示例
            // 单存档：一个角色在指定命名空间下的单存档应只有一个
            const string ns = "Test";
            
            // 获取单存档信息
            // string dataStr = SaveManager.Instance.ToJson();
            var item = await GetLinearMetadata(ns);
            Debug.Log($"linear save metadata: {JsonConvert.SerializeObject(item)}");
            if (item == null)
            {
                SaveManager.Instance.GetStorage<SaveCommon>().PlayerId = userId;
                SaveManager.Instance.GetStorage<SaveCommon>().Name = "Player" + userId;
            }
            // 获取存档数据或初始化存档数据
            var fileBytes = item == null ? Encoding.UTF8.GetBytes("Test U.") : await LoadFileBytesAsync(item.SaveId);
            Debug.Log($"linear save bytes data: {Encoding.Default.GetString(fileBytes)}");
            
            // 本地更新存档数据
            // fileBytes = fileBytes.Concat(Encoding.UTF8.GetBytes(" update")).ToArray();
            
            // 保存存档
            var options = new UpdateOptions
            {
                Name = "PlayerInfo",
                File = new FileOptions()
                {
                    UpdateFileWay = UpdateFileWay.ByFileBytes,
                    FileBytes = SaveManager.Instance.ToByte2()
                }
            };
            var id = await SaveLinear(ns, options);
            Debug.Log($"successfully save linear, save id: {id}");
        }


        private async Task<byte[]> LoadFileBytesAsync(string saveId)
        {
            try
            {
                return await CloudSaveSDK.Instance.Files.LoadBytesAsync(saveId);
            }
            catch (CloudSaveClientException e)
            {
                Debug.LogErrorFormat("failed to load file bytes, id {0}, clientEx: {1}", saveId, e);
                throw;
            }
            catch (CloudSaveServerException e)
            {
                Debug.LogErrorFormat("failed to load file bytes, id {0}, serverEx: {1}", saveId, e);
                throw;
            }
        }

        private async Task<SaveItem> GetLinearMetadata(string targetNamespace)
        {
            try
            {
                return await CloudSaveSDK.Instance.Files.GetLinearAsync(targetNamespace);
            }
            catch (CloudSaveClientException e)
            {
                Debug.LogErrorFormat("failed to get metadata of linear save: {0}", e);
                throw;
            }
            catch (CloudSaveServerException e)
            {
                Debug.LogErrorFormat("failed to get metadata of linear save: {0}", e);
                throw;
            }
        }

        private async Task<string> SaveLinear(string targetNamespace, UpdateOptions options)
        {
            try
            {
                return await CloudSaveSDK.Instance.Files.SaveLinearAsync(targetNamespace, options);
            }
            catch (CloudSaveClientException e)
            {
                Debug.LogErrorFormat("failed to create or update linear save, clientEx: {0}", e);
                throw;
            }
            catch (CloudSaveServerException e)
            {
                Debug.LogErrorFormat("failed to create or update linear save, serverEx: {0}", e);
                throw;
            }
        }
    }
}