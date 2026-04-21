using System;
using System.Collections.Generic;
using Jellyfin.Plugin.BazarrPlus.Configuration;
using MediaBrowser.Common.Configuration;
using MediaBrowser.Common.Plugins;
using MediaBrowser.Model.Plugins;
using MediaBrowser.Model.Serialization;

namespace Jellyfin.Plugin.BazarrPlus;

/// <summary>
/// The Bazarr+ subtitles plugin.
/// </summary>
public class BazarrPlusPlugin : BasePlugin<PluginConfiguration>, IHasWebPages
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BazarrPlusPlugin"/> class.
    /// </summary>
    /// <param name="applicationPaths">Instance of the <see cref="IApplicationPaths"/> interface.</param>
    /// <param name="xmlSerializer">Instance of the <see cref="IXmlSerializer"/> interface.</param>
    public BazarrPlusPlugin(IApplicationPaths applicationPaths, IXmlSerializer xmlSerializer)
        : base(applicationPaths, xmlSerializer)
    {
        Instance = this;

        ConfigurationChanged += (_, _) =>
        {
            BazarrPlusDownloader.Instance?.ConfigurationChanged(Configuration);
        };

        BazarrPlusDownloader.Instance?.ConfigurationChanged(Configuration);
    }

    /// <inheritdoc />
    public override string Name
        => "Bazarr+ Subtitles";

    /// <inheritdoc />
    public override string Description
        => "Fetch subtitles from a Bazarr+ instance via the OpenSubtitles-compatible REST endpoint.";

    /// <inheritdoc />
    public override Guid Id
        => Guid.Parse("3e94f7ec-acca-42b4-ac3b-15a8cf446ba9");

    /// <summary>
    /// Gets the plugin instance.
    /// </summary>
    public static BazarrPlusPlugin? Instance { get; private set; }

    /// <inheritdoc />
    public IEnumerable<PluginPageInfo> GetPages()
    {
        return new[]
        {
            new PluginPageInfo
            {
                Name = "bazarrplus",
                EmbeddedResourcePath = GetType().Namespace + ".Web.bazarrplus.html",
            },
            new PluginPageInfo
            {
                Name = "bazarrplusjs",
                EmbeddedResourcePath = GetType().Namespace + ".Web.bazarrplus.js"
            }
        };
    }
}
