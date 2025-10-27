using Quartz;

namespace web_api.Jobs
{
    public class LogCleanJob : IJob
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public LogCleanJob(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public Task Execute(IJobExecutionContext context)
        {
            var logPath = Path.Combine(_webHostEnvironment.ContentRootPath, "Logs");
            var logs = Directory.GetFiles(logPath);

            foreach (var log in logs)
            {
                var file = new FileInfo(log);
                var days = (DateTime.Now - file.CreationTime).Days;

                if (days >= 7)
                {
                    File.Delete(log);
                }
            }

            return Task.CompletedTask;
        }
    }
}
