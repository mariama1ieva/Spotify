const passwordInputs = document.querySelectorAll('.eye-funck');


passwordInputs.forEach(input => {
  const eyeicon = input.parentElement.querySelector('.fa-eye-slash');
  const eyeiconOpen = input.parentElement.querySelector('.fa-eye');

  eyeicon.onclick = function () {
      input.type = "text";
      eyeicon.style.display = "none";
      eyeiconOpen.style.display = "inline";
  };

  eyeiconOpen.onclick = function () {
      input.type = "password";
      eyeicon.style.display = "inline";
      eyeiconOpen.style.display = "none";
  };
});


