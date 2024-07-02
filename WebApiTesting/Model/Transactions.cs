using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiTesting.Model
{
    public class Transactions
    {
        public int Id { get; set; }
        public int Product_Id {  get; set; }
        [ForeignKey(nameof(Product_Id))]
        public Products Products { get; set; }
        public string User_Id {  get; set; }
        public double price_paid {  get; set; }
    }
}
