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
                document.getElementById('success-disable-plan').classList.add('show');
                document.getElementById('success-disable-plan').classList.remove('hide');
                document.getElementById('bg-blur').classList.add('show');
                document.getElementById('bg-blur').classList.remove('hide');
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

document.getElementById('bg-blur').addEventListener('click', function (event) {
    event.stopPropagation

    document.getElementById('success-disable-plan').classList.add('hide');
    document.getElementById('success-disable-plan').classList.remove('show');
    document.getElementById('success-activate-plan').classList.add('hide');
    document.getElementById('success-activate-plan').classList.remove('show');
    document.getElementById('success-deleted-plan').classList.add('hide');
    document.getElementById('success-deleted-plan').classList.remove('show');
    document.getElementById('bg-blur').classList.add('hide');
    document.getElementById('bg-blur').classList.remove('show');

    window.location.reload();
})

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
                document.getElementById('success-activate-plan').classList.add('show');
                document.getElementById('success-activate-plan').classList.remove('hide');
                document.getElementById('bg-blur').classList.add('show');
                document.getElementById('bg-blur').classList.remove('hide');
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


document.querySelectorAll('.delete-plan').forEach(item => {
    item.addEventListener('click', function () {
        const row = this.closest('tr');
        selectedPlanId = row.dataset.planId;
        document.querySelector('.delete-modal').style.display = 'flex';
    });
});

document.getElementById('ok-delete').addEventListener('click', function () {
    fetch(`/Plan/DeletePlan?id=${selectedPlanId}`, { 
        method: 'DELETE',
        headers: {
            'Content-Type': 'application/json',
        },
    })
        .then(response => {
            if (response.ok) {
                const row = document.querySelector(`tr[data-plan-id="${selectedPlanId}"]`);
                document.getElementById('success-deleted-plan').classList.add('show');
                document.getElementById('success-deleted-plan').classList.remove('hide');
                document.getElementById('bg-blur').classList.add('show');
                document.getElementById('bg-blur').classList.remove('hide');
                row.remove();
            } else {
                alert('Failed to delete the plan.');
            }
        })
        .catch(error => {
            console.error('Error:', error);
            alert('An error occurred.');
        })
        .finally(() => {
            document.querySelector('.delete-modal').style.display = 'none';
        });
});
