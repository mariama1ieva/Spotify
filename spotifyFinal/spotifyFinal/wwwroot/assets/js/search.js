document.getElementById("searchInput").addEventListener("keyup", function () {
    if (this.value !== "") {
        document.querySelector(".search-filter").style.display = "block";
    } else {
        document.querySelector(".search-filter").style.display = "none";
    }

    fetch("Search/Search?searchText=" + this.value)
        .then(response => {
            if (!response.ok) {
                throw new Error("network respons was not ok");
            }
            return response.text();
        })
        .then(data => {
            document.getElementById("cards-area").innerHTML = data;
        })
        .catch(error => {
            console.error("Fetch error:", error);
        });
});

document.querySelectorAll(".search-filter button").forEach(btn => {
    btn.addEventListener("click", function (e) {        
        e.preventDefault();
        fetch("Search/Filter?id=" + this.getAttribute("filter-id") + "&searchText=" + this.parentNode.nextElementSibling.firstElementChild.firstElementChild.nextElementSibling.value)
            .then(response => {
                if (!response.ok) {
                    throw new Error("network respons was not ok");
                }
                return response.text();
            })
            .then(data => {
                document.getElementById("cards-area").innerHTML = data;
            })
            .catch(error => {
                console.error("Fetch error:", error);
            });
    });
});