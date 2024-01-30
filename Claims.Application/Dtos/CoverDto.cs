using Claims.Application.Enums;
using Newtonsoft.Json;

namespace Claims.Application.Dtos
{
    public class CoverDto
    {
        [JsonProperty(PropertyName = "startDate")]
        public DateOnly StartDate { get; set; }

        [JsonProperty(PropertyName = "endDate")]
        public DateOnly EndDate { get; set; }

        [JsonProperty(PropertyName = "coverType")]
        public CoverType Type { get; set; }
    }
}