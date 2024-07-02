using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiTesting.Model
{
    public class Inventory
    {
        public int Id { get; set; }
        public string User_Id {  get; set; }
        public int Product_id {  get; set; }
        [ForeignKey(nameof(Product_id))]
        public Products Products { get; set; }

    }
}
