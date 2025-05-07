using Microsoft.AspNetCore.Mvc;
using CodeSharingPlatform.Models;
using CodeSharingPlatform.Services;
using System;

namespace CodeSharingPlatform.Controllers
{
    [Route("api/code")]
    [ApiController]
    public class ApiCodeController : ControllerBase
    {
        private readonly CodeSnippetService _service = new CodeSnippetService();

        [HttpPost("new")]
        public IActionResult CreateSnippet([FromBody] CodeSnippet snippet)
        {
            if (snippet == null)
            {
                return BadRequest("Invalid snippet data.");
            }

            _service.AddSnippet(snippet);
            return Ok(new { Id = snippet.ID });
        }

        [HttpGet("{id}")]
        public IActionResult GetSnippet(string id)
        {
            var snippet = _service.GetSnippetById(id);
            if (snippet == null)
            {
                return NotFound();
            }
            return Ok(snippet);
        }

        [HttpGet("latest")]
        public IActionResult GetLatestSnippets()
        {
            var snippets = _service.GetLatestSnippets();
            return Ok(snippets);
        }
    }
}
