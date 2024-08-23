document.getElementById("ArtistIds").addEventListener("change", function () {
    var artistId = this.value;
    if (artistId) {
        fetch(`/AdminArea/Song/GetAlbums?artistId=${artistId}`)
            .then(response => {
                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
                return response.json();
            })
            .then(data => {
                var albumsList = document.getElementById("albumsList");
                if (albumsList) {
                    albumsList.innerHTML = "";
                    if (data.length > 0) {
                        data.forEach(album => {
                            var option = document.createElement("option");
                            option.value = album.id;
                            option.textContent = album.name;
                            albumsList.appendChild(option);
                        });
                    } else {
                        var option = document.createElement("option");
                        option.textContent = "Artist does not have any albums.";
                        albumsList.appendChild(option);
                    }
                }
            })
            .catch(error => {
                console.error("Error:", error);
            });

    }
});