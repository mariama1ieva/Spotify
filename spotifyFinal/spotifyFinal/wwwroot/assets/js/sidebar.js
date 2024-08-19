const settingElement=document.getElementById("setting");
const settingBtn=document.getElementsByClassName("setting-menu")[0];

settingElement.addEventListener("click",(e)=>{
    e.stopPropagation();
    settingBtn.classList.add("open");
});

settingBtn.addEventListener("click",(e)=>{
    e.stopPropagation();
});

window.onclick=()=>{
    settingBtn.classList.remove("open");
} 