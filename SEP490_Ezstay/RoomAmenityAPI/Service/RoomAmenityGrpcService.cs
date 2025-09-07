

using AutoMapper;
using Grpc.Core;
using RoomAmenityAPI.Grpc;
using RoomAmenityAPI.Service.Interface;


public class RoomAmenityGrpcService: RoomAmenityService.RoomAmenityServiceBase
{

    private readonly IRoomAmenityService _roomAmenityService;
    private readonly IMapper _mapper;

    public RoomAmenityGrpcService(IRoomAmenityService roomAmenityService, IMapper mapper)
    {
        _roomAmenityService = roomAmenityService;
        _mapper = mapper;
    }

    public override async Task<GetRoomAmenityResponse> GetRoomAmenity(GetRoomAmenityRequest request, ServerCallContext context)
    {
        var amenity = await _roomAmenityService.GetByIdAsync(Guid.Parse(request.Id));
        return new GetRoomAmenityResponse
        {
            Id = amenity.Id.ToString(),
            RoomId = amenity.RoomId.ToString(),
            AmenityId = amenity.AmenityId.ToString(),
            
        };
    }
    
     public override async Task<GetRoomAmenitiesByRoomIdResponse> GetRoomAmenitiesByRoomId(
         GetRoomAmenitiesByRoomIdRequest request, ServerCallContext context)
    {
        try
        {
            if (!Guid.TryParse(request.RoomId, out var roomId))
                throw new RpcException(new Status(StatusCode.InvalidArgument, "RoomId không hợp lệ"));

            var amenities = await _roomAmenityService.GetRoomAmenitiesByRoomIdAsync(roomId);

            return new GetRoomAmenitiesByRoomIdResponse
            {
                Amenities = { _mapper.Map<IEnumerable<GetRoomAmenityResponse>>(amenities) }
            };
        }
        catch (Exception ex)
        {
            throw new RpcException(new Status(StatusCode.Internal, ex.Message));
        }
    }
    
   
}