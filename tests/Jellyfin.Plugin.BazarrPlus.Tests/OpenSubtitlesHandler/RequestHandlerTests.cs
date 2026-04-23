using System.Collections.Generic;
using Xunit;

namespace Jellyfin.Plugin.BazarrPlus.OpenSubtitlesHandler.Tests;

public static class RequestHandlerTests
{
    public static TheoryData<string, Dictionary<string, string>, string> ComputeHash_Success_TestData()
        => new TheoryData<string, Dictionary<string, string>, string>
        {
            {
                "/subtitles",
                new Dictionary<string, string>()
                {
                    { "b", "c and d" },
                    { "a", "1" }
                },
                "/subtitles?a=1&b=c+and+d"
            },
            {
                // Keys lowercased, values preserve case (language tags, filenames, release groups).
                "/subtitles",
                new Dictionary<string, string>()
                {
                    { "LANGUAGES", "zh-CN" },
                    { "query", "For.All.Mankind.S01E01.mkv" }
                },
                "/subtitles?languages=zh-CN&query=For.All.Mankind.S01E01.mkv"
            },
            {
                // Moviehash is hex — purely cosmetic here but documents case pass-through.
                "/subtitles",
                new Dictionary<string, string>()
                {
                    { "moviehash", "8F2E11DA1F87EC5C" }
                },
                "/subtitles?moviehash=8F2E11DA1F87EC5C"
            }
        };

    [Theory]
    [MemberData(nameof(ComputeHash_Success_TestData))]
    public static void ComputeHash_Success(string path, Dictionary<string, string> param, string expected)
        => Assert.Equal(expected, RequestHandler.AddQueryString(path, param));
}
