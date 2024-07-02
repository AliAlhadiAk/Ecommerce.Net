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
            var checkifProductAlreadyAdded = await _appDbContext.AddToCart.FirstOrDefaultAsync(x => x.User_id == userId.ToString() && x.Product_id == productId);
    
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
                        User_id = userId.ToString(),
                        Product_id = productId,
                    };
                    await _appDbContext.AddToCart.AddAsync(addCart);
                }
                var addToCart = await _appDbContext.AddToCart.Where(x => x.User_id == userId.ToString()).ToListAsync();
                checkifProductAlreadyAdded.Quantity += 1;
                _cacheService.UpdateData<List<Products>>($"addToCart_{User_id.ToString()}",addToCart,DateTime.UtcNow.AddMinutes(2));
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
            var CartUser = await _appDbContext.AddToCart.Where(x => x.User_id == User_id.ToString()).Select(x => x.Products).ToListAsync();
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

    }
}
