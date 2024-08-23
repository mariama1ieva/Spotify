//document.addEventListener("DOMContentLoaded", () => {
//    document.querySelectorAll(".delete-comment-btn").forEach(button => {
//        button.addEventListener("click", async event => {
//            event.preventDefault(); // Prevent the default link action

//            const commentId = button.getAttribute('data-comment-id');
//            const confirmation = confirm("Are you sure you want to delete this comment?");

//            if (confirmation) {
//                try {
//                    const response = await fetch(`/Comments/Delete/${commentId}`, {
//                        method: 'DELETE',
//                        headers: {
//                            'Content-Type': 'application/json',
//                            'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
//                        }
//                    });

//                    if (response.ok) {
//                        // Find and remove the comment from the DOM
//                        const commentItem = button.closest(".comment-item");
//                        if (commentItem) {
//                            commentItem.remove();
//                        }
//                    } else {
//                        console.error("Failed to delete comment. Status:", response.status);
//                    }
//                } catch (error) {
//                    console.error("Error during deletion:", error);
//                }
//            }
//        });
//    });
//});
