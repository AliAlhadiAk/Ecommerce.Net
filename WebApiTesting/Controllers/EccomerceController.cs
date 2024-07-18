using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MimeKit.Encodings;
using System.Security.Cryptography.X509Certificates;
using WebApiTesting.Interfaces;
using WebApiTesting.Model;

namespace WebApiTesting.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EccomerceController : ControllerBase
    {
        private readonly IProductService _productService;
        public EccomerceController(IProductService productService)
        {
            _productService = productService;
            

        }

        [HttpGet("get/products")]

        public async Task<IActionResult> GetProducts()
        {
         
            var product = await _productService.GetProducts();
            if(product == null)
            {
                return BadRequest("Ap problem occured");
            }
            if(product.Count() == 0) 
            {
                return NotFound("no products available right now");
            }
          
            return Ok(product);
        }
        [HttpPost(("AddToCart"))]
        [Authorize(Roles = "USER")]
        public async Task<IActionResult> AddToCart(Guid User_id,int Produtc_id)
        {
            
            if(!ModelState.IsValid)
            {
                return BadRequest("Invalid Credentials");
            }
            var function = await _productService.AddToCart(User_id, Produtc_id);
          
            
            if(function == false)
            {
                return BadRequest("failed to add to you cart");
            }

            
            return Ok("added to cart succefully");
        }
        [HttpPost(("GetCartUser"))]
        [Authorize(Roles = "USER")]
        public async Task<IActionResult> GetCartUser(Guid User_id)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Credentials");
            }
            var function = await _productService.GetaCartUser(User_id);
            if (function == null)
            {
                return BadRequest("User not found");
            }
            return Ok(function);
        }



        [HttpPost("Purchase")]
        [Authorize(Roles = "USER")]

        public async Task<IActionResult> Purchase(Guid User_id, int Product_id)
        {
            var purchase = await _productService.Purchase(User_id, Product_id); 
            if(purchase == false)
            {
                return BadRequest("Ann error occured while trying to purchase please try again later");
            }
            return Ok("You have purchased succefully");
        }
        [HttpPost("AddBookAdmin")]
        [Authorize(Roles = "ADMIN", Policy = "AdminAcces")]

        public async Task<IActionResult> AddProductAdmin(Products dto)
        {
            var addProducts = await _productService.addProductAdmin(dto);
            if(addProducts == false)
            {
                return BadRequest("Failed to add Product");
            }
            return Ok("Product added succefully");
        }
        [HttpPost("AddBookAdmin")]
        [Authorize(Roles = "ADMIN",Policy = "AdminAcces")]

        public async Task<IActionResult> DeletProducAdmin(int Product_Id)
        {
            var delete_product = await _productService.RemoveProductAdmin(Product_Id);
            if (delete_product == false)
            {
                return BadRequest("Failed to add product");
            }
            if (delete_product == null)
            {
                return NotFound("Product Not Found");
            }
            return Ok("Product deleted Succefully");
        }
        [HttpPost]
        [Authorize(Roles = "USER")]

        public async Task<IActionResult> GetOrderHistory(Guid User_id)
        {
            var orderHistory = await _productService.UserOrderHistory(User_id);
            if(ModelState.IsValid)
            {
                if (null == orderHistory)
                {
                    return BadRequest("User Not Found");
                }
                return Ok(orderHistory);
            }
            return BadRequest("Please fill credentials");
        }



    }
}

