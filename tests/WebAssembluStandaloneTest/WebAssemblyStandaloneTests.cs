using BlazorStandaloneTestSite;
using JanoPL.RoutesList.Webassembly.Web.Application.Factory.DataProviders;

namespace JanoPL.RoutesList.Webassembly.Web.Application.Factory;

public class WebAssemblyStandaloneTests
{
    private readonly WebApplication<Program> _application = new();

    /**
     * Test Response for Blazor Webassembly standalone app
     */
    [Theory]
    [ClassData(typeof(UrlsData))]
    public async Task ResponseTestAsync(string url, string contentType)
    {
        using var client = _application.CreateClient();
        using var response = await client.GetAsync(url);

        response.EnsureSuccessStatusCode();

        Assert.Equal(
            contentType,
            response?.Content?.Headers?.ContentType?.ToString()
        );
    }
}