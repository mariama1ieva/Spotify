document.getElementById("loadMore").addEventListener("click", function (e) {
    e.stopImmediatePropagation();
    let songsList = document.getElementById("songs");
    let skipCount = document.querySelectorAll(".music-row").length;
    let albumId = document.getElementById("song_count").getAttribute("albumId");
    let songCount = document.getElementById("song_count").value;

    fetch(`/Album/LoadMore?albumId=${albumId}&skip=${skipCount}`)
        .then(response => response.text())
        .then(data => {
            if (data.trim().length > 0) {
                songsList.innerHTML += data;
                skipCount = document.querySelectorAll(".music-row").length;
                if (skipCount >= songCount) {
                    document.getElementById("loadMore").style.display = "none";
                }

                let songs = document.querySelectorAll(".music-row");
                let count = 0;

                songs.forEach(song => {
                    song.querySelector(".song_number").innerHTML = ++count;
                });
            } else {
                document.getElementById("loadMore").style.display = "none";
            }
        })
        .catch(error => {
            console.error("loadMore error:", error);
        });
});

let songs = document.querySelectorAll(".music-row");
let count = 0;

songs.forEach(song => {
    song.querySelector(".song_number").innerHTML = ++count;
});
