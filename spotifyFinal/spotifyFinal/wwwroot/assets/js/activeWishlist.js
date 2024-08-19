//const wishlistButtons = document.querySelectorAll("#wishlist");

//wishlistButtons.forEach(button => {
//    button.addEventListener("click", function() {
//        const songInfo = JSON.parse(localStorage.getItem("songInfo"));

//        if (songInfo.isDeleted === false) {
//            this.classList.add("active");
//        } else {
//            this.classList.remove("active");
//        }
//    });
//});


const playButtons = document.querySelectorAll(".play");


playButtons.forEach(function(playButton) {
    const playIcon = playButton.querySelector("#play-icon");
    const pauseIcon = playButton.querySelector("#pause-icon");

    playButton.addEventListener("click", function() {
        if (playIcon.style.display !== "none") {
            playIcon.style.display = "none";
            pauseIcon.style.display = "inline";
        } else {
            playIcon.style.display = "inline";
            pauseIcon.style.display = "none";
        }
    });
});

//active page
const homeLink = document.getElementById("homeicon-mobile");
const searchLink = document.getElementById("searchicon-mobile");



homeLink.addEventListener("click", function(event) {
    event.preventDefault(); 
    homeLink.classList.add("active"); 
    searchLink.classList.remove("active"); 
});

searchLink.addEventListener("click", function(event) {
    event.preventDefault(); 
    searchLink.classList.add("active"); 
    homeLink.classList.remove("active");
});


const sidebarItems = document.querySelectorAll("#menu li");
sidebarItems.forEach(item => {
    item.addEventListener("click", () => {
        sidebarItems.forEach(item => {
            item.classList.remove("active");
        });
        item.classList.add("active");
    });
});