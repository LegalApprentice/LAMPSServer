namespace LAMPSServer.Controllers;
using System;
using Microsoft.AspNetCore.Mvc;
using LAMPSServer.Models;
using Microsoft.AspNetCore.Http;
using LAMPSServer.Helpers;
using System.Collections.Generic;
using Nest;



[ApiController]
[Route("/api/[controller]")]
public class ElasticSearchController : ControllerBase
{
    private readonly IElasticWrapper _client;

    public ElasticSearchController(IElasticWrapper client)
    {
        _client = client;
    }

    // [HttpGet("FilterTest")]
    // public ActionResult<PayloadWrapper<LASearchResult<LASentence>>> FilterTesting()
    // {
    //     try
    //     {
    //         var result = _client.FilterTesting("la-sentence");
    //         var wrap = new PayloadWrapper<LASearchResult<LASentence>>(result);
    //         return Ok(wrap);
    //     }
    //     catch (Exception ex)
    //     {
    //         var wrap = new PayloadWrapper<LASearchResult<LASentence>>(ex.Message);
    //         return BadRequest(wrap);
    //     }
    // }


    // [HttpGet("FindCaseTest")]
    // public IActionResult FindCaseTest()
    // {
    //     try
    //     {
    //         var caseid = "1302554";
    //         var result = _client.FindCase("la-case", caseid);
    //         return Content(result.Body, "application/json"); ;
    //     }
    //     catch (Exception ex)
    //     {
    //         return BadRequest(ex);
    //     }
    // }

    // [HttpGet("GetSomeJSON")]
    // public IActionResult GetSomeJSON()
    // {
    //     try
    //     {
    //         return Content("{ \"name\":\"John\", \"age\":31, \"city\":\"New York\" }", "application/json");
    //     }
    //     catch (Exception ex)
    //     {
    //         return BadRequest(ex);
    //     }
    // }



    [HttpGet("Uri")]
    public ActionResult<PayloadWrapper<string>> DefaultUri()
    {
        try
        {
            var result = _client.ElasticSearchURL().ToString();
            var wrap = new PayloadWrapper<string>(new List<string>() { result });
            return Ok(wrap);
        }
        catch (Exception ex)
        {
            var wrap = new PayloadWrapper<string>(ex.Message);
            return BadRequest(wrap);
        }
    }

    [HttpGet("Cases")]
    public ActionResult<PayloadWrapper<LACase>> Cases()
    {
        try
        {
            var result = _client.ReadIndex<LACase>("la-case");
            var wrap = new PayloadWrapper<LACase>(result);
            return Ok(wrap);
        }
        catch (Exception ex)
        {
            var wrap = new PayloadWrapper<LACase>(ex.Message);
            return BadRequest(wrap);
        }
    }

    [HttpGet("Case/{id}")]
    public IActionResult Case(string id)
    {
        try
        {
            var result = _client.FindCase("la-case", id);
            return Content(result.Body, "application/json"); ;
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }

    [HttpGet("Paragraphs")]
    public ActionResult<PayloadWrapper<LAParagraph>> Paragraphs()
    {
        try
        {
            var result = _client.ReadIndex<LAParagraph>("la-paragraph");
            var wrap = new PayloadWrapper<LAParagraph>(result);
            return Ok(wrap);
        }
        catch (Exception ex)
        {
            var wrap = new PayloadWrapper<LAParagraph>(ex.Message);
            return BadRequest(wrap);
        }
    }

    [HttpGet("Paragraph/{id}")]
    public IActionResult Paragraph(string id)
    {
        try
        {
            var result = _client.FindParagraph("la-paragraph", id);
            return Content(result.Body, "application/json"); ;
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }

    [HttpGet("Sentences")]
    public ActionResult<PayloadWrapper<LASentence>> Sentences()
    {
        try
        {
            var result = _client.ReadIndex<LASentence>("la-sentence");
            var wrap = new PayloadWrapper<LASentence>(result);
            return Ok(wrap);
        }
        catch (Exception ex)
        {
            var wrap = new PayloadWrapper<LASentence>(ex.Message);
            return BadRequest(wrap);
        }
    }

    [HttpGet("Sentence/{id}")]
    public IActionResult Sentence(string id)
    {
        try
        {
            var result = _client.FindSentence("la-sentence", id);
            return Content(result.Body, "application/json"); ;
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }



    [HttpPost("Query")]
    public ActionResult<PayloadWrapper<LASearchResult<LASentence>>> QuerySentence(QuerySpec spec)
    {
        try
        {
            var result = _client.QueryForSentence("la-sentence", spec);
            var wrap = new PayloadWrapper<LASearchResult<LASentence>>(result);
            return Ok(wrap);
        }
        catch (Exception ex)
        {
            var wrap = new PayloadWrapper<LASearchResult<LASentence>>(ex.Message);
            return BadRequest(wrap);
        }
    }


}
