$(document).ready(function () {
    $('.dropdown-item').on('click', function () {
        var actionType = $(this).text().trim();
        var userId = $(this).data('users-id');

        if (actionType === "Edit") {
            console.log("Selected User ID for Edit: " + userId);

            if (userId) {
                $.ajax({
                    url: '/Home/GetFoodItemsByUserId',
                    type: 'GET',
                    data: { userId: userId },
                    success: function (data) {
                        console.log("Food Items Data for Edit: ", data);
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
        } else if (actionType === "Delete") {
            console.log("Delete button clicked for User ID: " + userId);
        }
    });
});


