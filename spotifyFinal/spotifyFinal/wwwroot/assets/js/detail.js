const detailArea = document.getElementById("detail-area");
const musicName = document.querySelector("#spotify-header span");

const visibilityDetail = new IntersectionObserver(function (entries) {
  entries.forEach(function (e) {
    if (e.isIntersecting) {
      musicName.style.opacity = "0";
    } else {
      musicName.style.opacity = "1";
    }
  });
});

visibilityDetail.observe(detailArea);