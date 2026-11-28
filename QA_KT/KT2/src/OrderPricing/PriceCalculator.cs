namespace OrderPricing;

public static class PriceCalculator
{
    public static decimal CalculateTotal(decimal subtotal, bool loyalty, bool express)
    {
        if (subtotal < 0m || subtotal > 100000m)
            throw new ArgumentOutOfRangeException(nameof(subtotal));

        decimal discount = 0m;
        if (subtotal > 5000m)
            discount += 0.05m;
        if (loyalty)
            discount += 0.10m;

        decimal goodsTotal = decimal.Round(
            subtotal * (1m - discount), 2, MidpointRounding.AwayFromZero);
        decimal delivery = subtotal >= 4000m ? 0m : (express ? 500m : 300m);
        return goodsTotal + delivery;
    }
}
