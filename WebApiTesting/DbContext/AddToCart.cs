using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiTesting.Model
{
    public class AddToCart
    {
        public int Id { get; set; }
        public string User_Id {  get; set; }
        [ForeignKey(nameof(User_Id))]
        public User User { get; set; }
        public int ProductId { get; set; }
        [ForeignKey(nameof(ProductId))]
        public Products Products { get; set; }
        public int Quantity {  get; set; }
    }
}