using WebApiTesting.Model;

namespace WebApiTesting.Interfaces
{
    public interface IProductService
    {
        public Task<List<Products>> GetProducts();
        public Task<bool> AddToCart(Guid User_id, int Product_id);
        public Task<List<Products>> GetaCartUser (Guid User_id);
    }
}
