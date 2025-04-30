using System.Collections.Generic;
using System.Text;
using GameLogic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TEngine;
using UnityEngine;

namespace GameScripts.Core
{
    public class SaveManager : SingletonBehaviour<SaveManager>
    {
        const string storageKey = "StorageData";
        const string localVersionKey = "StorageVersion";
        const string remoteVersionAckKey = "RemoteStorageVersion";
        const string remoteVersionLocalKey = "RemoteVersionLocal";
        const string runOnceKey = "RunOnce";

        
        public static System.Action onRebuild;

        static Dictionary<System.Type, string> gType2Name = new Dictionary<System.Type, string>();

        Dictionary<string, SaveBase> storageMap;
        public bool uploadEnable = true;
        
        
        public T GetStorage<T>() where T : SaveBase
        {
            System.Type storage_type = typeof(T);
            if (!gType2Name.TryGetValue(storage_type, out string name))
            {
                name = storage_type.Name;
                gType2Name[storage_type] = name;
            }

            return (T)storageMap[name];            
        }

        
        float LocalTickTime
        {
            get;
            set;
        }

        float RemoteTickTime
        {
            get;
            set;
        }

        // 本地存档版本
        public ulong LocalVersion
        {
            get
            {
                return _localVersion;
            }
            set
            {
                _localVersion = value;
                //DebugUtil.Log("[storage] changed local_version to {0}", value);
            }
        }
        
        // 服务器存档版本
        public ulong RemoteVersionACK
        {
            get
            {
                return _remoteVersionACK;
            }
            set
            {
                _remoteVersionACK = value;
#if !DISABLE_STORAGE_LOG
                // Debug.Log("[storage] changed remote_verion_ack to {0}", value);
#endif
                Utility.PlayerPrefs.SetString(remoteVersionAckKey, System.Convert.ToBase64String(RijndaelManager.Instance.EncryptStringToBytes(RemoteVersionACK.ToString())));
            }
        }
        
        public ulong RemoteVersionSYN
        {
            get
            {
                return _remoteVersionSYN;
            }
            set
            {
                _remoteVersionSYN = value;
#if !DISABLE_STORAGE_LOG
                // Debug.Log("[storage] changed remote_verion_syn to {0}", value);
#endif
                PlayerPrefs.SetString(remoteVersionLocalKey, System.Convert.ToBase64String(RijndaelManager.Instance.EncryptStringToBytes(RemoteVersionSYN.ToString())));
            }
        }
        
        
        bool InConflict = false;
        // Profile InConflictProfile;
        System.Action<bool> conflictResolvedCallback;

        ulong lastSavedLocalVersion;
        // 强制同步，用于存储敏感数据
        public bool SyncForce
        {
            get;
            set;
        }

        public bool SyncForceRemote
        {
            get;
            set;
        }

        // 本地同步间隔
        float localInterval = 1.0f;
        public float LocalInterval
        {
            get
            {
                return localInterval;
            }
            set
            {
                if (value > 0.0f)
                {
                    localInterval = value;
                }
            }
        }

        float refreshIntervalMax = 64.0f;
        float refreshInterval = 1.0f;
        float refreshTick = 0.0f;

        // 远端同步间隔
        float remoteInterval = 30.0f;
        private ulong _remoteVersionACK;
        private ulong _remoteVersionSYN;
        private ulong _localVersion;

        public float RemoteInterval
        {
            get
            {
                return remoteInterval;
            }
            set
            {
                if (value > 0.0f)
                {
                    remoteInterval = value;
                }
            }
        }
        
        bool syncLock
        {
            get;
            set;
        }

        public bool Inited
        {
            get;
            private set;
        }

        
        
        public void SaveToLocal()
        {
            if (!isFullInitialized)
            {
                return;
            }
            
            var storageCommon = GetStorage<SaveCommon>();
            if (storageCommon != null)
            {
                _localVersion -= 1;
                storageCommon.UpdatedAt = CurrentTimeMillis();
            }
            //fixed:ArgumentNullException: Value cannot be null. Parameter name: obj
            JsonSerializerSettings setting = new JsonSerializerSettings();
            setting.NullValueHandling = NullValueHandling.Ignore;
            string jsonData = JsonConvert.SerializeObject(storageMap,setting);
            byte[] encryptData = RijndaelManager.Instance.EncryptStringToBytes(jsonData);
            PlayerPrefs.SetString(storageKey, System.Convert.ToBase64String(encryptData));
            PlayerPrefs.SetString(localVersionKey, System.Convert.ToBase64String(RijndaelManager.Instance.EncryptStringToBytes(LocalVersion.ToString())));
            PlayerPrefs.SetString(remoteVersionAckKey, System.Convert.ToBase64String(RijndaelManager.Instance.EncryptStringToBytes(RemoteVersionACK.ToString())));
            PlayerPrefs.SetString(remoteVersionLocalKey, System.Convert.ToBase64String(RijndaelManager.Instance.EncryptStringToBytes(RemoteVersionSYN.ToString())));
            lastSavedLocalVersion = LocalVersion;
        }

        private ulong CurrentTimeMillis()
        {
            
            System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
            return (ulong)(System.DateTime.UtcNow - epochStart).TotalMilliseconds;
        }
        public string ToJson()
        {
            return JsonConvert.SerializeObject(storageMap);
        }
        public byte[] ToByte2()
        {
            string content = JsonConvert.SerializeObject(storageMap);
            byte[] buffer = Encoding.UTF8.GetBytes(content);
            byte[] newBuffer = new byte[buffer.Length];
            // newBuffer[0] = Encoding.UTF8.GetBytes("0")[0]; //todo 弄单个数字就好
            for (int i = 0; i < buffer.Length; i++)
            {
                newBuffer[i] = buffer[i];
            }
            return newBuffer;
        } 

        public void FromJson(string jsonData)
        {
            JObject jObj = JObject.Parse(jsonData);
            foreach (var type in storageMap.Keys)
            {
                var token = jObj[type];
                if (token == null)
                {
                    continue;
                }

                var str = token.ToString();
                JsonSerializerSettings setting = new JsonSerializerSettings();

                setting.NullValueHandling = NullValueHandling.Ignore;
                JsonConvert.PopulateObject(str, storageMap[type], setting);
            }

            onRebuild?.Invoke();
        }

        void ReadFromLocal()
        {
            // 读取本地存档
            string jsonData = "{}";
            if (PlayerPrefs.HasKey(storageKey))
            {
                byte[] encryptData = System.Convert.FromBase64String(PlayerPrefs.GetString(storageKey));
                jsonData = RijndaelManager.Instance.DecryptStringFromBytes(encryptData);
#if !DISABLE_STORAGE_LOG
                Debug.Log(" read storage json from local : " + jsonData);
#endif
                FromJson(jsonData);
            }
            else
            {
#if !DISABLE_STORAGE_LOG
                Debug.Log("No local storage data can read! ");
#endif
            }

            // 读取本地存档版本
            if (PlayerPrefs.HasKey(localVersionKey))
            {
                string strVersion = RijndaelManager.Instance.DecryptStringFromBytes(System.Convert.FromBase64String(PlayerPrefs.GetString(localVersionKey)));
                LocalVersion = ulong.Parse(strVersion);
#if !DISABLE_STORAGE_LOG
                Debug.Log(" read local version : " + LocalVersion);
#endif
            }
            else
            {
                LocalVersion = 0;
#if !DISABLE_STORAGE_LOG
                Debug.Log("No local storage version can read! ");
#endif
            }
            lastSavedLocalVersion = LocalVersion;

            // 读取远端档版本
            if (PlayerPrefs.HasKey(remoteVersionAckKey))
            {
                string strVersion = RijndaelManager.Instance.DecryptStringFromBytes(System.Convert.FromBase64String(PlayerPrefs.GetString(remoteVersionAckKey)));
                RemoteVersionACK = ulong.Parse(strVersion);
#if !DISABLE_STORAGE_LOG
                Debug.Log(" read remote version : " + RemoteVersionACK);
#endif
            }
            else
            {
                RemoteVersionACK = 0;
#if !DISABLE_STORAGE_LOG
                Debug.Log("No remote storage version can read! ");
#endif
            }

            if (PlayerPrefs.HasKey(remoteVersionLocalKey))
            {
                string strVersion = RijndaelManager.Instance.DecryptStringFromBytes(System.Convert.FromBase64String(PlayerPrefs.GetString(remoteVersionLocalKey)));
                RemoteVersionSYN = ulong.Parse(strVersion);
#if !DISABLE_STORAGE_LOG
                Debug.Log(" read last upload version : " + RemoteVersionSYN);
#endif
            }
            else
            {
                RemoteVersionSYN = 0;
#if !DISABLE_STORAGE_LOG
                Debug.Log("No remote storage version can read! ");
#endif
            }
            
        }
        
//         
//         #region sync_with_server
//         public void UploadProfile()
//         {
//             if (!Account.AccountManager.Instance.HasLogin)
//             {
//                 return;
//             }
//
//             if (!isFullInitialized)
//             {
//                 return;
//             }
//             
//             if (syncLock)
//             {
//                 return;
//             }
//             syncLock = true;
//
//             if (RemoteVersionSYN <= RemoteVersionACK)
//             {
//                 RemoteVersionSYN = LocalVersion;
//             }
//
//             string jsonData = JsonConvert.SerializeObject(storageMap);
//             var cUpdateProfile = new CUpdateProfile
//             {
//                 Profile = new Profile
//                 {
//                     Json = jsonData,
//                     Version = LocalVersion,
//                 },
//                 OldVersion = RemoteVersionACK,
//             };
// #if !DISABLE_STORAGE_LOG
//             DebugUtil.Log("start upload profile : " + cUpdateProfile.Profile.Json);
// #endif
//             APIManager.Instance.Send(cUpdateProfile, (SUpdateProfile sUpdateProfile) =>
//             {
// #if !DISABLE_STORAGE_LOG
//                 DebugUtil.Log("upload profile successs, version  = " + sUpdateProfile.Profile.Version);
// #endif
//                 if (sUpdateProfile.Profile.Version > RemoteVersionACK)
//                 {
//                     RemoteVersionACK = sUpdateProfile.Profile.Version;
//                 }
//                 syncLock = false;
//             }, (ErrorCode err, string msg, SUpdateProfile sUpdateProfile) =>
//             {
//                 DebugUtil.Log("upload profile error " + err + "   " + msg);
//                 if (err == ErrorCode.ProfileNotExistsError)
//                 {
//                     CreateProfile((result) =>
//                     {
//                         DebugUtil.Log("create profile when upload not exists result: " + result.ToString());
//                     });
//                     return;
//                 }
//                 
//                 if (err == ErrorCode.ProfileVersionConflictionError)
//                 {
//                     OnServerProfileConfict(sUpdateProfile.Profile);
//                 }
//                 syncLock = false;
//             });
//             SaveToLocal();
//         }
//
//         public void GetOrCreateProfile(System.Action<bool> cb)
//         {
//             if (!Account.AccountManager.Instance.HasLogin)
//             {
//                 if (cb != null)
//                 {
//                     cb.Invoke(false);
//                 }
//                 return;
//             }
//
//             syncLock = true;
//             CGetProfile getProfile = new CGetProfile
//             {
//                 OldVersion = RemoteVersionACK
//             };
//             DragonU3DSDK.Network.BI.BIManager.Instance.SendCommonGameEvent(DragonU3DSDK.Network.API.Protocol.BiEventCommon.Types.CommonGameEventType.ProfileGetStart);
//             APIManager.Instance.Send(getProfile, (SGetProfile sGetProfile) =>
//             {
//                 DragonU3DSDK.Network.BI.BIManager.Instance.SendCommonGameEvent(DragonU3DSDK.Network.API.Protocol.BiEventCommon.Types.CommonGameEventType.ProfileGetSucess);
//                 syncLock = false;
//                 cb?.Invoke(true);
//                 EventManager.Instance.Trigger<SDKEvents.ProfileFetchedEvent>().Trigger();
//
//             }, (errno, errmsg, resp) =>
//             {
//                 DragonU3DSDK.Network.BI.BIManager.Instance.SendCommonGameEvent(DragonU3DSDK.Network.API.Protocol.BiEventCommon.Types.CommonGameEventType.ProfileGetFailure, errno.ToString(), errmsg);
//                 if (errno == ErrorCode.ProfileNotExistsError)
//                 {
//                     CreateProfile(cb);
//                     return;
//                 }
//
//                 syncLock = false;
//                 if (errno == ErrorCode.ProfileVersionConflictionError)
//                 {
//                     var sGetProfile = (SGetProfile)resp;
//                     OnServerProfileConfict(sGetProfile.Profile, cb);
//                 }
//                 else
//                 {
//                     cb?.Invoke(false);
//                 }
//             });
//         }
//
//         void CreateProfile(System.Action<bool> cb)
//         {
//             CCreateProfile cCreateProfile = new CCreateProfile
//             {
//                 Profile = new Profile
//                 {
//                     Json = ToJson(),
//                     Version = LocalVersion,
//                 }
//             };
//
//             DragonU3DSDK.Network.BI.BIManager.Instance.SendCommonGameEvent(DragonU3DSDK.Network.API.Protocol.BiEventCommon.Types.CommonGameEventType.ProfileCreateStart);
//             APIManager.Instance.Send(cCreateProfile, (SCreateProfile resp) =>
//             {
//                 DragonU3DSDK.Network.BI.BIManager.Instance.SendCommonGameEvent(DragonU3DSDK.Network.API.Protocol.BiEventCommon.Types.CommonGameEventType.ProfileCreateSuccess);
//                 syncLock = false;
// #if !DISABLE_STORAGE_LOG
//                 DebugUtil.Log("create profile success");
// #endif
//                 RemoteVersionACK = cCreateProfile.Profile.Version;
//                 cb?.Invoke(true);
//                 EventManager.Instance.Trigger<SDKEvents.ProfileCreatedEvent>().Trigger();
//             }, (errno, errmsg, resp) =>
//             {
//                 DragonU3DSDK.Network.BI.BIManager.Instance.SendCommonGameEvent(DragonU3DSDK.Network.API.Protocol.BiEventCommon.Types.CommonGameEventType.ProfileCreateFailure, errno.ToString(), errmsg);
//                 syncLock = false;
//                 DebugUtil.LogError("create profile failed errno = {0} errmsg = {1}", errno, errmsg);
//                 cb?.Invoke(false);
//             });
//         }
//
//         void OnServerProfileConfict(Profile serverProfile, System.Action<bool> callback = null)
//         {
//             DebugUtil.LogWarning("profile conflict: remote_version={0} remote_version_ack={1} remote_version_local={2}, local_version={3} force={4}", serverProfile.Version, RemoteVersionACK, RemoteVersionSYN, LocalVersion, serverProfile.Force);
//
//             if (callback != null)
//             {
//                 conflictResolvedCallback = callback;
//             }
//
//             if (serverProfile.Force)
//             {
//                 ResolveProfileConfict(serverProfile, true);
//             }
//             /*else if (RemoteVersionAck == 0)
//             {
//                 ResolveProfileConfict(serverProfile, true);
//             }
//             */
//             else if (serverProfile.Version == RemoteVersionACK)
//             {
//                 RemoteVersionSYN = RemoteVersionACK;
//             }
//             else if (serverProfile.Version == RemoteVersionSYN)
//             {
//                 RemoteVersionACK = RemoteVersionSYN;
//             }
//             else
//             {
//                 InConflict = true;
//                 InConflictProfile = serverProfile;
//                 EventManager.Instance.Trigger<SDKEvents.ProfileConflictEvent>().Data(serverProfile).Trigger();
//                 Network.BI.BIManager.Instance.SendCommonGameEvent(BiEventCommon.Types.CommonGameEventType.ProfileConflict, serverProfile.Version.ToString(), RemoteVersionACK.ToString(), RemoteVersionSYN.ToString(), LocalVersion.ToString(), serverProfile.Force.ToString());
//             }
//         }
//
//
//         public void ResolveProfileConfict(Profile serverProfile, bool useServer)
//         {
//             if (useServer)
//             {
//                 // backup firebase token
//                 string firebasePushToken = null;
//                 var storageCommon = GetStorage<StorageCommon>();
//                 if (storageCommon != null)
//                 {
//                     firebasePushToken = storageCommon.FirebaseInstanceId;
//                 }
//
//                 Clear();
//                 FromJson(serverProfile.Json);
//
//                 RemoteVersionACK = serverProfile.Version;
//                 RemoteVersionSYN = serverProfile.Version;
//                 LocalVersion = serverProfile.Version;
//
//                 storageCommon = GetStorage<StorageCommon>();
//                 if (storageCommon != null && !string.IsNullOrEmpty(firebasePushToken))
//                 {
//                     storageCommon.FirebaseInstanceId = firebasePushToken;
//                 }
//
//                 var clear = false;
//                 if (storageCommon != null && storageCommon.PlayerId == 0)
//                 {
//                     clear = true;
//                 }
//                 EventManager.Instance.Trigger<SDKEvents.ProfileReplacedEvent>().Data(clear).Trigger();
//                 DragonU3DSDK.Network.BI.BIManager.Instance.SendCommonGameEvent(DragonU3DSDK.Network.API.Protocol.BiEventCommon.Types.CommonGameEventType.ProfileReplaced, clear.ToString());
//             }
//             else
//             {
//                 RemoteVersionACK = serverProfile.Version;
//                 RemoteVersionSYN = RemoteVersionACK;
//                 if (LocalVersion <= RemoteVersionACK)
//                 {
//                     LocalVersion = RemoteVersionACK + 1;
//                 }
//             }
//             SaveToLocal();
//             EventManager.Instance.Trigger<SDKEvents.ProfileResolvedEvent>().Data(useServer).Trigger();
//             DragonU3DSDK.Network.BI.BIManager.Instance.SendCommonGameEvent(DragonU3DSDK.Network.API.Protocol.BiEventCommon.Types.CommonGameEventType.ProfileConflictResolved, useServer.ToString());
//
//             if (conflictResolvedCallback != null)
//             {
//                 conflictResolvedCallback.Invoke(true);
//                 conflictResolvedCallback = null;
//             }
//             InConflict = false;
//         }
//
//         /// <summary>
//         /// 存档冲突时直接使用服务器的
//         /// </summary>
//         public void ResolveProfileConfictWithServer()
//         {
//             if (InConflictProfile != null)
//             {
//                 ResolveProfileConfict(InConflictProfile, true);
//                 InConflictProfile = null;
//             }
//         }
//         #endregion

        // public bool hasChange;
        void Update()
        {
            if (!Inited || !uploadEnable)
                return;
        
            LocalTickTime += Time.deltaTime;
            if (SyncForce || (LocalTickTime > localInterval && LocalVersion > lastSavedLocalVersion))
            {
                SyncForce = false;
                LocalTickTime = 0.0f;
                SaveToLocal();
            }
        
            // RemoteTickTime += Time.deltaTime;
            // if ((RemoteTickTime > remoteInterval || SyncForceRemote)
            //     && !syncLock
            //     && !InConflict
            //     && LocalVersion > RemoteVersionACK
            //     && APIManager.Instance.HasNetwork
            //     && Account.AccountManager.Instance.HasLogin)
            // {
            //     RemoteTickTime = 0.0f;
            //     SyncForceRemote = false;
            //     UploadProfile();
            // }
        
            refreshTick += Time.deltaTime;
            if (refreshTick > refreshInterval)
            {
                refreshTick = 0.0f;
                if (refreshInterval < refreshIntervalMax)
                {
                    refreshInterval *= 2.0f;
                }
                UpdateStorageCommon();
            }
        }
        
        //Storage是否完整初始化了
        //为了解决支持热更新后，Storage的初始化放在了热更新层
        //在Launcher层BI发送日志依赖StorageCommon，为了收集到玩家在游戏加载完成之前的BI日志，
        //这里添加一个状态，标记StorageManger是否是完全初始化了
        //如果未完全初始化，StorageManger处于一个只读状态，不能存盘，不能上传存档
        private bool isFullInitialized = false;
        
        public void Init(List<SaveBase> storages, bool fullInitialized = true)
        {
            if (!Inited || !isFullInitialized)
            {
                storageMap = new Dictionary<string, SaveBase>();
                foreach (var storage in storages)
                {
                    var type = storage.GetType().Name;
                    storageMap[type] = storage;
                }
                
                Inited = true;
                
                isFullInitialized = fullInitialized;
                
                ReadFromLocal();
            }
            else
            {
                Debug.Assert(false, "Error Init Storage !!!");
            }
        }

        public void Reload()
        {
            if (Inited) return;
            Inited = true;
            Clear();
            ReadFromLocal();
        }

        public void Release()
        {
            if (Inited)
            {
                SaveToLocal();
                Inited = false;
            }
        }

        void UpdateStorageCommon()
        {
            var storageCommon = GetStorage<SaveCommon>();
            if (storageCommon == null)
            {
                return;
            }
        
            // storageCommon.Platform = (int)DeviceHelper.GetPlatform();
        
            // update adjust infos
            // var adjustPlugin = Dlugin.SDK.GetInstance().adjustPlugin;
           
            // if (string.IsNullOrEmpty(storageCommon.DeviceType))
            // {
            //     storageCommon.DeviceType = DeviceHelper.GetDeviceType();
            // }
        
            // if (string.IsNullOrEmpty(storageCommon.DeviceModel))
            // {
            //     storageCommon.DeviceModel = DeviceHelper.GetDeviceModel();
            // }
            // if (string.IsNullOrEmpty(storageCommon.DeviceMemory))
            // {
            //     storageCommon.DeviceMemory = DeviceHelper.GetTotalMemory().ToString();
            // }
        
            // string nativeVersion = DragonNativeBridge.GetVersionCode().ToString();
            // if (!string.IsNullOrEmpty(nativeVersion))
            // {
            //     // clear login status when native upgrade
            //     if (!string.IsNullOrEmpty(storageCommon.NaviveVersion) && nativeVersion != storageCommon.NaviveVersion)
            //     {
            //         //Account.AccountManager.Instance.Clear();
            //         EventManager.Instance.Trigger<SDKEvents.NativeVersionChanged>().Data(storageCommon.NaviveVersion, nativeVersion).Trigger();
            //         DragonU3DSDK.Network.BI.BIManager.Instance.SendCommonGameEvent(DragonU3DSDK.Network.API.Protocol.BiEventCommon.Types.CommonGameEventType.NativeVersionChanged, storageCommon.NaviveVersion, nativeVersion);
            //     }
            //     storageCommon.NaviveVersion = nativeVersion;
            // }
        }

        // public bool RunOnce(System.Action cb)
        // {
        //     if (PlayerPrefs.HasKey(runOnceKey))
        //     {
        //         return false;
        //     }
        //
        //     cb();
        //     PlayerPrefs.SetString(runOnceKey, true.ToString());
        //     return true;
        // }

        void Clear()
        {
            foreach (var key in storageMap.Keys)
            {
                storageMap[key].Clear();
            }
        }

        // public void ClearAll()
        // {
        //     // TODO
        //     PlayerPrefs.DeleteAll();
        //     Clear();
        //     var serverProfile = new Profile
        //     {
        //         Json = JsonConvert.SerializeObject(storageMap),
        //         Version = 0,
        //         Force = true
        //     };
        //     OnServerProfileConfict(serverProfile);
        // }

        
        
        
    }
}