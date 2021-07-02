using Newtonsoft.Json;

namespace TaskManager
{
    public class CFile
    {
        [JsonProperty("15")]
        public int ID { get; set; }
        
        [JsonProperty("16")]
        public int TaskID { get; set; }

        [JsonProperty("17")]
        public string FullPath { get; set; }

        [JsonProperty("18")]
        public string AdditionalPath { get; set; }

        [JsonProperty("19")]
        public string Name { get; set; }

        [JsonProperty("20")]
        public byte[] Hash { get; set; }

        public CFile(int iD, int taskID, string fullPath, string additionalPath, string name, byte[] hash)
        {
            ID = iD;
            TaskID = taskID;
            FullPath = fullPath;
            AdditionalPath = additionalPath;
            Name = name;
            Hash = hash;
        }
    }
}
