using LAMPSServer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace LAMPSServer.Controllers
{
    [ApiController]
    [Route("/")]
    public class HomeController : ControllerBase
    {

        public HomeController()
        {
        }


        //[HttpGet]
        //public ActionResult GetDefault()
        //{

        //    return Redirect("/index.html");
        //}


        // https://localhost:44360/legalmarker/index.html/#/marker/case1800023

        // https://localhost:44360/legalmarker/#/marker/case1800023


        //CREATE THIS!!!
        //"http://localhost:4200/#/marker/case1800023",
        //http://localhost:4200/#/case/case1800023

        [HttpGet("marker/case/{caseid}")]
        public ActionResult Marker(string caseid)
        {

            return Redirect($"/legalmarker/#/case/{caseid}");
        }


        [HttpGet("case")]
        public ActionResult Case()
        {

            return Redirect("/legalmarker/index.html");
        }

        //[HttpGet("legalmarker")]
        //public ActionResult GetMarker()
        //{

        //    return Redirect("/legalmarker/index.html");
        //}

        //[HttpGet("legalpad")]
        //public ActionResult GetPad()
        //{

        //    return Redirect("/legalpad/index.html");
        //}

        //[HttpGet("legalsearch")]
        //public ActionResult GetSearch()
        //{

        //    return Redirect("/legalsearch/index.html");
        //}

        [HttpGet("TestCase")]
        public ActionResult GetTestCase()
        {
            //
            return Redirect("https://localhost:44360/legalmarker/#/case/case1800023");
        }

        [HttpGet("swagger")]
        public ActionResult GetSwagger()
        {
            return Redirect("/swagger/index.html");
        }

        [HttpGet("api/ping")]
        public ActionResult<PayloadWrapper<Success>> GetPing()
        {
            try
            {
                var wrap = new PayloadWrapper<Success>(new Success() { 
                    Message = "up",
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
