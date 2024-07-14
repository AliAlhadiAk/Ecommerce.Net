using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Net;
using System.Security.Claims;
using WebApiTesting.DbContext;
using WebApiTesting.DTO_s;
using WebApiTesting.Model;
using System.Reflection.Metadata.Ecma335;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace WebApiTesting.Interfaces
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _appDbContext;
        private readonly JwtAuthentication _jwt;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<AuthService> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;
        public readonly IConfiguration Configuration;
        public AuthService(AppDbContext appDbContext, JwtAuthentication jwt, UserManager<User> userManager, ILogger<AuthService> logger, RoleManager<IdentityRole> roleManager, IConfiguration configurationManager)
        {
            _appDbContext = appDbContext;
            _jwt = jwt;
            _userManager = userManager;
            _logger = logger;
            _roleManager = roleManager;
            configurationManager = Configuration;
            
        }

        public async Task<bool> ForgotPass(string Email)
        {
            var response = false;
            var checkEmail = await _userManager.FindByEmailAsync(Email);
            if (checkEmail == null)
            {
                response = false;
            }
            var ResetToken = await _userManager.GeneratePasswordResetTokenAsync(checkEmail);

            // await SendPasswordResetEmail(Email, "www.google.com");
            checkEmail.ResetToken = ResetToken;
            checkEmail.ResetTokenExpiry = DateTime.UtcNow.AddDays(1);
            SendEmail(Email);
            _logger.LogInformation("Send EmailSuccefully");
            response = true;
            await _appDbContext.SaveChangesAsync();
            return response;
        }

        public string GenerateToken(List<Claim> claims)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwt.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var tokenCreate = tokenHandler.CreateToken(tokenDescriptor);

            var token = tokenHandler.WriteToken(tokenCreate);
            return token.ToString();
        }

        public async Task<Response> Login(LoginDTO dto)
        {

            var response = false;
            var checkEmail = await _userManager.FindByEmailAsync(dto.Email);
            if (checkEmail == null)
            {
                response = false;
                return null;
            }

            var checkPass = await _userManager.CheckPasswordAsync(checkEmail, dto.Password);
            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, dto.Email),
                new Claim(JwtRegisteredClaimNames.Name, dto.Email)
            };

            var userRoles = await _userManager.GetRolesAsync(checkEmail);

            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            if (checkPass)
            {
                var token = GenerateToken(claims);
                var refresh = Guid.NewGuid().ToString();
                checkEmail.RefreshToken = refresh;
                checkEmail.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
                await _appDbContext.SaveChangesAsync();
                return new Response()
                {
                    UserId = checkEmail.Id.ToString(),
                    Token = token,
                    RefreshToen = refresh,
                    Role = userRoles.ToString()
                };
                response = true ;
            }
            return null;
          
        }

        public async Task<bool> RefreshTokenrequest(string Refresh_token)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ResetPass(ResetPassDTO dto)
        {
            bool response;
            var checkEmail = await _appDbContext.Users.FirstOrDefaultAsync(x => x.ResetToken == dto.token);
            if (checkEmail == null)
            {
                response = false;
            }
            await _userManager.ResetPasswordAsync(checkEmail, dto.token, dto.ConfirmPass);
            response = true;
            return response;
        }

        public async Task<bool> SeedRoles()
            {

                bool response = true;
                bool checkAdmin = await _roleManager.RoleExistsAsync(UserROLES.Admin);
                var checkUser = await _roleManager.RoleExistsAsync(UserROLES.User);

                if (checkAdmin && checkUser)
                {
                    response = false;
                }

                await _roleManager.CreateAsync(new IdentityRole(UserROLES.Admin));
                await _roleManager.CreateAsync(new IdentityRole(UserROLES.User));
            return response;
            
            }

        public void SendEmail(string To)
        {
            string senderEmail = "alialhadiabokhalil@outlook.com"; // Replace with your Outlook email address
            string senderPassword = "//"; // Replace with your Outlook email password

            // Recipient's email address
            string recipientEmail = To;

            // Outlook SMTP server address and port
            string smtpServer = "smtp.office365.com";
            int smtpPort = 587; // Outlook SMTP port (TLS)

            try
            {
                // Create SMTP client and configure credentials
                using (var client = new System.Net.Mail.SmtpClient(smtpServer, smtpPort))
                {
                    client.EnableSsl = true; // Enable SSL/TLS
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential(senderEmail, senderPassword);
                    string link = "https://localhost:7189/swagger/index.html";

                    // Create email message
                    using (var message = new MailMessage(senderEmail, recipientEmail))
                    {
                        message.Subject = "Forgot Reset Password using al5aaaaaaaaaaaal smtp !!!!!!";
                        message.Body = $"To reset your password please click on the link below {link} ";
                        message.IsBodyHtml = false;

                        // Send email
                        client.Send(message);


                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send email. Error message: {ex.Message}");

            }
        }

        public async Task<bool> SignUp(RegisterDTO dto)
        {
            bool response;
            var checkEmail = await _userManager.FindByEmailAsync(dto.Email);
            if (checkEmail != null)
            {
                return response = false;
            }

            var account = new User
            {
                Email = dto.Email,
                UserName = dto.UserName,
            
            };

            var checkPass = await _userManager.CreateAsync(account, dto.Password);

            if (!checkPass.Succeeded)
            {
                var errors = string.Join(", ", checkPass.Errors.Select(e => e.Description));
                response = false;
            }

            var claims = new List<Claim>()
    {
        new Claim(JwtRegisteredClaimNames.UniqueName, Guid.NewGuid().ToString()),
        new Claim(JwtRegisteredClaimNames.Email, dto.Email),
        new Claim(JwtRegisteredClaimNames.Name, dto.Email)
    };


            if (dto.Email == "alialhadiabokhalil@gmail.com")
            {
                var roleResult = await _userManager.AddToRoleAsync(account, UserROLES.Admin);
                if (!roleResult.Succeeded)
                {
                    var errors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                    response = false;
                }
            }
            else
            {
                var roleResult = await _userManager.AddToRoleAsync(account, UserROLES.User);
                if (!roleResult.Succeeded)
                {
                    var errors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                    response = false;
                }
            }

            var userRoles = await _userManager.GetRolesAsync(account);

            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var token = GenerateToken(claims);
            var refresh = Guid.NewGuid().ToString();
            account.RefreshToken = refresh;
            account.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
            await _appDbContext.SaveChangesAsync();
            response = true;
            return response;
        }

        public object Login()
        {
            throw new NotImplementedException();
        }
    }
}
