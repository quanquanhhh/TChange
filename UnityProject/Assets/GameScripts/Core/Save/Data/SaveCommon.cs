using GameScripts.Core.Data;
using Newtonsoft.Json;

namespace GameScripts.Core
{
    [System.Serializable]
    public class SaveCommon : SaveBase
    {
        
        
        // 
        [JsonProperty]
        ulong playerId;
        [JsonIgnore]
        public ulong PlayerId
        {
            get
            {
                return playerId;
            }
            set
            {
                if(playerId != value)
                {
                    playerId = value;
                    SaveManager.Instance.LocalVersion++;
                    SaveManager.Instance.SyncForce = true;
                    
                }
            }
        }
        // ---------------------------------//
        
        // 
        // [JsonProperty]
        // string deviceId = "";
        // [JsonIgnore]
        // public string DeviceId
        // {
        //     get
        //     {
        //         return deviceId;
        //     }
        //     set
        //     {
        //         if(deviceId != value)
        //         {
        //             deviceId = value;
        //             SaveManager.Instance.LocalVersion++;
        //             SaveManager.Instance.SyncForce = true;
        //             
        //         }
        //     }
        // }
        // ---------------------------------//
        
        // 
        // [JsonProperty]
        // string email = "";
        // [JsonIgnore]
        // public string Email
        // {
        //     get
        //     {
        //         return email;
        //     }
        //     set
        //     {
        //         if(email != value)
        //         {
        //             email = value;
        //             SaveManager.Instance.LocalVersion++;
        //             SaveManager.Instance.SyncForce = true;
        //             
        //         }
        //     }
        // }
        // ---------------------------------//
        
        // 
        // [JsonProperty]
        // string facebookId = "";
        // [JsonIgnore]
        // public string FacebookId
        // {
        //     get
        //     {
        //         return facebookId;
        //     }
        //     set
        //     {
        //         if(facebookId != value)
        //         {
        //             facebookId = value;
        //             SaveManager.Instance.LocalVersion++;
        //             SaveManager.Instance.SyncForce = true;
        //             
        //         }
        //     }
        // }
        // ---------------------------------//
        
        // 
        // [JsonProperty]
        // string facebookName = "";
        // [JsonIgnore]
        // public string FacebookName
        // {
        //     get
        //     {
        //         return facebookName;
        //     }
        //     set
        //     {
        //         if(facebookName != value)
        //         {
        //             facebookName = value;
        //             SaveManager.Instance.LocalVersion++;
        //             
        //             
        //         }
        //     }
        // }
        // ---------------------------------//
        
        // 
        // [JsonProperty]
        // string facebookEmail = "";
        // [JsonIgnore]
        // public string FacebookEmail
        // {
        //     get
        //     {
        //         return facebookEmail;
        //     }
        //     set
        //     {
        //         if(facebookEmail != value)
        //         {
        //             facebookEmail = value;
        //             SaveManager.Instance.LocalVersion++;
        //             
        //             
        //         }
        //     }
        // }
        // ---------------------------------//
        
        // 
        [JsonProperty]
        string name = "";
        [JsonIgnore]
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                if(name != value)
                {
                    name = value;
                    SaveManager.Instance.LocalVersion++;
                    
                    
                }
            }
        }
        // ---------------------------------//
        
        // 
        // [JsonProperty]
        // uint revenueCount;
        // [JsonIgnore]
        // public uint RevenueCount
        // {
        //     get
        //     {
        //         return revenueCount;
        //     }
        //     set
        //     {
        //         if(revenueCount != value)
        //         {
        //             revenueCount = value;
        //             SaveManager.Instance.LocalVersion++;
        //             SaveManager.Instance.SyncForce = true;
        //             
        //         }
        //     }
        // }
        // ---------------------------------//
        
        // 
        // [JsonProperty]
        // ulong revenueUSDCents;
        // [JsonIgnore]
        // public ulong RevenueUSDCents
        // {
        //     get
        //     {
        //         return revenueUSDCents;
        //     }
        //     set
        //     {
        //         if(revenueUSDCents != value)
        //         {
        //             revenueUSDCents = value;
        //             SaveManager.Instance.LocalVersion++;
        //             SaveManager.Instance.SyncForce = true;
        //             
        //         }
        //     }
        // }
        // ---------------------------------//
        
        // 
        // [JsonProperty]
        // ulong lastRevenueTime;
        // [JsonIgnore]
        // public ulong LastRevenueTime
        // {
        //     get
        //     {
        //         return lastRevenueTime;
        //     }
        //     set
        //     {
        //         if(lastRevenueTime != value)
        //         {
        //             lastRevenueTime = value;
        //             SaveManager.Instance.LocalVersion++;
        //             SaveManager.Instance.SyncForce = true;
        //             
        //         }
        //     }
        // }
        // ---------------------------------//
        
        // 
        // [JsonProperty]
        // ulong installedAt;
        // [JsonIgnore]
        // public ulong InstalledAt
        // {
        //     get
        //     {
        //         return installedAt;
        //     }
        //     set
        //     {
        //         if(installedAt != value)
        //         {
        //             installedAt = value;
        //             SaveManager.Instance.LocalVersion++;
        //             
        //             
        //         }
        //     }
        // }
        // ---------------------------------//
        
        // 
        [JsonProperty]
        ulong updatedAt;
        [JsonIgnore]
        public ulong UpdatedAt
        {
            get
            {
                return updatedAt;
            }
            set
            {
                if(updatedAt != value)
                {
                    updatedAt = value;
                    SaveManager.Instance.LocalVersion++;
                    
                    
                }
            }
        }
        // ---------------------------------//
        
        // 
        // [JsonProperty]
        // string adjustId = "";
        // [JsonIgnore]
        // public string AdjustId
        // {
        //     get
        //     {
        //         return adjustId;
        //     }
        //     set
        //     {
        //         if(adjustId != value)
        //         {
        //             adjustId = value;
        //             SaveManager.Instance.LocalVersion++;
        //             SaveManager.Instance.SyncForce = true;
        //             
        //         }
        //     }
        // }
        // ---------------------------------//
        
        // 
        // [JsonProperty]
        // string adid = "";
        // [JsonIgnore]
        // public string Adid
        // {
        //     get
        //     {
        //         return adid;
        //     }
        //     set
        //     {
        //         if(adid != value)
        //         {
        //             adid = value;
        //             SaveManager.Instance.LocalVersion++;
        //             
        //             
        //         }
        //     }
        // }
        // ---------------------------------//
        
        // 
        // [JsonProperty]
        // string idfa = "";
        // [JsonIgnore]
        // public string Idfa
        // {
        //     get
        //     {
        //         return idfa;
        //     }
        //     set
        //     {
        //         if(idfa != value)
        //         {
        //             idfa = value;
        //             SaveManager.Instance.LocalVersion++;
        //             
        //             
        //         }
        //     }
        // }
        // ---------------------------------//
        
        // 
        // [JsonProperty]
        // string idfv = "";
        // [JsonIgnore]
        // public string Idfv
        // {
        //     get
        //     {
        //         return idfv;
        //     }
        //     set
        //     {
        //         if(idfv != value)
        //         {
        //             idfv = value;
        //             SaveManager.Instance.LocalVersion++;
        //             
        //             
        //         }
        //     }
        // }
        // ---------------------------------//
        
        // 
        // [JsonProperty]
        // string gaid = "";
        // [JsonIgnore]
        // public string Gaid
        // {
        //     get
        //     {
        //         return gaid;
        //     }
        //     set
        //     {
        //         if(gaid != value)
        //         {
        //             gaid = value;
        //             SaveManager.Instance.LocalVersion++;
        //             
        //             
        //         }
        //     }
        // }
        // ---------------------------------//
        
        // 
        // [JsonProperty]
        // int platform;
        // [JsonIgnore]
        // public int Platform
        // {
        //     get
        //     {
        //         return platform;
        //     }
        //     set
        //     {
        //         if(platform != value)
        //         {
        //             platform = value;
        //             SaveManager.Instance.LocalVersion++;
        //             
        //             
        //         }
        //     }
        // }
        // ---------------------------------//
        
        // 
        // [JsonProperty]
        // string locale = "";
        // [JsonIgnore]
        // public string Locale
        // {
        //     get
        //     {
        //         return locale;
        //     }
        //     set
        //     {
        //         if(locale != value)
        //         {
        //             locale = value;
        //             SaveManager.Instance.LocalVersion++;
        //             
        //             
        //         }
        //     }
        // }
        // ---------------------------------//
        
        // 
        // [JsonProperty]
        // string origLocale = "";
        // [JsonIgnore]
        // public string OrigLocale
        // {
        //     get
        //     {
        //         return origLocale;
        //     }
        //     set
        //     {
        //         if(origLocale != value)
        //         {
        //             origLocale = value;
        //             SaveManager.Instance.LocalVersion++;
        //             
        //             
        //         }
        //     }
        // }
        // ---------------------------------//
        
        // 
        // [JsonProperty]
        // string country = "";
        // [JsonIgnore]
        // public string Country
        // {
        //     get
        //     {
        //         return country;
        //     }
        //     set
        //     {
        //         if(country != value)
        //         {
        //             country = value;
        //             SaveManager.Instance.LocalVersion++;
        //             
        //             
        //         }
        //     }
        // }
        // ---------------------------------//
        
        // 
        // [JsonProperty]
        // string region = "";
        // [JsonIgnore]
        // public string Region
        // {
        //     get
        //     {
        //         return region;
        //     }
        //     set
        //     {
        //         if(region != value)
        //         {
        //             region = value;
        //             SaveManager.Instance.LocalVersion++;
        //             
        //             
        //         }
        //     }
        // }
        // ---------------------------------//
        
        // 
        // [JsonProperty]
        // string timeZone = "";
        // [JsonIgnore]
        // public string TimeZone
        // {
        //     get
        //     {
        //         return timeZone;
        //     }
        //     set
        //     {
        //         if(timeZone != value)
        //         {
        //             timeZone = value;
        //             SaveManager.Instance.LocalVersion++;
        //             
        //             
        //         }
        //     }
        // }
        // ---------------------------------//
        
        // 
        [JsonProperty]
        string resVersion = "";
        [JsonIgnore]
        public string ResVersion
        {
            get
            {
                return resVersion;
            }
            set
            {
                if(resVersion != value)
                {
                    resVersion = value;
                    SaveManager.Instance.LocalVersion++;
                    
                    
                }
            }
        }
        // ---------------------------------//
        
        // 
        // [JsonProperty]
        // string naviveVersion = "";
        // [JsonIgnore]
        // public string NaviveVersion
        // {
        //     get
        //     {
        //         return naviveVersion;
        //     }
        //     set
        //     {
        //         if(naviveVersion != value)
        //         {
        //             naviveVersion = value;
        //             SaveManager.Instance.LocalVersion++;
        //             
        //             
        //         }
        //     }
        // }
        // ---------------------------------//
        
        // 
        // [JsonProperty]
        // string deviceModel = "";
        // [JsonIgnore]
        // public string DeviceModel
        // {
        //     get
        //     {
        //         return deviceModel;
        //     }
        //     set
        //     {
        //         if(deviceModel != value)
        //         {
        //             deviceModel = value;
        //             SaveManager.Instance.LocalVersion++;
        //             
        //             
        //         }
        //     }
        // }
        // ---------------------------------//
        
        // 
        // [JsonProperty]
        // string deviceType = "";
        // [JsonIgnore]
        // public string DeviceType
        // {
        //     get
        //     {
        //         return deviceType;
        //     }
        //     set
        //     {
        //         if(deviceType != value)
        //         {
        //             deviceType = value;
        //             SaveManager.Instance.LocalVersion++;
        //             
        //             
        //         }
        //     }
        // }
        // ---------------------------------//
        
        // 
        // [JsonProperty]
        // string deviceMemory = "";
        // [JsonIgnore]
        // public string DeviceMemory
        // {
        //     get
        //     {
        //         return deviceMemory;
        //     }
        //     set
        //     {
        //         if(deviceMemory != value)
        //         {
        //             deviceMemory = value;
        //             SaveManager.Instance.LocalVersion++;
        //             
        //             
        //         }
        //     }
        // }
        // ---------------------------------//
        
        // 
        // [JsonProperty]
        // string inviteCode = "";
        // [JsonIgnore]
        // public string InviteCode
        // {
        //     get
        //     {
        //         return inviteCode;
        //     }
        //     set
        //     {
        //         if(inviteCode != value)
        //         {
        //             inviteCode = value;
        //             SaveManager.Instance.LocalVersion++;
        //             
        //             
        //         }
        //     }
        // }
        // ---------------------------------//
        
        // 
        // [JsonProperty]
        // SaveDictionary<string,string> abtests = new SaveDictionary<string,string>();
        // [JsonIgnore]
        // public SaveDictionary<string,string> Abtests
        // {
        //     get
        //     {
        //         return abtests;
        //     }
        // }
        // // ---------------------------------//
        //
        // // 
        // [JsonProperty]
        // string firebaseInstanceId = "";
        // [JsonIgnore]
        // public string FirebaseInstanceId
        // {
        //     get
        //     {
        //         return firebaseInstanceId;
        //     }
        //     set
        //     {
        //         if(firebaseInstanceId != value)
        //         {
        //             firebaseInstanceId = value;
        //             SaveManager.Instance.LocalVersion++;
        //             
        //             
        //         }
        //     }
        // }
        // ---------------------------------//
        
        // 
        // [JsonProperty]
        // string appleAccountId = "";
        // [JsonIgnore]
        // public string AppleAccountId
        // {
        //     get
        //     {
        //         return appleAccountId;
        //     }
        //     set
        //     {
        //         if(appleAccountId != value)
        //         {
        //             appleAccountId = value;
        //             SaveManager.Instance.LocalVersion++;
        //             
        //             
        //         }
        //     }
        // }
        // // ---------------------------------//
        //
        // // 广告预测用户组
        // [JsonProperty]
        // int adsPredictUserGroup;
        // [JsonIgnore]
        // public int AdsPredictUserGroup
        // {
        //     get
        //     {
        //         return adsPredictUserGroup;
        //     }
        //     set
        //     {
        //         if(adsPredictUserGroup != value)
        //         {
        //             adsPredictUserGroup = value;
        //             SaveManager.Instance.LocalVersion++;
        //             
        //             
        //         }
        //     }
        // }
        // ---------------------------------//
        
        // // 市场追踪相关
        // [JsonProperty]
        // StorageMarketing marketing = new StorageMarketing();
        // [JsonIgnore]
        // public StorageMarketing Marketing
        // {
        //     get
        //     {
        //         return marketing;
        //     }
        // }
        // ---------------------------------//
        
        // // 市场广告分组
        // [JsonProperty]
        // bool adsUserGroupMarketingOverride;
        // [JsonIgnore]
        // public bool AdsUserGroupMarketingOverride
        // {
        //     get
        //     {
        //         return adsUserGroupMarketingOverride;
        //     }
        //     set
        //     {
        //         if(adsUserGroupMarketingOverride != value)
        //         {
        //             adsUserGroupMarketingOverride = value;
        //             SaveManager.Instance.LocalVersion++;
        //             
        //             
        //         }
        //     }
        // }
        // ---------------------------------//
        //
        // // 玩家活跃统计数据
        // [JsonProperty]
        // ActiveData activeData = new ActiveData();
        // [JsonIgnore]
        // public ActiveData ActiveData
        // {
        //     get
        //     {
        //         return activeData;
        //     }
        // }
        // ---------------------------------//
        
        // // 广告计数重置时间
        // [JsonProperty]
        // long adCountResetTime;
        // [JsonIgnore]
        // public long AdCountResetTime
        // {
        //     get
        //     {
        //         return adCountResetTime;
        //     }
        //     set
        //     {
        //         if(adCountResetTime != value)
        //         {
        //             adCountResetTime = value;
        //             SaveManager.Instance.LocalVersion++;
        //             
        //             
        //         }
        //     }
        // }
        // ---------------------------------//
        
        // // 广告计数阶段状态
        // [JsonProperty]
        // int adCountStageState;
        // [JsonIgnore]
        // public int AdCountStageState
        // {
        //     get
        //     {
        //         return adCountStageState;
        //     }
        //     set
        //     {
        //         if(adCountStageState != value)
        //         {
        //             adCountStageState = value;
        //             SaveManager.Instance.LocalVersion++;
        //             
        //             
        //         }
        //     }
        // }
        // ---------------------------------//
        
        // // 激励视频累计观看次数
        // [JsonProperty]
        // int adRewardWatchedCount;
        // [JsonIgnore]
        // public int AdRewardWatchedCount
        // {
        //     get
        //     {
        //         return adRewardWatchedCount;
        //     }
        //     set
        //     {
        //         if(adRewardWatchedCount != value)
        //         {
        //             adRewardWatchedCount = value;
        //             SaveManager.Instance.LocalVersion++;
        //             
        //             
        //         }
        //     }
        // }
        // ---------------------------------//
        
        // // 服务器分组结果
        // [JsonProperty]
        // SaveDictionary<string,uint> remoteGroupIdDatas = new SaveDictionary<string,uint>();
        // [JsonIgnore]
        // public SaveDictionary<string,uint> RemoteGroupIdDatas
        // {
        //     get
        //     {
        //         return remoteGroupIdDatas;
        //     }
        // }
        // ---------------------------------//
         
        // // 配置中心缓存，以模块为单位
        // [JsonProperty]
        // SaveDictionary<string,SaveConfigHub> configHub = new SaveDictionary<string,SaveConfigHub>();
        // [JsonIgnore]
        // public SaveDictionary<string,SaveConfigHub> ConfigHub
        // {
        //     get
        //     {
        //         return configHub;
        //     }
        // }
        // ---------------------------------//
        //
        // // SOURCECAMPAIGN
        // [JsonProperty]
        // int campaignTypeCode;
        // [JsonIgnore]
        // public int CampaignTypeCode
        // {
        //     get
        //     {
        //         return campaignTypeCode;
        //     }
        //     set
        //     {
        //         if(campaignTypeCode != value)
        //         {
        //             campaignTypeCode = value;
        //             SaveManager.Instance.LocalVersion++;
        //             
        //             
        //         }
        //     }
        // }
        // ---------------------------------//
        
        // 拓展,各项目自己维护key value
        [JsonProperty]
        SaveDictionary<string,string> extensions = new SaveDictionary<string,string>(true, true);
        [JsonIgnore]
        public SaveDictionary<string,string> Extensions
        {
            get
            {
                return extensions;
            }
        }
        // ---------------------------------//
        
    }
}