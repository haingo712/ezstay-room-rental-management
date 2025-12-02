using AuthApi.DTO.Request;
using AuthApi.DTO.Response;

namespace AuthApi.Services.Interfaces
{
    /// <summary>
    /// Interface for face login service
    /// </summary>
    public interface IFaceLoginService
    {
        /// <summary>
        /// Login using face recognition
        /// </summary>
        /// <param name="dto">Face image for login</param>
        /// <returns>Login response with token if successful</returns>
        Task<FaceLoginResponseDto> LoginWithFaceAsync(FaceLoginRequestDto dto);

        /// <summary>
        /// Register a new face for the current user
        /// </summary>
        /// <param name="userId">Current user's ID</param>
        /// <param name="dto">Face registration data</param>
        /// <returns>Registration response</returns>
        Task<RegisterFaceResponseDto> RegisterFaceAsync(Guid userId, RegisterFaceRequestDto dto);

        /// <summary>
        /// Verify if a face matches the current user
        /// </summary>
        /// <param name="userId">Current user's ID</param>
        /// <param name="dto">Face image to verify</param>
        /// <returns>Verification response</returns>
        Task<VerifyFaceResponseDto> VerifyFaceAsync(Guid userId, VerifyFaceRequestDto dto);

        /// <summary>
        /// Get all registered faces for the current user
        /// </summary>
        /// <param name="userId">Current user's ID</param>
        /// <returns>List of registered faces</returns>
        Task<GetFacesResponseDto> GetFacesAsync(Guid userId);

        /// <summary>
        /// Update a registered face
        /// </summary>
        /// <param name="userId">Current user's ID</param>
        /// <param name="dto">Update data</param>
        /// <returns>Update response</returns>
        Task<UpdateFaceResponseDto> UpdateFaceAsync(Guid userId, UpdateFaceRequestDto dto);

        /// <summary>
        /// Delete a registered face
        /// </summary>
        /// <param name="userId">Current user's ID</param>
        /// <param name="faceId">Face ID to delete</param>
        /// <returns>Delete response</returns>
        Task<DeleteFaceResponseDto> DeleteFaceAsync(Guid userId, Guid faceId);

        /// <summary>
        /// Check if a user has any registered faces
        /// </summary>
        /// <param name="userId">User's ID</param>
        /// <returns>True if user has registered faces</returns>
        Task<bool> HasRegisteredFaceAsync(Guid userId);
    }
}
