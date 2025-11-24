using AutoMapper;
using FavoritePostAPI.DTO.Request;
using FavoritePostAPI.Models;
using FavoritePostAPI.Repository.Interface;
using FavoritePostAPI.Service;
using FavoritePostAPI.Service.Interface;
using System.Security.Claims;

public class FavoritePostService : IFavoritePostService
{
    private readonly IFavoritePostRepository _repository;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;

    public FavoritePostService(
        IFavoritePostRepository repository,
        ITokenService tokenService,
        IMapper mapper)
    {
        _repository = repository;
        _tokenService = tokenService;
        _mapper = mapper;
    }

    public async Task<FavoritePostDTO> AddFavoriteAsync(ClaimsPrincipal user, FavoritePostCreateDTO dto)
    {
        var accountId = _tokenService.GetUserIdFromClaims(user);

        var existing = await _repository.GetByAccountAndPostAsyn(accountId, dto.PostId);
        if (existing != null)
            throw new InvalidOperationException("Bài đăng đã được thêm vào yêu thích.");

        // Map dto -> entity
        var favorite = _mapper.Map<FavoritePost>(dto);
        favorite.AccountId = accountId;

        var created = await _repository.CreateAsync(favorite);

        // Map entity -> dto
        return _mapper.Map<FavoritePostDTO>(created);
    }

    public async Task<IEnumerable<FavoritePostDTO>> GetFavoritesByUserAsync(ClaimsPrincipal user)
    {
        var accountId = _tokenService.GetUserIdFromClaims(user);

        var favorites = await _repository.GetByAccountAsync(accountId);

        return _mapper.Map<IEnumerable<FavoritePostDTO>>(favorites);
    }

    public async Task<bool> RemoveFavoriteAsync(ClaimsPrincipal user, Guid favoriteId)
    {
        var accountId = _tokenService.GetUserIdFromClaims(user);

        var favorite = await _repository.GetByIdAsync(favoriteId);
        
        // Nếu không tìm thấy, return false
        if (favorite == null)
            return false;
            
        // Nếu không phải chủ sở hữu, throw exception
        if (favorite.AccountId != accountId)
            throw new UnauthorizedAccessException("Không có quyền xóa mục yêu thích này.");

        return await _repository.DeleteAsyn(favoriteId);
    }
}
