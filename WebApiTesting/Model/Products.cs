namespace WebApiTesting.Model
{
    public class Products
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl {  get; set; }
        public double price {  get; set; }
        public List<Sizes_Quantity> sizes_Quantities { get; set; }
        public List<Transactions> transactions { get; set; }
        public List<AddToCart> addToCarts { get; set; }
        public List<Inventory> inventories { get; set; }
    }
}
