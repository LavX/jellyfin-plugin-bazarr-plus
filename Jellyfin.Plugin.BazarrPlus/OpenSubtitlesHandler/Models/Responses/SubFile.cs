using System.Text.Json.Serialization;

namespace Jellyfin.Plugin.BazarrPlus.OpenSubtitlesHandler.Models.Responses;

/// <summary>
/// The sub file.
/// </summary>
public class SubFile
{
    /// <summary>
    /// Gets or sets the file id.
    /// </summary>
    [JsonPropertyName("file_id")]
    public int? FileId { get; set; }

    /// <summary>
    /// Gets or sets the file name.
    /// </summary>
    [JsonPropertyName("file_name")]
    public string? FileName { get; set; }
}
