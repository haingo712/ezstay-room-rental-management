using System.Text.Json;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using BoardingHouseAPI.DTO.Request;
using BoardingHouseAPI.DTO.Response;
using BoardingHouseAPI.Enum;
using BoardingHouseAPI.Models;
using BoardingHouseAPI.Repository.Interface;
using BoardingHouseAPI.Service.Interface;

namespace BoardingHouseAPI.Service
{
    public class BoardingHouseService : IBoardingHouseService
    {
        private readonly IBoardingHouseRepository _boardingHouseRepo;
        private readonly IMapper _mapper;
        private readonly IImageClientService _imageClient;
        private readonly IRoomClientService _roomClient;
        private readonly IReviewClientService _reviewClient;
        private readonly ISentimentAnalysisClientService _sentimentAnalysisClient;
        private readonly HttpClient _http;

        public BoardingHouseService(IBoardingHouseRepository boardingHouseRepo, IMapper mapper,
            IImageClientService imageClient, IRoomClientService roomClient,
            IReviewClientService reviewClientService, ISentimentAnalysisClientService sentimentAnalysisClientService, 
            IHttpClientFactory factory)
        {
            _boardingHouseRepo = boardingHouseRepo;
            _mapper = mapper;
            _imageClient = imageClient;
            _roomClient = roomClient;
            _reviewClient = reviewClientService;
            _sentimentAnalysisClient = sentimentAnalysisClientService;
            _http = factory.CreateClient("Gateway");
        }

        private async Task<string?> GetProvinceNameAsync(string provinceId)
        {
            var response = await _http.GetFromJsonAsync<JsonElement>("/api/provinces");
            var provinces = response.GetProperty("provinces").EnumerateArray();
            return provinces.FirstOrDefault(p => p.GetProperty("code").GetString() == provinceId)
                            .GetProperty("name").GetString();
        }

        private async Task<string?> GetCommuneNameAsync(string provinceId, string communeId)
        {
            var response = await _http.GetFromJsonAsync<JsonElement>($"/api/provinces/{provinceId}/communes");
            var communes = response.GetProperty("communes").EnumerateArray();
            return communes.FirstOrDefault(c => c.GetProperty("code").GetString() == communeId)
                           .GetProperty("name").GetString();
        }

        public IQueryable<BoardingHouseDTO> GetAll()
        {
            var houses = _boardingHouseRepo.GetAll();
            return houses.ProjectTo<BoardingHouseDTO>(_mapper.ConfigurationProvider);
        }

        public IQueryable<BoardingHouseDTO> GetByOwnerId(Guid ownerId)
        {
            return _boardingHouseRepo.GetBoardingHousesByOwnerId(ownerId)
                .ProjectTo<BoardingHouseDTO>(_mapper.ConfigurationProvider);
        }

        public async Task<BoardingHouseDTO?> GetByIdAsync(Guid id)
        {
            var house = await _boardingHouseRepo.GetByIdAsync(id);
            return house == null ? throw new KeyNotFoundException("HouseId not found!") :
                _mapper.Map<BoardingHouseDTO>(house);
        }

        public async Task<ApiResponse<BoardingHouseDTO>> CreateAsync(Guid ownerId, CreateBoardingHouseDTO createDto)
        {

            var houseLocation = _mapper.Map<HouseLocation>(createDto.Location);

            houseLocation.ProvinceName = await GetProvinceNameAsync(houseLocation.ProvinceId) ?? "";
            houseLocation.CommuneName = await GetCommuneNameAsync(houseLocation.ProvinceId, houseLocation.CommuneId) ?? "";

            houseLocation.FullAddress = $"{houseLocation.AddressDetail}, {houseLocation.CommuneName}, {houseLocation.ProvinceName}";

            // check trùng địa chỉ
            var locationExist = await _boardingHouseRepo.LocationExists(houseLocation.FullAddress);
            if (locationExist)
                return ApiResponse<BoardingHouseDTO>.Fail("Địa chỉ này đã tồn tại.");

            //var exist = await _boardingHouseRepo.LocationExistsWithHouseName(createDto.HouseName, houseLocation.FullAddress);
            //if (exist)
            //    return ApiResponse<BoardingHouseDTO>.Fail("Nhà trọ với tên và địa chỉ này đã tồn tại.");
            var house = _mapper.Map<BoardingHouse>(createDto);
            house.OwnerId = ownerId;            
            house.Location = houseLocation;

            await _boardingHouseRepo.AddAsync(house);
            return ApiResponse<BoardingHouseDTO>.Success(
                _mapper.Map<BoardingHouseDTO>(house), "Thêm nhà trọ thành công");
        }

        public async Task<ApiResponse<bool>> UpdateAsync(Guid id, UpdateBoardingHouseDTO updateDto)
        {
            var exist = await _boardingHouseRepo.GetByIdAsync(id);
            if (exist == null) throw new KeyNotFoundException("HouseId not found!");
            var oldAddress = exist.Location?.FullAddress ?? "";
            _mapper.Map(updateDto, exist);
            if (exist.Location != null && updateDto.Location != null)
            {
                _mapper.Map(updateDto.Location, exist.Location);

                exist.Location.ProvinceName = await GetProvinceNameAsync(exist.Location.ProvinceId) ?? "";
                exist.Location.CommuneName = await GetCommuneNameAsync(exist.Location.ProvinceId, exist.Location.CommuneId) ?? "";

                exist.Location.FullAddress =
                    $"{exist.Location.AddressDetail}, {exist.Location.CommuneName}, {exist.Location.ProvinceName}";

                // check trùng địa chỉ (nếu địa chỉ thay đổi)
                var existLocation = await _boardingHouseRepo.LocationExists(exist.Location.FullAddress);
                if (existLocation && exist.Location.FullAddress != oldAddress)
                    return ApiResponse<bool>.Fail("Địa chỉ này đã tồn tại.");

                // Check trùng địa chỉ + tên
                //var existAddress = await _boardingHouseRepo.LocationExistsWithHouseName(updateDto.HouseName, exist.Location.FullAddress);
                //if (existAddress && (exist.HouseName != updateDto.HouseName || exist.Location.FullAddress != oldAddress))
                //    return ApiResponse<bool>.Fail("Nhà trọ với tên và địa chỉ này đã tồn tại.");
            }            
            await _boardingHouseRepo.UpdateAsync(exist);
            return ApiResponse<bool>.Success(true, "Cập nhật trọ thành công");
        }

        public async Task<ApiResponse<bool>> DeleteAsync(Guid id)
        {
            var house = await _boardingHouseRepo.GetByIdAsync(id);
            if (house == null) throw new KeyNotFoundException("HouseId not found!");
            var rooms = await _roomClient.GetRoomsByHouseIdAsync(id);
            if (rooms != null && rooms.Count > 0)
                return ApiResponse<bool>.Fail("Không thể xoá nhà trọ khi còn tồn tại phòng!");


            await _boardingHouseRepo.DeleteAsync(house);
            return ApiResponse<bool>.Success(true, "Xoá trọ thành công");
        }

        public async Task<List<BoardingHouseRankResponse>> GetRankedBoardingHousesAsync(RankType type, string order, int limit)
        {
            var allHouses = _boardingHouseRepo.GetAll().ToList();
            var rankedList = new List<BoardingHouseRankResponse>();

            foreach (var house in allHouses)
            {
                var rooms = await _roomClient.GetRoomsByHouseIdAsync(house.Id);
                if (rooms == null || !rooms.Any())
                    continue;

                var reviews = await _reviewClient.GetReviewsByRoomsAsync(rooms);
                if (reviews == null || !reviews.Any())
                    continue;

                double score = 0;

                if (type == RankType.Rating)
                {
                    score = reviews.Average(r => r.Rating);
                }
                else if (type == RankType.Sentiment)
                {
                    var sentiments = await _sentimentAnalysisClient.SentimentAnalysisAsync(reviews);
                    score = sentiments.Average(s => s.Label switch
                    {
                        "positive" => 1.0,
                        "neutral" => 0.5,
                        "negative" => 0.0,
                        _ => 0.5
                    });
                }

                rankedList.Add(new BoardingHouseRankResponse
                {
                    BoardingHouseId = house.Id,
                    HouseName = house.HouseName,
                    FullAddress = house.Location.FullAddress,
                    Score = Math.Round(score, 2),
                    Type = type
                });
            }

            rankedList = order.ToLower() == "asc"
                ? rankedList.OrderBy(h => h.Score).ToList()
                : rankedList.OrderByDescending(h => h.Score).ToList();

            if (limit > 0)
                rankedList = rankedList.Take(limit).ToList();

            return rankedList;
        }

        public async Task<ApiResponse<SentimentSummaryResponse>> GetSentimentSummaryAsync(Guid boardingHouseId)
        {
            var rooms = await _roomClient.GetRoomsByHouseIdAsync(boardingHouseId);
            if (rooms == null || !rooms.Any())
                return ApiResponse<SentimentSummaryResponse>.Fail("Không có phòng nào trong nhà trọ này.");
            var reviews = await _reviewClient.GetReviewsByRoomsAsync(rooms);
            if (reviews == null || !reviews.Any())
            {
                return ApiResponse<SentimentSummaryResponse>.Success(new SentimentSummaryResponse
                    {
                        BoardingHouseId = boardingHouseId,
                        TotalReviews = 0,
                        PositiveCount = 0,
                        NeutralCount = 0,
                        NegativeCount = 0,
                        Details = new List<SentimentResponse>()
                    }, "Không có đánh giá nào cho nhà trọ này.");
            }

            var sentiments = await _sentimentAnalysisClient.SentimentAnalysisAsync(reviews);
            var summary = new SentimentSummaryResponse
            {
                BoardingHouseId = boardingHouseId,
                TotalReviews = sentiments.Count,
                PositiveCount = sentiments.Count(s => s.Label == "positive"),
                NeutralCount = sentiments.Count(s => s.Label == "neutral"),
                NegativeCount = sentiments.Count(s => s.Label == "negative"),
                Details = sentiments
            };

            return ApiResponse<SentimentSummaryResponse>.Success(summary, "Thống kê sentiment thành công.");


        }

        public async Task<ApiResponse<RatingSummaryResponse>> GetRatingSummaryAsync(Guid boardingHouseId)
        {
            // 1️⃣ Lấy danh sách phòng trong nhà trọ
            var rooms = await _roomClient.GetRoomsByHouseIdAsync(boardingHouseId);
            if (rooms == null || !rooms.Any())
                return ApiResponse<RatingSummaryResponse>.Fail("Không có phòng nào trong nhà trọ này.");

            // 2️⃣ Lấy tất cả review theo phòng
            var reviews = await _reviewClient.GetReviewsByRoomsAsync(rooms);
            if (reviews == null || !reviews.Any())
            {
                return ApiResponse<RatingSummaryResponse>.Success(new RatingSummaryResponse
                {
                    BoardingHouseId = boardingHouseId,
                    TotalReviews = 0,
                    AverageRating = 0,
                    OneStarCount = 0,
                    TwoStarCount = 0,
                    ThreeStarCount = 0,
                    FourStarCount = 0,
                    FiveStarCount = 0,
                    Details = new List<ReviewResponse>()
                }, "Không có đánh giá nào cho nhà trọ này.");
            }
            
            var totalReviews = reviews.Count;
            var avgRating = reviews.Average(r => r.Rating);

            var summary = new RatingSummaryResponse
            {
                BoardingHouseId = boardingHouseId,
                TotalReviews = totalReviews,
                AverageRating = Math.Round(avgRating, 2), 
                OneStarCount = reviews.Count(r => r.Rating == 1),
                TwoStarCount = reviews.Count(r => r.Rating == 2),
                ThreeStarCount = reviews.Count(r => r.Rating == 3),
                FourStarCount = reviews.Count(r => r.Rating == 4),
                FiveStarCount = reviews.Count(r => r.Rating == 5),
                Details = reviews
            };

            return ApiResponse<RatingSummaryResponse>.Success(summary, "Thống kê rating thành công.");
        }

    }
}
