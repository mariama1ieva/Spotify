const player = document.querySelector(".player-control");
const audio = document.getElementById("audio");
const playPause = document.getElementById("play-pause");
const prevBtn = document.getElementById("backward-step");
const nextBtn = document.getElementById("forward-step");
const playlist = document.getElementById("songs");
const progressBar = document.getElementById("playback-progressbar");
const progress = document.querySelector(".progress");
const volume = document.getElementById("volume");
const currentTime = document.querySelector("#current-time");
const duration = document.querySelector("#duration");
const randomSong = document.getElementById("random");
const maxValue = playlist.children.length;
const playPauseMobile = document.getElementById("play-pause-responsive");

let currentSong = 0;

playlist.addEventListener("click", async function (e) {
  if (e.target.classList.contains("fa-play")) {
    const row = e.target.closest(".music-row");
    currentSong = Array.from(playlist.children).indexOf(row);
    if (!audio.paused) {
      audio.pause();
    }
    audio.src = row.getAttribute("src");

    playPause.querySelector("#play-icon").classList.toggle("fa-play", false);
    playPause.querySelector("#play-icon").classList.toggle("fa-pause", true);
    playPauseMobile
      .querySelector("#play-icon")
      .classList.toggle("fa-play", false);
    playPauseMobile
      .querySelector("#play-icon")
      .classList.toggle("fa-pause", true);
    try {
      await audio.play();
      setActiveSong(currentSong);
      changeMusicInfo(this);
      setMusicNameColor();
    } catch (error) {
      console.error("audio play failed", error);
    }
  } else if (e.target.classList.contains("fa-pause")) {
    audio.pause();
    playPause.querySelector("#play-icon").classList.toggle("fa-play", true);
    playPause.querySelector("#play-icon").classList.toggle("fa-pause", false);
    playPauseMobile
      .querySelector("#play-icon")
      .classList.toggle("fa-play", true);
    playPauseMobile
      .querySelector("#play-icon")
      .classList.toggle("fa-pause", false);
  }
});
function changeMusicInfo(music){
const imgUrl = music.querySelector(".active-song img").getAttribute("src");
  document.querySelector("#music-player img").setAttribute("src", imgUrl);

  const musicName = music.querySelector(
    ".active-song .music-name p"
  ).textContent;
  document.querySelector("#song-name span").textContent = musicName;

  const artistName = music.querySelector(
    ".active-song .artist-name p"
  ).textContent;
  document.querySelector(".artist-name span").textContent = artistName;

  const imgUrlMobile = music
    .querySelector(".active-song img")
    .getAttribute("src");
  document
    .querySelector(".mobile-images img")
    .setAttribute("src", imgUrlMobile);

  const musicNameMobile = music.querySelector(
    ".active-song .music-name p"
  ).textContent;
  document.querySelector("#mobile-song-name span").textContent =
    musicNameMobile;

  const artistNameMobile = music.querySelector(
    ".active-song .artist-name p"
  ).textContent;
  document.querySelector(".mobile-artist-name span").textContent =
    artistNameMobile;
}

const setActiveSong = (index) => {
  const activeSong = playlist.querySelector(".active-song");
  if (activeSong) {
    activeSong.classList.remove("active-song");
    const activeMusicName = activeSong.querySelector(".music-name p");
    activeMusicName.style.color = "white";
  }

  const newActiveSong = playlist.querySelectorAll(".music-row")[index];
  if (newActiveSong) {
    newActiveSong.classList.add("active-song");
    const newActiveMusicName = newActiveSong.querySelector(".music-name p");
    newActiveMusicName.style.color = "#1ed760";
  }
};

playPause.addEventListener("click", () => {
  if (audio.paused) {
    audio.play();
    playPause.innerHTML = '<i class="fa-solid fa-pause"></i>';
  } else {
    audio.pause();
    playPause.innerHTML = '<i class="fa-solid fa-play"></i>';
  }
});
playPauseMobile.addEventListener("click", () => {
  if (audio.paused) {
    audio.play();
    playPauseMobile.innerHTML = '<i class="fa-solid fa-pause"></i>';
  } else {
    audio.pause();
    playPauseMobile.innerHTML = '<i class="fa-solid fa-play"></i>';
  }
});
prevBtn.addEventListener("click", () => {
  if (currentSong === 0) {
    audio.currentTime = 0;
    audio.play();
  } else {
    currentSong--;
    if (currentSong < 0) {
      currentSong = playlist.children.length - 1;
    }
    audio.src = playlist.children[currentSong].getAttribute("src");
    audio.play();
  }
  setActiveSong(currentSong);
  setMusicNameColor();

  const imgUrl = document
    .querySelector(".music-row.active-song img")
    .getAttribute("src");
  document.querySelector("#music-player img").setAttribute("src", imgUrl);

  const musicName = document.querySelector(
    ".music-row.active-song .music-name p"
  ).textContent;
  document.querySelector("#song-name span").textContent = musicName;

  const artistName = document.querySelector(
    ".active-song .artist-name p"
  ).textContent;
  document.querySelector(".artist-name span").textContent = artistName;

  const imgUrlMobile = document
    .querySelector(".active-song img")
    .getAttribute("src");
  document
    .querySelector(".mobile-images img")
    .setAttribute("src", imgUrlMobile);

  const musicNameMobile = document.querySelector(
    ".active-song .music-name p"
  ).textContent;
  document.querySelector("#mobile-song-name span").textContent =
    musicNameMobile;

  const artistNameMobile = document.querySelector(
    ".active-song .artist-name p"
  ).textContent;
  document.querySelector(".mobile-artist-name span").textContent =
    artistNameMobile;
});

nextBtn.addEventListener("click", () => {
  if (isRepeatActive) {
    audio.currentTime = 0;
    audio.play();
    return;
  }
  if (isRandomClicked) {
    let randValue = Math.floor(Math.random() * maxValue);
    audio.src = playlist.children[randValue].getAttribute("src");
    audio.play();
    setActiveSong(randValue);
    return;
  }

  currentSong++;
  if (currentSong === playlist.children.length) {
    currentSong = 0;
  }
  audio.src = playlist.children[currentSong].getAttribute("src");
  audio.play();
  setActiveSong(currentSong);
  setMusicNameColor();

  const imgUrl = document
    .querySelector(".music-row.active-song img")
    .getAttribute("src");
  document.querySelector("#music-player img").setAttribute("src", imgUrl);

  const musicName = document.querySelector(
    ".music-row.active-song .music-name p"
  ).textContent;
  document.querySelector("#song-name span").textContent = musicName;

  const artistName = document.querySelector(
    ".active-song .artist-name p"
  ).textContent;
  document.querySelector(".artist-name span").textContent = artistName;

  const imgUrlMobile = document
    .querySelector(".active-song img")
    .getAttribute("src");
  document
    .querySelector(".mobile-images img")
    .setAttribute("src", imgUrlMobile);

  const musicNameMobile = document.querySelector(
    ".active-song .music-name p"
  ).textContent;
  document.querySelector("#mobile-song-name span").textContent =
    musicNameMobile;

  const artistNameMobile = document.querySelector(
    ".active-song .artist-name p"
  ).textContent;
  document.querySelector(".mobile-artist-name span").textContent =
    artistNameMobile;
});

volume.addEventListener("input", (e) => {
  audio.volume = e.target.value;
});

audio.addEventListener("timeupdate", (e) => {
  const percent = (audio.currentTime / audio.duration) * 100;
  progress.style.width = `${percent}%`;
  currentTime.textContent = formatTime(audio.currentTime);
  duration.textContent = formatTime(audio.duration);
});
function formatTime(time) {
  const minutes = Math.floor(time / 60);
  const seconds = Math.floor(time % 60);
  return `${minutes}:${seconds.toString().padStart(2, "0")}`;
}

audio.addEventListener("ended", () => {
  if (isRepeatActive) {
    audio.currentTime = 0;
    audio.play();
  } else {
    currentSong++;
    if (currentSong >= playlist.children.length) {
      currentSong = 0;
    }
    audio.src = playlist.children[currentSong].getAttribute("src");
    audio.play();
    setActiveSong(currentSong);
    setMusicNameColor();
    updateMusicRowIcons();
  }
});

progressBar.addEventListener("click", function (event) {
  const totalWidth = this.offsetWidth;
  const clickPositionX = event.clientX - this.offsetLeft;

  const clickRatio = clickPositionX / totalWidth;
  const totalTime = audio.duration;

  audio.currentTime = clickRatio * totalTime;

  progress.style.width = clickRatio * 100 + "%";
});

let isRepeatActive = false;

document.getElementById("repeat").addEventListener("click", () => {
  isRepeatActive = !isRepeatActive;
});

const button = document.getElementById("repeat");
let isClicked = false;

button.addEventListener("click", () => {
  if (!isClicked) {
    button.style.color = "#1ed760";
    isClicked = true;
  } else {
    button.style.color = "rgb(147, 147, 147)";
    isClicked = false;
  }
});

const randomButton = document.getElementById("random");
let isRandomClicked = false;

randomButton.addEventListener("click", () => {
  if (!isRandomClicked) {
    randomButton.style.color = "#1ed760";
    isRandomClicked = true;
  } else {
    randomButton.style.color = "rgb(147, 147, 147)";
    isRandomClicked = false;
  }
});

randomSong.addEventListener("click", () => {
  let randValue = Math.floor(Math.random() * maxValue);
  audio.src = playlist.children[randValue].getAttribute("src");
  audio.play();
  setActiveSong(randValue);

  updateMusicRowIcons();

  const imgUrl = document
    .querySelector(".music-row.active-song img")
    .getAttribute("src");
  document.querySelector("#music-player img").setAttribute("src", imgUrl);

  const musicName = document.querySelector(
    ".music-row.active-song .music-name p"
  ).textContent;
  document.querySelector("#song-name span").textContent = musicName;

  const artistName = document.querySelector(
    ".active-song .artist-name p"
  ).textContent;
  document.querySelector(".artist-name span").textContent = artistName;
});

function getRandomInt(max) {
  return Math.floor(Math.random() * max);
}

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
