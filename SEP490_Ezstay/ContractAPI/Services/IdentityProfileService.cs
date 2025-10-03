using System.Text.Json;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using ContractAPI.DTO.Requests;
using ContractAPI.DTO.Response;
using ContractAPI.Model;
using ContractAPI.Repository.Interface;
using ContractAPI.Services.Interfaces;

namespace ContractAPI.Services;

public class IdentityProfileService: IIdentityProfileService
{
    private readonly IMapper _mapper;
    private readonly IIdentityProfileRepository _identityProfileRepository;
    private readonly HttpClient _httpClientLocation;

   
    public IdentityProfileService(IMapper mapper, IIdentityProfileRepository identityProfileRepository, HttpClient httpClient)
    {
        _mapper = mapper;
        _identityProfileRepository = identityProfileRepository;
        _httpClientLocation = httpClient;
    }
    
    private async Task<string?> GetProvinceNameAsync(string provinceId)
    {
        var response = await _httpClientLocation.GetFromJsonAsync<JsonElement>("/api/provinces");
        var provinces = response.GetProperty("provinces").EnumerateArray();
        return provinces.FirstOrDefault(p => p.GetProperty("code").GetString() == provinceId)
            .GetProperty("name").GetString();
    }

    private async Task<string?> GetCommuneNameAsync(string provinceId, string communeId)
    {
        var response = await _httpClientLocation.GetFromJsonAsync<JsonElement>($"/api/provinces/{provinceId}/communes");
        var communes = response.GetProperty("communes").EnumerateArray();
        return communes.FirstOrDefault(c => c.GetProperty("code").GetString() == communeId)
            .GetProperty("name").GetString();
    }
    // public async Task<List<IdentityProfileResponseDto>> GetAllByTenantId(Guid tenantId)
    // {
    //    var identityProfile=  _identityProfileRepository.GetAllQueryable().Where(x => x.Id == tenantId);
    //    return identityProfile.ProjectTo<IdentityProfileResponseDto>(_mapper.ConfigurationProvider);
    // }
    public IQueryable<IdentityProfileResponseDto> GetAllByTenantId(Guid tenantId)
    {
       var identityProfile=  _identityProfileRepository.GetAllQueryable().Where(x => x.Id == tenantId).OrderByDescending(x => x.CreatedAt);
       return identityProfile.ProjectTo<IdentityProfileResponseDto>(_mapper.ConfigurationProvider);
    }
    
    public IQueryable<IdentityProfileResponseDto> GetAllByOwnerId(Guid ownerId)
    {
        var identityProfile=  _identityProfileRepository.GetAllQueryable().Where(x => x.Id == ownerId).OrderByDescending(x => x.CreatedAt);
        return identityProfile.ProjectTo<IdentityProfileResponseDto>(_mapper.ConfigurationProvider);
    }
    public IQueryable<IdentityProfileResponseDto> GetAllQueryable()
    {
        return _identityProfileRepository.GetAllQueryable()
            .ProjectTo<IdentityProfileResponseDto>(_mapper.ConfigurationProvider);
    }

    public async Task<IdentityProfileResponseDto?> GetByIdAsync(Guid id)
    {
        var entity = await _identityProfileRepository.GetByIdAsync(id);
        return _mapper.Map<IdentityProfileResponseDto>(entity);
    }

    // public async Task<ApiResponse<IdentityProfileResponseDto>> AddAsync( CreateIdentityProfileDto request)
    // {
    //     var entity = _mapper.Map<IdentityProfile>(request);
    //     entity.CreatedAt = DateTime.UtcNow;
    //     await _identityProfileRepository.AddAsync(entity);
    //     var dto = _mapper.Map<IdentityProfileResponseDto>(entity);
    //     return ApiResponse<IdentityProfileResponseDto>.Success(dto, "Tạo hồ sơ thành công");
    // }
    // public async Task<ApiResponse<List<IdentityProfileResponseDto>>> AddManyAsync(Guid contractId, List<CreateIdentityProfileDto> request)
    // {
    //     var entity = _mapper.Map<List<IdentityProfile>>(request);
    //     foreach (var e in entity)
    //     {
    //         e.ContractId = contractId;
    //         e.CreatedAt = DateTime.UtcNow;
    //     }
    //     await _identityProfileRepository.AddMany(entity);
    //     var dto = _mapper.Map<List<IdentityProfileResponseDto>>(entity);
    //     return ApiResponse<List<IdentityProfileResponseDto>>.Success(dto, "Tạo hồ sơ thành công");
    // }
    public async Task<ApiResponse<IdentityProfileResponseDto>> AddAsync(Guid contractId, CreateIdentityProfileDto request)
    {
        var entity = _mapper.Map<IdentityProfile>(request);
        entity.ContractId = contractId;
        entity.CreatedAt = DateTime.UtcNow;
        await _identityProfileRepository.AddAsync(entity);
        var dto = _mapper.Map<IdentityProfileResponseDto>(entity);
        return ApiResponse<IdentityProfileResponseDto>.Success(dto, "Tạo hồ sơ thành công");
    }

    public async Task<ApiResponse<IdentityProfileResponseDto>> UpdateAsync(Guid id, UpdateIdentityProfileDto request)
    {
        var entity = await _identityProfileRepository.GetByIdAsync(id);
        if (entity == null)
            return ApiResponse<IdentityProfileResponseDto>.Fail("Không tìm thấy hồ sơ");

        _mapper.Map(request, entity);
        entity.UpdatedAt = DateTime.UtcNow;
        await _identityProfileRepository.UpdateAsync(entity);
        var dto = _mapper.Map<IdentityProfileResponseDto>(entity);
        return ApiResponse<IdentityProfileResponseDto>.Success(dto, "Cập nhật hồ sơ thành công");
    }
    

    public async Task DeleteAsync(Guid id)
    {
        var entity = await _identityProfileRepository.GetByIdAsync(id);
        if (entity == null)
            throw new KeyNotFoundException("Không tìm thấy hồ sơ");
        await _identityProfileRepository.DeleteAsync(entity);
    }
}