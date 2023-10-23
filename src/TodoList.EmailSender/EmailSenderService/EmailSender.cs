using System.Net;
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
        var smtpServer = _configuration["EmailSender:SmtpServer"];
        var port = int.Parse(_configuration["EmailSender:Port"]!);
        var emailFrom = _configuration["EmailSender:EmailFrom"]!;
        var passwordFrom = _configuration["EmailSender:PasswordFrom"]!;
        
        using (var emailClient = new SmtpClient(smtpServer, port))
        {
            emailClient.EnableSsl = true;
            emailClient.Credentials = new NetworkCredential(emailFrom, passwordFrom);
            
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
        }

        _logger.LogInformation("Email has sent to {To} from {From} with subject {Subject}", to, emailFrom, subject);
    }
}