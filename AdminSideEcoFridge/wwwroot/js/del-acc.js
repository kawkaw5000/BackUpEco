document.querySelectorAll('.delete-user-btn').forEach(button => {
    button.addEventListener('click', function (event) {
        event.preventDefault();
        const userId = this.getAttribute('data-del-users-id');
        const delAccModal = document.getElementById('modal-delacc');

        delAccModal.setAttribute('data-user-id', userId);

        delAccModal.classList.remove('hide');
        delAccModal.classList.add('show');
    });
});

document.getElementById('ok').addEventListener('click', function (event) {
    event.stopPropagation();

    const delAccModal = document.getElementById('modal-delacc');
    const userId = delAccModal.getAttribute('data-user-id');

    const token = document.querySelector('input[name="__RequestVerificationToken"]').value;

    fetch(`/Account/Delete/${userId}`, {
        method: 'DELETE',
        headers: {
            'Content-Type': 'application/json',
            'X-Requested-With': 'XMLHttpRequest',
            'RequestVerificationToken': token
        }
    })
        .then(response => {
            if (response.ok) {
                const row = document.querySelector(`tr[data-user-id='${userId}']`);
                if (row) row.remove();

                delAccModal.classList.remove('show');
                delAccModal.classList.add('hide');
            } else {
                console.error("Failed to delete the user.");
            }
        })
        .catch(error => console.error('Error:', error));
});

document.getElementById('cancel').addEventListener('click', function (event) {
    event.stopPropagation();

    const delAccModal = document.getElementById('modal-delacc');
    delAccModal.classList.remove('show');
    delAccModal.classList.add('hide');
});

