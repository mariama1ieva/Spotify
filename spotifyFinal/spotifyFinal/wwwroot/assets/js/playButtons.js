// other play buttons active

function setMusicNameColor() {
    const musicRows = document.querySelectorAll(".music-row");
    musicRows.forEach((row, index) => {
        const musicName = row.querySelector(".music-name p");
        const playIcon = row.querySelector("#play-icon");
        const pauseIcon = row.querySelector("#pause-icon");
        if (index === currentSong) {
            musicName.style.color = "#1ed760";
            playIcon.style.display = "none";
            pauseIcon.style.display = "inline-block";
        } else {
            musicName.style.color = "white";
            playIcon.style.display = "inline-block";
            pauseIcon.style.display = "none";
        }
    });
}

const actionBarPlayButton = document.getElementById("actionbar-play");

let pauseTine = 0;
actionBarPlayButton.addEventListener("click", async () => {
    const playIcon = actionBarPlayButton.querySelector(".fa-play");
    const pauseIcon = actionBarPlayButton.querySelector(".fa-pause");
    const playPause = document.getElementById("play-pause");

    playPause.querySelector("#play-icon").classList.toggle("fa-play", false);
    playPause.querySelector("#play-icon").classList.toggle("fa-pause", true);

    try {
        const isPlaying = !audio.paused;

        if (isPlaying) {
            audio.pause();
            playIcon.style.display = "inline-block";
            pauseIcon.style.display = "none";
            pauseTine = audio.currentTime;
        } else {
            if (audio.currentTime === 0) {
                audio.src = playlist.children[currentSong].getAttribute("src");
            }
            audio.currentTime = pauseTine;
            await audio.play();
            playlist.querySelectorAll(".music-row")[0].classList.add("active-song");
            changeMusicInfo();
            playIcon.style.display = "none";
            pauseIcon.style.display = "inline-block";
        }
    } catch (error) {
        console.error("Error playing audio", error);
    }

    setMusicNameColor();
    updateMusicRowIcons();
});

audio.addEventListener("pause", () => {
    const playIcon = actionBarPlayButton.querySelector("#play-icon");
    const pauseIcon = actionBarPlayButton.querySelector("#pause-icon");
    playIcon.style.display = "inline-block";
    pauseIcon.style.display = "none";
});

audio.addEventListener("play", () => {
    const playIcon = actionBarPlayButton.querySelector("#play-icon");
    const pauseIcon = actionBarPlayButton.querySelector("#pause-icon");
    playIcon.style.display = "none";
    pauseIcon.style.display = "inline-block";

    changeMusicInfo();
});

function updateMusicRowIcons() {
    const musicRows = document.querySelectorAll(".music-row");
    musicRows.forEach((row, index) => {
        const playIcon = row.querySelector("#play-icon");
        const pauseIcon = row.querySelector("#pause-icon");
        if (index === currentSong) {
            playIcon.style.display = audio.paused ? "inline-block" : "none";
            pauseIcon.style.display = audio.paused ? "none" : "inline-block";
        }
    });
}

function changeMusicInfo() {
    const songId = document.querySelector(
        ".music-row.active-song"
    ).getAttribute("song-id");

    const songColor = document.querySelector(
        ".music-row.active-song"
    ).getAttribute("song-color");

    const artistId = document.querySelector(
        ".music-row.active-song .artist-name p"
    ).getAttribute("artist-id");

    const imgUrl = document
        .querySelector(".music-row.active-song img")
        .getAttribute("src");

    const name = document.querySelector(
        ".music-row.active-song .music-name p"
    ).textContent;

    const artist = document.querySelector(
        ".music-row.active-song .artist-name p"
    ).textContent;

    document.querySelector("#music-player img").parentNode.setAttribute("href", `/song/detail/${songId}`);
    document.querySelector("#song-name").setAttribute("href", `/song/detail/${songId}`);
    document.querySelector(".artist-name a").setAttribute("href", `/artist/detail/${artistId}`);
    document.querySelector("#music-player img").setAttribute("src", imgUrl);
    document.querySelector("#song-name span").textContent = name;
    document.querySelector(".artist-name span").textContent = artist;
    document.querySelector(".mobile-playercontroller .content").style.backgroundColor = songColor;
    document.querySelector(".mobile-images img").setAttribute("src", imgUrl);
    document.querySelector("#mobile-song-name span").textContent = name;
    document.querySelector(".mobile-artist-name span").textContent = artist;
}