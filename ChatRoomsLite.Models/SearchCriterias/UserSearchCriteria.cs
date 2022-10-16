using ChatRoomsLite.Models.Entities.User;

namespace ChatRoomsLite.Models.SearchCriterias;

public class UserSearchCriteria : SearchCriteria<User>
{
    public string? Username { get; set; }
    public string? Email { get; set; }

    public override bool Predicate(User entity)
    {
        if (!base.Predicate(entity)) return false;
        if (!string.IsNullOrEmpty(Username) && !entity.Username.Contains(Username)) return false;
        if (!string.IsNullOrEmpty(Email) && !entity.Email.Contains(Email)) return false;
        return true;
    }
}