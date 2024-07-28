const form = document.getElementById("form");
const emailOrUsername = document.getElementById("username");
const password = document.getElementById("password");

form.addEventListener("submit", function (e) {
  e.preventDefault();
  validateInputs();
});

function validateInputs() {
  const emailOrUsernameValue = emailOrUsername.value.trim();
  const passwordValue = password.value.trim();

  if (emailOrUsernameValue === "") {
    errorMessage(emailOrUsername, "Email or username is required");
  } else {
    removeError(emailOrUsername);
  }

  if (passwordValue === "") {
    errorMessageforPassword(password, "Password is required");
  } else if (!isValidPassword(passwordValue)) {
    errorMessageforPassword(password, "Please enter a valid password");
  } else {
    removeErrorforPassword(password);
  }

  const inputs = document.querySelectorAll('input');
  inputs.forEach(input => {
      input.value = '';
  });
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
  