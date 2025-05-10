using System;
using System.Text.Json.Serialization;

namespace CodeSharingPlatform.Models
{
    /// <summary>
    /// Abstract base class representing a code snippet.
    /// </summary>
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "Type")]
    [JsonDerivedType(typeof(ExpiringSnippet), "Expiring")]
    [JsonDerivedType(typeof(BasicSnippet), "Basic")]
    public abstract class CodeSnippet
    {
        /// <summary>
        /// Gets or sets the unique identifier of the snippet.
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// Gets or sets the content of the snippet.
        /// </summary>
        public string Content { get; set; } = "N/A";

        /// <summary>
        /// Gets or sets the name of the snippet.
        /// </summary>
        public string Name { get; set; } = "N/A";

        /// <summary>
        /// Gets or sets the creation timestamp of the snippet.
        /// </summary>
        public DateTime CreationTimestamp { get; set; }

        /// <summary>
        /// Gets or sets the type discriminator of the snippet.
        /// </summary>
        public string Type { get; set; }
    }

    /// <summary>
    /// Represents a code snippet with expiration and view limits.
    /// </summary>
    public class ExpiringSnippet : CodeSnippet
    {
        /// <summary>
        /// Gets or sets the expiration time of the snippet.
        /// </summary>
        public DateTime ExpirationTime { get; set; }

        /// <summary>
        /// Gets or sets the remaining view count for the snippet.
        /// </summary>
        public int ViewCounter { get; set; }
    }

    /// <summary>
    /// Represents a basic code snippet without expiration.
    /// </summary>
    public class BasicSnippet : CodeSnippet
    {
        // No additional properties
    }
}
