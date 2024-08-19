let songInfos = [];

if (JSON.parse(localStorage.getItem("songInfos")) !== null) {
    songInfos = JSON.parse(localStorage.getItem("songInfos"));
} else {
    localStorage.setItem("songInfos", JSON.stringify(songInfos));
}

document.querySelectorAll(".wish").forEach(wishBtn => {
    wishBtn.addEventListener("click", function (e) {
        e.stopImmediatePropagation();

        const songId = this.getAttribute("songId");

        fetch("/Song/AddWishlist?id=" + songId)
            .then(response => {
                if (!response.ok) {
                    throw new Error("network respons was not ok");
                }
                return response.json();
            })
            .then(data => {
                document.querySelector(".song_count").innerHTML = data.songCount;

                const findSong = songInfos.find(m => m.id == songId);

                if (findSong === undefined) {
                    songInfos.push(data);
                } else {
                    findSong.isDeleted = data.isDeleted;
                }

                localStorage.setItem("songInfos", JSON.stringify(songInfos));

                activeWishlist(this);
            })
            .catch(error => {
                Swal.fire({
                    title: "Log in to your account to add to favorites",
                    color: `black`,
                    showConfirmButton: true,
                    showCancelButton: false,
                    confirmButtonText: `Ok`,
                    confirmButtonColor: `#1ed760`,
                    cancelButtonColor: `white`,
                }).then((result) => {
                    if (result.isConfirmed) {
                    } else if (result.isDenied) {
                    }
                });
                console.error("Fetch error:", error);
            });
    });
});

document.querySelectorAll(".dlt-wish").forEach(dltWish => {
    dltWish.addEventListener("click", function (e) {
        e.stopImmediatePropagation();

        const songId = this.getAttribute("songId");

        fetch("/Song/AddWishlist?id=" + songId)
            .then(response => {
                if (!response.ok) {
                    throw new Error("network respons was not ok");
                }
                return response.json();
            })
            .then(data => {
                document.querySelector(".song_count").innerHTML = data.songCount;
                this.parentNode.parentNode.parentNode.remove();

                const findSong = songInfos.find(m => m.id == songId);
                findSong.isDeleted = data.isDeleted;

                localStorage.setItem("songInfos", JSON.stringify(songInfos));
            })
            .catch(error => {
                console.error("Fetch error:", error);
            });
    });
});

function activeWishlist(btn) {
    const songId = btn.getAttribute("songId");
    //const heart = document.querySelector("#wishlist");

    for (const item of songInfos) {
        if (item.id == songId) {
            if (item.isDeleted === false) {
                btn.classList.add("active");
            } else {
                btn.classList.remove("active");
            }
        }
    }
}

function totalActiveWishlist() {
    document.querySelectorAll(".wish").forEach(wishBtn => {
        for (const item of songInfos) {
            if (item.id == wishBtn.getAttribute("songId")) {
                if (item.isDeleted === false) {
                    wishBtn.classList.add("active");
                } else {
                    wishBtn.classList.remove("active");
                }
            }
        }
    });
}

totalActiveWishlist();