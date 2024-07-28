document.querySelector(".card-number-input").oninput = () => {
  let cardNumberInput = document.querySelector(".card-number-input");
  let cardNumberBox = document.querySelector(".card-number-box");
  let cardImg = document.querySelector(".card-image");
  let visaImg = document.querySelector(".visa-image");

  let numericValue = cardNumberInput.value.replace(/\D/g, "");

  if (numericValue.startsWith("4182")) {
    document.querySelector(".front").style.background = "#099A71";
    visaImg.src = "/client/assets/images/pasha.png";
} else if (numericValue.startsWith("4169")) {
    document.querySelector(".front").style.background = "#991338";
    visaImg.src = "/client/assets/images/kapital.png";
} else {
    document.querySelector(".front").style.background = "#171616";
    visaImg.src = "/client/assets/images/";
}

cardNumberBox.innerHTML = numericValue;
};
document.querySelectorAll(".card-number-input, .amount").forEach(function(input) {
    input.addEventListener("input", function() {
        this.value = this.value.replace(/\D/g, "");
    });
});

document.querySelector(".card-holder-input").addEventListener("input", function() {
    this.value = this.value.replace(/[^a-zA-Z ]/g, "");
});
document.querySelector(".cvv-input").addEventListener("input", function() {
    this.value = this.value.replace(/\D/g, ""); 
});

document.querySelector(".amount").addEventListener("input", function() {
    this.value = this.value.replace(/\D/g, "");
});


document.querySelector(".card-holder-input").oninput = () => {
    document.querySelector(".card-holder-name").innerText = document.querySelector(".card-holder-input").value;
}

document.querySelector(".month-input").oninput = () => {
    document.querySelector(".exp-month").innerText = document.querySelector(".month-input").value;
}

document.querySelector(".year-input").oninput = () => {
    document.querySelector(".exp-year").innerText = document.querySelector(".year-input").value;
}

document.querySelector(".cvv-input").oninput = () => {
    document.querySelector(".cvv-box").innerText = document.querySelector(".cvv-input").value;
}
document.querySelector(".cvv-input").onmouseenter = () => {
    document.querySelector(".front").style.transform = "perspective(1000px) rotateY(-180deg)";
    document.querySelector(".back").style.transform = "perspective(1000px) rotateY(0deg)";
}

document.querySelector(".cvv-input").onmouseleave = () => {
    document.querySelector(".front").style.transform = "perspective(1000px) rotateY(0deg)";
    document.querySelector(".back").style.transform = "perspective(1000px) rotateY(180deg)";
}


