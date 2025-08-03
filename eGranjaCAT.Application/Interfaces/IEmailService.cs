using eGranjaCAT.Application.Common;
using System.Net.Mail;


namespace eGranjaCAT.Infrastructure.Services
{
    public interface IEmailService
    {
        Task<ServiceResult<bool>> SendEmailAsync(string to, string subject, string templateName, Dictionary<string, string>? variables = null, IEnumerable<Attachment>? attachments = null);
    }
}