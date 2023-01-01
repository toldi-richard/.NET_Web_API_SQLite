using Game.Dtos.Character;
using Game.Services.UserService;

namespace Game.Services.CharacterService;

public class CharacterService : ICharacterService
{
    private readonly IMapper _mapper;
    private readonly DataContext _context;
    private readonly IUserService _userService;
    public CharacterService(IMapper mapper, DataContext context, IUserService userService)
    {
        _mapper = mapper;
        _context = context;
        _userService = userService;
    }

    public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
    {
        var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
        var character = _mapper.Map<Character>(newCharacter);
        character.User = await _context.Users.FirstOrDefaultAsync(u => u.Id == _userService.GetUserId());
        
        _context.Characters.Add(character);
        await _context.SaveChangesAsync();

        serviceResponse.Data = await _context.Characters
            .Where(u => u.User!.Id == _userService.GetUserId())
            .Select(x => _mapper.Map<GetCharacterDto>(x))
            .ToListAsync();
        return serviceResponse;
    }

    public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
    {
        ServiceResponse<List<GetCharacterDto>> response = new();
        try
        {
            Character character = await _context.Characters.FirstOrDefaultAsync(c => c.Id == id && c.User!.Id == _userService.GetUserId());
            if (character is null)
            {
                throw new Exception($"Character with Id '{id}' not found.");
            }

            _context.Characters.Remove(character);
            await _context.SaveChangesAsync();
            response.Data = await _context.Characters
                .Where(c => c.User!.Id == _userService.GetUserId())
                .Select(c => _mapper.Map<GetCharacterDto>(c)).ToListAsync();
        }
        catch (Exception ex)
        {
            response.Succes = false;
            response.Message = ex.Message;
        }

        return response;
    }

    public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
    {
        var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
        var dbCharacters = await _context.Characters
            .Where(c => c.User!.Id == _userService.GetUserId())
            .Include(c => c.Weapon)
            .Include(c => c.Skills)
            .ToListAsync();
        serviceResponse.Data = dbCharacters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
        return serviceResponse;
    }

    public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
    {
        var serviceResponse = new ServiceResponse<GetCharacterDto>();
        var dbCharacter = await _context.Characters
            .Include(c => c.Weapon)
            .Include(c => c.Skills)
            .FirstOrDefaultAsync(x => x.Id == id && x.User!.Id == _userService.GetUserId());
        serviceResponse.Data = _mapper.Map<GetCharacterDto>(dbCharacter);
        return serviceResponse;
    }

    public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
    {
        ServiceResponse<GetCharacterDto> response = new();
        try
        {
            Character character = await _context.Characters
                .Include(c => c.User!)
                .FirstOrDefaultAsync(c => c.Id == updatedCharacter.Id);
            if (character is null || character.User!.Id != _userService.GetUserId())
            {
                throw new Exception($"Character with Id '{updatedCharacter.Id}' not found.");
            }

            _mapper.Map<Character>(updatedCharacter);
            _mapper.Map(updatedCharacter, character);

            await _context.SaveChangesAsync();
            response.Data = _mapper.Map<GetCharacterDto>(character);
        }
        catch (Exception ex)
        {
            response.Succes = false;
            response.Message= ex.Message;
        }

        return response;
    }

    public async Task<ServiceResponse<GetCharacterDto>> AddCharacterSkill(AddCharacterSkillDto newCharacterSkill)
    {
        var response = new ServiceResponse<GetCharacterDto>();
        try
        {
            var character = await _context.Characters
                .Include(c => c.Weapon)
                .Include(c => c.Skills)
                .FirstOrDefaultAsync(c => c.Id == newCharacterSkill.CharacterId &&
                    c.User!.Id == _userService.GetUserId());

            if (character is null)
            {
                response.Succes = false;
                response.Message = "Character not found!";
                return response;
            }

            var skill = await _context.Skills
                .FirstOrDefaultAsync(s => s.Id == newCharacterSkill.SkillId);

            if (skill is null)
            {
                response.Succes = false;
                response.Message = "Skill not found!";
                return response;
            }

            character.Skills!.Add(skill);
            await _context.SaveChangesAsync();
            response.Data = _mapper.Map<GetCharacterDto>(character);
        }
        catch (Exception ex)
        {
            response.Succes = false;
            response.Message= ex.Message;
        }

        return response;
    }
}

