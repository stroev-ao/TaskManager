using Newtonsoft.Json;

namespace TaskManager
{
    public class CReminder
    {
        [JsonProperty("21")]
        public int Seconds { get; set; }

        [JsonProperty("22")]
        public string Comment { get; set; }

        [JsonProperty("23")]
        public bool Shown { get; set; }

        public CReminder(int seconds, string comment = null)
        {
            Seconds = seconds;
            Comment = comment;
            Shown = false;
        }
    }
}
