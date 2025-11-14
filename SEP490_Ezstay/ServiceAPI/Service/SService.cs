using AutoMapper;
using ServiceAPI.DTO.Response;
using ServiceAPI.DTO.Resquest;
using ServiceAPI.Model;
using ServiceAPI.Repositories.Interfaces;
using ServiceAPI.Service.Interfaces;

namespace ServiceAPI.Service
{
    public class SService : ISService
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly IMapper _mapper;

        public SService(IServiceRepository serviceRepository, IMapper mapper)
        {
            _serviceRepository = serviceRepository;
            _mapper = mapper;
        }

        public async Task<ServiceResponseDto> CreateServiceAsync(ServiceRequestDto request)
        {
            // Validate dữ liệu
            if (string.IsNullOrEmpty(request.ServiceName))
                throw new ArgumentException("Service name cannot be empty.");
            if (request.Price < 0)
                throw new ArgumentException("Price must be non-negative.");

            // Map từ RequestDTO -> Model
            var serviceModel = _mapper.Map<ServiceModel>(request);

            // Tạo service trong MongoDB
            await _serviceRepository.CreateServiceAsync(serviceModel);

            // Map ngược lại Model -> ResponseDTO
            var response = _mapper.Map<ServiceResponseDto>(serviceModel);
            return response;
        }

        public async Task<List<ServiceResponseDto>> GetAllServicesAsync()
        {
            var services = await _serviceRepository.GetAllServicesAsync();
            return _mapper.Map<List<ServiceResponseDto>>(services);
        }

        public async Task<ServiceResponseDto> GetServiceByIdAsync(string id)
        {
            var service = await _serviceRepository.GetServiceByIdAsync(id);
            return _mapper.Map<ServiceResponseDto>(service);
        }

        public async Task UpdateServiceAsync(string id, ServiceRequestDto updatedService)
        {
            var serviceModel = _mapper.Map<ServiceModel>(updatedService);
            await _serviceRepository.UpdateServiceAsync(id, serviceModel);
        }

        public async Task DeleteServiceAsync(string id)
        {
            await _serviceRepository.DeleteServiceAsync(id);
        }

    }
}
