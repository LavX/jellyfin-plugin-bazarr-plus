# Bazarr+ Subtitles (Jellyfin plugin)

A Jellyfin 10.11+ subtitle provider plugin that fetches subtitles from a self-hosted
[Bazarr+](https://github.com/LavX/bazarr) instance via the OpenSubtitles-compatible REST
endpoint.

## Requirements

- Jellyfin **10.11.0+**
- A running Bazarr+ **v2.2.0+** instance with:
  - **External Integration** enabled (Settings → External Integration)
  - At least one provider enabled under **Settings → Subtitles Sources**
- The Bazarr+ API Token (shown in Settings → External Integration)

## Installation

Three ways to install, from easiest to hardest.

### Option A: Plugin repository (recommended)

1. In Jellyfin: **Dashboard → Plugins → Repositories → `+`**.
2. Add a repository:
   - **Name**: `Bazarr+ Subtitles`
   - **URL**: `https://LavX.github.io/jellyfin-plugin-bazarr-plus/manifest.json`
3. Go to the **Catalog** tab. Under `Subtitles`, find **Bazarr+ Subtitles** → **Install**.
4. Restart Jellyfin when prompted.
5. Configure (see [Configuration](#configuration) below).

> The manifest URL is live once a release has been published; if you see a 404, no
> release exists yet — use Option B or C.

### Option B: Manual install from a GitHub Release

1. Download the latest `jellyfin-plugin-bazarr-plus-<version>.zip` from the
   [Releases page](https://github.com/LavX/jellyfin-plugin-bazarr-plus/releases).
2. Extract into your Jellyfin `plugins/` directory so you end up with:

   ```
   plugins/
     Jellyfin.Plugin.BazarrPlus_<version>/
       Jellyfin.Plugin.BazarrPlus.dll
       meta.json
   ```

   Common `plugins/` locations:

   | Install type | Path |
   |---|---|
   | Docker | `/config/plugins/` |
   | Linux (package) | `/var/lib/jellyfin/plugins/` |
   | Linux (user) | `~/.local/share/jellyfin/plugins/` |
   | Windows (service) | `%ProgramData%\Jellyfin\Server\plugins\` |
   | Windows (user) | `%LocalAppData%\jellyfin\plugins\` |
   | macOS | `~/Library/Application Support/jellyfin/plugins/` |

3. Restart Jellyfin.
4. Configure (see below).

### Option C: Build from source

Requires .NET 9 SDK.

```sh
dotnet build Jellyfin.Plugin.BazarrPlus -c Release
```

The DLL lands at `Jellyfin.Plugin.BazarrPlus/bin/Release/net9.0/Jellyfin.Plugin.BazarrPlus.dll`.
Drop it into a `Jellyfin.Plugin.BazarrPlus/` subdirectory of your `plugins/` folder
(see table above) and restart Jellyfin.

## Configuration

1. **Dashboard → Plugins → My Plugins → Bazarr+ Subtitles → Settings**.
2. Fill in:
   - **Bazarr+ URL**: e.g. `https://bazarr.example.com` or `http://bazarr.local:6767`
   - **Bazarr+ API Token**: paste from Bazarr+'s Settings → External Integration.
3. **Save**.
4. Enable the provider per library at **Dashboard → Libraries → [library] → Subtitle Downloaders**.
5. Scan a library or click "Find Subtitles" on an item to verify end-to-end.

## Troubleshooting

- **No results returned** → Confirm at least one provider is enabled in Bazarr+ under
  Settings → Subtitles Sources, and that it can reach the internet.
- **401 / "Unable to login"** → API Token mismatch. Regenerate the token in Bazarr+ and
  re-paste it in the plugin settings.
- **Downloads fail with HTTP 503** → Bazarr+'s upstream provider call failed. Check
  Bazarr+ logs for the real error (rate-limit, missing provider credentials, etc.).

## License

GPLv3. See [LICENSE](./LICENSE).

## Relationship to upstream

This plugin is a fork of
[jellyfin/jellyfin-plugin-opensubtitles](https://github.com/jellyfin/jellyfin-plugin-opensubtitles),
retargeted at a self-hosted Bazarr+ instance. Namespaces, branding, and configuration
surface have been changed; the HTTP client code remains structurally similar because
Bazarr+'s External Integration endpoint implements the same REST shape. "OpenSubtitles"
is a trademark of its respective owner; this plugin is not affiliated with or endorsed
by OpenSubtitles.com.
