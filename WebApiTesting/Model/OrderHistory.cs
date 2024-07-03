using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiTesting.Model
{
    public class OrderHistory
    {
        public string Id { get; set; }
        public string User_id { get; set; }
        [ForeignKey(nameof(User_id))]
        public User User { get; set; }
        public int ProductId {  get; set; }
        [ForeignKey(nameof(ProductId))]
        public Products Products { get; set; }
        public int Quanity {  get; set; }
       
    }
}
