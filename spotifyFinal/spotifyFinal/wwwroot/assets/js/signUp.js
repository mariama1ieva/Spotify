const form = document.getElementById("form");
const username = document.getElementById("username");
const email = document.getElementById("email");
const passwords = document.getElementById("password");
const confirmPassword = document.getElementById("password2");

form.addEventListener("submit", (e) => {
  e.preventDefault();
  validateInputs();
});

function validateInputs() {
  const usernameValue = username.value.trim();
  const emailValue = email.value.trim();
  const passwordValue = passwords.value.trim();
  const confirmPasswordValue = confirmPassword.value.trim();

  let valid = true;

  if (emailValue == "") {
    errorMessage(email, "Required field");
  } else if (!isValidEmail(emailValue)) {
    errorMessage(
      email,
      "This email is invalid. Make sure it's written like example@email.com"
    );
  } else {
    removeError(email);
  }

  if (usernameValue == "") {
    errorMessage(username, "Required field");
  } else {
    removeError(username);
  }

  if (passwordValue == "") {
    errorMessageforPassword(password, "Required field");
  } else if (!isValidPassword(passwordValue)) {
    errorMessageforPassword(
      password,
      "Please enter atleast 8 charatcer with number,symbol,small and capital letter"
    );
  } else {
    removeErrorforPassword(password);
  }

  if (confirmPasswordValue == "") {
    errorMessageforPassword(confirmPassword, "Required field");
  } else if (passwordValue !== confirmPasswordValue) {
    errorMessageforPassword(confirmPassword, "Passwords do not match");
  } else {
    removeErrorforPassword(confirmPassword);
  }

  if (valid) {
    const inputs = document.querySelectorAll('input');
    inputs.forEach(input => {
        input.value = '';
    });
}
}
function isValidEmail(email) {
  const emailRegex =
    /[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?/;
  return emailRegex.test(email);
}

function isValidPassword(password) {
  const passwordRegex =
    /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,18}$/;
  return passwordRegex.test(password);
}

function errorMessage(input, message) {
  const error = input.parentElement;
  const checkOut = error.querySelector(".error");
  checkOut.innerHTML = `<i class="fa-solid fa-circle-exclamation"></i><span>${message}</span>`;
  error.classList.add("error");
}

function removeError(input) {
  const errorMsg = input.parentElement;
  errorMsg.classList.remove("error");
  const checkOut = errorMsg.querySelector(".error");
  checkOut.innerText = "";
}

function errorMessageforPassword(input, message) {
  const container = input.closest(".input-container");
  const checkOut = container.querySelector(".error");
  checkOut.innerHTML = `<i class="fa-solid fa-circle-exclamation"></i><span>${message}</span>`;
  container.classList.add("error");
}

function removeErrorforPassword(input) {
  const container = input.closest(".input-container");
  container.classList.remove("error");
  const checkOut = container.querySelector(".error");
  checkOut.innerHTML = "";
}
