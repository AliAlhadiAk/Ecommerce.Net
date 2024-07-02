using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiTesting.Model
{
    public class AddToCart
    {
        public int Id { get; set; }
        public string User_id {  get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
        public int Product_id {  get; set; }
        [ForeignKey(nameof(Product_id))]
        public Products Products { get; set; }
        public int Quantity { get; set; }
    }
}
