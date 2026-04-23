using System;
using System.Text;
using Jellyfin.Plugin.BazarrPlus.OpenSubtitlesHandler.Models.Responses;
using Xunit;

namespace Jellyfin.Plugin.BazarrPlus.OpenSubtitlesHandler.Tests;

public static class LoginInfoTests
{
    private const string DummyHeader = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9";
    private const string DummySignature = "sig";

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("not-a-jwt")]
    [InlineData("only.two")]
    [InlineData("a.!@#.c")]
    [InlineData("a.eyJub3RfanNvbn0.c")]
    public static void ExpirationDate_MalformedToken_ReturnsMinValue(string? token)
    {
        var info = new LoginInfo { Token = token };

        Assert.Equal(DateTime.MinValue, info.ExpirationDate);
    }

    [Fact]
    public static void ExpirationDate_MissingExpClaim_ReturnsEpoch()
    {
        var token = BuildJwt("{\"sub\":\"bazarr\"}");

        var info = new LoginInfo { Token = token };

        Assert.Equal(DateTimeOffset.FromUnixTimeSeconds(0).UtcDateTime, info.ExpirationDate);
    }

    [Fact]
    public static void ExpirationDate_ValidExpClaim_ReturnsMatchingUtc()
    {
        const long exp = 1_800_000_000L;
        var token = BuildJwt($"{{\"exp\":{exp}}}");

        var info = new LoginInfo { Token = token };

        Assert.Equal(DateTimeOffset.FromUnixTimeSeconds(exp).UtcDateTime, info.ExpirationDate);
    }

    [Fact]
    public static void ExpirationDate_PayloadWithUrlSafeChars_DecodesCorrectly()
    {
        // Payload containing '>' and '?' forces base64url-specific chars ('_' and '-') in the encoding.
        // Standard base64 would produce '/' and '+'; JWT spec requires base64url. Verify we accept it.
        var json = "{\"exp\":1800000000,\"note\":\"???>>>\"}";
        var urlSafe = Convert.ToBase64String(Encoding.UTF8.GetBytes(json))
            .Replace('+', '-')
            .Replace('/', '_')
            .TrimEnd('=');
        var token = $"{DummyHeader}.{urlSafe}.{DummySignature}";

        var info = new LoginInfo { Token = token };

        Assert.Equal(DateTimeOffset.FromUnixTimeSeconds(1_800_000_000L).UtcDateTime, info.ExpirationDate);
    }

    [Fact]
    public static void ExpirationDate_IsCachedAfterFirstRead()
    {
        var token = BuildJwt("{\"exp\":1800000000}");
        var info = new LoginInfo { Token = token };

        var first = info.ExpirationDate;
        info.Token = null;
        var second = info.ExpirationDate;

        Assert.Equal(first, second);
    }

    private static string BuildJwt(string payloadJson)
    {
        var payload = Convert.ToBase64String(Encoding.UTF8.GetBytes(payloadJson))
            .Replace('+', '-')
            .Replace('/', '_')
            .TrimEnd('=');
        return $"{DummyHeader}.{payload}.{DummySignature}";
    }
}
