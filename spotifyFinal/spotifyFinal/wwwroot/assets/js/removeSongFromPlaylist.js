//$(document).ready(function () {
//    $('.remove').on('click', function () {
//        var songId = $(this).attr('songId');
//        var playlistId = $('.playlistId').val(); // Adjust selector if necessary

//        $.ajax({
//            url: '/Playlist/RemoveSongFromPlaylist',
//            type: 'POST',
//            data: JSON.stringify({ playlistId: playlistId, songId: songId }),
//            contentType: 'application/json; charset=utf-8',
//            headers: {
//                'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
//            },
//            success: function (data) {
//                if (data.success) {
//                    // Use SweetAlert2 to show a success message
//                    Swal.fire({
//                        icon: 'success',
//                        title: 'Success!',
//                        text: 'The song has been removed from the playlist.',
//                        timer: 1500, // Optional: auto-close the alert after 1.5 seconds
//                        showConfirmButton: false // Optional: hide the confirm button
//                    }).then(() => {
//                        // Remove the song element from the DOM
//                        $(this).closest('.music-row').remove();
//                    }.bind(this)); // Ensure `this` is bound correctly to the button
//    } 
//});