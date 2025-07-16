using System.Net.Mail;

namespace web_api.BLL.Services.Email
{
    public interface IEmailService
    {
        Task SendMailAsync(MailMessage message);
        Task SendMailAsync(string to, string subject, string body, bool isHtml = false);
    }
}