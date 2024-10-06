using Microsoft.AspNetCore.Mvc;
using System.Net;
using UserManager.Models.DTO;
using UserManager.Repository;
using static UserManager.Models.Response.ServiceResponse;

namespace UserManager.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IAccountRespository _accountRepository;

        public UserController(IAccountRespository accountRespository) { 
            _accountRepository = accountRespository;
        }

        [HttpPost("/register")]
        public IActionResult Register([FromBody] UserDto user)
        {
            GeneralResponse response = _accountRepository.CreateAccount(user).ConfigureAwait(false).GetAwaiter().GetResult();

            if (response != null && response.Flag)
                return new OkObjectResult(response);
            else
                return new ObjectResult(response) { StatusCode = (int)HttpStatusCode.BadRequest };

        }

        [HttpPost]
        [Route("/login")]
        public IActionResult UserLogin([FromBody] LoginDto loginDto)
        {
            LoginResponse response = _accountRepository.LoginAccount(loginDto).ConfigureAwait(false).GetAwaiter().GetResult();

            if (response != null && response.Flag)
                return new OkObjectResult(response);
            else
                return new ObjectResult(response) { StatusCode = (int)HttpStatusCode.BadRequest };
        }
    }
}
