const profileAreas = document.querySelectorAll(".a");
const spanUser = document.querySelector("#spotify-header span");

const visibilitySection = new IntersectionObserver(function (entries) {
  entries.forEach(function (e) {
    if (e.isIntersecting) {
      spanUser.style.opacity = "0";
    } else {
      spanUser.style.opacity = "1";
    }
  });
});

profileAreas.forEach(function(profileArea) {
  visibilitySection.observe(profileArea);
});
