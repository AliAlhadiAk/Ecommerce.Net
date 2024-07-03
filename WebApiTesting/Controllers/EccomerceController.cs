using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
        [Authorize(Roles = "ADMIN")]
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
        [ServiceFilter(typeof(ApiKeyAuthFilter))]   
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
        [Authorize(Roles = "ADMIN")]

        public async Task<IActionResult> AddProductAdmin(Products dto)
        {
            var addProducts = await _productService.addProductAdmin(dto);
            if(addProducts == false)
            {
                return BadRequest("Failed to add Product");
            }
            return Ok("Product added succefully");
        }


    }
}
