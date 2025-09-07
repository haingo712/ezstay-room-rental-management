using Grpc.Core;
using AmenityAPI.Grpc;
using AmenityAPI.Models;
using AmenityAPI.Repository.Interface;

public class AmenityGrpcService : AmenityService.AmenityServiceBase
{
    private readonly IAmenityRepository _repository;

    public AmenityGrpcService(IAmenityRepository repository)
    {
        _repository = repository;
    }

    public override async Task<GetAmenityResponse> GetAmenityById(GetAmenityRequest request, ServerCallContext context)
    {
        var amenity = await _repository.GetByIdAsync(Guid.Parse(request.Id));
        if (amenity == null)
            throw new RpcException(new Status(StatusCode.NotFound, $"Amenity {request.Id} not found"));
        return new GetAmenityResponse
        {
            Id = amenity.Id.ToString(),
            Name = amenity.AmenityName
        };
    }
}