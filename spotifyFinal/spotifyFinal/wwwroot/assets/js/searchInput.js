document.querySelectorAll('.search-area-nav, .search-area').forEach(e => {
    const input = e.querySelector('input');
    const deleteIcon = e.querySelector('.delete');

    input.addEventListener('input', function() {
        if (this.value.length > 0) {
            deleteIcon.style.display = 'inline-block';
        } else {
            deleteIcon.style.display = 'none';
        }
    });

    deleteIcon.addEventListener('click', function() {
        input.value = '';
        deleteIcon.style.display = 'none';
    });
});
