# Manual test cases — Login (`/Account/Login`)

**Feature:** a user logs in with *email or username* + password.
**Rules taken from the code:** email must be confirmed; account locks for 3 minutes after 3 failed attempts; username field is required, 3–30 characters.

**Test data (preconditions)**

| User | Username | Email | Password | State |
|---|---|---|---|---|
| U1 | `testuser` | `testuser@example.com` | `Test123!` | Email confirmed |
| U2 | `newuser` | `newuser@example.com` | `Test123!` | Email **not** confirmed |

---

| ID | Title | Steps | Test data | Expected result | Priority | Auto |
|---|---|---|---|---|---|---|
| TC-L01 | Login with username | 1. Open `/Account/Login` 2. Enter data 3. Click **Log In** | `testuser` / `Test123!` | Redirect to Home page, user is logged in (profile shown in header) | High | ✅ |
| TC-L02 | Login with email | Same as TC-L01 | `testuser@example.com` / `Test123!` | Same as TC-L01 | High | ✅ |
| TC-L03 | Wrong password | Same as TC-L01 | `testuser` / `WrongPass1!` | Stays on Login page. Message: *"Email, UserName or Password is incorrect"*. Not logged in | High | ✅ |
| TC-L04 | Unregistered user | Same as TC-L01 | `nobody@example.com` / `Test123!` | Same message as TC-L03. The message must **not** reveal that the user doesn't exist | High | ✅ |
| TC-L05 | Email not confirmed | Same as TC-L01 | `newuser` / `Test123!` | Login refused, user not logged in | High | ✅ |
| TC-L06 | Lockout after 3 failed attempts | 1. Enter a wrong password 3 times 2. Enter the correct password | `testuser` | 4th attempt is refused even with the correct password, and a lockout message is shown | High | ✅ |
| TC-L07 | Lockout expires | 1. Do TC-L06 2. Wait 3 minutes 3. Log in with the correct password | `testuser` / `Test123!` | Login succeeds | Medium | — |
| TC-L08 | Empty fields | Click **Log In** with both fields empty | — | Required messages under both fields. Form is not submitted to the server (client-side check) | Medium | — |
| TC-L09 | Username too short | Same as TC-L01 | `ab` / `Test123!` | Validation message: length must be 3–30 | Low | — |
| TC-L10 | Password is masked and can be shown | Type a password, click the eye icon, click again | any | Shown as dots by default; eye icon toggles plain text and back | Low | — |
| TC-L11 | Case sensitivity | Log in with `TESTUSER@EXAMPLE.COM` / `Test123!`, then `testuser` / `test123!` | — | Email/username: case-insensitive (login works). Password: case-sensitive (login fails) | Medium | — |
| TC-L12 | Leading/trailing spaces | Log in with `␣testuser␣` / `Test123!` | — | Spaces should be trimmed and login should work. If not, report as a usability bug | Low | — |
| TC-L13 | Logged-in session survives a page refresh | Do TC-L01, press F5, open another page | — | User stays logged in | Medium | — |
| TC-L14 | Browser back button after logout | Log in → log out → press Back | — | Protected pages must not be shown as logged in | Medium | — |

✅ = covered by the automated tests in `spotifyFinal/spotifyFinal.Tests/Tests/LoginTests.cs`

---

## Quick checklist — Registration (`/Account/Register`)

- [ ] Valid data → "check your email" page, confirmation email arrives, the link confirms the account
- [ ] Unconfirmed user can't log in until the link is clicked
- [ ] Invalid email format is rejected *(automated)*
- [ ] Password ≠ Confirm password is rejected *(automated)*
- [ ] Empty username is rejected *(automated)*
- [ ] Password < 6 characters / no digit is rejected *(automated)*
- [ ] Password without uppercase / special character → **accepted, but should be rejected** (see BUG-001)
- [ ] Duplicate email is rejected *(automated)*
- [ ] Duplicate username is rejected
- [ ] Username with characters outside `a-z A-Z 0-9 . _ @ +` (e.g. `ali ahmad`, `əli`) is rejected with a clear message
- [ ] After a validation error, the text fields keep what was typed (except the password fields)
- [ ] Confirmation link used twice / with a changed token → handled gracefully, no error page
