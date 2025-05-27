using System.Collections.Generic;
using Newtonsoft.Json;

namespace GameScripts.Model
{
    public class ActivityResult 
    {
        // 这个属性名必须和JSON中的"activityInfos"完全一致（包括大小写）
        public List<ActivityInfo> activityInfos { get; set; }
    }

    public class ActivityInfo 
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("activityname")] // 必须明确映射
        public string ActivityName { get; set; }

        [JsonProperty("startTime")] 
        public string StartTime { get; set; }

        [JsonProperty("endTime")]
        public string EndTime { get; set; }

        [JsonProperty("InfoJson")]
        public string InfoJson { get; set; }
    }


}