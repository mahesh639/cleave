using UserManager.Models.DTO;
using static UserManager.Models.Response.ServiceResponse;

namespace UserManager.Repository
{
    public interface IAccountRespository
    {
        Task<GeneralResponse> CreateAccount(UserDto userDto);
        Task<LoginResponse> LoginAccount(LoginDto loginDto);
    }
}
