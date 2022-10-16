using ChatRoomsLite.Models;
using ChatRoomsLite.Models.Entities.User;
using ChatRoomsLite.Models.Repositories;

namespace ChatRoomsLite.Application.Authorization;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    
    public AuthService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public bool TryAuthorize(AuthModel authModel, out User? user)
    {
        var success = _userRepository.TryLogin(authModel, out user);
        if (success) return true;
        return false;
    }
}

public interface IAuthService
{
    bool TryAuthorize(AuthModel autoModel, out User? token);
}