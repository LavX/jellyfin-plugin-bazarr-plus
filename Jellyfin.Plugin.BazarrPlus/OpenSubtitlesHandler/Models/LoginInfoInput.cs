using System.ComponentModel.DataAnnotations;

namespace Jellyfin.Plugin.BazarrPlus.OpenSubtitlesHandler.Models;

/// <summary>
/// The login model.
/// </summary>
public class LoginInfoInput
{
    /// <summary>
    /// Gets or sets the username.
    /// </summary>
    [Required]
    public string Username { get; set; } = null!;

    /// <summary>
    /// Gets or sets the password.
    /// </summary>
    [Required]
    public string Password { get; set; } = null!;
}
