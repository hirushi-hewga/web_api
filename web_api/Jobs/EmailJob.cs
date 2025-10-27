using Quartz;
using web_api.BLL.Services.Email;

namespace web_api.Jobs
{
    public class EmailJob : IJob
    {
        private readonly IEmailService _emailService;

        public EmailJob(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public Task Execute(IJobExecutionContext context)
        {
            return _emailService.SendMailAsync("lo067803@gmail.com", "Quartz text", "Email job send mail");
        }
    }
}
