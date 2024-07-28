document.addEventListener("DOMContentLoaded", function() {
    const links = document.querySelectorAll(".search-filter button");
  
    links.forEach(link => {
      link.addEventListener("click", function() {
        links.forEach(otherLink => {
          otherLink.classList.remove("active");
        });
        this.classList.add("active");
      });
    });
  });
  