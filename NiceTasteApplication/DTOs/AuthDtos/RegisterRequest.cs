namespace NiceTasteApplication.DTOs.AuthDtos
{
     public record RegisterRequest
        (
         string Name,
         string Email,
         string PhoneNumber,
         string Password,
         string ConfirmPassword
         );
}
