using Microsoft.AspNetCore.Mvc;
using CodeSharingPlatform.Models;
using CodeSharingPlatform.Services;
using System;  // Added necessary using directive for DateTime

namespace CodeSharingPlatform.Controllers
{
    /// <summary>
    /// Controller for handling code snippet web requests.
    /// </summary>
    [Route("code")]
    public class WebCodeController : Controller
    {
        private readonly CodeSnippetService _service = new CodeSnippetService();

        /// <summary>
        /// Displays the form for creating a new code snippet.
        /// </summary>
        /// <returns>The new snippet form view.</returns>
        [HttpGet("new")]
        public IActionResult New()
        {
            return View("~/Views/NewSnippetForm.cshtml");
        }

        /// <summary>
        /// Creates a new code snippet with optional expiration and view limits.
        /// </summary>
        /// <param name="content">The content of the snippet.</param>
        /// <param name="name">The name of the snippet.</param>
        /// <param name="viewLimit">Optional view limit for the snippet.</param>
        /// <param name="expireMinutes">Optional expiration time in minutes.</param>
        /// <returns>Redirects to the view page of the created snippet.</returns>
        [HttpPost("new")]
        public IActionResult CreateSnippet([FromForm] string content, [FromForm] string name, [FromForm] int? viewLimit, [FromForm] int? expireMinutes)
        {
            CodeSnippet snippet;
            bool isExpiring = (viewLimit.HasValue && viewLimit.Value > 0) || (expireMinutes.HasValue && expireMinutes.Value > 0);
            if (isExpiring)
            {
                snippet = new ExpiringSnippet
                {
                    Content = content,
                    Name = name,
                    ExpirationTime = DateTime.UtcNow.AddMinutes(expireMinutes ?? 60),
                    ViewCounter = viewLimit ?? 10,
                    Type = "Expiring"
                };
            }
            else
            {
                snippet = new BasicSnippet
                {
                    Content = content,
                    Name = name,
                    Type = "Basic"
                };
            }

            _service.AddSnippet(snippet);
            return RedirectToAction("ViewSnippet", new { id = snippet.ID });
        }

        /// <summary>
        /// Displays a code snippet by its ID.
        /// </summary>
        /// <param name="id">The ID of the snippet to view.</param>
        /// <returns>The view snippet page with the snippet data.</returns>
        [HttpGet("view/{id}")]
        public IActionResult ViewSnippet(string id)
        {
            var snippet = _service.GetSnippetById(id);
            if (snippet == null)
            {
                snippet = new CodeSharingPlatform.Models.BasicSnippet
                {
                    Content = "Snippet not found.",
                    CreationTimestamp = DateTime.UtcNow,
                    ID = id
                };
            }
            return View("~/Views/ViewSnippet.cshtml", snippet);
        }

        /// <summary>
        /// Displays the latest code snippets.
        /// </summary>
        /// <returns>The latest snippets view page.</returns>
        [HttpGet("latest")]
        [HttpGet("/latestsnippets.htmlcs")]
        [HttpGet("/latestsnippets")]
        public IActionResult Latest()
        {
            var snippets = _service.GetLatestSnippets();
            return View("~/Views/LatestSnippets.cshtml", snippets);
        }

        /// <summary>
        /// Deletes a code snippet by its ID.
        /// </summary>
        /// <param name="id">The ID of the snippet to delete.</param>
        /// <returns>Redirects to the latest snippets page.</returns>
        [HttpPost("delete/{id}")]
        public IActionResult DeleteSnippet(string id)
        {
            _service.DeleteSnippetById(id);
            return RedirectToAction("Latest");
        }
    }
}
