using Microsoft.AspNetCore.Identity;

namespace WebApiTesting.Model
{
    public class User:IdentityUser
    {
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiry { get; set; }
        public string? ResetToken { get; set; }
        public DateTime? ResetTokenExpiry { get; set; }
   
      
        public List<Transactions> Carts { get; set; }
        public List<Inventory> Books { get; set; }
        public List<AddToCart> addToCarts { get; set; }
    }
}
