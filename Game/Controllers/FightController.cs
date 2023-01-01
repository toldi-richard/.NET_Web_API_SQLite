using Game.Dtos.Fight;

namespace Game.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class FightController : ControllerBase
{
    private readonly IFightService _fightService;

    public FightController(IFightService fightService)
    {
        _fightService = fightService;
    }

    [HttpPost("WeaponAttack")]
    public async Task<ActionResult<ServiceResponse<AttackResultDto>>> WeaponAttack (WeaponAttackDto request)
    {
        return Ok(await _fightService.WeaponAttack(request));
    }

    [HttpPost("SkillAttack")]
    public async Task<ActionResult<ServiceResponse<AttackResultDto>>> SkillAttack(SkillAttackDto request)
    {
        return Ok(await _fightService.SkillAttack(request));
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<ActionResult<ServiceResponse<FightResultDto>>> Fight(FightRequestDto request)
    {
        return Ok(await _fightService.Fight(request));
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<ServiceResponse<List<HighscoreDto>>>> GetHighscore()
    {
        return Ok(await _fightService.GetHighscore());
    }
}
