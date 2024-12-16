using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiceTasteApplication.Interface.AuthenticationInterface
{
    internal interface IEmailService
    {
        Task SendVerificationEmailAsync(string email, string token);
        Task SendPasswordResetEmailAsync(string email, string token);
        Task SendPasswordChangedEmailAsync(string email);
        Task SendAccountLockedEmailAsync(string email);
        Task SendAccountDeletionConfirmationAsync(string email);
        Task SendLoginAlertAsync(string email, string ipAddress, string location, string deviceInfo);
    }
}
