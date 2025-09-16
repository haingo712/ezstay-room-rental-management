using AutoMapper;
using AutoMapper.QueryableExtensions;
using TenantAPI.DTO.Requests;
using TenantAPI.DTO.Response;
using TenantAPI.Enum;
using TenantAPI.Model;
using TenantAPI.Repository.Interface;
using TenantAPI.Service.Interface;

namespace TenantAPI.Service;

public class TenantService: ITenantService
{
    private readonly IMapper _mapper;
    private readonly ITenantRepository _tenantRepository;

    public TenantService(IMapper mapper, ITenantRepository tenantRepository)
    {
        _mapper = mapper;
        _tenantRepository = tenantRepository;
    }

    public IQueryable<TenantDto> GetAllQueryable()
    {
        var tenant =   _tenantRepository.GetAllQueryable();
    return tenant.ProjectTo<TenantDto>(_mapper.ConfigurationProvider);
    }
    public IQueryable<TenantDto> GetAllByUserId(Guid userId)
    {
        var tenant =   _tenantRepository.GetAllQueryable().Where(x=> x.UserId == userId);
        return tenant.ProjectTo<TenantDto>(_mapper.ConfigurationProvider);
    }
    public IQueryable<TenantDto> GetAllByOwnerId(Guid ownerId)
    {
        var tenant =   _tenantRepository.GetAllQueryable().Where(x=> x.OwnerId == ownerId);
        return tenant.ProjectTo<TenantDto>(_mapper.ConfigurationProvider);
    }
    // public IQueryable<TenantDto> GetAllByRoomId(int roomId)
    // {
    //     var tenant =   _tenantRepository.GetAllQueryable().Where(x=> x.RoomId == roomId);
    //     return tenant.ProjectTo<TenantDto>(_mapper.ConfigurationProvider);
    // }

    public async Task<TenantDto?> GetByIdAsync(Guid id)
    {
        var tenant = await _tenantRepository.GetByIdAsync(id);
      return   _mapper.Map<TenantDto>(tenant);
    }

    public async Task<ApiResponse<TenantDto>> AddAsync(Guid ownerId ,CreateTenantDto request)
    { 
         // var exist = await _tenantRepository.TenantRoomIsActiveAsync(request.RoomId);
         // if (exist)
         //     return ApiResponse<TenantDto>.Fail("Phòng trọ này đã có người thuê");
        if(request.CheckinDate < DateTime.Now)
            return ApiResponse<TenantDto>.Fail("Ngày nhận phòng phải lớn hơn hoặc bằng ngày hiện tại");
        if (request.CheckoutDate <  request.CheckinDate.AddMonths(1))
             return ApiResponse<TenantDto>.Fail("Ngày trả phòng phải ít nhất 1 tháng sau ngày nhận phòng.");
           var tenant = _mapper.Map<Tenant>(request);
        tenant.OwnerId = ownerId;
        tenant.CreatedAt = DateTime.Now;
        tenant.TenantStatus = TenantStatus.Active;
        await _tenantRepository.AddAsync(tenant);
        var result = _mapper.Map<TenantDto>(tenant);
        return ApiResponse<TenantDto>.Success(result, "thuê  thành công.");
    }

    public async Task<ApiResponse<TenantDto>> UpdateAsync(Guid id, UpdateTenantDto request)
    {
        var  tenant =await _tenantRepository.GetByIdAsync(id);
        if (tenant == null)
            throw new KeyNotFoundException("Tenant Id not found");
        // if (!tenant.IsActive  && request.IsActive)
        //     return ApiResponse<TenantDto>.Fail("Is Active false nên k thể cập nhật. vui lòng làm lại đơn mới");
        if (DateTime.Now - tenant.CreatedAt > TimeSpan.FromHours(1))
            return ApiResponse<TenantDto>.Fail("Đơn này đã quá 1 giờ, không thể cập nhật nữa.");
        if(request.CheckinDate < DateTime.Now)
            return ApiResponse<TenantDto>.Fail("Ngày nhận phòng phải lớn hơn hoặc bằng ngày hiện tại");
        if (request.CheckoutDate <  request.CheckinDate.AddMonths(1))
            return ApiResponse<TenantDto>.Fail("Ngày trả phòng phải ít nhất 1 tháng sau ngày nhận phòng.");
        
        // if (!request.IsActive)
        //     tenant.CheckoutDate = DateTime.Now;
        
        _mapper.Map(request, tenant);
        await _tenantRepository.UpdateAsync(tenant);
        var result= _mapper.Map<TenantDto>(tenant);
         return ApiResponse<TenantDto>.Success(result, "Cập nhật đơn  thành công.");
    }

    // public async Task DeleteAsync(int id)
    // {
    //     var room = await _tenantRepository.GetByIdAsync(id);
    //     if (room==null) 
    //         throw new KeyNotFoundException("k tim thay phong tro");
    //     await _tenantRepository.DeleteAsync(room);
    // }
}

  

  