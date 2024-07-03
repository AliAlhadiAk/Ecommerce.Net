using System.Security.Claims;
using WebApiTesting.DTO_s;
using WebApiTesting.Model;

namespace WebApiTesting.Interfaces
{
    public interface IAuthService
    {
        public Task<Response> Login(LoginDTO dto);
        public Task<bool> SignUp(RegisterDTO dto);
        public string GenerateToken (List<Claim> claims);
        public Task<bool> SeedRoles();
        public Task<bool> RefreshTokenrequest (string Refresh_token);
        public Task<bool> ForgotPass(string Email);
        public Task<bool> ResetPass(ResetPassDTO dto);

        public void SendEmail(string To);
        object Login();
    }
}
