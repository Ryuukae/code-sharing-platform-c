using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using CodeSharingPlatform.Models;

namespace CodeSharingPlatform.Services
{
    /// <summary>
    /// Service class for managing code snippets.
    /// </summary>
    public class CodeSnippetService
    {
        // Use the directory of the executing assembly as base path for snippets directory
        private readonly string snippetsDirectory = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "snippets");

        /// <summary>
        /// Adds a new code snippet and saves it to the file system.
        /// </summary>
        /// <param name="snippet">The code snippet to add.</param>
        public void AddSnippet(CodeSnippet snippet)
        {
            snippet.ID = Guid.NewGuid().ToString();
            snippet.CreationTimestamp = DateTime.UtcNow;
            string filePath = Path.Combine(snippetsDirectory, $"{snippet.ID}.json");
            var options = new JsonSerializerOptions { WriteIndented = true };
            File.WriteAllText(filePath, JsonSerializer.Serialize(snippet, snippet.GetType(), options));
        }

        /// <summary>
        /// Retrieves a code snippet by its ID.
        /// </summary>
        /// <param name="id">The ID of the snippet to retrieve.</param>
        /// <returns>The code snippet if found and valid; otherwise, null.</returns>
        public CodeSnippet GetSnippetById(string id)
        {
            string filePath = Path.Combine(snippetsDirectory, $"{id}.json");
            if (!File.Exists(filePath))
            {
                return null;
            }

            string json = File.ReadAllText(filePath);
            var snippet = DeserializeSnippet(json);

            if (snippet is ExpiringSnippet expiring)
            {
                if (expiring.ExpirationTime < DateTime.UtcNow || expiring.ViewCounter <= 0)
                {
                    return null;
                }
                expiring.ViewCounter--;
                var options = new JsonSerializerOptions { WriteIndented = true };
                File.WriteAllText(filePath, JsonSerializer.Serialize(expiring, typeof(ExpiringSnippet), options));
            }

            return snippet;
        }

        /// <summary>
        /// Retrieves the latest 10 code snippets.
        /// </summary>
        /// <returns>An array of the latest code snippets.</returns>
        public CodeSnippet[] GetLatestSnippets()
        {
            return Directory.GetFiles(snippetsDirectory)
                .Where(file => Path.GetExtension(file) == ".json")
                .Select(file => DeserializeSnippet(File.ReadAllText(file)))
                .OrderByDescending(s => s.CreationTimestamp)
                .Take(10)
                .ToArray();
        }

        /// <summary>
        /// Deletes a code snippet by its ID.
        /// </summary>
        /// <param name="id">The ID of the snippet to delete.</param>
        public void DeleteSnippetById(string id)
        {
            string filePath = Path.Combine(snippetsDirectory, $"{id}.json");
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        /// <summary>
        /// Deserializes a JSON string into a CodeSnippet object.
        /// </summary>
        /// <param name="json">The JSON string representing the snippet.</param>
        /// <returns>The deserialized CodeSnippet object.</returns>
        private static CodeSnippet DeserializeSnippet(string json)
        {
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            bool hasExpirationTime = root.TryGetProperty("ExpirationTime", out _);
            bool hasViewCounter = root.TryGetProperty("ViewCounter", out _);

            if (hasExpirationTime || hasViewCounter)
            {
                return JsonSerializer.Deserialize<ExpiringSnippet>(json);
            }
            else
            {
                return JsonSerializer.Deserialize<BasicSnippet>(json);
            }
        }
    }
}
