using eGranjaCAT.Application.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Mail;


namespace eGranjaCAT.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<EmailService> _logger;
        private readonly IWebHostEnvironment _env;

        public EmailService(IConfiguration config, ILogger<EmailService> logger, IWebHostEnvironment env)
        {
            _config = config;
            _logger = logger;
            _env = env;
        }

        public async Task<ServiceResult<bool>> SendEmailAsync(string to, string subject, string templateName, Dictionary<string, string>? variables = null, IEnumerable<Attachment>? attachments = null)
        {
            try
            {
                string body = string.Empty;
                if (!string.IsNullOrEmpty(templateName))
                {
                    body = await ReplaceEmailBodyVarsAsync(templateName, variables ?? new Dictionary<string, string>());
                }

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_config["EmailSettings:EmailSender"]!),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };
                mailMessage.To.Add(to);

                if (attachments != null)
                {
                    foreach (var attachment in attachments)
                        mailMessage.Attachments.Add(attachment);
                }

                using var smtpClient = new SmtpClient
                {
                    Host = _config["EmailSettings:Host"]!,
                    Port = _config.GetValue<int>("EmailSettings:Port"),
                    EnableSsl = _config.GetValue<bool>("EmailSettings:EnableSsl"),
                    Credentials = new System.Net.NetworkCredential
                    {
                        UserName = _config["EmailSettings:EmailSender"]!,
                        Password = _config["EmailSettings:EmailPassword"]!
                    }
                };

                await smtpClient.SendMailAsync(mailMessage);
                _logger.LogInformation("Email sent to {To} with subject '{Subject}'", to, subject);

                return ServiceResult<bool>.Ok(true, 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email to {To}", to);
                return ServiceResult<bool>.FromException(ex, 500);
            }
        }

        private async Task<string> GetEmailTemplateAsync(string templateName)
        {
            var path = Path.Combine(_env.ContentRootPath, "EmailTemplates", templateName);
            return await File.ReadAllTextAsync(path);
        }

        private async Task<string> ReplaceEmailBodyVarsAsync(string templateName, Dictionary<string, string> variables)
        {
            var template = await GetEmailTemplateAsync(templateName);
            foreach (var variable in variables)
            {
                template = template.Replace($"{{{variable.Key}}}", variable.Value);
            }
            return template;
        }
    }
}