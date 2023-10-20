namespace TodoList.WebApi.Messaging.Commands;

public interface ISendEmailCommand
{
    public string Message { get; set; }
    public ICollection<string> Emails { get; set; }
}