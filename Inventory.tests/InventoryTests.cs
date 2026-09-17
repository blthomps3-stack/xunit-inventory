using InventorySystem;
using System;
using System.Xml.Linq;
using Xunit;

namespace Inventory.tests
{
    public class InventoryTests
    {
        private static readonly InventoryOrderService _orderService = new();
        private static readonly Product _exProduct1 = new Product { Id = "P100", Name = "TestProduct", UnitPrice = 100.00m, StockQuantity = 100 };
        private static readonly Product _exProduct2 = new Product { Id = "P200", Name = "TestProduct2", UnitPrice = 0m, StockQuantity = 0 };

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

        [Theory]
        [InlineData(1, 0.0)]
        [InlineData(10, 0.10)]
        [InlineData(50, 0.20)]
        // Decimals aren't allowed to be attribute parameters (CS0182) so I used doubles instead here
        public void ProcessOrder_ValidOrder_ProcessOrderSuccessfully(int quantity, double discount)
        {
            // Arrange
            _orderService.AddProduct(_exProduct1);

            // Act
            OrderResult result = _orderService.ProcessOrder("P100", quantity, 0.25m);
            decimal expectedCost = _exProduct1.UnitPrice * quantity;
            expectedCost -= expectedCost * (decimal)discount;
            expectedCost += expectedCost * 0.25m;

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(Math.Round(expectedCost, 2), result.TotalCost);
            Assert.Equal("Order processed successfully.", result.Message);
        }

        [Fact]
        public void ProcessOrder_InsufficientStock_ReturnsUnsuccessfulOrder()
        {
            // Arrange
            _orderService.AddProduct(_exProduct1);

            // Act
            OrderResult result = _orderService.ProcessOrder("P100", 200, 0);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Insufficient stock.", result.Message);
        }

        // Edge/Boundary Case Tests
        [Fact]
        public void ProcessOrder_ZeroQuantity_ReturnsUnsuccessfulOrder()
        {
            // Arrange
            _orderService.AddProduct(_exProduct2);

            // Act
            OrderResult result = _orderService.ProcessOrder("P200", 0, 0);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Quantity must be positive.", result.Message);
        }
    }
}
