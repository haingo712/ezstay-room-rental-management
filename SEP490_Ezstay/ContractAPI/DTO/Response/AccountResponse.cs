using ContractAPI.Enum;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ContractAPI.DTO.Response
{
    public class AccountResponse
    {
        public Guid Id { get; set; } 
        public GenderEnum? Gender { get; set; }
        public string? Avatar { get; set; }
        public string? Bio { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string? DetailAddress { get; set; }
        public string Email { get; set; }
        public string ProvinceId { get; set; }     // Mã tỉnh
        public string ProvinceName { get; set; }   // Tên tỉnh
        public string WardId { get; set; }         // Mã xã/phường
        public string WardName { get; set; }       // Tên xã/phường

        public string? FrontImageUrl { get; set; }
        public string? BackImageUrl { get; set; }
        public string? TemporaryResidence { get; set; }
        public string? CitizenIdNumber { get; set; }
        public DateTime ? CitizenIdIssuedDate { get; set; }
        public string ?CitizenIdIssuedPlace { get; set; }
    }
}
