using AutoMapper;
using AutoMapper.QueryableExtensions;
using UtilityBillAPI.DTO.Request;
using UtilityBillAPI.DTO.Response;
using UtilityBillAPI.Enum;
using UtilityBillAPI.Models;
using UtilityBillAPI.Repository.Interface;
using UtilityBillAPI.Service.Interface;

namespace UtilityBillAPI.Service
{
    public class UtilityBillService : IUtilityBillService
    {
        private readonly IUtilityBillRepository _utilityBillRepo;
        private readonly IRoomClient _roomClient;
        private readonly IUtilityReadingClient _utilityReadingClient;
        private readonly IMapper _mapper;
        private readonly ILogger<UtilityBillService> _logger;

        public UtilityBillService(IUtilityBillRepository utilityBillRepo, IMapper mapper, 
            IRoomClient roomClient, IUtilityReadingClient utilityReadingClient, ILogger<UtilityBillService> logger)
        {
            _utilityBillRepo = utilityBillRepo;
            _mapper = mapper;           
            _roomClient = roomClient;
            _utilityReadingClient = utilityReadingClient;
            _logger = logger;
        }

        public IQueryable<UtilityBillDTO> GetAll()
        {
            var bills = _utilityBillRepo.GetAll();
            return bills.ProjectTo<UtilityBillDTO>(_mapper.ConfigurationProvider);
        }

        public IQueryable<UtilityBillDTO> GetAllByOwnerId(Guid ownerId)
        {
            var bills = _utilityBillRepo.GetAll()
                .Where(b => b.OwnerId == ownerId);           

            return bills.ProjectTo<UtilityBillDTO>(_mapper.ConfigurationProvider);
        }

        public IQueryable<UtilityBillDTO> GetAllByTenantId(Guid tenantId)
        {
            var bills = _utilityBillRepo.GetAll()
                .Where(b => b.TenantId == tenantId);           
            return bills.ProjectTo<UtilityBillDTO>(_mapper.ConfigurationProvider);
        }        

        /*public async Task<IEnumerable<UtilityBillDTO>> GetOverdueBillsAsync()
        {
            var overdueBills = await _utilityBillRepo.GetOverdueBills();
            return _mapper.Map<IEnumerable<UtilityBillDTO>>(overdueBills);
        }*/

        public async Task<UtilityBillDTO?> GetByIdAsync(Guid id)
        {
            var bill = await _utilityBillRepo.GetByIdAsync(id);
            return bill == null ? throw new KeyNotFoundException("Bill not found!") : _mapper.Map<UtilityBillDTO>(bill);
        }

        public async Task<ApiResponse<UtilityBillDTO>> GenerateBillForRoomAsync(Guid ownerId, Guid roomId, Guid? tenantId)
        {
            try
            {
                // Check if spam bill
                var recentBill = _utilityBillRepo.GetAll()
                    .Where(b => b.RoomId == roomId && b.CreatedAt >= DateTime.UtcNow.AddMinutes(-1))
                    .OrderByDescending(b => b.CreatedAt)
                    .FirstOrDefault();

                if (recentBill != null)
                {
                    return ApiResponse<UtilityBillDTO>.Fail("Vui lòng chờ ít phút trước khi tạo hoá đơn mới cho phòng này.");
                }

                // Check presence of tenant in the room
                if (tenantId == null || tenantId == Guid.Empty)
                {
                    return ApiResponse<UtilityBillDTO>.Fail("Phòng hiện không có người thuê, không thể tạo hoá đơn.");
                }               

                // Get room information
                var room = await _roomClient.GetRoomAsync(roomId);
                if (room == null)
                {
                    return ApiResponse<UtilityBillDTO>.Fail("Không tìm thấy thông tin phòng");
                }

                // Get electricity and water readings              
                var electricity = await _utilityReadingClient.GetElectricityReadingAsync(roomId);
                var water = await _utilityReadingClient.GetWaterReadingAsync(roomId);

                var missingData = new List<string>();
                if (electricity == null) missingData.Add("điện");
                if (water == null) missingData.Add("nước");
                if (missingData.Any())
                {
                    return ApiResponse<UtilityBillDTO>.Fail($"Không thể tạo hoá đơn do thiếu thông tin {string.Join(" và ", missingData)} cho phòng này.");
                }

                var bill = new UtilityBillDTO
                {
                    OwnerId = ownerId,
                    TenantId = tenantId.Value,
                    RoomId = roomId,
                    ElectricityId = electricity.Id,
                    WaterId = water.Id,
                    Amount = room.Price + electricity.Total + water.Total,
                    CreatedAt = DateTime.UtcNow,                    
                    Status = UtilityBillStatus.Unpaid,
                    Note = null
                };

                await _utilityBillRepo.CreateAsync(_mapper.Map<UtilityBill>(bill));
                return ApiResponse<UtilityBillDTO>.Success(bill, "Tạo hoá đơn thành công");
            }            
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating bill for room {RoomId} by owner {OwnerId}", roomId, ownerId);
                return ApiResponse<UtilityBillDTO>.Fail("Đã xảy ra lỗi khi tạo hoá đơn, vui lòng thử lại.");
            }
        }

        public async Task<ApiResponse<bool>> UpdateBillAsync(Guid id, UpdateUtilityBillDTO dto)
        {
            var bill = await _utilityBillRepo.GetByIdAsync(id);          

            // Map and update the bill
            _mapper.Map(dto, bill);
            await _utilityBillRepo.UpdateAsync(bill);            

            return ApiResponse<bool>.Success(true, "Cập nhật hoá đơn thành công");
        }

        public async Task<ApiResponse<bool>> MarkAsPaidAsync(Guid billId, string paymentMethod)
        {
            var bill = await _utilityBillRepo.GetByIdAsync(billId);
            if (bill.Status == UtilityBillStatus.Cancelled)
            {
                return ApiResponse<bool>.Fail("Không thể thanh toán hoá đơn đã huỷ");
            }

            await _utilityBillRepo.MarkAsPaidAsync(billId, paymentMethod);
            return ApiResponse<bool>.Success(true, "Đã thanh toán hoá đơn thành công");
        }

        public async Task<ApiResponse<bool>> CancelAsync(Guid billId, string? cancelNote)
        {
            var bill = await _utilityBillRepo.GetByIdAsync(billId);
            if (bill.Status == UtilityBillStatus.Paid)
            {
                return ApiResponse<bool>.Fail("Không thể huỷ hoá đơn đã thanh toán");
            }

            await _utilityBillRepo.CancelAsync(billId, cancelNote);
            return ApiResponse<bool>.Success(true, "Đã huỷ hoá đơn thành công");
        }

        /*public async Task<ApiResponse<bool>> GenerateBillAsync(Guid ownerId, DateTime dueDate)
        {
            try
            {
                var roomsResponse = await _httpClient.GetAsync($"{_configuration["ServiceUrls:RoomAPI"]}/owner/{ownerId}");
                if (!roomsResponse.IsSuccessStatusCode)
                {
                    return ApiResponse<bool>.Fail("Không thể lấy danh sách phòng");
                }

                var rooms = await roomsResponse.Content.ReadFromJsonAsync<List<RoomDTO>>();
                if (rooms == null || !rooms.Any())
                {
                    return ApiResponse<bool>.Fail("Không có phòng nào để tạo hoá đơn");
                }
                foreach (var room in rooms)
                {
                    var electricityResponse = await _httpClient.GetAsync($"{_configuration["ServiceUrls:ReadingAPI"]}/api/electricity/{room.Id}");
                    var waterResponse = await _httpClient.GetAsync($"{_configuration["ServiceUrls:ReadingAPI"]}/api/water/{room.Id}");
                    if (!electricityResponse.IsSuccessStatusCode || !waterResponse.IsSuccessStatusCode)
                    {
                        return ApiResponse<bool>.Fail("Không thể lấy thông tin điện nước cho phòng " + room.Id);
                    }
                    var electricity = await electricityResponse.Content.ReadFromJsonAsync<ReadingDTO>();
                    var water = await waterResponse.Content.ReadFromJsonAsync<ReadingDTO>();
                    if (electricity == null || water == null)
                    {
                        continue; 
                    }
                   

                    var bill = new UtilityBillDTO
                    {                    
                        RoomId = room.Id,
                        ElectricityId = electricity.Id,
                        WaterId = water.Id,
                        Amount = room.Price + electricity.Total + water.Total, 
                        CreatedAt = DateTime.Now,
                        DueDate = dueDate,
                        IsPaid = false,
                        IsCancelled = false,
                        Note = null
                    };
                    await _utilityBillRepo.CreateAsync(_mapper.Map<UtilityBill>(bill));
                }
                return ApiResponse<bool>.Success(true, "Tạo hoá đơn thành công cho tất cả các phòng");
            }
            catch (HttpRequestException ex)
            {
                return ApiResponse<bool>.Fail($"Lỗi kết nối đến dịch vụ: {ex.Message}");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Fail($"Lỗi tạo hoá đơn: {ex.Message}");
            }
        }*/




        /* public async Task<ApiResponse<bool>> MarkAsOverdueAsync(Guid billId)
         {
             var bill = await _utilityBillRepo.GetByIdAsync(billId);
             if (bill == null) throw new KeyNotFoundException("Bill not found!");
             if (bill.Status == UtilityBillStatus.Paid)
             {
                 return ApiResponse<bool>.Fail("Không thể đặt hoá đơn đã thanh toán thành quá hạn");
             }
             if (bill.Status == UtilityBillStatus.Cancelled)
             {
                 return ApiResponse<bool>.Fail("Không thể đặt hoá đơn đã huỷ thành quá hạn");
             }
             await _utilityBillRepo.MarkAsOverdueAsync(billId);
             return ApiResponse<bool>.Success(true, "Đã đặt hoá đơn thành quá hạn thành công");
         }*/
    }


}
