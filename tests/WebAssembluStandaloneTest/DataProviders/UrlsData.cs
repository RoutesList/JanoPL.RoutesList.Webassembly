using System.Collections;
using System.Net.Mime;

namespace JanoPL.RoutesList.Webassembly.Web.Application.Factory.DataProviders;

public class UrlsData : IEnumerable<object[]>
{
    private static IEnumerator<object[]> GetUrls()
    {
        yield return ["/", MediaTypeNames.Text.Html];
        yield return new object[] {"/routes", MediaTypeNames.Application.Json};
    }

    public IEnumerator<object[]> GetEnumerator() => GetUrls();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}