namespace Game.Services.UserService;

public class UserService : IUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public UserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    public int GetUserId() => int.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
}
