using AutoMapper;
using AutoMapper.QueryableExtensions;
using ContractAPI.DTO.Requests;
using ContractAPI.DTO.Requests.UtilityReading;
using ContractAPI.DTO.Response;
using ContractAPI.Enum;
using ContractAPI.Model;
using ContractAPI.Repository.Interface;
using ContractAPI.Services.Interfaces;

namespace ContractAPI.Services;

public class ContractService : IContractService
{
    private readonly IMapper _mapper;
    private readonly IContractRepository _contractRepository;
    private readonly IRoomClientService _roomClient; 
    private readonly IIdentityProfileService _identityProfileService; 
    private readonly IUtilityReadingClientService _utilityReadingClientService;
    public ContractService(IMapper mapper, IContractRepository contractRepository, IRoomClientService roomClient, IIdentityProfileService identityProfileService, IUtilityReadingClientService utilityReadingClientService)
    {
        _mapper = mapper;
        _contractRepository = contractRepository;
        _roomClient = roomClient;
        _identityProfileService = identityProfileService;
        _utilityReadingClientService = utilityReadingClientService;
    }
    // public ContractService(IMapper mapper, IContractRepository contractRepository, IRoomClientService roomClient,
    //     IIdentityProfileService identityProfileService)
    // {
    //     _mapper = mapper;
    //     _contractRepository = contractRepository;
    //     _roomClient = roomClient;
    //     _identityProfileService = identityProfileService;
    // }

    public IQueryable<ContractResponseDto> GetAllQueryable()
        => _contractRepository.GetAllQueryable().ProjectTo<ContractResponseDto>(_mapper.ConfigurationProvider);

    public IQueryable<ContractResponseDto> GetAllByTenantId(Guid tenantId)
        => _contractRepository.GetAllQueryable()
                              .Where(x => x.TenantId == tenantId).OrderByDescending(d => d.CreatedAt)
                              .ProjectTo<ContractResponseDto>(_mapper.ConfigurationProvider);

    public IQueryable<ContractResponseDto> GetAllByOwnerId(Guid ownerId)
        => _contractRepository.GetAllQueryable()
                              .Where(x => x.OwnerId == ownerId).OrderByDescending(d => d.CreatedAt)
                              .ProjectTo<ContractResponseDto>(_mapper.ConfigurationProvider);

        
    public IQueryable<ContractResponseDto> GetAllByOwnerId(Guid ownerId, ContractStatus contractStatus)
        => _contractRepository.GetAllQueryable()
            .Where(x => x.OwnerId == ownerId && x.ContractStatus == contractStatus).OrderByDescending(d => d.CreatedAt)
            .ProjectTo<ContractResponseDto>(_mapper.ConfigurationProvider);

    public async Task<ContractResponseDto?> GetByIdAsync(Guid id)
    => _mapper.Map<ContractResponseDto>(await _contractRepository.GetByIdAsync(id));
    
    
    
    public async Task<ApiResponse<ContractResponseDto>> Add(Guid ownerId, CreateContractDto request)
    {
        if (request.CheckinDate < DateTime.UtcNow.Date)
            return ApiResponse<ContractResponseDto>.Fail("Ngày nhận phòng phải lớn hơn hoặc bằng ngày hiện tại");
        
        if (request.CheckoutDate < request.CheckinDate.AddMonths(1))
            return ApiResponse<ContractResponseDto>.Fail("Ngày trả phòng phải ít nhất 1 tháng sau ngày nhận phòng.");
        
        var room = await _roomClient.GetRoomByIdAsync(request.RoomId);
        if (room == null)
            return ApiResponse<ContractResponseDto>.Fail("Không tìm thấy phòng");
        
        if (room.RoomStatus == "Occupied")
            return ApiResponse<ContractResponseDto>.Fail("Phòng đã có người thuê");
        
        var contract = _mapper.Map<Contract>(request);
        contract.OwnerId = ownerId;
        contract.CreatedAt = DateTime.UtcNow;
        contract.ContractStatus = ContractStatus.Active;
        await _roomClient.UpdateRoomStatusAsync(request.RoomId, "Occupied");
        var saveContract =await _contractRepository.AddAsync(contract);
        
        foreach (var ur in request.UtilityReadingContracts)
        {
            var created =  await _utilityReadingClientService.AddAsync(saveContract.RoomId, new CreateUtilityReadingContract
            {
                CurrentIndex = ur.CurrentIndex,
                Price = ur.Price,
                Note = ur.Note,
                Type = ur.Type
            });
        }
        var savedProfiles =await _identityProfileService.AddAsync(saveContract.Id, request.IdentityProfiles);
        var result = _mapper.Map<ContractResponseDto>(saveContract);
        result.IdentityProfiles = savedProfiles.Data;
        
        return ApiResponse<ContractResponseDto>.Success(result, "Thuê thành công.");
    }

    // public async Task<ApiResponse<ContractResponseDto>> AddAsync(Guid ownerId, CreateContractDto request)
    // {
    //     if (request.CheckinDate < DateTime.UtcNow.Date)
    //         return ApiResponse<ContractResponseDto>.Fail("Ngày nhận phòng phải lớn hơn hoặc bằng ngày hiện tại");
    //     
    //     if (request.CheckoutDate < request.CheckinDate.AddMonths(1))
    //         return ApiResponse<ContractResponseDto>.Fail("Ngày trả phòng phải ít nhất 1 tháng sau ngày nhận phòng.");
    //     
    //     var room = await _roomClient.GetRoomByIdAsync(request.RoomId);
    //     if (room == null)
    //         return ApiResponse<ContractResponseDto>.Fail("Không tìm thấy phòng");
    //     
    //     if (room.RoomStatus == "Occupied")
    //         return ApiResponse<ContractResponseDto>.Fail("Phòng đã có người thuê");
    //     
    //     var contract = _mapper.Map<Contract>(request);
    //     contract.OwnerId = ownerId;
    //     contract.CreatedAt = DateTime.UtcNow;
    //     contract.ContractStatus = ContractStatus.Active;
    // var electric = await _utilityReadingClientService.AddAsync(saveContract.RoomId,"Electric" , new CreateUtilityReadingContract()
    // {
    //  CurrentIndex  = request.UtilityReadingContracts[0].CurrentIndex,
    //  Price = request.ElectricityReading.Price,
    //  Note = request.ElectricityReading.Note
    // });
    // var water = await _utilityReadingClientService.AddAsync(saveContract.RoomId,"Water" , new CreateUtilityReadingContract()
    // {
    //     CurrentIndex  = request.WaterReading.CurrentIndex,
    //     Price = request.WaterReading.Price,
    //     Note = request.WaterReading.Note
    // });
    // if (!utilityReading.IsSuccess)
    // {
    //     return ApiResponse<ContractResponseDto>.Fail("Tạo hợp đồng thành công nhưng không khởi tạo UtilityReading được.");
    // }
    //     await _roomClient.UpdateRoomStatusAsync(request.RoomId, "Occupied");
    //     await _contractRepository.AddAsync(contract);
    //     var result = _mapper.Map<ContractResponseDto>(contract);
    //     return ApiResponse<ContractResponseDto>.Success(result, "Thuê thành công.");
    // }

    public async Task<ApiResponse<ContractResponseDto>> CancelContractAsync(Guid contractId, string reason)
    {
        var contract = await _contractRepository.GetByIdAsync(contractId);
        if (contract == null)
            return ApiResponse<ContractResponseDto>.Fail("Không tìm thấy hợp đồng thuê");

        if (contract.ContractStatus != ContractStatus.Active)
            return ApiResponse<ContractResponseDto>.Fail("Chỉ hợp đồng đang hoạt động mới có thể huỷ");

        contract.ContractStatus = ContractStatus.Cancelled;
        contract.UpdatedAt = DateTime.UtcNow;
        contract.Reason = reason;
        
        await _roomClient.UpdateRoomStatusAsync(contract.RoomId, "Available");
        await _contractRepository.UpdateAsync(contract);
        var dto = _mapper.Map<ContractResponseDto>(contract);
        return ApiResponse<ContractResponseDto>.Success(dto, "Huỷ hợp đồng thành công");
    }

    public async Task<ApiResponse<ContractResponseDto>> UpdateAsync(Guid id, UpdateContractDto request)
    {
        var contract = await _contractRepository.GetByIdAsync(id);
        if (contract == null)
            throw new KeyNotFoundException("Contract Id not found");

        if (DateTime.UtcNow - contract.CreatedAt > TimeSpan.FromHours(1))
            return ApiResponse<ContractResponseDto>.Fail("Đơn này đã quá 1 giờ, không thể cập nhật nữa.");
        
        if (contract.CheckinDate < DateTime.UtcNow)
            return ApiResponse<ContractResponseDto>.Fail("Ngày nhận phòng phải lớn hơn hoặc bằng ngày hiện tại");
        
        if (request.CheckoutDate < contract.CheckinDate.AddMonths(1))
            return ApiResponse<ContractResponseDto>.Fail("Ngày trả phòng phải ít nhất 1 tháng sau ngày nhận phòng.");
        
        if (contract.ContractStatus != ContractStatus.Active)
            await _roomClient.UpdateRoomStatusAsync(contract.RoomId, "Available");
        
        contract.UpdatedAt = DateTime.UtcNow;
        _mapper.Map(request, contract);

        await _contractRepository.UpdateAsync(contract);

        var result = _mapper.Map<ContractResponseDto>(contract);
        
        return ApiResponse<ContractResponseDto>.Success(result, "Cập nhật hợp đồng thành công.");
    }

    public async Task<ApiResponse<ContractResponseDto>> ExtendContractAsync(Guid contractId, ExtendContractDto request)
    {
        var contract = await _contractRepository.GetByIdAsync(contractId);
        if (contract == null)
            return ApiResponse<ContractResponseDto>.Fail("Không tìm thấy hợp đồng thuê");
        
        if (contract.ContractStatus != ContractStatus.Active)
            return ApiResponse<ContractResponseDto>.Fail("Chỉ hợp đồng đang hoạt động mới được gia hạn");

        if (request.CheckoutDate <= contract.CheckoutDate)
            return ApiResponse<ContractResponseDto>.Fail("Ngày trả phòng mới phải lớn hơn ngày trả phòng hiện tại");

        if (request.CheckoutDate < DateTime.UtcNow.Date)
            return ApiResponse<ContractResponseDto>.Fail("Ngày trả phòng mới phải lớn hơn ngày hiện tại");
        
        if (request.CheckoutDate < contract.CheckinDate.AddMonths(1))
            return ApiResponse<ContractResponseDto>.Fail("Ngày trả phòng mới phải cách ngày nhận phòng ít nhất 1 tháng");

        contract.UpdatedAt = DateTime.UtcNow;
        contract.CheckoutDate = request.CheckoutDate;
        await _contractRepository.UpdateAsync(contract);
        
        var result = _mapper.Map<ContractResponseDto>(contract);
        return ApiResponse<ContractResponseDto>.Success(result, "Gia hạn hợp đồng thành công");
    }

    public async Task<ApiResponse<bool>> DeleteAsync(Guid id)
    {
        var contract = await _contractRepository.GetByIdAsync(id);
        if (contract == null) 
            throw new KeyNotFoundException("Không tìm thấy hợp đồng");
        if (contract.ContractStatus != ContractStatus.Active)
            return ApiResponse<bool>.Fail("Phòng k thể xoá vì hợp đồng đã kí");
        await _contractRepository.DeleteAsync(contract);
        return ApiResponse<bool>.Success(true, "Xoá hợp đồng thành công");
    }
}
