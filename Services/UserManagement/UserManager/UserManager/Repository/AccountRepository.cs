using Microsoft.AspNetCore.Identity;
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
        private readonly IConfiguration configuration;

        public AccountRepository(UserManager<UserDetails> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            this.configuration = configuration;
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
                if(checkAdmin == null)
                {
                    await _roleManager.CreateAsync(new IdentityRole() {Name = "Admin" });
                }
                else
                {
                    await _roleManager.CreateAsync(new IdentityRole() { Name = "User" });
                }
            }

            await _userManager.AddToRoleAsync(newUser, "User");

            return new GeneralResponse(true, "User Created");
        }

        public Task<ServiceResponse.LoginResponse> LoginAccount(LoginDto loginDto)
        {
            throw new NotImplementedException();
        }
    }
}
