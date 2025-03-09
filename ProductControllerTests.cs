using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductAPIManager.Controller;
using ProductAPIManager.Data;
using ProductAPIManager.Models.Entities;
using Microsoft.EntityFrameworkCore.InMemory;
using ProductAPIManager.Models;
using Newtonsoft.Json.Linq;

namespace ProductAPIManagerTest
{
    public class ProductControllerTests
    {
        private readonly ApplicationDbContext _context;
        private readonly ProductsController _controller;

        public ProductControllerTests()
        {
            // Setting up in-memory database for testing
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            _context.Products.AddRange(new List<Product>
            {
                new Product { ProductId = 343437, Name = "Product A", Price = 100, Stock = 10, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
                new Product { ProductId = 343438, Name = "Product B", Price = 1000, Stock = 19, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now }
            });

            _context.SaveChanges();

            _controller = new ProductsController(_context);
        }

        [Fact]
        public async Task GetAllProducts_ProductsExist_ShouldReturnAllProducts()
        {
            // Act
            var result = await _controller.GetAllProducts();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result); // Ensure response is 200 OK
            var responseJson = Newtonsoft.Json.JsonConvert.SerializeObject(okResult.Value);
            var products = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Product>>(responseJson);

            Assert.NotNull(products);
            Assert.Equal(2, products.Count); // Two products seeded
            Assert.Contains(products, p => p.Name == "Product A");
            Assert.Contains(products, p => p.Name == "Product B");
        }

        [Fact]
        public async Task UpdateProduct_ValidRequest_ShouldReturnOk()
        {
            // Arrange
            var expectedProduct = new UpdateProductDto
            { 
                ProductId = 343437,
                Name = "Updated Product 1",
                Stock = 10,
                Price = 150M,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            // Act
            var result = await _controller.UpdateProduct(343437, expectedProduct);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;

            // Assert the returned product matches the expected product
            var product = Assert.IsType<Product>(okResult.Value); // Ensure the Value is of type Product
            Assert.Equal(expectedProduct.ProductId, 343437);
            Assert.Equal(expectedProduct.Name, product.Name);
            Assert.Equal(expectedProduct.Stock, product.Stock);
            Assert.Equal(expectedProduct.Price, product.Price);
        }

        //unit test for product not found
        [Fact]
        public async Task GetProduct_InvalidId_ShouldReturnNotFound()
        {
            // Arrange
            var invalidProductId = 999999; // Assume this ID does not exist

            // Act
            var result = await _controller.GetProductById(invalidProductId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            // Convert the response value to JSON and parse it
            var responseJson = Newtonsoft.Json.JsonConvert.SerializeObject(notFoundResult.Value);
            var response = JObject.Parse(responseJson);

            Assert.Equal("Product not found.", response["Message"]?.ToString());
            
        }

        [Fact]
        public async Task DecrementStock_ValidRequest_ShouldReturnUpdatedStock()
        {
            // Act
            var result = await _controller.DecrementStock(343437, 3); // Decrement stock by 3

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result); // Ensure response is 200 OK
            var responseJson = Newtonsoft.Json.JsonConvert.SerializeObject(okResult.Value);
            var response = JObject.Parse(responseJson);

            Assert.Equal(343437, (int)response["ProductId"]);
            Assert.Equal(7, (int)response["UpdatedStock"]); // Initial stock was 10, decremented by 3
        }

        [Fact]
        public async Task DecrementStock_InsufficientStock_ShouldReturnBadRequest()
        {
            // Act
            var result = await _controller.DecrementStock(343438, 100); // Decrement more than available stock

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result); // Ensure response is 400 BadRequest
            var responseJson = Newtonsoft.Json.JsonConvert.SerializeObject(badRequestResult.Value);
            var response = JObject.Parse(responseJson);

            Assert.Equal("Insufficient stock.", response["Message"]?.ToString());
        }

    }
}
