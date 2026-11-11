using System.Net;
using FluentAssertions;
using NUnit.Framework;
using spotifyFinal.Tests.Infrastructure;

namespace spotifyFinal.Tests.Tests;

/// <summary>
/// Functional tests for POST /Account/Login.
/// Each test goes through the full stack: routing -> controller -> ASP.NET Identity -> database -> Razor view.
/// </summary>
[TestFixture]
public class LoginTests
{
    private const string LoginUrl = "/Account/Login";
    private const string AuthCookie = ".AspNetCore.Identity.Application";
    private const string InvalidCredentialsMessage = "Email, UserName or Password is incorrect";

    private SpotifyAppFactory _app = null!;

    [SetUp]
    public async Task SetUp()
    {
        // Fresh app + fresh DB for every test: lockout counters from one test can't affect another.
        _app = new SpotifyAppFactory();
        await _app.SeedAsync();
    }

    [TearDown]
    public void TearDown() => _app.Dispose();

    // ---------- Positive: the form accepts "Email or username" ----------

    [TestCase("testuser", TestName = "Login succeeds with username")]
    [TestCase("testuser@example.com", TestName = "Login succeeds with email")]
    public async Task Login_WithValidCredentials_RedirectsToHomeAndSetsAuthCookie(string login)
    {
        var client = _app.CreateBrowserLikeClient();

        var response = await client.SubmitFormAsync(LoginUrl, new Dictionary<string, string>
        {
            ["UserName"] = login,
            ["Password"] = TestUsers.Confirmed.Password
        });

        // Expected: 302 to Home/Index and the Identity auth cookie is issued.
        response.StatusCode.Should().Be(HttpStatusCode.Redirect);
        response.Headers.Location!.OriginalString.Should().Be("/");
        response.HasCookie(AuthCookie).Should().BeTrue("a logged-in user must receive the auth cookie");
    }

    // ---------- Negative: one scenario, different test data ----------

    [TestCase("testuser", "WrongPass1!", TestName = "Login fails: correct username, wrong password")]
    [TestCase("nobody@example.com", "Test123!", TestName = "Login fails: email that is not registered")]
    [TestCase("newuser", "Test123!", TestName = "Login fails: user has not confirmed email")]
    public async Task Login_WithInvalidCredentials_StaysOnLoginPageWithError(string login, string password)
    {
        var client = _app.CreateBrowserLikeClient();

        var response = await client.SubmitFormAsync(LoginUrl, new Dictionary<string, string>
        {
            ["UserName"] = login,
            ["Password"] = password
        });

        // Expected: no redirect, same page re-rendered, generic error shown, no auth cookie.
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        (await response.GetValidationErrorsAsync()).Should().Contain(InvalidCredentialsMessage);
        response.HasCookie(AuthCookie).Should().BeFalse();
    }

    // ---------- Security rule: lockout after 3 failed attempts (IdentityOptions in ServiceRegistration.cs) ----------

    [Test]
    public async Task Login_AfterThreeFailedAttempts_AccountIsLockedEvenWithCorrectPassword()
    {
        var client = _app.CreateBrowserLikeClient();
        var wrong = new Dictionary<string, string> { ["UserName"] = "testuser", ["Password"] = "WrongPass1!" };

        for (var attempt = 1; attempt <= 3; attempt++)
            await client.SubmitFormAsync(LoginUrl, wrong);

        // Now the CORRECT password must still be rejected because the account is locked.
        var response = await client.SubmitFormAsync(LoginUrl, new Dictionary<string, string>
        {
            ["UserName"] = "testuser",
            ["Password"] = TestUsers.Confirmed.Password
        });

        response.StatusCode.Should().Be(HttpStatusCode.OK, "a locked user must not be redirected to Home");
        response.HasCookie(AuthCookie).Should().BeFalse();
        (await response.GetValidationErrorsAsync())
            .Should().ContainSingle(e => e.Contains("try again later"), "the lockout message should be shown");
    }
}
