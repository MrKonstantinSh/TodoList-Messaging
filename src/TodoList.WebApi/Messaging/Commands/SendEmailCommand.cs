namespace TodoList.WebApi.Messaging.Commands;

public class SendEmailCommand : ISendEmailCommand
{
    public string Message { get; set; }
    public ICollection<string> Emails { get; set; }

    public SendEmailCommand(string message, ICollection<string> emails)
    {
        Message = message;
        Emails = emails;
    }
}