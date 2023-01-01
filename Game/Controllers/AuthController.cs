using Game.Dtos.User;

namespace Game.Controllers;

[ApiController]
[Route("controller")]
public class AuthController : ControllerBase
{
    public readonly IAuthRepository _authRepo;

    public AuthController(IAuthRepository authRepo)
    {
        _authRepo = authRepo;
    }

    [HttpPost("Register")]
    public async Task<ActionResult<ServiceResponse<int>>> Register(UserRegisterDto request)
    {
        var response = await _authRepo.Register
        (
            new User 
            { 
                Username = request.UserName 
            }, request.Password
        );

        if (!response.Succes)
        {
            return BadRequest(response);
        }
        return Ok(response);
    }

    [HttpPost("Login")]
    public async Task<ActionResult<ServiceResponse<int>>> Login(UserLoginDto request)
    {
        var response = await _authRepo.Login(request.UserName, request.Password);

        if (!response.Succes)
        {
            return BadRequest(response);
        }
        return Ok(response);
    }
}
