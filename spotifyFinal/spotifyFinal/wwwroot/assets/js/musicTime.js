document.querySelectorAll(".music-row").forEach((card) => {
    const audioSrc = card.getAttribute("src");
    const audio = new Audio(audioSrc);
  
    audio.addEventListener("loadedmetadata", () => {
      const minutes = Math.floor(audio.duration / 60);
      const seconds = Math.floor(audio.duration % 60);
      const formattedTime = `${minutes}:${seconds.toString().padStart(2, "0")}`;
  
      const musicTimeElement = card.querySelector(".music-time p");
      musicTimeElement.textContent = formattedTime;
    });
  });