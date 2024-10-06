$(document).ready(function () {
    $('table.user-list-table tbody tr').on('click', function () {
        var userId = $(this).data('user-id');
        console.log("Selected User ID: " + userId);

        if (userId) {
            $.ajax({
                url: '/Home/GetFoodItemsByUserId',
                type: 'GET',
                data: { userId: userId },
                success: function (data) {
                    console.log("Food Items Data: ", data);
                    $('.fridge-list').empty();

                    if (data.length === 0) {
                        $('.fridge-list').append(
                            '<tr><td colspan="3" class="no-items">No food items found.</td></tr>'
                        );
                    } else {
                        $.each(data, function (index, food) {
                            $('.fridge-list').append(
                                '<tr class="fridge-tr">' +
                                '<td class="fridge-td">' + (food.quantity || '') + '</td>' +
                                '<td class="fridge-td">' + (food.servings || '') + '</td>' +
                                '<td class="fridge-td">' + (food.foodName || '') + '</td>' +
                                '</tr>'
                            );
                        });
                    }
                },
                error: function (xhr, status, error) {
                    console.error("Error fetching food items:", error);
                    alert('Error fetching food items.');
                }
            });
        } else {
            console.error("User ID is undefined.");
        }
    });


 
});
