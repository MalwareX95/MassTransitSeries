 using MassTransit;
using MassTransit.Courier;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Contracts;

namespace Sample.Components.CourierActivities
{
    public class AllocateInventoryActivity
        : IActivity<AllocateInventoryArguments, AllocateInventoryLog>
    {
        readonly IRequestClient<AllocateInventory> client;

        public AllocateInventoryActivity(IRequestClient<AllocateInventory> client)
        {
            this.client = client;
        }

        public async Task<ExecutionResult> Execute(ExecuteContext<AllocateInventoryArguments> context)
        {
            var orderId = context.Arguments.OrderId;

            var itemNumber = context.Arguments.ItemNumber;

            if (string.IsNullOrEmpty(itemNumber))
            {
                throw new ArgumentNullException(nameof(itemNumber));
            }

            var quantity = context.Arguments.Quantity;
            if (quantity <= 0.0m)
            {
                throw new ArgumentNullException(nameof(quantity));
            }

            var allocationId = NewId.NextGuid();

            var response = await client.GetResponse<InventoryAllocated>(new
            {
                AllocationId = allocationId,
                ItemNumber = itemNumber,
                Quantity = quantity
            });

            return context.Completed(new { AllocationId = allocationId });
        }
        public async Task<CompensationResult> Compensate(CompensateContext<AllocateInventoryLog> context)
        {
            await context.Publish<AllocationReleaseRequested>(new
            {
                context.Log.AllocationId,
                Reason = "Order faulted"
            });

            return context.Compensated();
        }
    }
 }
