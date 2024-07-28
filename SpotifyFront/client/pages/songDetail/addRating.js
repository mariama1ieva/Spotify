function fillStars(rating) {
    const stars = document.querySelectorAll('.rate-it');
    stars.forEach((star, i) => {
        star.addEventListener("click", function () {
            star.firstElementChild.click();
            let currentStar = i + 1;
            stars.forEach((star, j) => {
                if (currentStar >= j + 1) {
                    star.classList.add("active");
                    star.style.color = "#FEC006";
                } else {
                    star.classList.remove("active");
                    star.style.color = "lightgray";
                }
            });
        })
    });
}

let starCount = document.querySelector(".add-rate");
console.log('fff')