using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using ShoppingCart.Controllers;
using ShoppingCart.Entities;
using ShoppingCart.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Test.Controllers
{
    public class HomeControllerTest
    {
        private readonly Mock<ILogger<HomeController>> _mockLogger;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly HomeController _controller;

        public HomeControllerTest()
        {
            _mockLogger = new Mock<ILogger<HomeController>>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();

            _controller = new HomeController(_mockLogger.Object, _mockUnitOfWork.Object);
        }

        [Fact]
        public void HomeController_Index_Returns_viewResuly_WithProductList()
        {
            //Arrange
            var productList = new List<Product>
            {
                new Product { ProductId = 1, ISBN = "JHK" },
                new Product { ProductId = 2, ISBN = "yup" }
            };

            _mockUnitOfWork.Setup(u => u.Product.GetAllExpression(It.IsAny<Expression<Func<Product, bool>>>(), "Category,ProductImages")).
                Returns(productList);
            //Act
            var result = _controller.Index();

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Product>>(viewResult.Model);
            Assert.Equal(2,model.Count());
        }

        [Fact]
        public void HomeController_Index_Returns_viewResult_WithEmptyProductList()
        {
            var productList = new List<Product>();
            

            _mockUnitOfWork.Setup(u => u.Product.GetAllExpression(It.IsAny<Expression<Func<Product, bool>>>(), "Category,ProductImages")).
                Returns(productList);
            var result = _controller.Index();
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Product>>(viewResult.Model);
            Assert.Empty(model);
        }

        [Fact]
        public void HomeController_Index_Returns_viewResult_WithNullProductList()
        {
            _mockUnitOfWork.Setup(u => u.Product.GetAllExpression(It.IsAny<Expression<Func<Product, bool>>>(), "Category,ProductImages")).
                Returns((List<Product>)null);
            var result = _controller.Index();
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Product>>(viewResult.Model);
            Assert.Empty(model);
        }

        [Fact]
        public void Details_ReturnsViewWithCorrectModel()
        {
            // Arrange
            var productId = 1;

            // Create mock data for the Product and ShoppingKart
            var mockProduct = new Product
            {
                ProductId = productId,
                Author = "Test Product",
                Price = 100
            };

            var mockShoppingKart = new ShoppingKart
            {
                Product = mockProduct,
                Count = 1,
                ProductId = productId
            };

            // Mock the unit of work
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(u => u.GetWithIncludes(It.IsAny<int>())).Returns(mockProduct);

            // Create controller instance with mocked unit of work

            // Act
            var result = _controller.Details(productId) as ViewResult;

            // Assert
            Assert.NotNull(result); // Ensure the result is not null
            var model = result.Model as ShoppingKart; // Get the model from the view
            Assert.NotNull(model); // Ensure the model is not null
            Assert.Equal(mockShoppingKart.ProductId, model.ProductId); // Ensure the ProductId matches
        }
    }
}
