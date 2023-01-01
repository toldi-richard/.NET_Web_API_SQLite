using Game.Dtos.Character;
using Game.Dtos.Fight;
using Game.Dtos.Skill;
using Game.Dtos.Weapon;

namespace Game;

public class AutoMapperProfile : Profile
{
	public AutoMapperProfile()
	{
		CreateMap<Character, GetCharacterDto>();
        CreateMap<AddCharacterDto, Character>();
        CreateMap<UpdateCharacterDto, Character>();
        CreateMap<AddWeaponDto, Weapon>();
        CreateMap<Weapon, GetWeaponDto>();
        CreateMap<Skill, GetSkillDto>();
        CreateMap<Character, HighscoreDto>();
    }
}
