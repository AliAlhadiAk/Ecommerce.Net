using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Org.BouncyCastle.Asn1.Cms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiTesting.Controllers;
using WebApiTesting.Interfaces;
using WebApiTesting.Model;

namespace TestingWebApi
{
    public class EccomerceTesting
    {
        private EccomerceTesting _controller;
        private Mock<IProductService> _mockProductService;

    
        [Fact]
        public async Task OnFetchEmptyListReturnNotFouund()
        {
            var mockCache = new Mock<ICacheService>();
            //Act 
            var mockService = new Mock<IProductService>();
            mockService.Setup(service => service.GetProducts())
                .ReturnsAsync(new List<Products>());


            var controller = new EccomerceController(mockService.Object);
            var controllerEnPoint = await controller.GetProducts();

            mockService.Verify(service=>service.GetProducts(),Times.Once);
            controllerEnPoint.Should().BeOfType<NotFoundObjectResult>();
        }
        [Fact]
        public async Task OnFetchSuccesReturnListofProducts()
        {
            //Act 
            var mockCache = new Mock<ICacheService>();
            var mockService = new Mock<IProductService>();
            mockService.Setup(service => service.GetProducts())
                .ReturnsAsync(new List<Products>()
                {
                    new Products() { Id = 1,Name="T-Shirt",price =10.0,Description="aaaaaaa",ImageUrl = "https://amazon.tshirt.com"}
                });


            var controller = new EccomerceController(mockService.Object);
            var controllerEnPoint = await controller.GetProducts();

            mockService.Verify(service => service.GetProducts(), Times.Once);
            controllerEnPoint.Should().BeOfType<OkObjectResult>();
        }

        
        [Theory]
        [InlineData("2a7eaf6d-89e0-4d9c-af66-4a6f5b5b7b1c",1)]
        public async Task OnSuccesFullAddToCartReturn200_Succes(Guid id,int Produt_id)
        {
            var mockService = new Mock<IProductService>();
            mockService.Setup(service => service.AddToCart(It.IsAny<Guid>(), It.IsAny<int>()))
               .ReturnsAsync(true);

            var controller = new EccomerceController(mockService.Object);
            var addToCartfunc = await controller.AddToCart(id,Produt_id);

            mockService.Verify(service => service.AddToCart(It.IsAny<Guid>(), It.IsAny<int>()),Times.Once);
            addToCartfunc.Should().BeOfType<OkObjectResult>();
           
        }
       
        [Theory]
        [InlineData("2a7eaf6d-89e0-4d9c-af66-4a6f5b5b7b1c")]

        public async Task OnSuccefulGetCartForUserReturn200_OK(Guid user_id)
        {
            var addToCartList = new List<Products>
            {
               

                  new Products { Id = 1, Name = "T-Shirt", Description = "Comfortable cotton T-shirt", ImageUrl = "https://example.com/tshirt.jpg", price = 19.99 },
                  new Products { Id = 2, Name = "Jeans", Description = "Slim-fit denim jeans", ImageUrl = "https://example.com/jeans.jpg", price = 39.99 },
                  new Products { Id = 3, Name = "Sneakers", Description = "Sporty sneakers", ImageUrl = "https://example.com/sneakers.jpg", price = 59.99 },
                  new Products { Id = 4, Name = "Backpack", Description = "Water-resistant backpack", ImageUrl = "https://example.com/backpack.jpg", price = 29.99 },
                  new Products { Id = 5, Name = "Watch", Description = "Classic wristwatch", ImageUrl = "https://example.com/watch.jpg", price = 99 }
             };
            var mockService = new Mock<IProductService>();
            mockService.Setup(service => service.GetaCartUser(It.IsAny<Guid>()))
               .ReturnsAsync(addToCartList);

            var controller = new EccomerceController(mockService.Object);
            var GetCart =(OkObjectResult) await controller.GetCartUser(user_id);

            mockService.Verify(service => service.GetaCartUser(It.IsAny<Guid>()), Times.Once);
            GetCart.Should().NotBeNull();
            GetCart.Should().BeOfType<OkObjectResult>();
            GetCart.Value.Should().NotBeNull();
            GetCart.Value.Should().BeOfType<Task<List<Products>>>();

        }

        [Fact]
        public async Task OnFailurePurchaseReturn400Badrequest()
        {
            var mockService = new Mock<IProductService>();
            mockService.Setup(service => service.Purchase(It.IsAny<Guid>(), It.IsAny<int>()))
               .ReturnsAsync(false);

            var controller = new EccomerceController(mockService.Object);
            var Purchase = (NotFoundObjectResult)await controller.Purchase("2a7eaf6d-89e0-4d9c-af66-4a6f5b5b7b1c", 1);
            mockService.Verify(service => service.Purchase(It.IsAny<Guid>(),It.IsAny<int>()), Times.Once);
            Purchase.Should().BeOfType<NotFoundObjectResult>();

        }
        [Fact]
        public async Task AddBookAdmin()
        {
            var product = new Products
            {
                Id = 2,
                Name = "Jeans",
                Description = "Slim-fit denim jeans",
                ImageUrl = "https://example.com/jeans.jpg",
                price = 39.99

            };
            var mockService = new Mock<IProductService>();
            mockService.Setup(service => service.addProductAdmin(product))
               .ReturnsAsync(true);

            var controller = new EccomerceController(mockService.Object);
            var Purchase = (OkObjectResult)await controller.AddProductAdmin(product);
            mockService.Verify(service => service.addProductAdmin(It.IsAny<Products>()), Times.Once);
            Purchase.Should().BeOfType<OkObjectResult>();
        }
        [Fact]
        public async Task OnFAilingReturn400_BadRequest()
        {
            int product_Id = 999;
            var mockService = new Mock<IProductService>();
            mockService.Setup(service => service.RemoveProductAdmin(product_Id))
               .ReturnsAsync(null);
            var controller = new EccomerceController(mockService.Object);
            var Purchase = await controller.DeletProducAdmin(product_Id);
            mockService.Verify(service => service.addProductAdmin(It.IsAny<Products>()), Times.Once);
            Purchase.Should().BeOfType<NotFoundObjectResult>();
        }
        [Fact]
        public async Task GetOrderHistory_Returns_OkObjectResult_With_Valid_Data()
        {
        
            Guid userId = Guid.NewGuid();
            var orderHistoryData = new object();
            var mockService = new Mock<IProductService>();
            mockService.Setup(x => x.UserOrderHistory(Guid.NewGuid))
                               .ReturnsAsync(orderHistoryData);

            // Act
            var controller = new EccomerceController(mockService.Object);
            var result = await controller.GetOrderHistory(Guid.NewGuid)

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.Equal(orderHistoryData, okResult.Value);
        }

    }
}
