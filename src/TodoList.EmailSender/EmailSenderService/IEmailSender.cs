namespace TodoList.EmailSender.EmailSenderService;

public interface IEmailSender
{
    public Task SendEmailAsync(ICollection<string> to, string subject, string body);
}