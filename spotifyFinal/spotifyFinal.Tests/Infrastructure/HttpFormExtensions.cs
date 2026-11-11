using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;

namespace spotifyFinal.Tests.Infrastructure;

/// <summary>
/// Small helpers that do what a browser does with an MVC form:
/// load the page, read the hidden antiforgery token, post the fields back.
/// </summary>
public static class HttpFormExtensions
{
    private const string AntiforgeryField = "__RequestVerificationToken";

    public static async Task<HttpResponseMessage> SubmitFormAsync(
        this HttpClient client, string url, IDictionary<string, string> fields)
    {
        // 1. GET the page — this also sets the antiforgery cookie on the client.
        var page = await client.GetAsync(url);
        page.EnsureSuccessStatusCode();
        var html = await ParseAsync(page);

        // 2. Read the hidden token that the <form> tag helper rendered.
        var token = html.QuerySelector($"input[name='{AntiforgeryField}']")?.GetAttribute("value")
                    ?? throw new InvalidOperationException($"No antiforgery token found on {url}");

        // 3. POST the form the same way the browser would.
        var body = new Dictionary<string, string>(fields) { [AntiforgeryField] = token };
        return await client.PostAsync(url, new FormUrlEncodedContent(body));
    }

    public static async Task<IHtmlDocument> ParseAsync(HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();
        return new HtmlParser().ParseDocument(content);
    }

    /// <summary>
    /// All validation messages the page shows to the user:
    /// field errors (asp-validation-for) and the validation summary (asp-validation-summary).
    /// </summary>
    public static async Task<IReadOnlyList<string>> GetValidationErrorsAsync(this HttpResponseMessage response)
    {
        var html = await ParseAsync(response);
        return html.QuerySelectorAll(".field-validation-error, .validation-summary-errors li")
                   .Select(e => e.TextContent.Trim())
                   .Where(t => t.Length > 0)
                   .ToList();
    }

    public static bool HasCookie(this HttpResponseMessage response, string cookieName) =>
        response.Headers.TryGetValues("Set-Cookie", out var cookies) &&
        cookies.Any(c => c.StartsWith(cookieName + "=", StringComparison.Ordinal));
}
