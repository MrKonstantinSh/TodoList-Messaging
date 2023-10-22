using System.Net.Mail;

namespace TodoList.EmailSender.EmailSenderService;

public sealed class EmailSender : IEmailSender
{
    private readonly ILogger<EmailSender> _logger;
    private readonly IConfiguration _configuration;

    public EmailSender(ILogger<EmailSender> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    public async Task SendEmailAsync(ICollection<string> to, string subject, string body)
    {
        try
        {
            var smtpServer = _configuration["EmailSender:SmtpServer"];
            var port = int.Parse(_configuration["EmailSender:Port"]!);
            var emailClient = new SmtpClient(smtpServer, port);

            var emailFrom = _configuration["EmailSender:EmailFrom"]!;
            var message = new MailMessage
            {
                From = new MailAddress(emailFrom),
                Subject = subject,
                Body = body
            };
            
            foreach (var emailTo in to)
            {
                message.To.Add(new MailAddress(emailTo));
            }
            
            await emailClient.SendMailAsync(message);

            _logger.LogWarning("Sending email to {To} from {From} with subject {Subject}.", to, emailFrom, subject);
        }
        catch (Exception ex)
        {
            _logger.LogError("An error occurred while sending the email: {Exception}", ex);
        }
    }
}