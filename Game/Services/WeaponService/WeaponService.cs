using Game.Dtos.Character;
using Game.Dtos.Weapon;

namespace Game.Services.WeaponService;

public class WeaponService : IWeaponService
{
    private readonly DataContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;

    public WeaponService(DataContext context, IHttpContextAccessor httpContextAccessor, IMapper mapper)
    {
        _context= context;
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;
    }
    public async Task<ServiceResponse<GetCharacterDto>> AddWeapon(AddWeaponDto newWeapon)
    {
        var response = new ServiceResponse<GetCharacterDto>();
        try
        {
            var character = await _context.Characters
                .FirstOrDefaultAsync(c => c.Id == newWeapon.CharacterId &&
                c.User!.Id == int.Parse(_httpContextAccessor.HttpContext!.User
                .FindFirstValue(ClaimTypes.NameIdentifier)!));

            if (character is null)
            {
                response.Succes = false;
                response.Message = "Character not found!";
                return response;
            }

            var weapon = _mapper.Map<Weapon>(newWeapon);
            _context.Weapons.Add(weapon);
            await _context.SaveChangesAsync();

            response.Data = _mapper.Map<GetCharacterDto>(character);
        }
        catch (Exception ex)
        {
            response.Succes = false;
            response.Message = ex.Message;
        }

        return response;
    }
}
