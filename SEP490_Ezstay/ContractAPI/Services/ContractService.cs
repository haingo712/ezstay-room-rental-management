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
    {
        var contracts = _contractRepository.GetAllByOwnerId(ownerId);
        foreach (var contract in contracts)
        {
            if (contract.CheckoutDate < DateTime.UtcNow.Date &&
                contract.ContractStatus == ContractStatus.Active)
            {
                contract.ContractStatus = ContractStatus.Expired;
                contract.UpdatedAt = DateTime.UtcNow;
                _contractRepository.Update(contract);
            }
        }
      return  contracts.OrderByDescending(x=> x.CreatedAt).ProjectTo<ContractResponse>(_mapper.ConfigurationProvider);
    }

    public async Task<ContractResponse?> GetByIdAsync(Guid id)
    {
        var contract = await _contractRepository.GetById(id);
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
            return ApiResponse<ContractResponse>.Fail("The check-in date must be today or a future date.");
        
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
        
      
        var ownerProfile = await _accountService.GetProfileByUserId(ownerId);
     
        if (ownerProfile == null) 
            return ApiResponse<ContractResponse>.Fail("Không tìm thấy thông tin chủ trọ.");
        
        var members = new List<IdentityProfile>();
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
            CitizenIdIssuedPlace = ownerProfile.CitizenIdIssuedPlace,
            ProvinceId = ownerProfile.ProvinceId,
            WardId = ownerProfile.WardId
        };
        
        ownerIdentity.IsSigner = true; 
        members.Add(ownerIdentity);

        foreach (var p in request.ProfilesInContract)
        {
            var profile = _mapper.Map<IdentityProfile>(p);
            profile.ContractId = contract.Id;
            profile.IsSigner = members.Count == 1;
            members.Add(profile);
        }
        contract.ProfilesInContract = members;
        var saveContract =await _contractRepository.Add(contract);
        var createUtility = await _utilityReadingService.Add(contract.RoomId,  UtilityType.Water, request.WaterReading);
        var createUtiliyw = await _utilityReadingService.Add(contract.RoomId,  UtilityType.Electric, request.ElectricityReading);
        var result = _mapper.Map<ContractResponse>(saveContract);
        result.WaterReading = createUtility.Data;
        result.ElectricityReading = createUtiliyw.Data;
        return ApiResponse<ContractResponse>.Success(result, "Create new contract.");
    }
    public async Task<ApiResponse<ContractResponse>> CancelContract(Guid contractId, string reason)
    {
        var contract = await _contractRepository.GetById(contractId);
        if (contract == null)
            return ApiResponse<ContractResponse>.Fail("Contract not found.");

        if (contract.ContractStatus != ContractStatus.Active)
            return ApiResponse<ContractResponse>.Fail("The contract has not been signed yet, so it cannot be cancelled.");
        
        contract.ContractStatus = ContractStatus.Cancelled;
        contract.UpdatedAt = DateTime.UtcNow;
        contract.Reason = reason;
        await _roomService.UpdateRoomStatusAsync(contract.RoomId, RoomStatus.Available);
        await _contractRepository.Update(contract);
        var dto = _mapper.Map<ContractResponse>(contract);
        return ApiResponse<ContractResponse>.Success(dto, "Cancel contract successful.");
    }

    public async Task<ApiResponse<bool>> UpdateAsync(Guid id, UpdateContract request)
    {
        var contract = await _contractRepository.GetById(id);
        if (contract == null)
            return ApiResponse<bool>.Fail("Contract not found");
       
        if (contract.ContractStatus == ContractStatus.Active )
            return ApiResponse<bool>.Fail("The contract has already been signed, so you cannot update it anymore.");
        
        if (contract.CheckinDate < DateTime.UtcNow.Date)
            return ApiResponse<bool>.Fail("The check-in date must be today or a future date.");
        
        if (request.CheckoutDate < contract.CheckinDate.AddMonths(1))
            return ApiResponse<bool>.Fail("The checkout date must be at least 1 month after the check-in date.");
        // if (request.ProfilesInContract != null && request.ProfilesInContract.Any())
        // {
            var members = new List<IdentityProfile>();
            var ownerProfile = contract.ProfilesInContract.FirstOrDefault(p => p.IsSigner);
            if (ownerProfile != null)
                members.Add(ownerProfile);
            
            foreach (var p in request.ProfilesInContract)
            {
                var profile = _mapper.Map<IdentityProfile>(p);
                profile.ContractId = contract.Id;
                profile.IsSigner = members.Count == 1; 
                members.Add(profile);
            }
            contract.ProfilesInContract = members;
      //  }
        await _utilityReadingService.Update(contract.RoomId, UtilityType.Electric, request.ElectricityReading);
        await _utilityReadingService.Update(contract.RoomId, UtilityType.Water, request.WaterReading);
        contract.UpdatedAt = DateTime.UtcNow;
        _mapper.Map(request, contract);
        await _contractRepository.Update(contract);
        return ApiResponse<bool>.Success(true, "Contract updated successfully.");
    }
    public async Task<ApiResponse<ContractResponse>> ExtendContract(Guid contractId, ExtendContractDto request)
    {
        var contract = await _contractRepository.GetById(contractId);
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
        await _contractRepository.Update(contract);
        
        var result = _mapper.Map<ContractResponse>(contract);
        return ApiResponse<ContractResponse>.Success(result, "Gia hạn hợp đồng thành công");
    }

    public async Task<ApiResponse<bool>> Delete(Guid id)
    {
        var contract = await _contractRepository.GetById(id);
        if (contract == null) 
            throw new KeyNotFoundException("Contract not found");
        if (contract.ContractStatus == ContractStatus.Active)
            return ApiResponse<bool>.Fail("The contract has already been signed, so it cannot be deleted.");
        await _contractRepository.Delete(contract);
        return ApiResponse<bool>.Success(true, "Delete contract successfully.");
    }
    public async Task<ApiResponse<List<string>>> UploadContractImages(Guid contractId, IFormFileCollection images)
    {
        var contract = await _contractRepository.GetById(contractId);
        if (contract == null)
            return ApiResponse<List<string>>.Fail("Contract not found");
        var uploadedUrls = await _imageService.UploadMultipleImage(images);
        contract.ContractImage = uploadedUrls;
        contract.ContractUploadedAt = DateTime.UtcNow;
        contract.ContractStatus = ContractStatus.Active;
        await _contractRepository.Update(contract);
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
        var contract = await _contractRepository.GetById(contractId);
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
        await _contractRepository.Update(contract);
        var result = _mapper.Map<ContractResponse>(contract);
        return ApiResponse<ContractResponse>.Success(result, 
            contract.ContractStatus == ContractStatus.Active 
                ? "Hợp đồng đã được ký thành công bởi cả hai bên" 
                : $"Chữ ký {role} đã được lưu");
    }
}
