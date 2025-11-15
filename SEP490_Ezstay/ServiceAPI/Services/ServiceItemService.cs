using APIGateway.Helper.Interfaces;
using AutoMapper;
using ServiceAPI.DTO.Response;
using ServiceAPI.DTO.Resquest;
using ServiceAPI.Model;
using ServiceAPI.Repositories.Interfaces;
using ServiceAPI.Service.Interfaces;

namespace ServiceAPI.Service
{
    public class ServiceItemService : IServiceItemService
    {
        private readonly IServiceItemRepository _serviceRepository;
        private readonly IMapper _mapper;
        private readonly IUserClaimHelper _userClaimHelper;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public ServiceItemService(
     IServiceItemRepository serviceRepository,
     IMapper mapper,
     IUserClaimHelper userClaimHelper,
     IHttpContextAccessor httpContextAccessor)
        {
            _serviceRepository = serviceRepository;
            _mapper = mapper;
            _userClaimHelper = userClaimHelper;
            _httpContextAccessor = httpContextAccessor;
        }


        public async Task<ServiceItemResponseDto> CreateServiceAsync(ServiceItemRequestDto request)
        {
            // Validate dữ liệu
            if (string.IsNullOrEmpty(request.ServiceName))
                throw new ArgumentException("Service name cannot be empty.");
            if (request.Price < 0)
                throw new ArgumentException("Price must be non-negative.");

            // Map từ RequestDTO -> Model
           var user = _httpContextAccessor.HttpContext.User;
            var userId = _userClaimHelper.GetUserId(user);
            var serviceModel = _mapper.Map<ServiceItem>(request);
            serviceModel.OwnerId = userId;

            // Tạo service trong MongoDB
            await _serviceRepository.CreateServiceAsync(serviceModel);

            // Map ngược lại Model -> ResponseDTO
            var response = _mapper.Map<ServiceItemResponseDto>(serviceModel);
            return response;
        }

        public async Task<List<ServiceItemResponseDto>> GetAllServicesAsync()
        {
            var services = await _serviceRepository.GetAllServicesAsync();
            return _mapper.Map<List<ServiceItemResponseDto>>(services);
        }

        public async Task<ServiceItemResponseDto> GetServiceByIdAsync(Guid id)
        {
            var service = await _serviceRepository.GetServiceByIdAsync(id);
            return _mapper.Map<ServiceItemResponseDto>(service);
        }

        public async Task UpdateServiceAsync(Guid id, ServiceItemRequestDto updatedService)
        {
            var user = _httpContextAccessor.HttpContext.User;
            var userId = _userClaimHelper.GetUserId(user);
            var serviceModel = _mapper.Map<ServiceItem>(updatedService);
            serviceModel.OwnerId = userId;
            await _serviceRepository.UpdateServiceAsync(id, serviceModel);
        }

        public async Task DeleteServiceAsync(Guid id)
        {
            await _serviceRepository.DeleteServiceAsync(id);
        }

    }
}
