using System;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Jellyfin.Plugin.BazarrPlus.OpenSubtitlesHandler.Models.Responses;

/// <summary>
/// The login info.
/// </summary>
public class LoginInfo
{
    private DateTime? _expirationDate;

    /// <summary>
    /// Gets or sets the user info.
    /// </summary>
    [JsonPropertyName("user")]
    public UserInfo? User { get; set; }

    /// <summary>
    /// Gets or sets the token.
    /// </summary>
    [JsonPropertyName("token")]
    public string? Token { get; set; }

    /// <summary>
    /// Gets the expiration date.
    /// </summary>
    /// <remarks>
    /// A malformed or otherwise unparseable token resolves to <see cref="DateTime.MinValue"/>,
    /// which callers interpret as "already expired" and triggers a fresh login.
    /// </remarks>
    [JsonIgnore]
    public DateTime ExpirationDate
    {
        get
        {
            if (_expirationDate.HasValue)
            {
                return _expirationDate.Value;
            }

            _expirationDate = ParseExpiration(Token);
            return _expirationDate.Value;
        }
    }

    private static DateTime ParseExpiration(string? token)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            return DateTime.MinValue;
        }

        var segments = token.Split('.');
        if (segments.Length < 3)
        {
            return DateTime.MinValue;
        }

        try
        {
            var payload = Encoding.UTF8.GetString(Base64UrlDecode(segments[1]));
            var sec = JsonSerializer.Deserialize<JWTPayload>(payload)?.Exp ?? 0;
            return DateTimeOffset.FromUnixTimeSeconds(sec).UtcDateTime;
        }
        catch (FormatException)
        {
            return DateTime.MinValue;
        }
        catch (JsonException)
        {
            return DateTime.MinValue;
        }
        catch (ArgumentOutOfRangeException)
        {
            return DateTime.MinValue;
        }
    }

    private static byte[] Base64UrlDecode(string input)
    {
        var s = input.Replace('-', '+').Replace('_', '/');
        s = s.PadRight(s.Length + ((4 - (s.Length % 4)) % 4), '=');
        return Convert.FromBase64String(s);
    }
}
