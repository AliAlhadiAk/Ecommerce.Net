using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Formatters.Xml;
using Microsoft.EntityFrameworkCore;
using WebApiTesting.DbContext;
using WebApiTesting.Model;

namespace WebApiTesting.Interfaces
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _appDbContext;
        private readonly ILogger<ProductService> _logger;
        private readonly UserManager<User> _userManager;
        private readonly ICacheService _cacheService;

        public ProductService(AppDbContext appDbContext, ILogger<ProductService> logger, UserManager<User> userManager,ICacheService caheService)
        {
            _appDbContext = appDbContext;
            _logger = logger;
            _userManager = userManager;
            _cacheService = caheService;
        }

        public async Task<bool> AddToCart(Guid userId, int productId)
        {
            bool response = false;

            // Check if the user exists
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return false; // User does not exist
            }

            // Check if the product exists
            var product = await _appDbContext.Products.FirstOrDefaultAsync(p => p.Id == productId);
            var checkifProductAlreadyAdded = await _appDbContext.AddToCart.FirstOrDefaultAsync(x => x.User_Id == userId.ToString() && x.ProductId == productId);
    
            if (product == null)
            {
                return false; // Product does not exist
            }

            // Check if there's sufficient quantity for any size of the product
            var sizeQuantities = await _appDbContext.Sizes
                                                .FirstOrDefaultAsync(sq => sq.Product_id == productId);
            if (sizeQuantities != null && sizeQuantities.Quantity > 0)
            {
                response = true;
                if (checkifProductAlreadyAdded == null)
                {
                    var addCart = new AddToCart
                    {
                        User_Id = userId.ToString(),
                        ProductId = productId,
                    };
                    await _appDbContext.AddToCart.AddAsync(addCart);
                }
                var addToCart = await _appDbContext.AddToCart.Where(x => x.User_Id == userId.ToString()).Select(x => x.Products).ToListAsync();
                checkifProductAlreadyAdded.Quantity += 1;
                _cacheService.UpdateData<List<Products>>($"addToCart_{userId.ToString()}",addToCart,DateTime.UtcNow.AddMinutes(2));
                await _appDbContext.SaveChangesAsync();
            }
            else
            {
                response = false; 
            }

            return response;
        }

        public async Task<List<Products>> GetaCartUser(Guid User_id)
        {
            var CheckUser = await _userManager.FindByIdAsync(User_id.ToString());
            if (CheckUser == null)
            {
                return null;
            }
            var CartUser = await _appDbContext.AddToCart.Where(x => x.User_Id == User_id.ToString()).Select(x => x.Products).ToListAsync();
            if (CartUser == null)
            {
                return new List<Products>();
                _logger.LogInformation("Users Cart is empty");
            }
            return CartUser;

        }

        public async Task<List<Products>> GetProducts()
        {
            var products = await _appDbContext.Products.ToListAsync();
            return products;
        }

        public async Task<bool> Purchase(Guid User_id, int Product_id)
        {
            bool response = false;
            var CheckUser = await _userManager.FindByIdAsync(User_id.ToString());
            if (CheckUser == null)
            {
                response = false;
            }
            var CheckProduct = await _appDbContext.Products.FirstOrDefaultAsync(x => x.Id == Product_id);
            if (CheckProduct == null)
            {
                response = false;
            }

            var productSize = await _appDbContext.Sizes
                .FirstOrDefaultAsync(ps => ps.Product_id == Product_id);
            var checkPoductOrderHistory = await _appDbContext.OrderHistory.FirstOrDefaultAsync(x => x.User_id == User_id.ToString() && x.ProductId == Product_id);
            if (checkPoductOrderHistory == null)
            {
                var newOrder = new OrderHistory
                {
                    User_id = User_id.ToString(),
                    ProductId = Product_id,
                };
                await _appDbContext.OrderHistory.AddAsync(newOrder);
                await _appDbContext.SaveChangesAsync();
            }
            checkPoductOrderHistory.Quanity += 1;
            await _appDbContext.SaveChangesAsync();
            var inventory = new Inventory
            {
                Product_id = Product_id,
                User_Id = User_id.ToString()
        };
            await _appDbContext.AddAsync(inventory);
            await _appDbContext.SaveChangesAsync();
            if (productSize != null && productSize != null && productSize.Quantity > 0)
            {
                // Decrease quantity by one
                productSize.Quantity--;

                // Save changes to the database
                await _appDbContext.SaveChangesAsync();

                response = true;// Purchase successful
            }
          //  var checkProductHistoryexists = await _appDbContext.h
            return response;


        }

        public async Task<bool> RemovefromCart(Guid User_id, int Product_Id)
        {
            var response = false;
            var checkUser = await _userManager.FindByIdAsync(User_id.ToString());
            var checkProduct = await _appDbContext.Products.FirstOrDefaultAsync(x => x.Id == Product_Id);
            if ( checkProduct == null || checkUser == null)
            {
                response = false;
            }
            var removeSpecificProduct = _appDbContext.AddToCart.Where(x => x.User_Id == User_id.ToString() && x.ProductId == Product_Id);
            _appDbContext.Remove(removeSpecificProduct);
            await _appDbContext.SaveChangesAsync();
            return response;
        }
        public async Task<bool> addProductAdmin(Products products)
        {
            try
            {
                var response = false;

                var addrpoduct = new Products()
                {
                    Name = products.Name,
                    Description = products.Description,
                    price = products.price,
                    ImageUrl = products.ImageUrl,
                };
                await _appDbContext.AddAsync(addrpoduct);
                await _appDbContext.SaveChangesAsync();
                response = true;
                return response;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

       
    }
}
