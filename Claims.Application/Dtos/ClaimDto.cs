using Claims.Application.Enums;
using Newtonsoft.Json;

namespace Claims.Application.Dtos;

public class ClaimDto
{
    [JsonProperty(PropertyName = "coverId")]
    public string CoverId { get; set; }

    [JsonProperty(PropertyName = "created")]
    public DateTime Created { get; set; }

    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; }

    [JsonProperty(PropertyName = "claimType")]
    public ClaimType Type { get; set; }

    [JsonProperty(PropertyName = "damageCost")]
    public decimal DamageCost { get; set; }
}