document.getElementById('new-plan').addEventListener('click', function (event) {
    event.stopPropagation();

    const planName = document.getElementById('storage-plan-name').value.trim();
    const storagePrice = parseFloat(document.getElementById('storage-price').value.trim());
    const storageSize = parseInt(document.getElementById('storage-size').value, 10);
    const duration = parseInt(document.getElementById('duration').value, 10);

    const storageData = {
        StoragePlanName: planName,
        Price: storagePrice,
        StorageSize: storageSize,
        Duration: duration,
    };

    fetch('/Plan/AddPlan', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(storageData)
    })
        .then(response => {
            if (response.ok) {
                document.getElementById('storage-plan-name').value = '';
                document.getElementById('storage-price').value = '';
                document.getElementById('storage-size').value = '';
                document.getElementById('duration').value = '';
            } else {
                alert('Failed to add plan. Please try again.');
            }
        })
        .catch(error => console.error('Error:', error));
});
