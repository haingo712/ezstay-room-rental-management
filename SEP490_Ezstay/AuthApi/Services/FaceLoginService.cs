using AuthApi.DTO.Request;
using AuthApi.DTO.Response;
using AuthApi.Models;
using AuthApi.Repositories.Interfaces;
using AuthApi.Services.Interfaces;
using AuthApi.Utils;
using System.Text.Json;
using System.Net.Http.Json;

namespace AuthApi.Services
{
    public class FaceLoginService : IFaceLoginService
    {
        private readonly IAuthRepository _authRepository;
        private readonly GenerateJwtToken _tokenGenerator;
        private readonly HttpClient _httpClient;
        private readonly ILogger<FaceLoginService> _logger;
        private readonly string _pythonApiUrl;
        private const double SIMILARITY_THRESHOLD = 0.87;
        private const int MAX_FACES_PER_USER = 5;

        public FaceLoginService(
            IAuthRepository authRepository,
            GenerateJwtToken tokenGenerator,
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            ILogger<FaceLoginService> logger)
        {
            _authRepository = authRepository;
            _tokenGenerator = tokenGenerator;
            _httpClient = httpClientFactory.CreateClient();
            _logger = logger;
            _pythonApiUrl = configuration["FaceRecognition:PythonApiUrl"] ?? "http://localhost:5001";
        }
        public async Task<FaceLoginResponseDto> LoginWithFaceAsync(FaceLoginRequestDto dto)
        {
            try
            {
                // Get face embedding from Python service
                var embedding = await GetFaceEmbeddingAsync(dto.FaceImage);
                if (embedding == null || embedding.Length == 0)
                {
                    return new FaceLoginResponseDto
                    {
                        Success = false,
                        Message = "Could not detect face in the image. Please try again with a clear face photo."
                    };
                }

                // Get all users with registered faces
                var allUsers = await _authRepository.GetAllAsync();
                var usersWithFaces = allUsers.Where(u => u.FaceEmbeddings != null && u.FaceEmbeddings.Count > 0).ToList();

                if (!usersWithFaces.Any())
                {
                    return new FaceLoginResponseDto
                    {
                        Success = false,
                        Message = "No registered faces found. Please register your face first."
                    };
                }

                // Find best matching face
                Account? matchedUser = null;
                double bestSimilarity = 0;
                string? matchedEmail = null;

                foreach (var user in usersWithFaces)
                {
                    foreach (var face in user.FaceEmbeddings!)
                    {
                        var similarity = CalculateCosineSimilarity(embedding, face.Embedding);
                        _logger.LogInformation("Face comparison: User {Email}, Similarity: {Similarity:F4}", 
                            user.Email, similarity);
                        
                        if (similarity > bestSimilarity && similarity >= SIMILARITY_THRESHOLD)
                        {
                            bestSimilarity = similarity;
                            matchedUser = user;
                            matchedEmail = user.Email;
                        }
                    }
                }

                _logger.LogInformation("Best match: {Email} with similarity {Similarity:F4}", 
                    matchedEmail ?? "None", bestSimilarity);

                if (matchedUser == null)
                {
                    return new FaceLoginResponseDto
                    {
                        Success = false,
                        Message = "Face not recognized. Please try again or login with email/password."
                    };
                }

                // Check if user is banned
                if (matchedUser.IsBanned)
                {
                    return new FaceLoginResponseDto
                    {
                        Success = false,
                        Message = "Account has been banned."
                    };
                }

                // Check if user is verified
                if (!matchedUser.IsVerified)
                {
                    return new FaceLoginResponseDto
                    {
                        Success = false,
                        Message = "Account has not been verified."
                    };
                }

                // Generate token
                var token = _tokenGenerator.CreateToken(
                    role: matchedUser.Role.ToString(),
                    userId: matchedUser.Id.ToString()
                );

                return new FaceLoginResponseDto
                {
                    Success = true,
                    Message = "Face login successful",
                    Token = token,
                    Confidence = Math.Round(bestSimilarity * 100, 2)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during face login");
                return new FaceLoginResponseDto
                {
                    Success = false,
                    Message = "An error occurred during face login. Please try again."
                };
            }
        }
        public async Task<RegisterFaceResponseDto> RegisterFaceAsync(Guid userId, RegisterFaceRequestDto dto)
        {
            try
            {
                var user = await _authRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    return new RegisterFaceResponseDto
                    {
                        Success = false,
                        Message = "User not found."
                    };
                }

                // Initialize face list if null
                user.FaceEmbeddings ??= new List<FaceData>();

                // Check max faces limit
                if (user.FaceEmbeddings.Count >= MAX_FACES_PER_USER)
                {
                    return new RegisterFaceResponseDto
                    {
                        Success = false,
                        Message = $"Maximum {MAX_FACES_PER_USER} faces can be registered per account."
                    };
                }

                // Get face embedding from Python service
                var embedding = await GetFaceEmbeddingAsync(dto.FaceImage);
                if (embedding == null || embedding.Length == 0)
                {
                    return new RegisterFaceResponseDto
                    {
                        Success = false,
                        Message = "Could not detect face in the image. Please try again with a clear face photo."
                    };
                }

                // Check if similar face already exists for this user
                foreach (var existingFace in user.FaceEmbeddings)
                {
                    var similarity = CalculateCosineSimilarity(embedding, existingFace.Embedding);
                    if (similarity >= 0.8) // High threshold for same face
                    {
                        return new RegisterFaceResponseDto
                        {
                            Success = false,
                            Message = "This face is already registered for your account."
                        };
                    }
                }

                // Create thumbnail (resize image for storage)
                var thumbnail = CreateThumbnail(dto.FaceImage);

                // Create new face data
                var faceData = new FaceData
                {
                    Id = Guid.NewGuid(),
                    Embedding = embedding,
                    Label = dto.Label ?? $"Face {user.FaceEmbeddings.Count + 1}",
                    ImageThumbnail = thumbnail,
                    CreatedAt = DateTime.UtcNow
                };

                user.FaceEmbeddings.Add(faceData);
                await _authRepository.UpdateAsync(user);

                return new RegisterFaceResponseDto
                {
                    Success = true,
                    Message = "Face registered successfully.",
                    FaceId = faceData.Id
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error registering face for user {UserId}", userId);
                return new RegisterFaceResponseDto
                {
                    Success = false,
                    Message = "An error occurred while registering face. Please try again."
                };
            }
        }
        public async Task<VerifyFaceResponseDto> VerifyFaceAsync(Guid userId, VerifyFaceRequestDto dto)
        {
            try
            {
                var user = await _authRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    return new VerifyFaceResponseDto
                    {
                        Success = false,
                        Message = "User not found.",
                        IsMatch = false
                    };
                }

                if (user.FaceEmbeddings == null || user.FaceEmbeddings.Count == 0)
                {
                    return new VerifyFaceResponseDto
                    {
                        Success = false,
                        Message = "No registered faces found for this account.",
                        IsMatch = false
                    };
                }

                // Get face embedding from Python service
                var embedding = await GetFaceEmbeddingAsync(dto.FaceImage);
                if (embedding == null || embedding.Length == 0)
                {
                    return new VerifyFaceResponseDto
                    {
                        Success = false,
                        Message = "Could not detect face in the image.",
                        IsMatch = false
                    };
                }

                // Find best matching face
                double bestSimilarity = 0;
                foreach (var face in user.FaceEmbeddings)
                {
                    var similarity = CalculateCosineSimilarity(embedding, face.Embedding);
                    if (similarity > bestSimilarity)
                    {
                        bestSimilarity = similarity;
                    }
                }

                var isMatch = bestSimilarity >= SIMILARITY_THRESHOLD;

                return new VerifyFaceResponseDto
                {
                    Success = true,
                    Message = isMatch ? "Face verified successfully." : "Face does not match.",
                    IsMatch = isMatch,
                    Confidence = Math.Round(bestSimilarity * 100, 2)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error verifying face for user {UserId}", userId);
                return new VerifyFaceResponseDto
                {
                    Success = false,
                    Message = "An error occurred during verification.",
                    IsMatch = false
                };
            }
        }
        public async Task<GetFacesResponseDto> GetFacesAsync(Guid userId)
        {
            try
            {
                var user = await _authRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    return new GetFacesResponseDto
                    {
                        Success = false,
                        Message = "User not found."
                    };
                }

                var faces = user.FaceEmbeddings?.Select(f => new FaceDataDto
                {
                    Id = f.Id,
                    Label = f.Label,
                    ImageThumbnail = f.ImageThumbnail,
                    CreatedAt = f.CreatedAt,
                    UpdatedAt = f.UpdatedAt
                }).ToList() ?? new List<FaceDataDto>();

                return new GetFacesResponseDto
                {
                    Success = true,
                    Message = "Faces retrieved successfully.",
                    Faces = faces
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting faces for user {UserId}", userId);
                return new GetFacesResponseDto
                {
                    Success = false,
                    Message = "An error occurred while retrieving faces."
                };
            }
        }
        public async Task<UpdateFaceResponseDto> UpdateFaceAsync(Guid userId, UpdateFaceRequestDto dto)
        {
            try
            {
                var user = await _authRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    return new UpdateFaceResponseDto
                    {
                        Success = false,
                        Message = "User not found."
                    };
                }

                var face = user.FaceEmbeddings?.FirstOrDefault(f => f.Id == dto.FaceId);
                if (face == null)
                {
                    return new UpdateFaceResponseDto
                    {
                        Success = false,
                        Message = "Face not found."
                    };
                }

                // Update label if provided
                if (!string.IsNullOrEmpty(dto.Label))
                {
                    face.Label = dto.Label;
                }

                // Update image if provided
                if (!string.IsNullOrEmpty(dto.FaceImage))
                {
                    var embedding = await GetFaceEmbeddingAsync(dto.FaceImage);
                    if (embedding == null || embedding.Length == 0)
                    {
                        return new UpdateFaceResponseDto
                        {
                            Success = false,
                            Message = "Could not detect face in the new image."
                        };
                    }

                    face.Embedding = embedding;
                    face.ImageThumbnail = CreateThumbnail(dto.FaceImage);
                }

                face.UpdatedAt = DateTime.UtcNow;
                await _authRepository.UpdateAsync(user);

                return new UpdateFaceResponseDto
                {
                    Success = true,
                    Message = "Face updated successfully."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating face for user {UserId}", userId);
                return new UpdateFaceResponseDto
                {
                    Success = false,
                    Message = "An error occurred while updating face."
                };
            }
        }
        public async Task<DeleteFaceResponseDto> DeleteFaceAsync(Guid userId, Guid faceId)
        {
            try
            {
                var user = await _authRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    return new DeleteFaceResponseDto
                    {
                        Success = false,
                        Message = "User not found."
                    };
                }

                var face = user.FaceEmbeddings?.FirstOrDefault(f => f.Id == faceId);
                if (face == null)
                {
                    return new DeleteFaceResponseDto
                    {
                        Success = false,
                        Message = "Face not found."
                    };
                }

                user.FaceEmbeddings!.Remove(face);
                await _authRepository.UpdateAsync(user);

                return new DeleteFaceResponseDto
                {
                    Success = true,
                    Message = "Face deleted successfully."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting face for user {UserId}", userId);
                return new DeleteFaceResponseDto
                {
                    Success = false,
                    Message = "An error occurred while deleting face."
                };
            }
        }
        public async Task<bool> HasRegisteredFaceAsync(Guid userId)
        {
            var user = await _authRepository.GetByIdAsync(userId);
            return user?.FaceEmbeddings != null && user.FaceEmbeddings.Count > 0;
        }

        #region Private Helper Methods
        private async Task<double[]?> GetFaceEmbeddingAsync(string base64Image)
        {
            try
            {
                var request = new { image = base64Image };
                var response = await _httpClient.PostAsJsonAsync($"{_pythonApiUrl}/api/face/embedding", request);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Face embedding API returned status {StatusCode}", response.StatusCode);
                    return null;
                }

                var result = await response.Content.ReadFromJsonAsync<FaceEmbeddingResponse>();
                return result?.Embedding;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling face embedding API");
                return null;
            }
        }
        private double CalculateCosineSimilarity(double[] vec1, double[] vec2)
        {
            if (vec1.Length != vec2.Length || vec1.Length == 0)
                return 0;

            double dotProduct = 0;
            double norm1 = 0;
            double norm2 = 0;

            for (int i = 0; i < vec1.Length; i++)
            {
                dotProduct += vec1[i] * vec2[i];
                norm1 += vec1[i] * vec1[i];
                norm2 += vec2[i] * vec2[i];
            }

            if (norm1 == 0 || norm2 == 0)
                return 0;

            return dotProduct / (Math.Sqrt(norm1) * Math.Sqrt(norm2));
        }

        private string CreateThumbnail(string base64Image)
        {
            if (base64Image.StartsWith("data:image"))
            {
                return base64Image.Length > 10000 ? base64Image.Substring(0, 10000) : base64Image;
            }
            return base64Image;
        }

        #endregion

        private class FaceEmbeddingResponse
        {
            public bool Success { get; set; }
            public double[]? Embedding { get; set; }
            public string? Error { get; set; }
        }
    }
}
