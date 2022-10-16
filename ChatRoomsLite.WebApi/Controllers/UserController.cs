using AutoMapper;
using ChatRoomsLite.Application.Authorization;
using ChatRoomsLite.Application.InvitationService;
using ChatRoomsLite.Models;
using ChatRoomsLite.Models.Entities.User;
using ChatRoomsLite.Models.Entities.User.DTOs;
using ChatRoomsLite.Models.Repositories;
using ChatRoomsLite.Models.SearchCriterias;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatRoomsLite.WebApi.Controllers;

public class UserController : BaseController
{
    private readonly IUserRepository _userRepository;
    private readonly IRoomInvitationService _roomInvitationService;
    private readonly IMapper _mapper;
    
    public UserController(IUserRepository userRepository, IMapper mapper, IRoomInvitationService roomInvitationService)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _roomInvitationService = roomInvitationService;
    }
    
    [HttpGet]
    //[Authorize]
    public Task<ActionResult<IEnumerable<UserDto>>> Get()
    {
        var users = _userRepository.GetAll();
        return Task.FromResult<ActionResult<IEnumerable<UserDto>>>(Ok(_mapper.Map<IEnumerable<UserDto>>(users)));
    }
    
    [HttpGet("search")]
    //[Authorize]
    public Task<ActionResult<IEnumerable<UserDto>>> Get([FromQuery] UserSearchCriteria searchCriteria)
    {
        var users = _userRepository.Find(searchCriteria);
        return Task.FromResult<ActionResult<IEnumerable<UserDto>>>(Ok(_mapper.Map<IEnumerable<UserDto>>(users)));
    }
    
    [HttpGet("{id}")]
    //[Authorize]
    public Task<ActionResult<UserDto>> Get(int id)
    {
        var user = _userRepository.Get(id);
        return Task.FromResult<ActionResult<UserDto>>(Ok(_mapper.Map<UserDto>(user)));
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public Task<IActionResult> Login(AuthModel authModel, [FromServices] ITokenService tokenService)
    {
        var success = _userRepository.TryLogin(authModel, out var user);
        if (!success)
            return Task.FromResult<IActionResult>(Unauthorized());
        var token = tokenService.CreateToken(user!);
        return Task.FromResult<IActionResult>(Ok(new {token}));
    }
    
    [HttpPost("register")]
    [AllowAnonymous]
    public Task<IActionResult> Register(UserRegisterDto userRegisterDto)
    {
        var user = _mapper.Map<User>(userRegisterDto);
        if (_userRepository.Find(new UserSearchCriteria {Username = userRegisterDto.Username}).Any()) 
            return Task.FromResult<IActionResult>(BadRequest("Username already exists"));
        
        _userRepository.Add(user);
        return Task.FromResult<IActionResult>(Ok());
    }
    
    [HttpGet("invitation/{roomName}")]
    public Task<IActionResult> GetInvitation(string roomName)
    {
        var invitation = _roomInvitationService.CreateInvitation(new InvitationModel{RoomName = roomName});
        return Task.FromResult<IActionResult>(Ok(invitation));
    }
}