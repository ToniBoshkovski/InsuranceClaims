using Newtonsoft.Json;

namespace Claims.Application.Models
{
    public abstract class Item
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "itemType")]
        public string ItemType { get; set; }
    }
}