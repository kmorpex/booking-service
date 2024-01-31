using GoVilla.Domain.Apartments;
using GoVilla.Domain.Apartments.Enums;
using GoVilla.Domain.Bookings.ValueObjects;
using GoVilla.Domain.Shared.ValueObjects;

namespace GoVilla.Domain.Bookings;

public class PricingService
{
    public PricingDetails CalculatePrice(Apartment apartment, DateRange period)
    {
        var currency = apartment.Price.Currency;
        var priceForPeriod = new Money(apartment.Price.Amount * period.LengthInDays, currency);
        decimal percentageUpCharge = 0;
        foreach (var amenity in apartment.Amenities)
            percentageUpCharge += amenity switch
            {
                Amenity.SeaView => 0.10m,
                Amenity.GardenView or Amenity.MountainView or Amenity.PoolView => 0.05m,
                Amenity.Parking => 0.02m,
                Amenity.AirConditioning => 0.01m,
                _ => 0
            };

        var amenitiesUpCharge = Money.Zero(currency);
        if (percentageUpCharge > 0) amenitiesUpCharge = new Money(priceForPeriod.Amount * percentageUpCharge, currency);

        var totalPrice = Money.Zero(currency);
        totalPrice += priceForPeriod;
        if (!apartment.CleaningFee.IsZero()) totalPrice += apartment.CleaningFee;

        totalPrice += amenitiesUpCharge;

        return new PricingDetails(priceForPeriod, apartment.CleaningFee, amenitiesUpCharge, totalPrice);
    }
}