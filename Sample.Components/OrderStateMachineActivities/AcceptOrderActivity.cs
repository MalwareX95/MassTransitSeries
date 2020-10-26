﻿using Automatonymous;
using GreenPipes;
using Sample.Components.StateMachines;
using Sample.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Components.OrderStateMachineActivities
{
    public class AcceptOrderActivity
        : Activity<OrderState, OrderAccepted>
    {
        public void Accept(StateMachineVisitor visitor)
        {
            visitor.Visit(this);
        }

        public async Task Execute(BehaviorContext<OrderState, OrderAccepted> context, Behavior<OrderState, OrderAccepted> next)
        {
            Console.WriteLine("Hello world, Order is {0}", context.Data.OrderId);
            await next.Execute(context).ConfigureAwait(false);
        }

        public Task Faulted<TException>(BehaviorExceptionContext<OrderState, OrderAccepted, TException> context, Behavior<OrderState, OrderAccepted> next) where TException : Exception
        {
            return next.Faulted(context);
        }

        public void Probe(ProbeContext context)
        {
            context.CreateScope("accept-order");
        }
    }
}