using ChatRoomsLite.Models.Entities.User;

namespace ChatRoomsLite.Models.Repositories;

public interface IUserRepository : IEntityRepository<User>
{
    bool TryLogin(AuthModel authModel, out User? user);
    
}