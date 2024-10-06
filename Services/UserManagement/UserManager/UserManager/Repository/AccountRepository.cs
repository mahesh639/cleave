using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserManager.Models;
using UserManager.Models.DTO;
using UserManager.Models.Response;
using static UserManager.Models.Response.ServiceResponse;

namespace UserManager.Repository
{
    public class AccountRepository : IAccountRespository
    {
        private readonly UserManager<UserDetails> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AccountRepository(UserManager<UserDetails> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            this._configuration = configuration;
        }

        public async Task<ServiceResponse.GeneralResponse> CreateAccount(UserDto userDto)
        {
            if(userDto == null) { return new ServiceResponse.GeneralResponse(false, "Input is empty"); }

            UserDetails newUser = new UserDetails()
            {
                Name = userDto.UserName,
                Email = userDto.EmailAddress,
                PasswordHash = userDto.Password,
                UserName = userDto.UserName
            };

            UserDetails? user = await _userManager.FindByEmailAsync(newUser.Email);
            if(user != null)
            {
                return new GeneralResponse(false, "This email is already associated with an account");
            }

            IdentityResult createUser = await _userManager.CreateAsync(newUser, userDto.Password);

            if (!createUser.Succeeded)
            {
                return new GeneralResponse(false, "Something went wrong while inserting into db \n please validate the inputs again");
            }

            //create new roles if it is not present in the db
            IdentityRole checkAdmin = await _roleManager.FindByNameAsync("Admin");
            IdentityRole checkUser = await _roleManager.FindByNameAsync("User");
            if(checkAdmin == null || checkUser == null)
            {
                if(checkAdmin == null && checkUser == null)
                {
                    await _roleManager.CreateAsync(new IdentityRole() {Name = "Admin" });
                    await _roleManager.CreateAsync(new IdentityRole() { Name = "User" });
                }
                else if(checkUser==null)
                {
                    await _roleManager.CreateAsync(new IdentityRole() { Name = "User" });
                }
                else
                {
                    await _roleManager.CreateAsync(new IdentityRole() { Name = "User" });
                }
            }

            await _userManager.AddToRoleAsync(newUser, "User");

            return new GeneralResponse(true, "User Created");
        }

        public async Task<ServiceResponse.LoginResponse> LoginAccount(LoginDto loginDto)
        {
            if(loginDto == null) { return new LoginResponse(false, string.Empty, "Please enter login details"); }

            UserDetails? userDetails = await _userManager.FindByEmailAsync(loginDto.Email);

            if(userDetails == null)
            {
                return new LoginResponse(false, string.Empty, "Email Or Password entered is incorrect");
            }

            bool passwordCheck = await _userManager.CheckPasswordAsync(userDetails, loginDto.Password);

            if(passwordCheck)
            {
                var role = await _userManager.GetRolesAsync(userDetails);
                string userRole = role.First();
                string token = GenerateToken(userDetails, userRole);
                return new LoginResponse(true, token, "Credential Authenticated, token generated");
            }
            else
            {
                return new LoginResponse(false, string.Empty, "Email Or Password is Incorrect");
            }
        }

        private string GenerateToken(UserDetails userDetails, string userRole)
        {
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            Claim[] claimArr =
            {
                new Claim(ClaimTypes.NameIdentifier, userDetails.Id),
                new Claim("Email", userDetails.Email),
                new Claim("Role", userRole),
                new Claim("Name", userDetails.Name)
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claimArr,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: signingCredentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
