namespace TodoList.MessagingContracts.Commands;

public sealed record SendEmailCommand(string Message, ICollection<string> Emails);