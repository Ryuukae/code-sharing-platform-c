using Microsoft.AspNetCore.Mvc;
using CodeSharingPlatform.Models;
using CodeSharingPlatform.Services;
using System;

namespace CodeSharingPlatform.Controllers
{
    /// <summary>
    /// API controller for managing code snippets.
    /// Provides endpoints to create, retrieve by ID, and get the latest code snippets.
    /// </summary>
    [Route("api/code")]
    [ApiController]
    public class ApiCodeController : ControllerBase
    {
        /// <summary>
        /// Service for managing code snippets.
        /// </summary>
        private readonly CodeSnippetService _service = new CodeSnippetService();

        /// <summary>
        /// Creates a new code snippet.
        /// </summary>
        /// <param name="snippet">The code snippet to create.</param>
        /// <returns>Returns the ID of the created snippet if successful; otherwise, returns a bad request response.</returns>
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

        /// <summary>
        /// Retrieves a code snippet by its ID.
        /// </summary>
        /// <param name="id">The ID of the code snippet to retrieve.</param>
        /// <returns>Returns the code snippet if found; otherwise, returns a not found response.</returns>
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

        /// <summary>
        /// Retrieves the latest code snippets.
        /// </summary>
        /// <returns>Returns a list of the latest code snippets.</returns>
        [HttpGet("latest")]
        public IActionResult GetLatestSnippets()
        {
            var snippets = _service.GetLatestSnippets();
            return Ok(snippets);
        }
    }
}
