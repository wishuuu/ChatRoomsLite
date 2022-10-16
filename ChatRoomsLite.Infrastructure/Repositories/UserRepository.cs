using ChatRoomsLite.Models;
using ChatRoomsLite.Models.Entities.User;
using ChatRoomsLite.Models.Repositories;

namespace ChatRoomsLite.Infrastructure.Repositories;

public class UserRepository : EntityRepository<User>, IUserRepository
{
    public UserRepository(DataContext context) : base(context)
    {
    }

    public bool TryLogin(AuthModel authModel, out User? user)
    {
        user = DbSet.FirstOrDefault(u => u.Username == authModel.Username && u.Password == authModel.Password);
        return user != null;
    }
}