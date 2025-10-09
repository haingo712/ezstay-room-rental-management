using System.Text.Json;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using ContractAPI.APIs.Interfaces;
using ContractAPI.DTO.Requests;
using ContractAPI.DTO.Response;
using ContractAPI.Model;
using ContractAPI.Repository.Interface;
using ContractAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs;
using Shared.DTOs.Contracts.Responses;
using Shared.Enums;

namespace ContractAPI.Services;

public class ContractService : IContractService
{
    private readonly IMapper _mapper;
    private readonly IContractRepository _contractRepository;
    private readonly IRoomClientService _roomClient; 
    private readonly IImageAPI _imageClient;
    private readonly IAccountAPI _accountClient;
    private readonly IUtilityReadingClientService _utilityReadingClientService;

    public ContractService(IMapper mapper, IContractRepository contractRepository, IRoomClientService roomClient, IImageAPI imageClient, IAccountAPI accountClient, IUtilityReadingClientService utilityReadingClientService)
    {
        _mapper = mapper;
        _contractRepository = contractRepository;
        _roomClient = roomClient;
        _imageClient = imageClient;
        _accountClient = accountClient;
        _utilityReadingClientService = utilityReadingClientService;
    }

    public IQueryable<ContractResponse> GetAllQueryable()
        => _contractRepository.GetAllQueryable().ProjectTo<ContractResponse>(_mapper.ConfigurationProvider);

    // public IQueryable<ContractResponseDto> GetAllByTenantId(Guid tenantId)
    //     => _contractRepository.GetAllQueryable()
    //                           .Where(x => x.TenantId == tenantId).OrderByDescending(d => d.CreatedAt)
    //                           .ProjectTo<ContractResponseDto>(_mapper.ConfigurationProvider);

    public IQueryable<ContractResponse> GetAllByOwnerId(Guid ownerId)
        => _contractRepository.GetAllQueryable()
                              .Where(x => x.OwnerId == ownerId).OrderByDescending(d => d.CreatedAt)
                              .ProjectTo<ContractResponse>(_mapper.ConfigurationProvider);

        
    public IQueryable<ContractResponse> GetAllByOwnerId(Guid ownerId, ContractStatus contractStatus)
        => _contractRepository.GetAllQueryable()
            .Where(x => x.OwnerId == ownerId && x.ContractStatus == contractStatus).OrderByDescending(d => d.CreatedAt)
            .ProjectTo<ContractResponse>(_mapper.ConfigurationProvider);

    public async Task<ContractResponse?> GetByIdAsync(Guid id)
    => _mapper.Map<ContractResponse>(await _contractRepository.GetByIdAsync(id));
    
   // public async Task<bool> HasContractAsync(Guid tenantId, Guid roomId)=>await _contractRepository.HasContractAsync(tenantId, roomId);
  
    public async Task<ApiResponse<ContractResponse>> Add(Guid ownerId, CreateContract request)
    {
        if (request.CheckinDate < DateTime.UtcNow.Date)
            return ApiResponse<ContractResponse>.Fail("Ngày nhận phòng phải lớn hơn hoặc bằng ngày hiện tại");
        
        if (request.CheckoutDate < request.CheckinDate.AddMonths(1))
            return ApiResponse<ContractResponse>.Fail("Ngày trả phòng phải ít nhất 1 tháng sau ngày nhận phòng.");
        
        var room = await _roomClient.GetRoomByIdAsync(request.RoomId);
        if (room == null)
            return ApiResponse<ContractResponse>.Fail("Không tìm thấy phòng");
        if (room.RoomStatus == RoomStatus.Occupied)
            return ApiResponse<ContractResponse>.Fail("Phòng đã có người thuê");
        var contract = _mapper.Map<Contract>(request);
        contract.OwnerId = ownerId;
        contract.CreatedAt = DateTime.UtcNow;
        contract.ContractStatus = ContractStatus.Active;
        if (request.ProfilesInContract == null || !request.ProfilesInContract.Any())
            return  ApiResponse<ContractResponse>.Fail("Danh sách người thuê không được để trống");
        var members = request.ProfilesInContract
            .Select((p, index) =>
            {
                var profile = _mapper.Map<IdentityProfile>(p);
                profile.IsSigner = index == 0;
                //  profile.AvatarUrl = _imageClient.UploadImage(p.AvatarUrl).Result;
                // profile.BackImageUrl=   _imageClient.UploadImage(p.BackImageUrl).Result;
                //  profile.FrontImageUrl = _imageClient.UploadImage(p.FrontImageUrl).Result;
                return profile;
            }).ToList();
   
        contract.ProfilesInContract = members;
        // var signer = members.FirstOrDefault(p => p.IsSigner);
        // if (signer == null)
        //     return ApiResponse<ContractResponseDto>.Fail("Không tìm thấy người ký hợp đồng (IsSigner = true)");

       // contract.SignerProfile = signer;
        contract.SignerProfile = members.First(p => p.IsSigner); 
        var saveContract =await _contractRepository.AddAsync(contract);
        var elecCreated = await _utilityReadingClientService.AddElectric(saveContract.RoomId, request.ElectricityReading);
        var waterCreated = await _utilityReadingClientService.AddWater(saveContract.RoomId, request.WaterReading);
        var result = _mapper.Map<ContractResponse>(saveContract);
        result.ElectricityReading = elecCreated.Data;
        result.WaterReading = waterCreated.Data;
      //  await _roomClient.UpdateRoomStatusAsync(request.RoomId, RoomStatus.Occupied);
        return ApiResponse<ContractResponse>.Success(result, "Thuê thành công.");
    }
    public async Task<ApiResponse<ContractResponse>> CancelContractAsync(Guid contractId, string reason)
    {
        var contract = await _contractRepository.GetByIdAsync(contractId);
        if (contract == null)
            return ApiResponse<ContractResponse>.Fail("Không tìm thấy hợp đồng thuê");

        if (contract.ContractStatus != ContractStatus.Active)
            return ApiResponse<ContractResponse>.Fail("Chỉ hợp đồng đang hoạt động mới có thể huỷ");
        
        contract.ContractStatus = ContractStatus.Cancelled;
        contract.UpdatedAt = DateTime.UtcNow;
        contract.Reason = reason;
        await _roomClient.UpdateRoomStatusAsync(contract.RoomId, RoomStatus.Available);
        await _contractRepository.UpdateAsync(contract);
        var dto = _mapper.Map<ContractResponse>(contract);
        return ApiResponse<ContractResponse>.Success(dto, "Huỷ hợp đồng thành công");
    }

    public async Task<ApiResponse<bool>> UpdateAsync(Guid id, UpdateContractDto request)
    {
        var contract = await _contractRepository.GetByIdAsync(id);
        if (contract == null)
            throw new KeyNotFoundException("Contract Id not found");
       
        if (DateTime.UtcNow - contract.CreatedAt > TimeSpan.FromHours(1))
            return ApiResponse<bool>.Fail("Đơn này đã quá 1 giờ, không thể cập nhật nữa.");
        
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
            contract.SignerProfile = members.First(p => p.IsSigner);
        }
      //  Console.WriteLine("sss"+ utilityReading.Data.Id);
        if (request.ElectricityReading != null) 
            await _utilityReadingClientService.UpdateElectric(contract.RoomId, request.ElectricityReading);
        if (request.WaterReading != null) 
            await _utilityReadingClientService.UpdateWater(contract.RoomId, request.WaterReading);

        // if (contract.ContractStatus != ContractStatus.Active)
        //     await _roomClient.UpdateRoomStatusAsync(contract.RoomId, "Available");
        contract.UpdatedAt = DateTime.UtcNow;
        _mapper.Map(request, contract);
        await _contractRepository.UpdateAsync(contract);
        return ApiResponse<bool>.Success(true, "Cập nhật hợp đồng thành công.");
    }

    public async Task<ApiResponse<ContractResponse>> ExtendContractAsync(Guid contractId, ExtendContractDto request)
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


// nếu nhiều người kí
// contract.ProfilesInContract = request.ProfilesInContract
//     .Select(p =>
//     {
//         var profile = _mapper.Map<IdentityProfile>(p);
//         // Nếu IsSigner thì gán SignerProfile
//         if (p.IsSigner)
//         {
//             contract.SignerProfile = profile;
//             contract.SignerProfileId = profile.ProfileId != Guid.Empty
//                 ? profile.ProfileId
//                 : Guid.NewGuid();
//             profile.IsSigner = true;
//         }
//         else
//         {
//             profile.IsSigner = false;
//         }
//         return profile;
//     }).ToList();
        
//         // Map SignerProfile
//         var signer = _mapper.Map<IdentityProfile>(request.SignerProfile);
//         signer.IsSigner = true;
//
// // Map tất cả người ở
//         var members = request.ProfilesInContract
//             .Select(p =>
//             {
//                 var profile = _mapper.Map<IdentityProfile>(p);
//                 profile.IsSigner = false;
//                 return profile;
//             }).ToList();
//
// // Gán vào contract
//         contract.SignerProfile = signer;
//         contract.SignerProfile.UserId = signer.UserId != Guid.Empty ? signer.UserId : Guid.NewGuid();
//
// // Thêm signer vào danh sách members
//         members.Insert(0, signer);
//         contract.ProfilesInContract = members;