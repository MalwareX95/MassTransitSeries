using Automatonymous;
using MassTransit.RedisIntegration;
using System;

namespace Sample.Components.StateMachines
{
    public class OrderState: 
        SagaStateMachineInstance,
        IVersionedSaga
    {
        public Guid CorrelationId { get; set; }
        public string CurrentState { get; set; }
        public string CustomerNumber { get; set; }
        public int Version { get; set; }
        public DateTime? SubmitDate { get; set; }
        public DateTime? Updated { get; set; }
    }
}