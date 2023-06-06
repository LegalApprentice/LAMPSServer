using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;

using LAMPSServer.Models;
using LAMPSServer.Hubs;

namespace LAMPSServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentHubController : ControllerBase
    {
        private readonly IHubContext<DocumentHub> hubContext;

        public DocumentHubController(IHubContext<DocumentHub> hubContext)
        {
            this.hubContext = hubContext;

        }

        [HttpGet("Pong")]
        public async Task SendPing(string payload)
        {
            var data = $"Controller pong message {payload}";
            await this.hubContext.Clients.All.SendAsync("Pong", data);
        }



        [HttpPost("PayloadWrapper")]
        public async Task<ActionResult<PayloadWrapper<Success>>> SendContext(object context)
        {
            try {
                await this.hubContext.Clients.All.SendAsync("ReceiveContextPayload", context);
                var wrap = new PayloadWrapper<Success>(new Success()
                {
                    Status = true
                });
                return Ok(wrap);
            }
            catch (Exception ex)
            {
                 var wrap = new PayloadWrapper<Success>(ex.Message);
                return BadRequest(wrap);
            }
        }








    }
}
