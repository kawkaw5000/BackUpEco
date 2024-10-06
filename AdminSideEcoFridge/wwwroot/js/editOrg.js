$(document).ready(function () {
    $('.dropdown-item[data-users-id]').on('click', function () {
        var userId = $(this).data('users-id');     
        $.ajax({
            url: '/Account/EditOrg',
            type: 'GET',
            data: { id: userId },
            success: function (result) {              
                $('.edit-org-user-container').html(result);
            },
            error: function () {
                alert("Error loading user details.");
            }
        });
    });
});
