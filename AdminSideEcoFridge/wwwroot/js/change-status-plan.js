let selectedPlanId;

document.querySelectorAll('.disable-plan').forEach(item => {
    item.addEventListener('click', function () {
        const row = this.closest('tr');
        selectedPlanId = row.dataset.planId;
        document.querySelector('.disable-modal').style.display = 'flex';
    });
});

document.getElementById('ok-disable').addEventListener('click', function () {
    fetch('/Plan/DisablePlan', {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({ StoragePlanId: selectedPlanId }),
    })
        .then(response => {
            if (response.ok) {

                const row = document.querySelector(`tr[data-plan-id="${selectedPlanId}"]`);
                alert('Plan disabled successfully.');
                window.location.reload();
            } else {
                alert('Failed to disable the plan.');
            }
        })
        .catch(error => {
            console.error('Error:', error);
            alert('An error occurred.');
        })
        .finally(() => {
            document.querySelector('.disable-modal').style.display = 'none';
        });
});

document.getElementById('cancel-disable').addEventListener('click', function () {
    document.querySelector('.disable-modal').style.display = 'none';
});

document.getElementById('cancel-activate').addEventListener('click', function () {
    document.querySelector('.activate-modal').style.display = 'none';
});

document.querySelectorAll('.activate-plan').forEach(item => {
    item.addEventListener('click', function () {
        const row = this.closest('tr');
        selectedPlanId = row.dataset.planId;
        document.querySelector('.activate-modal').style.display = 'flex';
    });
});

document.getElementById('ok-activate').addEventListener('click', function () {
    fetch('/Plan/ActivatePlan', {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({ StoragePlanId: selectedPlanId }),
    })
        .then(response => {
            if (response.ok) {

                const row = document.querySelector(`tr[data-plan-id="${selectedPlanId}"]`);
                alert('Plan activated successfully.');
                window.location.reload();
            } else {
                alert('Failed to activate the plan.');
            }
        })
        .catch(error => {
            console.error('Error:', error);
            alert('An error occurred.');
        })
        .finally(() => {
            document.querySelector('.activate-modal').style.display = 'none';
        });
});
