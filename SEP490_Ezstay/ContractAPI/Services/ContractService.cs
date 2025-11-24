using System.Text.Json;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using ContractAPI.DTO.Requests;
using ContractAPI.DTO.Response;
using ContractAPI.Model;
using ContractAPI.Repository.Interface;
using ContractAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs;
using Shared.DTOs.Contracts.Responses;
using Shared.Enums;
using IdentityProfileResponse = Shared.DTOs.Contracts.Responses.IdentityProfileResponse;

namespace ContractAPI.Services;

public class ContractService : IContractService
{
    
    private readonly IMapper _mapper;
    private readonly IContractRepository _contractRepository;
    private readonly IRoomService _roomService;
    private readonly IImageService _imageService;
    private readonly IAccountService _accountService;
    private readonly IUtilityReadingService _utilityReadingService;

    public ContractService(IMapper mapper, IContractRepository contractRepository, IRoomService roomService, IImageService imageService, IAccountService accountService, IUtilityReadingService utilityReadingService)
    {
        _mapper = mapper;
        _contractRepository = contractRepository;
        _roomService = roomService;
        _imageService = imageService;
        _accountService = accountService;
        _utilityReadingService = utilityReadingService;
    }
    // public IQueryable<ContractResponse> GetAllByTenantId(Guid tenantId)
    //     => _contractRepository.GetAllQueryable()
    //                           .Where(x => x.SignerProfile.TenantId == tenantId).OrderByDescending(d => d.CreatedAt)
    //                           .ProjectTo<ContractResponse>(_mapper.ConfigurationProvider);
    public IQueryable<ContractResponse> GetAllByOwnerId(Guid ownerId)
        => _contractRepository.GetAllByOwnerId(ownerId).OrderByDescending(d => d.CreatedAt)
                              .ProjectTo<ContractResponse>(_mapper.ConfigurationProvider);

    public async Task<ContractResponse?> GetByIdAsync(Guid id)
    {
        var contract = await _contractRepository.GetByIdAsync(id);
        if (contract == null) return null;
        
        var response = _mapper.Map<ContractResponse>(contract);
            response.IdentityProfiles = contract.ProfilesInContract
                .Select(p => _mapper.Map<IdentityProfileResponse>(p))
                .ToList();
            response.ElectricityReading = await _utilityReadingService.GetLastestReading(contract.RoomId, UtilityType.Electric);
            response.WaterReading = await _utilityReadingService.GetLastestReading(contract.RoomId, UtilityType.Water);
            return response;
    }
  //  => _mapper.Map<ContractResponse>(await _contractRepository.GetByIdAsync(id));
    
   // public async Task<bool> HasContractAsync(Guid tenantId, Guid roomId)=>await _contractRepository.HasContractAsync(tenantId, roomId);
  
    public async Task<ApiResponse<ContractResponse>> Add(Guid ownerId, CreateContract request)
    {
        if (request.CheckinDate < DateTime.UtcNow.Date)
            return ApiResponse<ContractResponse>.Fail("Ngày nhận phòng phải lớn hơn hoặc bằng ngày hiện tại");
        
        if (request.CheckoutDate < request.CheckinDate.AddMonths(1))
            return ApiResponse<ContractResponse>.Fail("Ngày trả phòng phải ít nhất 1 tháng sau ngày nhận phòng.");
        
        var room = await _roomService.GetRoomByIdAsync(request.RoomId);
        if (room == null)
            return ApiResponse<ContractResponse>.Fail("Không tìm thấy phòng");
        if (room.RoomStatus == RoomStatus.Occupied)
            return ApiResponse<ContractResponse>.Fail("Phòng đã có người thuê");
        
        var contract = _mapper.Map<Contract>(request);
        
        contract.CreatedAt = DateTime.UtcNow;
        
        contract.ServiceInfors = request.ServiceInfors
            .Select((p, index) =>
            {
                var serviceInfor = _mapper.Map<ServiceInfor>(p);
                return serviceInfor;
            }).ToList();
        
        var members = request.ProfilesInContract
            .Select((p, index) =>
            {
                var profile = _mapper.Map<IdentityProfile>(p);
                profile.IsSigner = index == 0;
                profile.ContractId = contract.Id;
                return profile;
            }).ToList();
   
        var ownerProfile = await _accountService.GetProfileByUserId(ownerId);
        if (ownerProfile == null) 
            return ApiResponse<ContractResponse>.Fail("Không tìm thấy thông tin chủ trọ.");
        var ownerIdentity = new IdentityProfile
        {
            UserId = ownerProfile.Id,
            ContractId = contract.Id,
            Avatar = ownerProfile.Avatar,
            FullName = ownerProfile.FullName,
            Phone = ownerProfile.Phone,
            Email = ownerProfile.Email,
            Gender = ownerProfile.Gender.ToString(),
            Address = ownerProfile.DetailAddress,
            IsSigner = true,
            DateOfBirth = ownerProfile.DateOfBirth,
            ProvinceName = ownerProfile.ProvinceName,
            WardName = ownerProfile.WardName, 
            FrontImageUrl = ownerProfile.FrontImageUrl,
            BackImageUrl =  ownerProfile.BackImageUrl,
            TemporaryResidence = ownerProfile.TemporaryResidence,
            CitizenIdNumber = ownerProfile.CitizenIdNumber,
            CitizenIdIssuedDate= ownerProfile.CitizenIdIssuedDate,
            CitizenIdIssuedPlace = ownerProfile.CitizenIdIssuedPlace
        };
        ownerIdentity.IsSigner = true; 
        members.Add(ownerIdentity);
        contract.ProfilesInContract = members;
        var saveContract =await _contractRepository.AddAsync(contract);
        var createUtility = await _utilityReadingService.Add(contract.RoomId,  UtilityType.Water, request.WaterReading);
        var createUtiliyw = await _utilityReadingService.Add(contract.RoomId,  UtilityType.Electric, request.ElectricityReading);
        var result = _mapper.Map<ContractResponse>(saveContract);
        result.WaterReading = createUtility.Data;
        result.ElectricityReading = createUtiliyw.Data;
        return ApiResponse<ContractResponse>.Success(result, "Thuê thành công.");
    }
    public async Task<ApiResponse<ContractResponse>> CancelContract(Guid contractId, string reason)
    {
        var contract = await _contractRepository.GetByIdAsync(contractId);
        if (contract == null)
            return ApiResponse<ContractResponse>.Fail("Không tìm thấy hợp đồng thuê");

        if (contract.ContractStatus != ContractStatus.Active)
            return ApiResponse<ContractResponse>.Fail("Chỉ hợp đồng đang hoạt động mới có thể huỷ");
        
        contract.ContractStatus = ContractStatus.Cancelled;
        contract.UpdatedAt = DateTime.UtcNow;
        contract.Reason = reason;
        await _roomService.UpdateRoomStatusAsync(contract.RoomId, RoomStatus.Available);
        await _contractRepository.UpdateAsync(contract);
        var dto = _mapper.Map<ContractResponse>(contract);
        return ApiResponse<ContractResponse>.Success(dto, "Huỷ hợp đồng thành công");
    }

    public async Task<ApiResponse<bool>> UpdateAsync(Guid id, UpdateContract request)
    {
        var contract = await _contractRepository.GetByIdAsync(id);
        if (contract == null)
            throw new KeyNotFoundException("Contract Id not found");
       
        // if (DateTime.UtcNow - contract.CreatedAt > TimeSpan.FromHours(1))
        //     return ApiResponse<bool>.Fail("Đơn này đã quá 1 giờ, không thể cập nhật nữa.");
        //
        if (contract.CheckinDate < DateTime.UtcNow.Date)
            return ApiResponse<bool>.Fail("Ngày nhận phòng phải lớn hơn hoặc bằng ngày hiện tại");
        
        if (request.CheckoutDate < contract.CheckinDate.AddMonths(1))
            return ApiResponse<bool>.Fail("Ngày trả phòng phải ít nhất 1 tháng sau ngày nhận phòng.");
        if (request.ProfilesInContract != null && request.ProfilesInContract.Any())
        {
            var members = request.ProfilesInContract
                .Select((p, index) =>
                {
                    var profile = _mapper.Map<IdentityProfile>(p);
                    profile.IsSigner = index == 0;
                    return profile;
                }).ToList();

            contract.ProfilesInContract = members;
        }
            await _utilityReadingService.Update(contract.RoomId, UtilityType.Electric, request.ElectricityReading);
            await _utilityReadingService.Update(contract.RoomId, UtilityType.Water, request.WaterReading);

        if (contract.ContractStatus == ContractStatus.Active)
            return ApiResponse<bool>.Fail("K the cập nhật vì contract đã kí tên r");
        contract.UpdatedAt = DateTime.UtcNow;
        _mapper.Map(request, contract);
        await _contractRepository.UpdateAsync(contract);
        return ApiResponse<bool>.Success(true, "Cập nhật hợp đồng thành công.");
    }
    public async Task<ApiResponse<ContractResponse>> ExtendContract(Guid contractId, ExtendContractDto request)
    {
        var contract = await _contractRepository.GetByIdAsync(contractId);
        if (contract == null)
            return ApiResponse<ContractResponse>.Fail("Không tìm thấy hợp đồng thuê");
        
        if (contract.ContractStatus != ContractStatus.Active)
            return ApiResponse<ContractResponse>.Fail("Chỉ hợp đồng đang hoạt động mới được gia hạn");

        if (request.CheckoutDate <= contract.CheckoutDate)
            return ApiResponse<ContractResponse>.Fail("Ngày trả phòng mới phải lớn hơn ngày trả phòng hiện tại");

        if (request.CheckoutDate < DateTime.UtcNow.Date)
            return ApiResponse<ContractResponse>.Fail("Ngày trả phòng mới phải lớn hơn ngày hiện tại");
        
        if (request.CheckoutDate < contract.CheckinDate.AddMonths(1))
            return ApiResponse<ContractResponse>.Fail("Ngày trả phòng mới phải cách ngày nhận phòng ít nhất 1 tháng");

        contract.UpdatedAt = DateTime.UtcNow;
        contract.CheckoutDate = request.CheckoutDate;
        await _contractRepository.UpdateAsync(contract);
        
        var result = _mapper.Map<ContractResponse>(contract);
        return ApiResponse<ContractResponse>.Success(result, "Gia hạn hợp đồng thành công");
    }

    public async Task<ApiResponse<bool>> Delete(Guid id)
    {
        var contract = await _contractRepository.GetByIdAsync(id);
        if (contract == null) 
            throw new KeyNotFoundException("Không tìm thấy hợp đồng");
        if (contract.ContractStatus == ContractStatus.Active)
            return ApiResponse<bool>.Fail("Contract k thể xoá vì hợp đồng đã kí");
        await _contractRepository.DeleteAsync(contract);
        return ApiResponse<bool>.Success(true, "Xoá hợp đồng thành công");
    }
    public async Task<ApiResponse<List<string>>> UploadContractImages(Guid contractId, IFormFileCollection images)
    {
        var contract = await _contractRepository.GetByIdAsync(contractId);
        if (contract == null)
            return ApiResponse<List<string>>.Fail("Không tìm thấy hợp đồng");
        
        var uploadedUrls = await _imageService.UploadMultipleImage(images);
        contract.ContractImage = uploadedUrls;
        contract.ContractUploadedAt = DateTime.UtcNow;
        contract.ContractStatus = ContractStatus.Active;
        await _contractRepository.UpdateAsync(contract);
        await _roomService.UpdateRoomStatusAsync(contract.RoomId, RoomStatus.Occupied);
        return ApiResponse<List<string>>.Success(uploadedUrls, "Upload ảnh scan hợp đồng thành công");
    }

    public async Task<ApiResponse<bool>> ExistsByRoomId(Guid roomId)
    {
        var exists = await _contractRepository.ExistsByRoomId(roomId);
        return ApiResponse<bool>.Success(exists);
    }
    public async Task<ApiResponse<ContractResponse>> SignContract(Guid contractId, string ownerSignature, string role)
    {
        var contract = await _contractRepository.GetByIdAsync(contractId);
        if (contract.ContractStatus == ContractStatus.Active)
            return ApiResponse<ContractResponse>.Fail("Hợp đồng đã được ký");

        if (contract.ContractStatus == ContractStatus.Cancelled)
            return ApiResponse<ContractResponse>.Fail("Hợp đồng đã bị hủy");
        
        if (role.Equals("Owner"))
        {
            contract.OwnerSignature =ownerSignature.Trim('"');
            contract.OwnerSignedAt = DateTime.UtcNow;
        } 
        if (role.Equals("User"))
        {
            contract.TenantSignature = ownerSignature.Trim('"');
            contract.TenantSignedAt = DateTime.UtcNow;
        }
        
        if (!string.IsNullOrEmpty(contract.OwnerSignature) && !string.IsNullOrEmpty(contract.TenantSignature))
        {
            contract.ContractStatus = ContractStatus.Active;
            contract.UpdatedAt = DateTime.UtcNow;
            await _roomService.UpdateRoomStatusAsync(contract.RoomId, RoomStatus.Occupied);
        }
        await _contractRepository.UpdateAsync(contract);
        var result = _mapper.Map<ContractResponse>(contract);
        return ApiResponse<ContractResponse>.Success(result, 
            contract.ContractStatus == ContractStatus.Active 
                ? "Hợp đồng đã được ký thành công bởi cả hai bên" 
                : $"Chữ ký {role} đã được lưu");
    }
}
