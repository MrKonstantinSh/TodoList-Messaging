using MassTransit;

namespace TodoList.EmailSender.Consumers;

public sealed class EmailSenderConsumerDefinition : ConsumerDefinition<EmailSenderConsumer>
{
    public EmailSenderConsumerDefinition(IConfiguration configuration)
    {
        EndpointName = configuration["RabbitMQ:SendEmailQueue"]!;
        ConcurrentMessageLimit = 1;
    }
}