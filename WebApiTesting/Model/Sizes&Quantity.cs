using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiTesting.Model
{
    public class Sizes_Quantity
    {
        public int Id {  get; set; }
        public int Product_id {  get; set; }
        [ForeignKey(nameof(Product_id))]
        public Products Products { get; set; }
        
        public int Quantity {  get; set; }
    }
}
