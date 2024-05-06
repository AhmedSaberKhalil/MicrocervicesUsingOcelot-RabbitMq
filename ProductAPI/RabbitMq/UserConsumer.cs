using MassTransit;

namespace ProductAPI.RabbitMq
{
    public class UserConsumer : IConsumer<UserDeatails>
    {
        public async Task Consume(ConsumeContext<UserDeatails> context)
        {
            var data = context.Message;
            //Validate the Ticket Data
            //Store to Database
            //Notify the user via Email / SMS
        }
    }
}
