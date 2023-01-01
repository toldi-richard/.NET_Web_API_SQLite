using Game.Dtos.Character;
using Game.Dtos.Weapon;

namespace Game.Services.WeaponService;

public interface IWeaponService
{
    Task<ServiceResponse<GetCharacterDto>> AddWeapon(AddWeaponDto newWeapon);
}
