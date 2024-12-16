using NiceTasteApplication.DTOs;
using NiceTasteApplication.DTOs.AuthDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiceTasteApplication.Interface.AuthenticationInterface
{
    internal interface IAuthService
    {
        Task<Response> RegisterUserAsync(RegisterRequest request);
        Task<(string Token, Response)> LoginAsync(LoginRequest request);
        Task<bool> VerifyEmailAsync(string userId, string token);
        Task<Response> ResetPasswordAsync(ResetPassword resetPassword);
        Task<UserDto> GetUserByIdAsync(Guid userId);
        Task<bool> EnableTwoFactorAsync(string userId);
        Task<bool> VerifyTwoFactorCodeAsync(string userId, string code);
        Task<bool> ValidatePasswordStrengthAsync(string password);
        Task<(string Token, string RefreshToken)> RefreshTokenAsync(string refreshToken);
        Task<bool> ValidateTokenAsync(string token);
        Task<UserPreferenceDto> GetUserPreferencesAsync(Guid userId, UserPreferenceDto userPreferenceDto);
    }
}
