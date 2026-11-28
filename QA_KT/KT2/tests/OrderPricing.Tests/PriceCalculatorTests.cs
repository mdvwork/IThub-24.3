using Microsoft.VisualStudio.TestTools.UnitTesting;
using OrderPricing;

namespace OrderPricing.Tests;

[TestClass]
public class PriceCalculatorTests
{
    [TestMethod]
    public void RegularOrder_1000WithoutLoyalty_TotalIs1300()
    {
        // Arrange: данные и ожидание, рассчитанное по требованиям.
        decimal subtotal = 1000m;
        decimal expected = 1300m;

        // Act: один вызов проверяемого поведения.
        decimal actual = PriceCalculator.CalculateTotal(subtotal, false, false);

        // Assert: сравнение ожидания и результата.
        Assert.AreEqual(expected, actual);
    }

    // Добавьте собственные тесты. Исходный модуль менять нельзя.
}
