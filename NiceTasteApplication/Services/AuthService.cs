

using NiceTasteApplication.DTOs;
using NiceTasteApplication.DTOs.AuthDtos;
using NiceTasteApplication.Interface.AuthenticationInterface;
using System.Security.Authentication;

namespace NiceTasteApplication.Services
{
    public class AuthService : IAuthService
    {
        public Task<bool> EnableTwoFactorAsync(string userId)
        {
            
            throw new NotImplementedException();
        }

        public Task<UserPreferenceDto> GetUserPreferencesAsync(Guid userId, UserPreferenceDto userPreferenceDto)
        {
            throw new NotImplementedException();
        }

        public Task<(string Token, Response)> LoginAsync(LoginRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<(string Token, string RefreshToken)> RefreshTokenAsync(string refreshToken)
        {
            throw new NotImplementedException();
        }

        public Task<Response> RegisterUserAsync(RegisterRequest request)
        {
            throw new NotImplementedException();
            //    try
            //    {
            //        var existingUser = await _context.Users
            //            .FirstOrDefaultAsync(u => u.Email == registerDto.Email);

            //        if (existingUser != null)
            //        {
            //            return ApiResponse<AuthResponseDto>.Error(
            //                "Email already registered",
            //                "EMAIL_EXISTS",
            //                400);
            //        }

            //        var user = _mapper.Map<User>(registerDto);
            //        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);
            //        user.CreatedAt = DateTime.UtcNow;
            //        user.IsActive = true;

            //        // Create default preferences
            //        user.Preferences = new UserPreferences
            //        {
            //            NotificationPreferences = new NotificationPreferences(),
            //            PrivacyPreferences = new PrivacyPreferences()
            //        };

            //        _context.Users.Add(user);
            //        await _context.SaveChangesAsync();

            //        // Generate tokens
            //        var token = _jwtService.GenerateToken(
            //            user.Id.ToString(),
            //            new[] { "User" },
            //            new Dictionary<string, string> { { "email", user.Email } });

            //        var refreshToken = GenerateRefreshToken();
            //        user.RefreshToken = refreshToken;
            //        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

            //        await _context.SaveChangesAsync();

            //        var userDto = _mapper.Map<UserDto>(user);
            //        var response = new Response
            //        {
            //            Token = token,
            //            RefreshToken = refreshToken,
            //            ExpiresAt = DateTime.UtcNow.AddHours(1),
            //            User = userDto
            //        };

            //        return Response.Ok(response);
            //    }
            //    catch (Exception ex)
            //    {
            //        _logger.LogError(ex, "Error during user registration");
            //        throw new BaseException("Registration failed", ex);
            //    }
        }

        //private readonly IDistributedCache _cache;
        //private readonly ILogger<AuthService> _logger;
        //private readonly Random _random;

        //private async Task<string> GenerateEmailVerificationCodeAsync(string email)
        //{
        //    try
        //    {
        //        if (string.IsNullOrEmpty(email))
        //            throw new ArgumentNullException(nameof(email));

        //        string code = _random.Next(100000, 999999).ToString();

        //        await _cache.SetStringAsync(
        //            $"email_verification:{email}",
        //            code,
        //            new DistributedCacheEntryOptions
        //            {
        //                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15)
        //            }
        //        );

        //        return code;
        //    }
        //    catch (ArgumentNullException ex)
        //    {
        //        _logger.LogError(ex, "Invalid email provided for verification code generation");
        //        throw;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Failed to generate verification code for {Email}", email);
        //        throw new AuthenticationException("Failed to generate verification code", ex);
        //    }
        //}

        //public async Task<bool> VerifyEmailAsync(string email, string code)
        //{
        //    try
        //    {
        //        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(code))
        //            return false;

        //        var storedCode = await _cache.GetStringAsync($"email_verification:{email}");
        //        if (storedCode == null)
        //            return false;

        //        var isValid = storedCode == code;
        //        if (isValid)
        //        {
        //            await _cache.RemoveAsync($"email_verification:{email}");
        //        }

        //        return isValid;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Failed to verify code for {Email}", email);
        //        throw new AuthenticationException("Email verification failed", ex);
        //    }
        //}

        public Task<Response> ResetPasswordAsync(ResetPassword resetPassword)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ValidatePasswordStrengthAsync(string password)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ValidateTokenAsync(string token)
        {
            throw new NotImplementedException();
        }

        public Task<bool> VerifyEmailAsync(string userId, string token)
        {
            throw new NotImplementedException();
        }

        public Task<bool> VerifyTwoFactorCodeAsync(string userId, string code)
        {
            throw new NotImplementedException();
        }

        Task<UserDto> IAuthService.GetUserByIdAsync(Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}
