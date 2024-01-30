using Claims.Application.Dtos;
using Claims.Application.Enums;
using Newtonsoft.Json;

namespace Claims.Application.Models;

public class Claim : Item
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

    public static Claim MapToClaim(ClaimDto claimDto)
    {
        return new Claim
        {
            CoverId = claimDto.CoverId,
            Name = claimDto.Name,
            Type = claimDto.Type,
            DamageCost = claimDto.DamageCost
        };
    }
}