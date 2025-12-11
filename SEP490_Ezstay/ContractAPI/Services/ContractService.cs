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
    
    
    public async Task<List<ContractResponse>> GetAllByTenantId(Guid userId)
    {
        var contracts = _contractRepository.GetAllByTenantId(userId);
        foreach (var contract in contracts)
        {
            if (contract.CheckoutDate < DateTime.UtcNow.Date && contract.ContractStatus == ContractStatus.Active)
            {
                contract.ContractStatus = ContractStatus.Expired;
                contract.UpdatedAt = DateTime.UtcNow;
                await  _contractRepository.Update(contract);
                await _roomService.UpdateRoomStatusAsync(contract.RoomId, RoomStatus.Occupied);
            }
        }
        return _mapper.Map<List<ContractResponse>>(contracts.OrderByDescending(x => x.CreatedAt).ToList());
             
    }
    
    public async Task<List<ContractResponse>> GetAllByOwnerId(Guid ownerId)
    {
        var contracts = _contractRepository.GetAllByOwnerId(ownerId);
        foreach (var contract in contracts)
        {
            if (contract.CheckoutDate < DateTime.UtcNow.Date &&
                contract.ContractStatus == ContractStatus.Active)
            {
                contract.ContractStatus = ContractStatus.Expired;
                contract.UpdatedAt = DateTime.UtcNow;
                await  _contractRepository.Update(contract);
                await _roomService.UpdateRoomStatusAsync(contract.RoomId, RoomStatus.Occupied);
            }
        }
      return _mapper.Map<List<ContractResponse>>(contracts.OrderByDescending(x => x.CreatedAt).ToList()); 
    }

    public async Task<ContractResponse?> GetByIdAsync(Guid id)
    {
        var contract = await _contractRepository.GetById(id);
        if (contract == null) return null;
        
        var response = _mapper.Map<ContractResponse>(contract);
            response.IdentityProfiles = contract.ProfilesInContract
                .Select(p => _mapper.Map<IdentityProfileResponse>(p))
                .ToList();
            response.ElectricityReading = await _utilityReadingService.GetFirstReading(contract.Id, UtilityType.Electric);
            response.WaterReading = await _utilityReadingService.GetFirstReading(contract.Id, UtilityType.Water);
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
                serviceInfor.ContractId = contract.Id;
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
        var createUtility = await _utilityReadingService.Add(contract.Id, UtilityType.Water, request.WaterReading);
        var createUtiliyw = await _utilityReadingService.Add(contract.Id, UtilityType.Electric, request.ElectricityReading);
     
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
        
        contract.ContractStatus = ContractStatus.CancelledByOwner;
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
        await _utilityReadingService.Update(contract.Id, UtilityType.Electric, request.ElectricityReading);
        await _utilityReadingService.Update(contract.Id, UtilityType.Water, request.WaterReading);
        contract.UpdatedAt = DateTime.UtcNow;
        _mapper.Map(request, contract);
        
        // Update services - ensure list is initialized
        if (contract.ServiceInfors == null)
        {
            contract.ServiceInfors = new List<ServiceInfor>();
        }
        else
        {
            contract.ServiceInfors.Clear();
        }
        
        if (request.ServiceInfors != null && request.ServiceInfors.Any())
        {
            foreach (var item in request.ServiceInfors)
            {
                var newService = _mapper.Map<ServiceInfor>(item);
                newService.ContractId = contract.Id;
                contract.ServiceInfors.Add(newService);
            }
        }

        
        await _contractRepository.Update(contract);

        return ApiResponse<bool>.Success(true, "Contract updated successfully.");
    }
    public async Task<ApiResponse<ContractResponse>> ExtendContract(Guid contractId, ExtendContract request)
    {
        var oldContract = await _contractRepository.GetById(contractId);
        if (oldContract == null)
            return ApiResponse<ContractResponse>.Fail("Không tìm thấy hợp đồng thuê");
        
        if (oldContract.ContractStatus != ContractStatus.Active)
            return ApiResponse<ContractResponse>.Fail("Chỉ hợp đồng đang hoạt động mới được gia hạn");

        if (request.CheckoutDate <= oldContract.CheckoutDate)
            return ApiResponse<ContractResponse>.Fail("Ngày trả phòng mới phải lớn hơn ngày trả phòng hiện tại");

        if (request.CheckoutDate < DateTime.UtcNow.Date)
            return ApiResponse<ContractResponse>.Fail("Ngày trả phòng mới phải lớn hơn ngày hiện tại");
        
        if (request.CheckoutDate < oldContract.CheckinDate.AddMonths(1))
            return ApiResponse<ContractResponse>.Fail("Ngày trả phòng mới phải cách ngày nhận phòng ít nhất 1 tháng");
    
        var newContract = new Contract
        {
            RoomId = oldContract.RoomId,
            CheckinDate = DateTime.UtcNow.Date,
            CheckoutDate = request.CheckoutDate,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            ContractStatus = ContractStatus.Pending, 
            Notes = request.Notes,
            RoomPrice = oldContract.RoomPrice,
            DepositAmount = oldContract.DepositAmount,
        };
        
        newContract.ServiceInfors = oldContract.ServiceInfors
            .Select(s => new ServiceInfor
            {
                ContractId = newContract.Id,
                ServiceName = s.ServiceName,
                Price = s.Price
            })
            .ToList();
        
        newContract.ProfilesInContract = oldContract.ProfilesInContract
            .Select(p => new IdentityProfile
            {
                UserId = p.UserId,
                ContractId = newContract.Id,
                FullName = p.FullName,
                Phone = p.Phone,
                Email = p.Email,
                Gender = p.Gender,
                IsSigner = p.IsSigner,
                DateOfBirth = p.DateOfBirth,
                CitizenIdNumber = p.CitizenIdNumber,
                FrontImageUrl = p.FrontImageUrl,
                BackImageUrl = p.BackImageUrl,
                Address = p.Address,
                ProvinceName = p.ProvinceName,
                WardName = p.WardName
            })
            .ToList();
        
        var savedContract = await _contractRepository.Add(newContract);

        var response = _mapper.Map<ContractResponse>(savedContract);

        return ApiResponse<ContractResponse>.Success(response, "Gia hạn hợp đồng thành công");
    }
  

    public async Task<ApiResponse<bool>> Delete(Guid id)
    {
        var contract = await _contractRepository.GetById(id);
        if (contract == null) 
            throw new KeyNotFoundException("Contract not found");
        if (contract.ContractStatus != ContractStatus.Pending)
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
    
   // SignContractOwner
    public async Task<ApiResponse<ContractResponse>> SignContractUser(Guid contractId, string ownerSignature, Guid  userId)
    {
        var contract = await _contractRepository.GetById(contractId);
        if (contract.ContractStatus == ContractStatus.Active)
            return ApiResponse<ContractResponse>.Fail("Hợp đồng đã được ký");

        if (contract.ContractStatus == ContractStatus.Cancelled)
            return ApiResponse<ContractResponse>.Fail("Hợp đồng đã bị hủy");
     
        var signerProfile = contract.ProfilesInContract
            .FirstOrDefault(x => x.UserId == userId);
        if (!signerProfile.IsSigner)
        {
            return ApiResponse<ContractResponse>.Fail(
                "You are not the representative, so you cannot sign the contract."
            );
        }
            contract.TenantSignature = ownerSignature.Trim('"');
            contract.TenantSignedAt = DateTime.UtcNow;
        await _contractRepository.Update(contract);
        var result = _mapper.Map<ContractResponse>(contract);
        return ApiResponse<ContractResponse>.Success(result, 
            contract.ContractStatus == ContractStatus.Active 
                ? "Hợp đồng đã được ký thành công bởi cả hai bên" 
                : $"Chữ ký đã được lưu");
    }
    public async Task<ApiResponse<ContractResponse>> SignContractOwner(Guid contractId, string ownerSignature, Guid  ownerId)
    {
        var contract = await _contractRepository.GetById(contractId);
        if (contract.ContractStatus == ContractStatus.Active)
            return ApiResponse<ContractResponse>.Fail("Hợp đồng đã được ký");

        if (contract.ContractStatus == ContractStatus.Cancelled)
            return ApiResponse<ContractResponse>.Fail("Hợp đồng đã bị hủy");
        
            contract.OwnerSignature =ownerSignature.Trim('"');
            contract.OwnerSignedAt = DateTime.UtcNow;
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
                : $"Chữ ký đã được lưu");
    }    
   
    
    public async Task<ApiResponse<RentalRequestResponse>> Add(Guid ownerId, Guid userId, Guid roomId, CreateRentalRequest request)
    {
        if (request.CheckinDate < DateTime.UtcNow.Date)
            return ApiResponse<RentalRequestResponse>.Fail("The check-in date must be today or a future date.");

        // if (request.CheckoutDate <= request.CheckinDate)
        //     return ApiResponse<RentalRequestResponse>.Fail("Checkout phải sau Checkin");
        
        if (request.CheckoutDate < request.CheckinDate.AddMonths(1))
            return ApiResponse<RentalRequestResponse>.Fail("Ngày trả phòng phải ít nhất 1 tháng sau ngày nhận phòng.");
        var rentalRequest = _mapper.Map<RentalRequest>(request);
            rentalRequest.UserId = userId;
            rentalRequest.RoomId = roomId;
            rentalRequest.OwnerId = ownerId;
          //  rentalRequest.Status = "Pending";
            var result = await _contractRepository.Add(rentalRequest);
            var response = _mapper.Map<RentalRequestResponse>(result);
            
        return ApiResponse<RentalRequestResponse>.Success(response, "Add Sucessfully.");
    }

    public IQueryable<RentalRequestResponse> GetAllRentalByUserId(Guid userId)
    {
        var rentalRequest = _contractRepository.GetAllRentalByUserId(userId);
        return rentalRequest.OrderByDescending(x => x.CreatedAt).ProjectTo<RentalRequestResponse>(_mapper.ConfigurationProvider);
    }

    public IQueryable<RentalRequestResponse> GetAllRentalByOwnerId(Guid ownerId)
    {
        var rentalRequest = _contractRepository.GetAllRentalByOwnerId(ownerId);
        return rentalRequest.OrderByDescending(x => x.CreatedAt).ProjectTo<RentalRequestResponse>(_mapper.ConfigurationProvider);
    }
    
    // Return list with all tenant info already in RentalRequest - no need to call Account API
    public async Task<List<RentalRequestResponse>> GetAllRentalByOwnerIdWithUserInfoAsync(Guid ownerId)
    {
        var rentalRequests = _contractRepository.GetAllRentalByOwnerId(ownerId)
            .OrderByDescending(x => x.CreatedAt)
            .ToList();
        
        // All tenant info is already stored in RentalRequest, just map it
        var responses = rentalRequests.Select(r => _mapper.Map<RentalRequestResponse>(r)).ToList();
        return responses;
    }
    
    public async Task<List<RentalRequestResponse>> GetAllRentalByUserIdWithOwnerInfoAsync(Guid userId)
    {
        var rentalRequests = _contractRepository.GetAllRentalByUserId(userId)
            .OrderByDescending(x => x.CreatedAt)
            .ToList();
        
        // All tenant info is already stored in RentalRequest, just map it
        var responses = rentalRequests.Select(r => _mapper.Map<RentalRequestResponse>(r)).ToList();
        return responses;
    }
    
    /// <summary>
    /// Get all tenants (primary and cohabitants) from Active contracts
    /// Returns flattened list with contract and room info
    /// </summary>
    public async Task<List<TenantInfoResponse>> GetAllTenantsAsync(Guid ownerId)
    {
        // Get all Active contracts for owner
        var activeContracts = _contractRepository.GetAllByOwnerId(ownerId)
            .Where(c => c.ContractStatus == ContractStatus.Active)
            .ToList();

        var tenantList = new List<TenantInfoResponse>();

        foreach (var contract in activeContracts)
        {
            // Get room info
            var room = await _roomService.GetRoomByIdAsync(contract.RoomId);
            var roomName = room?.RoomName ?? "N/A";
            // Note: HouseName will be fetched by frontend using room.HouseId
            var houseName = "N/A"; // Placeholder - frontend will fetch from BoardingHouseAPI

            // Get all profiles in contract (ProfilesInContract contains both primary tenant and cohabitants)
            if (contract.ProfilesInContract != null && contract.ProfilesInContract.Any())
            {
                for (int i = 1; i < contract.ProfilesInContract.Count; i++)
                {
                    var profile = contract.ProfilesInContract[i];
                    
                    tenantList.Add(new TenantInfoResponse
                    {
                        Id = profile.Id,
                        ContractId = contract.Id,
                        UserId = profile.UserId,
                        IsPrimary = i == 0, // First profile is primary tenant
                        
                        // Contract info
                        RoomId = contract.RoomId,
                        RoomName = roomName,
                        HouseName = houseName,
                        CheckinDate = contract.CheckinDate,
                        CheckoutDate = contract.CheckoutDate,
                        
                        // Identity profile info
                        FullName = profile.FullName,
                        DateOfBirth = profile.DateOfBirth,
                        Phone = profile.Phone,
                        Email = profile.Email,
                        Gender = profile.Gender,
                        ProvinceId = profile.ProvinceId,
                        ProvinceName = profile.ProvinceName,
                        WardId = profile.WardId,
                        WardName = profile.WardName,
                        Address = profile.Address,
                        TemporaryResidence = profile.TemporaryResidence,
                        CitizenIdNumber = profile.CitizenIdNumber,
                        CitizenIdIssuedDate = profile.CitizenIdIssuedDate,
                        CitizenIdIssuedPlace = profile.CitizenIdIssuedPlace,
                        FrontImageUrl = profile.FrontImageUrl,
                        BackImageUrl = profile.BackImageUrl
                    });
                }
            }
        }

        return tenantList;
    }
}
