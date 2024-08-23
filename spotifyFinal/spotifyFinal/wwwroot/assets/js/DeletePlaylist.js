//$(document).ready(function () {
//    $(".delete-playlist").on("click", function (event) {
//        event.stopImmediatePropagation();

//        const button = $(this);
//        const itemId = button.data('id');

//        if (confirm("Are you sure you want to delete this playlist?")) {
//            $.ajax({
//                url: `/Playlist/Delete/${itemId}`,
//                type: 'DELETE',
//                headers: {
//                    'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
//                },
//                success: function (result) {
//                    // Remove the playlist card from the DOM
//                    const playlistCard = button.closest(".sidebar-playlist-cards");
//                    if (playlistCard.length) {
//                        playlistCard.remove();
//                    }
//                },
//                error: function (xhr, status, error) {
//                    console.error("Failed to delete playlist:", error);
//                }
//            });
//        }
//    });
//});