using InventorySystem;
using System;
using System.Xml.Linq;
using Xunit;

namespace Inventory.tests
{
    public class InventoryTests
    {
        private static readonly InventoryOrderService _orderService = new();
        private static readonly Product _exProduct1 = new Product { Id = "P100", Name = "Mechanical Keyboard", UnitPrice = 89.99m, StockQuantity = 25 };

        // Happy path tests
        [Fact]
        public void GetProduct_ValidProduct_ReturnsProductSuccessfully()
        {
            // Arrange
            _orderService.AddProduct(_exProduct1);

            // Act
            Product retrievedProduct = _orderService.GetProduct("P100");

            // Assert
            Assert.Equal(_exProduct1.Id, retrievedProduct.Id);
            Assert.Equal(_exProduct1.Name, retrievedProduct.Name);
            Assert.Equal(_exProduct1.UnitPrice, retrievedProduct.UnitPrice);
            Assert.Equal(_exProduct1.StockQuantity, retrievedProduct.StockQuantity);
        }
    }
}
