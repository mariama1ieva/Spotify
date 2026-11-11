namespace spotifyFinal.Tests.Infrastructure;

public record TestUser(string UserName, string Email, string FullName, string Password);

/// <summary>Users seeded into the in-memory database before tests run.</summary>
public static class TestUsers
{
    /// <summary>Active user with a confirmed email — should be able to log in.</summary>
    public static readonly TestUser Confirmed =
        new("testuser", "testuser@example.com", "Test User", "Test123!");

    /// <summary>Registered but never clicked the confirmation link — must NOT be able to log in.</summary>
    public static readonly TestUser Unconfirmed =
        new("newuser", "newuser@example.com", "New User", "Test123!");
}
