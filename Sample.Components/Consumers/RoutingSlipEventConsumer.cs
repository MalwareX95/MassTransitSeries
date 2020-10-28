using MassTransit;
using MassTransit.Courier.Contracts;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Components.Consumers
{
    public class RoutingSlipEventConsumer :
        IConsumer<RoutingSlipCompleted>,
        IConsumer<RoutingSlipActivityCompleted>,
        IConsumer<RoutingSlipFaulted>
    {
        private readonly ILogger<RoutingSlipEventConsumer> logger;

        public RoutingSlipEventConsumer(ILogger<RoutingSlipEventConsumer> logger)
        {
            this.logger = logger;
        }
        public Task Consume(ConsumeContext<RoutingSlipCompleted> context)
        {
            if (logger.IsEnabled(LogLevel.Information))
            {
                logger.LogInformation("Routing Slip Completed: {TrackingNumber}", context.Message.TrackingNumber);
            }

            return Task.CompletedTask;
        }

        public Task Consume(ConsumeContext<RoutingSlipActivityCompleted> context)
        {
            if (logger.IsEnabled(LogLevel.Information))
            {
                logger.LogInformation("Routing Slip Activity Completed: {TrackingNumber} {ActivityName}", context.Message.TrackingNumber);
            }

            return Task.CompletedTask;
        }

        public Task Consume(ConsumeContext<RoutingSlipFaulted> context)
        {
            if (logger.IsEnabled(LogLevel.Information))
            {
                logger.LogInformation("Routing Slip Faulted: {TrackingNumber} {ExceptionInfo}", context.Message.TrackingNumber, context.Message.ActivityExceptions.FirstOrDefault());
            }

            return Task.CompletedTask;
        }
    }
}
