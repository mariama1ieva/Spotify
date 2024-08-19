document.getElementById("searchInput").addEventListener("keyup", function (e) {
    fetch("Playlist/Search?searchText=" + this.value)
        .then(response => {
            if (!response.ok) {
                throw new Error("network respons was not ok");
            }
            return response.text();
        })
        .then(data => {
            const searchResult = document.querySelector(".search-result select");

            if (this.value === "") {
                searchResult.innerHTML = "";
            } else {
                searchResult.innerHTML = data;
            }
        })
        .catch(error => {
            console.error("Fetch error:", error);
        });
});

fetch("Playlist/GetAllPlaylistSong?playlistId=" + document.querySelector(".playlistId").value)
    .then(response => {
        if (!response.ok) {
            throw new Error("network respons was not ok");
        }
        return response.text();
    })
    .then(data => {
        document.querySelector(".liked-music").innerHTML += data;
    })
    .catch(error => {
        console.error("Fetch error:", error);
    });