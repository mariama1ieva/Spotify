using System.Net;
using FluentAssertions;
using NUnit.Framework;
using spotifyFinal.Tests.Infrastructure;

namespace spotifyFinal.Tests.Tests;

/// <summary>
/// Functional tests for POST /Account/Register — negative scenarios only.
///
/// Why only negative? On success the controller sends a real email through smtp.gmail.com
/// (SmtpClient is created inside the action, it is not injected), so the happy path
/// cannot be tested in isolation without changing the app. This is noted in the bug report as a testability issue.
/// </summary>
[TestFixture]
public class RegisterValidationTests
{
    private const string RegisterUrl = "/Account/Register";
    private SpotifyAppFactory _app = null!;

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        _app = new SpotifyAppFactory();
        await _app.SeedAsync();
    }

    [OneTimeTearDown]
    public void OneTimeTearDown() => _app.Dispose();

    /// <summary>One scenario ("invalid registration is rejected"), different data per row.</summary>
    private static IEnumerable<TestCaseData> InvalidRegistrations()
    {
        // userName, email, password, confirmPassword, expected error shown to the user
        yield return new TestCaseData("user_a", "not-an-email", "Test123!", "Test123!",
                "The Email field is not a valid e-mail address.")
            .SetName("Register rejected: invalid email format");

        yield return new TestCaseData("user_b", "user_b@example.com", "Test123!", "Test999!",
                "'Confirm password' and 'Password' do not match.")
            .SetName("Register rejected: passwords do not match");

        yield return new TestCaseData("", "user_c@example.com", "Test123!", "Test123!",
                "The UserName field is required.")
            .SetName("Register rejected: empty username");

        yield return new TestCaseData("user_d", "user_d@example.com", "ab1", "ab1",
                "Passwords must be at least 6 characters.")
            .SetName("Register rejected: password shorter than 6 characters");

        yield return new TestCaseData("user_e", "user_e@example.com", "abcdefg", "abcdefg",
                "Passwords must have at least one digit ('0'-'9').")
            .SetName("Register rejected: password without a digit");

        yield return new TestCaseData("user_f", TestUsers.Confirmed.Email, "Test123!", "Test123!",
                $"Email '{TestUsers.Confirmed.Email}' is already taken.")
            .SetName("Register rejected: email already registered");
    }

    [TestCaseSource(nameof(InvalidRegistrations))]
    public async Task Register_WithInvalidData_ShowsErrorAndDoesNotCreateUser(
        string userName, string email, string password, string confirmPassword, string expectedError)
    {
        var client = _app.CreateBrowserLikeClient();

        var response = await client.SubmitFormAsync(RegisterUrl, new Dictionary<string, string>
        {
            ["UserName"] = userName,
            ["Fullname"] = "Test Person",
            ["Email"] = email,
            ["Password"] = password,
            ["ConfirmPassword"] = confirmPassword
        });

        // 1. UI result: user stays on the Register page and sees the right message.
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        (await response.GetValidationErrorsAsync()).Should().Contain(expectedError);

        // 2. Data result: nothing was saved to the database.
        if (!string.IsNullOrEmpty(userName))
            (await _app.UserExistsAsync(userName)).Should().BeFalse("an invalid registration must not create a user");
    }
}
