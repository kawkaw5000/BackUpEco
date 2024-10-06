$('#editUserForm').off('submit').on('submit', function (e) {
    e.preventDefault();

    $.ajax({
        url: $(this).attr('action'),
        type: 'POST',
        data: $(this).serialize(),
        success: function (response) {
      
            console.log("Response: ", response);

            if (response.success) {
                alert(response.message);
                location.reload();
            } else {
                alert(response.message || "An unknown error occurred.");
            }
        },
        error: function (xhr, status, error) {
            console.error("AJAX Error: ", error);
            console.error("Status: ", status);
            console.error("Response Text: ", xhr.responseText); 


            alert("An error occurred while updating the user. Please try again.\n\n" +
                "Error Details: " + xhr.responseText);
        }
    });
});
