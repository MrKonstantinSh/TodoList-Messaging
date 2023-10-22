using MassTransit;
using TodoList.EmailSender.EmailSenderService;
using TodoList.MessagingContracts.Commands;

namespace TodoList.EmailSender.Consumers;

public sealed class EmailSenderConsumer : IConsumer<SendEmailCommand>
{
    private readonly IEmailSender _emailSender;

    public EmailSenderConsumer(ILogger<EmailSenderConsumer> logger, IEmailSender emailSender)
    {
        _emailSender = emailSender;
    }

    public async Task Consume(ConsumeContext<SendEmailCommand> context)
    {
        await _emailSender.SendEmailAsync(context.Message.Emails, "TodoList - Notification",
            context.Message.Message).ConfigureAwait(false);
    }
}