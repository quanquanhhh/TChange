using Newtonsoft.Json;

namespace GameScripts.Model
{
    [System.Serializable]
    public class QueryResult
    {
        [JsonProperty]
        int level;
        [JsonIgnore]
        public int Level
        {
            get
            {
                return level;
            }
            set
            {
                if(level != value)
                {
                    level = value;
                }
            }
        }
        
    }
}