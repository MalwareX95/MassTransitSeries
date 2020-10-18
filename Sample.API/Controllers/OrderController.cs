using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sample.Contracts;

namespace Sample.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> logger;
        private readonly IRequestClient<SubmitOrder> submitOrderRequestClient;
        private readonly ISendEndpointProvider sendEndpointProvider;
        private readonly IRequestClient<CheckOrder> checkOrderClient;

        public OrderController(
            ILogger<OrderController> logger,
            IRequestClient<SubmitOrder> submitOrderRequestClient,
            ISendEndpointProvider sendEndpointProvider,
            IRequestClient<CheckOrder> checkOrderClient)
        {
            this.logger = logger;
            this.submitOrderRequestClient = submitOrderRequestClient;
            this.sendEndpointProvider = sendEndpointProvider;
            this.checkOrderClient = checkOrderClient;
        }

        [HttpGet]
        public async Task<IActionResult> Get(Guid id)
        {
            var (status, notFound) = await checkOrderClient.GetResponse<OrderStatus, OrderNotFound>(new
            {
                OrderId = id
            });

            if (status.IsCompletedSuccessfully)
            {
                var response = await status;
                return Ok(response.Message);
            }
            else
            {
                var response = await notFound;
                return NotFound(response.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(Guid id, string customerNumber)
        {
            var (accepted, rejected) = await submitOrderRequestClient.GetResponse<OrderSubmissionAccepted, OrderSubmissionRejected>(new
            {
                OrderId = id,
                InVar.Timestamp,
                CustomerNumber = customerNumber
            });

            if (accepted.IsCompletedSuccessfully)
            {
                var response = await accepted;
                return Accepted(response);
            }
            else
            {
                var response = await rejected;
                return BadRequest(response.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put(Guid id, string customerNumber)
        {
            var endpoint = await sendEndpointProvider.GetSendEndpoint(new Uri("exchange:submit-order"));

            await endpoint.Send<SubmitOrder>(new
            {
                OrderId = id,
                InVar.Timestamp,
                CustomerNumber = customerNumber
            });

            return Accepted();
        }

    }
}
