using GreenPipes;
using MassTransit;
using MassTransit.ConsumeConfigurators;
using MassTransit.Definition;

namespace Sample.Components.Consumers
{
    public class SubmitOrderConsumerDefinition : ConsumerDefinition<SubmitOrderConsumer>
    {

        //public SubmitOrderConsumerDefinition()
        //{
        //    EndpointName = "Frank";
        //    ConcurrentMessageLimit = 4;
        //}

        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<SubmitOrderConsumer> consumerConfigurator)
        {
            endpointConfigurator.UseMessageRetry(r => r.Intervals(3, 1000));
            //filter dla wszystkich wiadomości
            //endpointConfigurator.UseExecute(context => Console.WriteLine("Hello" + context.SourceAddress))
            //filter dla konkretnej wiadomości
            //consumerConfigurator.Message<SubmitOrder>(x => x.UseExecute(context => context.Message.))
        }
    }
}
