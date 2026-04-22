using MediaBrowser.Model.Plugins;

namespace Jellyfin.Plugin.BazarrPlus.Configuration;

/// <summary>
/// Plugin configuration.
/// </summary>
public class PluginConfiguration : BasePluginConfiguration
{
    /// <summary>
    /// Gets or sets the Bazarr+ base URL (e.g. http://bazarr.local:6767).
    /// No trailing slash. The plugin appends "/api/v1/..." itself.
    /// </summary>
    public string BazarrUrl { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the Bazarr+ compat API token (shown in Settings → External Integration).
    /// </summary>
    public string BazarrToken { get; set; } = string.Empty;
}
