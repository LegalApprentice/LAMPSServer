using LAMPSServer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace LAMPSServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthController : ControllerBase
    {

        public HealthController()
        {
        }


        [HttpGet]
        public ActionResult<PayloadWrapper<Success>> GetHealth()
        {
            try
            {

                var wrap = new PayloadWrapper<Success>(new Success() { 
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
