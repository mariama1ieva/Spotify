// const deleteButton = document.querySelector("#delete");
// const modalContainerElement = document.querySelector(".modal-container");
// if (!deleteButton) {
//     console.error("delete button yoxdu!");
// }

// deleteButton.addEventListener("click", () => {
//     modalContainerElement.style.display = "block";

//     const iframe = document.createElement("iframe");
//     iframe.setAttribute("src", "/client/pages/popup/popup.html");
//     iframe.setAttribute("id", "popup-iframe");
//     iframe.classList.add("popup-iframe");

//     document.body.appendChild(iframe);

//     document.getElementById("popup-iframe").addEventListener("load", () => {
//         if (!iframe.contentWindow) {
//             console.error("is null");
//             return;
//         }

//         const closeButton = this.contentWindow.document.querySelector(".close-btn");
//         const confirmDeleteButton = iframe.contentWindow.document.querySelector("#delete-popup");

//         closeButton.addEventListener("click", () => {
//             modalContainerElement.style.display = "none";
//             iframe.remove();
//         });

//         confirmDeleteButton.addEventListener("click", () => {
//             console.log("d");
//             modalContainerElement.style.display = "none";
//             iframe.remove();
//         });
//     });
// });

const deleteButton = document.querySelectorAll(".fa-trash");

deleteButton.forEach((deleteBtn) => {
  deleteBtn.addEventListener("click", function () {
    Swal.fire({
      title: "Are you sure you want to delete?",
      color: `black`,
      showConfirmButton: true,
      showCancelButton: true,
      confirmButtonText: `Delete`,
      confirmButtonColor: `#1ed760`,
      cancelButtonColor: `white`,
    }).then((result) => {
      if (result.isConfirmed) {
      } else if (result.isDenied) {
      }
    });
  });
});
