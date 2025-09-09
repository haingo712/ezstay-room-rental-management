using System.Security.Claims;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using ReviewResponseAPI.DTO.Requests;
using ReviewResponseAPI.DTO.Response;
using ReviewResponseAPI.Model;
using ReviewResponseAPI.Repository.Interface;
using ReviewResponseAPI.Service.Interface;

namespace ReviewResponseAPI.Service;

public class ReviewResponseService : IReviewResponseService
{
    private readonly IMapper _mapper;
    private readonly IReviewResponseRepository _repository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ReviewResponseService(IMapper mapper, IReviewResponseRepository repository, IHttpContextAccessor httpContextAccessor)
    {
        _mapper = mapper;
        _repository = repository;
        _httpContextAccessor = httpContextAccessor;
    }

    private Guid GetOwnerIdFromToken()
    {
        var idStr = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(idStr))
            throw new UnauthorizedAccessException("Không xác định được OwnerId từ token.");
        return Guid.Parse(idStr);
    }

    public async Task<ApiResponse<IEnumerable<ReviewResponseDto>>> GetAllByOwnerId()
    {
        var ownerId = GetOwnerIdFromToken();
        var entities = await _repository.GetAllByOwnerId(ownerId);
        var dtos = _mapper.Map<IEnumerable<ReviewResponseDto>>(entities);

        if (!dtos.Any())
            return ApiResponse<IEnumerable<ReviewResponseDto>>.Fail("Không có phản hồi nào.");
        return ApiResponse<IEnumerable<ReviewResponseDto>>.Success(dtos);
    }

    public async Task<ApiResponse<IEnumerable<ReviewResponseDto>>> GetAll()
    {
        var entities = await _repository.GetAll();
        var dtos = _mapper.Map<IEnumerable<ReviewResponseDto>>(entities);
        if (!dtos.Any())
            return ApiResponse<IEnumerable<ReviewResponseDto>>.Fail("Không có phản hồi nào.");
        return ApiResponse<IEnumerable<ReviewResponseDto>>.Success(dtos);
    }

    public IQueryable<ReviewResponseDto> GetAllAsQueryable()
    {
        return _repository.GetAllAsQueryable()
            .ProjectTo<ReviewResponseDto>(_mapper.ConfigurationProvider);
    }

    public async Task<ReviewResponseDto> GetByIdAsync(Guid id)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null)
            throw new KeyNotFoundException("ReviewResponse not found");
        return _mapper.Map<ReviewResponseDto>(entity);
    }

    public async Task<ApiResponse<ReviewResponseDto>> AddAsync(Guid reviewId, CreateReviewResponseDto request)
    {
        var ownerId = GetOwnerIdFromToken();
        var entity = _mapper.Map<ReviewResponse>(request);
        entity.OwnerId = ownerId;
        entity.ReviewId = reviewId;
        entity.CreatedAt = DateTime.UtcNow;
        await _repository.AddAsync(entity);
        var dto = _mapper.Map<ReviewResponseDto>(entity);
        return ApiResponse<ReviewResponseDto>.Success(dto, "Thêm phản hồi thành công");
    }
    public async Task<ApiResponse<bool>> UpdateAsync(Guid id, UpdateReviewResponseDto request)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null)
            return ApiResponse<bool>.Fail("Không tìm thấy phản hồi");

        var ownerId = GetOwnerIdFromToken();
        if (entity.OwnerId != ownerId)
            return ApiResponse<bool>.Fail("Bạn không có quyền cập nhật phản hồi này");

        _mapper.Map(request, entity);
        entity.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(entity);
        return ApiResponse<bool>.Success(true, "Cập nhật thành công");
    }
    public async Task DeleteAsync(Guid id)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null)
            throw new KeyNotFoundException("ReviewResponse not found");

        var ownerId = GetOwnerIdFromToken();
        if (entity.OwnerId != ownerId)
            throw new UnauthorizedAccessException("Bạn không có quyền xóa phản hồi này");
        await _repository.DeleteAsync(entity);
    }
}
