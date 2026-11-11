# Testing

## 1. Automated tests (C# / .NET, NUnit)

Functional tests that start the **real application in memory** (`WebApplicationFactory`) and drive it over HTTP the way a browser does: open the page, read the antiforgery token, submit the form, check the response. They also check the database. SQL Server is swapped for EF Core InMemory, so you **don't need a database** to run them.

### How to run

Requirements: [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

```bash
git clone https://github.com/mariama1ieva/Spotify.git
cd Spotify/spotifyFinal
dotnet test spotifyFinal.Tests
```

Expected output: `Passed! - Failed: 0, Passed: 12`
In Visual Studio: open `spotifyFinal.sln` → **Test → Run All Tests**.

### Files to review

| File | What it shows |
|---|---|
| `spotifyFinal/spotifyFinal.Tests/Tests/LoginTests.cs` | Login: valid login by username and by email (data-driven); invalid credentials ×3 (data-driven); account lockout after 3 failed attempts |
| `spotifyFinal/spotifyFinal.Tests/Tests/RegisterValidationTests.cs` | One scenario, 6 data sets (`TestCaseSource`): each invalid registration must show the right error **and** must not create a user in the DB |
| `spotifyFinal/spotifyFinal.Tests/Infrastructure/SpotifyAppFactory.cs` | Starts the app, replaces the DB, seeds roles and users |
| `spotifyFinal/spotifyFinal.Tests/Infrastructure/HttpFormExtensions.cs` | Helpers that act like a browser: antiforgery token, form submit, reading validation messages (AngleSharp) |

### How results are verified
- **HTTP level:** status code (`302` vs `200`) and redirect target (`Location: /`)
- **Session:** the Identity auth cookie is (or is not) issued
- **UI:** the exact validation message the user sees, parsed from the returned HTML
- **Data:** the user was not saved to the database after an invalid registration

Stack: NUnit 3, FluentAssertions, Microsoft.AspNetCore.Mvc.Testing, EF Core InMemory, AngleSharp.

## 2. Manual QA

- [`docs/manual-qa/login-test-cases.md`](docs/manual-qa/login-test-cases.md): 14 test cases for Login and a Registration checklist
- [`docs/manual-qa/bug-report.md`](docs/manual-qa/bug-report.md): bugs found while testing (password policy conflict, committed secrets, lockout message, etc.)
