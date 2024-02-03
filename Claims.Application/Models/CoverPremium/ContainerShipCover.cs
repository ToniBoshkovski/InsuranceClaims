﻿namespace Claims.Application.Models.CoverPremium;

public class ContainerShipCover : InsuranceCoverage
{
    public override decimal Multiplier => 1.3m;

    public override decimal GetDiscount(int insuranceDuration)
    {
        return insuranceDuration switch
        {
            < 180 => 0.02m,
            < 365 => 0.01m,
            _ => 0m
        };
    }
}