# Bazarr+ Subtitles (Jellyfin plugin)

A Jellyfin 10.11+ subtitle provider plugin that fetches subtitles from a self-hosted
[Bazarr+](https://github.com/LavX/bazarr) instance via the OpenSubtitles-compatible REST
endpoint.

## Requirements

- Jellyfin **10.11.0+**
- A running Bazarr+ **v2.2.0+** instance with External Integration enabled in Settings
- The Bazarr+ API Token (shown in Settings → External Integration)

## Installation

1. Build `Jellyfin.Plugin.BazarrPlus.dll` (see below), or download a release zip.
2. Drop the DLL into your Jellyfin server's `plugins/Bazarr+ Subtitles_<version>/` directory.
3. Restart Jellyfin.
4. In the Jellyfin admin: **Dashboard → Plugins → Bazarr+ Subtitles → Settings**.
5. Fill in:
   - **Bazarr+ URL**: e.g. `http://bazarr.lavx.local:6767`
   - **Bazarr+ API Token**: paste from the Bazarr+ Settings page.
6. Enable the provider in **Dashboard → Libraries → [library] → Subtitles**.

## Build from source

Requires .NET 9 SDK.

    dotnet build -c Release

The compiled DLL lands under `Jellyfin.Plugin.BazarrPlus/bin/Release/net9.0/`.

## License

GPL v2 or later. Independent reimplementation of the OpenSubtitles REST protocol
for Bazarr+ interoperability. "OpenSubtitles" is a trademark of its respective
owner; this plugin is not affiliated with or endorsed by OpenSubtitles.com.

## Relationship to upstream

This plugin is derived in spirit from
[jellyfin/jellyfin-plugin-opensubtitles](https://github.com/jellyfin/jellyfin-plugin-opensubtitles).
The namespace and branding have been changed; the HTTP protocol is compatible because
Bazarr+'s External Integration endpoint implements an OpenSubtitles-compatible shape.
