using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Options;

namespace LocalServicesMarketplace.Identity.API.Infrastructure.Email
{
    public interface IEmailSender
    {
        Task SendEmailAsync(EmailMessage message);
        Task SendBulkEmailAsync(IEnumerable<EmailMessage> messages);
    }

    public class EmailSender : IEmailSender
    {
        private readonly EmailSettings _settings;
        private readonly ILogger<EmailSender> _logger;
        private readonly SmtpClient _smtpClient;

        public EmailSender(
            IOptions<EmailSettings> settings,
            ILogger<EmailSender> logger)
        {
            _settings = settings.Value;
            _logger = logger;

            _smtpClient = new SmtpClient(_settings.SmtpServer, _settings.SmtpPort)
            {
                Credentials = new NetworkCredential(_settings.SmtpUsername, _settings.SmtpPassword),
                EnableSsl = true
            };
        }

        public async Task SendEmailAsync(EmailMessage message)
        {
            try
            {
                var mailMessage = CreateMailMessage(message);
                await _smtpClient.SendMailAsync(mailMessage);

                _logger.LogInformation(
                    "Email sent successfully to {Recipient}",
                    message.To);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to send email to {Recipient}",
                    message.To);
                throw;
            }
        }

        public async Task SendBulkEmailAsync(IEnumerable<EmailMessage> messages)
        {
            var tasks = messages.Select(SendEmailAsync);
            await Task.WhenAll(tasks);
        }

        private MailMessage CreateMailMessage(EmailMessage message)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress(_settings.FromEmail, _settings.FromName),
                Subject = message.Subject,
                Body = message.Body,
                IsBodyHtml = message.IsHtml,
                Priority = message.Priority
            };

            mailMessage.To.Add(message.To);

            if (message.Cc != null)
            {
                foreach (var cc in message.Cc)
                {
                    mailMessage.CC.Add(cc);
                }
            }

            if (message.Bcc != null)
            {
                foreach (var bcc in message.Bcc)
                {
                    mailMessage.Bcc.Add(bcc);
                }
            }

            if (message.Attachments != null)
            {
                foreach (var attachment in message.Attachments)
                {
                    mailMessage.Attachments.Add(attachment);
                }
            }

            return mailMessage;
        }
    }

    public class EmailMessage
    {
        public string To { get; set; } = string.Empty;
        public List<string>? Cc { get; set; }
        public List<string>? Bcc { get; set; }
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public bool IsHtml { get; set; }
        public MailPriority Priority { get; set; } = MailPriority.Normal;
        public List<Attachment>? Attachments { get; set; }
    }

    public class EmailSettings
    {
        public string SmtpServer { get; set; } = string.Empty;
        public int SmtpPort { get; set; }
        public string SmtpUsername { get; set; } = string.Empty;
        public string SmtpPassword { get; set; } = string.Empty;
        public string FromEmail { get; set; } = string.Empty;
        public string FromName { get; set; } = string.Empty;
        public string WebsiteBaseUrl { get; set; } = string.Empty;
        public string SupportEmail { get; set; } = string.Empty;
    }
}
