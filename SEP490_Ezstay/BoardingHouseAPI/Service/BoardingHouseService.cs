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
        private readonly IImageService _imageService;
        private readonly IRoomService _roomService;
        private readonly IReviewService _reviewService;
        private readonly ISentimentAnalysisService _sentimentAnalysisService;
        private readonly HttpClient _http;

        public BoardingHouseService(IBoardingHouseRepository boardingHouseRepo, IMapper mapper,
            IImageService imageService, IRoomService roomClient,
            IReviewService reviewService, ISentimentAnalysisService sentimentAnalysisService, 
            IHttpClientFactory factory)
        {
            _boardingHouseRepo = boardingHouseRepo;
            _mapper = mapper;
            _imageService = imageService;
            _roomService = roomClient;
            _reviewService = reviewService;
            _sentimentAnalysisService = sentimentAnalysisService;
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
                return ApiResponse<BoardingHouseDTO>.Fail("This address already exists.");

            //var exist = await _boardingHouseRepo.LocationExistsWithHouseName(createDto.HouseName, houseLocation.FullAddress);
            //if (exist)
            //    return ApiResponse<BoardingHouseDTO>.Fail("Nhà trọ với tên và địa chỉ này đã tồn tại.");
            var house = _mapper.Map<BoardingHouse>(createDto);
            house.OwnerId = ownerId;
            house.ImageUrls = await _imageService.UploadMultipleImagesAsync(createDto.Files!);

            house.Location = houseLocation;

            await _boardingHouseRepo.AddAsync(house);
            return ApiResponse<BoardingHouseDTO>.Success(
                _mapper.Map<BoardingHouseDTO>(house), "Created boarding house successfully!");
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
                    return ApiResponse<bool>.Fail("This address already exists.");

                // Check trùng địa chỉ + tên
                //var existAddress = await _boardingHouseRepo.LocationExistsWithHouseName(updateDto.HouseName, exist.Location.FullAddress);
                //if (existAddress && (exist.HouseName != updateDto.HouseName || exist.Location.FullAddress != oldAddress))
                //    return ApiResponse<bool>.Fail("Nhà trọ với tên và địa chỉ này đã tồn tại.");
            }
            exist.ImageUrls = await _imageService.UploadMultipleImagesAsync(updateDto.Files!);
            await _boardingHouseRepo.UpdateAsync(exist);
            return ApiResponse<bool>.Success(true, "Updated boarding house successfully!");
        }

        public async Task<ApiResponse<bool>> DeleteAsync(Guid id)
        {
            var house = await _boardingHouseRepo.GetByIdAsync(id);
            if (house == null) throw new KeyNotFoundException("HouseId not found!");
            var rooms = await _roomService.GetRoomsByHouseIdAsync(id);
            if (rooms != null && rooms.Count > 0)
                return ApiResponse<bool>.Fail("Cannot delete a boarding house while rooms still exist!");


            await _boardingHouseRepo.DeleteAsync(house);
            return ApiResponse<bool>.Success(true, "Deleted boarding house successfully!");
        }

        public async Task<List<BoardingHouseRankResponse>> GetRankedBoardingHousesAsync(RankType type, string order, int limit)
        {
            var allHouses = _boardingHouseRepo.GetAll().ToList();
            var rankedList = new List<BoardingHouseRankResponse>();

            foreach (var house in allHouses)
            {
                var rooms = await _roomService.GetRoomsByHouseIdAsync(house.Id);
                if (rooms == null || !rooms.Any())
                    continue;

                var reviews = await _reviewService.GetReviewsByRoomsAsync(rooms);
                if (reviews == null || !reviews.Any())
                    continue;

                double score = 0;

                if (type == RankType.Rating)
                {
                    score = reviews.Average(r => r.Rating);
                }
                else if (type == RankType.Sentiment)
                {
                    var sentiments = await _sentimentAnalysisService.SentimentAnalysisAsync(reviews);
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
            var rooms = await _roomService.GetRoomsByHouseIdAsync(boardingHouseId);
            if (rooms == null || !rooms.Any())
                return ApiResponse<SentimentSummaryResponse>.Fail("There are no rooms available in this boarding house.");
            var reviews = await _reviewService.GetReviewsByRoomsAsync(rooms);
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
                    }, "There are no reviews for this boarding house.");
            }

            var sentiments = await _sentimentAnalysisService.SentimentAnalysisAsync(reviews);
            var summary = new SentimentSummaryResponse
            {
                BoardingHouseId = boardingHouseId,
                TotalReviews = sentiments.Count,
                PositiveCount = sentiments.Count(s => s.Label == "positive"),
                NeutralCount = sentiments.Count(s => s.Label == "neutral"),
                NegativeCount = sentiments.Count(s => s.Label == "negative"),
                Details = sentiments
            };

            return ApiResponse<SentimentSummaryResponse>.Success(summary, "Sentiment statistics success.");


        }

        public async Task<ApiResponse<RatingSummaryResponse>> GetRatingSummaryAsync(Guid boardingHouseId)
        {
            // 1️⃣ Lấy danh sách phòng trong nhà trọ
            var rooms = await _roomService.GetRoomsByHouseIdAsync(boardingHouseId);
            if (rooms == null || !rooms.Any())
                return ApiResponse<RatingSummaryResponse>.Fail("There are no rooms available in this boarding house.");

            // 2️⃣ Lấy tất cả review theo phòng
            var reviews = await _reviewService.GetReviewsByRoomsAsync(rooms);
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
                }, "There are no reviews for this boarding house.");
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

            return ApiResponse<RatingSummaryResponse>.Success(summary, "Rating statistics successful.");
        }

    }
}
