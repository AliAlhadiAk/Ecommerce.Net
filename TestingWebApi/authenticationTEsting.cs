using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MimeKit.Cryptography;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiTesting;
using WebApiTesting.Controllers;
using WebApiTesting.DTO_s;
using WebApiTesting.Interfaces;

namespace TestingWebApi
{
    public class AuthenticationTesting
    {
      
        [Fact]
        public async Task OnSeedRolesReturn_200_Response()
        {
            Mock<IAuthService> service = new Mock<IAuthService>();
            service.Setup(service => service.SeedRoles())
                .ReturnsAsync(true);


            var controller = new AuthenticationController(service.Object);
            var result = (OkObjectResult)await controller.SeedRoles();
            Assert.NotNull(result);
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeOfType<string>();
        }
        [Fact]
        public async Task OnSuccefullLogin_Rturn_List_Succes()
        {
            Mock<IAuthService> service = new Mock<IAuthService>();
            service.Setup(service => service.Login(new LoginDTO
            {

            } )).ReturnsAsync(false);

            var controller = new AuthenticationController(service.Object);

            var controllerGet = (BadRequestObjectResult)await controller.Login(new LoginDTO { });
            controllerGet.StatusCode.Should().Be(400);
            

                


        }
        [Fact]
        public async Task OnFailedLoginReturnBadRequest()
        {
            Mock<IAuthService> service = new Mock<IAuthService>();
            service.Setup(service=>service.Login(It.IsAny<LoginDTO>())).ReturnsAsync(true);

            var controller = new AuthenticationController(service.Object);

            var controllerGet = await controller.Login(new LoginDTO {
                Email = "alialhadiabokhalil@gmail.com",
                Password = "Ak04200_c3#"
            });
            service.Verify(service => service.Login(It.IsAny<LoginDTO>()),Times.Once);
            controllerGet.Should().BeOfType<OkObjectResult>(); 

        }
        [Fact]  
        public async Task OnSuccesfullRegisterreturn200OK()
        {
            Mock<IAuthService> service = new Mock<IAuthService>();
            service.Setup(service => service.SignUp(It.IsAny<RegisterDTO>())).ReturnsAsync(true);

            var controller = new AuthenticationController(service.Object);

            var controllerGet = await controller.SignUp(new RegisterDTO
            {
                UserName = "ALilahdi Admin",
                Email = "alialhadiabokhalil@gmail.com",
                Password = "Ak04200_c3#"
            });
            service.Verify(service => service.SignUp(It.IsAny<RegisterDTO>()), Times.Once);
            controllerGet.Should().BeOfType<OkObjectResult>();
        }
        [Fact]
        public async Task IfEmailDoesNotExistReturnNotFound204()
        {
            Mock<IAuthService> service = new Mock<IAuthService>();
            service.Setup(service => service.ForgotPass(It.IsAny<string>())).ReturnsAsync(false);

            var controller = new AuthenticationController(service.Object);

            var controllerGet = await controller.ForgotPass("alialhadiak2009@gmail.com");
            service.Verify(service => service.ForgotPass(It.IsAny<string>()), Times.Once);
            controllerGet.Should().BeOfType<NotFoundObjectResult>();
        }
    }
}
