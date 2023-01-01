using Game.Dtos.Fight;

namespace Game.Services.FightService;

public interface IFightService
{
    Task<ServiceResponse<AttackResultDto>> WeaponAttack(WeaponAttackDto request);
    Task<ServiceResponse<AttackResultDto>> SkillAttack(SkillAttackDto request);
    Task<ServiceResponse<FightResultDto>> Fight(FightRequestDto request);
    Task<ServiceResponse<List<HighscoreDto>>> GetHighscore();

}
