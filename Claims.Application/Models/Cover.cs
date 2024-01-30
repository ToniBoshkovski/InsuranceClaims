using Claims.Application.Dtos;
using Claims.Application.Enums;
using Newtonsoft.Json;

namespace Claims.Application.Models;

public class Cover : Item
{
    [JsonProperty(PropertyName = "startDate")]
    public DateOnly StartDate { get; set; }

    [JsonProperty(PropertyName = "endDate")]
    public DateOnly EndDate { get; set; }

    [JsonProperty(PropertyName = "coverType")]
    public CoverType Type { get; set; }

    [JsonProperty(PropertyName = "premium")]
    public decimal Premium { get; set; }

    public static Cover MapToCover(CoverDto coverDto)
    {
        return new Cover
        {
            StartDate = coverDto.StartDate,
            EndDate = coverDto.EndDate,
            Type = coverDto.Type
        };
    }
}