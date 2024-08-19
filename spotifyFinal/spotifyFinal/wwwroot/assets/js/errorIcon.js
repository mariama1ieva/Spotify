const error = document.querySelector(".error span");
const errorIcons = document.querySelectorAll(".error i");

errorIcons.forEach(icon => {
    if (error.innerHTML === "") {
        icon.style.opacity = "0";
    } else {
        icon.style.opacity = "1";
    }
})