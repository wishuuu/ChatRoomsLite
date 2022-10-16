using ChatRoomsLite.Models;
using Microsoft.AspNetCore.DataProtection;
using Newtonsoft.Json;

namespace ChatRoomsLite.Application.InvitationService;

public class RoomInvitationService : IRoomInvitationService
{
    private readonly IDataProtectionProvider _dataProtectionProvider;
    
    public RoomInvitationService(IDataProtectionProvider dataProtectionProvider)
    {
        _dataProtectionProvider = dataProtectionProvider;
    }
    
    public string CreateInvitation(InvitationModel invitation)
    {
        invitation.Expiration = DateTime.Now.AddDays(1);
        var serialized = JsonConvert.SerializeObject(invitation);
        return _dataProtectionProvider.CreateProtector("dupa1234dupa1234").Protect(serialized);
    }
    
    public bool ResolveInvitation(string invitation, out InvitationModel? invitationModel)
    {
        try
        {
            var serialized = _dataProtectionProvider.CreateProtector("dupa1234dupa1234").Unprotect(invitation);
            invitationModel = JsonConvert.DeserializeObject<InvitationModel>(serialized);
            return invitationModel?.Expiration > DateTime.Now;
        }
        catch
        {
            invitationModel = null;
            return false;
        }
    }
}

public interface IRoomInvitationService
{
    string CreateInvitation(InvitationModel invitation);
    bool ResolveInvitation(string invitation, out InvitationModel? invitationModel);
}