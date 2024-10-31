document.addEventListener('DOMContentLoaded', function () {
    const updateModal = document.getElementById('update-plan');
    const editPlanName = document.getElementById('edit-plan-name');
    const editPricePlan = document.getElementById('edit-price-plan');
    const editSizePlan = document.getElementById('edit-size-plan');
    const editDurationPlan = document.getElementById('edit-duration-plan');
    const editButton = document.getElementById('edit-plan');

    document.querySelectorAll('.dropdown-item').forEach(item => {
        item.addEventListener('click', function (event) {
            if (this.textContent.trim() === 'Update') {
                const row = this.closest('tr');

                editPlanName.value = row.getAttribute('data-plan-name');
                editPricePlan.value = row.getAttribute('data-plan-price');
                editSizePlan.value = row.getAttribute('data-plan-size');
                editDurationPlan.value = row.getAttribute('data-plan-duration');

                editButton.setAttribute('data-plan-id', row.getAttribute('data-plan-id'));

            }
        });
    });

    editButton.addEventListener('click', function () {
        const planId = this.getAttribute('data-plan-id');

        const updatedData = {
            storagePlanId: planId,
            storagePlanName: editPlanName.value,
            price: parseFloat(editPricePlan.value),
            storageSize: parseInt(editSizePlan.value),
            duration: parseInt(editDurationPlan.value)
        };

        fetch('/Plan/EditPlan', {
            method: 'PUT',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(updatedData)
        })
            .then(response => {
                if (response.ok) {
                    alert('Plan Updated successfully.');
                    window.location.reload();
                } else {
                    response.json().then(errorData => {
                        console.error('Validation errors:', errorData);

                        if (errorData.errors) {
                            if (errorData.errors.StorageSize) {
                                alert('Error: ' + errorData.errors.StorageSize[0]);
                            }
                            if (errorData.errors.Duration) {
                                alert('Error: ' + errorData.errors.Duration[0]);
                            }
                            if (errorData.errors.Price) {
                                alert('Error: ' + errorData.errors.Price[0]);
                            }
                        }
                    });
                }
            })
            .catch(error => console.error('Error:', error));
    });
});