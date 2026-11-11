# Bug reports — Spotify clone (Account module)

Found while preparing the automated tests for Login/Register.
Environment for all reports: `master` branch, local run (`dotnet run`, Development), SQL Server Express, browser: _(your browser + version)_, OS: _(your OS)_.

---

## BUG-001 — Password policy is configured twice; the stricter rules are silently ignored

| Field | Value |
|---|---|
| **Severity** | Major |
| **Priority** | High |
| **Component** | Account / Registration, `IdentityOptions` |
| **Type** | Functional / Security |

**Summary**
The app sets the password rules in two places with different values. Because `SpotifyServiceRegistration()` runs after `AddServiceLayer()`, it overwrites the rules, so passwords with **no uppercase letter and no special character** are accepted.

**Where it comes from**
- `Service/DependencyInjection.cs` lines 44–45: `RequireUppercase = true`, `RequireNonAlphanumeric = true`
- `spotifyFinal/ServiceRegistration.cs` lines 16–17: `RequireNonAlphanumeric = false`, `RequireUppercase = false`
- `Program.cs` lines 15–16: `AddServiceLayer()` is called first, then `SpotifyServiceRegistration()`, so the second one wins.

**Steps to reproduce**
1. Open `/Account/Register`.
2. Fill in: Email `qa.test1@example.com`, Username `qatest1`, Full name `QA Test`.
3. Password and Confirm password: `test123` (lowercase + digits only, no uppercase, no symbol).
4. Click **Sign up**.

**Expected result**
Registration is rejected with *"Passwords must have at least one uppercase ('A'-'Z')"* and *"Passwords must have at least one non alphanumeric character"*, as configured in the Service layer.

**Actual result**
No password error. The user is created and the confirmation email is sent.
The effective settings at runtime are: `RequireUppercase = false`, `RequireNonAlphanumeric = false`, `RequiredLength = 6`. (Checked by reading `IOptions<IdentityOptions>` in a test.)

**Suggested fix**
Keep one `Configure<IdentityOptions>` block. Agree with the product owner which rules are correct.

---

## BUG-002 — Email password and Stripe keys are committed to the public repository

| Field | Value |
|---|---|
| **Severity** | Critical |
| **Priority** | Highest |
| **Component** | Configuration / Security |

**Details**
- `AccountController.cs` lines 152 and 323: the Gmail SMTP login and app password are hard-coded in `smtp.Authenticate(...)`.
- `appsettings.json` lines 13–14: Stripe test keys are committed. The values are also **swapped**: `SecretKey` holds the `pk_test_…` key and `PublicKey` holds the `sk_test_…` key.

**Expected:** No secrets in source code. Read them from configuration (User Secrets or environment variables) and keep the real values out of git.
**Actual:** Anyone who can see the repo can send email from this account and call Stripe with the secret key.
**Action:** Revoke the Gmail app password and roll the Stripe keys first. Then move them to configuration and clean the git history.

---

## BUG-003 — Wrong and misspelled message when the account is locked

| Field | Value |
|---|---|
| **Severity** | Minor |
| **Priority** | Medium |
| **Component** | Account / Login |

**Steps:** Enter a wrong password 3 times for an existing user, then try a 4th time.
**Expected:** A clear message, e.g. *"Your account is temporarily locked after 3 failed attempts. Try again in 3 minutes."*
**Actual:** *"Server is eneabled at the moment,please try again later"*. It blames the server instead of the account, has a typo ("eneabled") and is missing a space after the comma. (`AccountController.cs` line 385)

---

## Other findings (short)

| ID | Severity | Finding |
|---|---|---|
| BUG-004 | Minor | `POST /Account/Login` has no `[ValidateAntiForgeryToken]`, unlike Register/ForgetPassword/ResetPassword (login CSRF). `AccountController.cs` line 367. |
| BUG-005 | Trivial | The Login page's browser tab title is **"Register"** (`Views/Account/Login.cshtml` line 17). `VerifyEmail` shows **"Document"**. |
| NOTE | Testability | `Register` and `ForgetPassword` create `SmtpClient` inside the action, so the success path can't be tested without sending a real email. An injected `IEmailSender` would let tests replace it with a fake. |
