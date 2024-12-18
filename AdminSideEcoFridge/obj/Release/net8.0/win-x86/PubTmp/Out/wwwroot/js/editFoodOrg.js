$(document).ready(function () {
    $('.dropdown-item[data-users-id]').on('click', function () {
        var userId = $(this).data('users-id');
        $.ajax({
            url: '/Account/EditFoodResto',
            type: 'GET',
            data: { id: userId },
            success: function (result) {
                $('.edit-food-user-container').html(result);
            },
            error: function () {
                alert("Error loading user details.");
            }
        });
    });
});
