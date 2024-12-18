document.getElementById('new-plan').addEventListener('click', function (event) {
    event.stopPropagation();

    const planName = document.getElementById('storage-plan-name').value.trim();
    const storagePriceInput = document.getElementById('storage-price').value.trim();
    const storageSizeInput = document.getElementById('storage-size').value.trim();
    const durationInput = document.getElementById('duration').value.trim();

    if (!planName || !storagePriceInput || !storageSizeInput || !durationInput) {
        alert('All fields are required.');
        return; 
    }

    const storagePrice = parseFloat(storagePriceInput);
    const storageSize = parseInt(storageSizeInput, 10);
    const duration = parseInt(durationInput, 10);

    if (storagePrice <= 0 || storageSize <= 0 || duration <= 0) {
        alert('Please enter valid positive values for price, size, and duration.');
        return; 
    }

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
                window.location.reload();
            } else {
                alert('Failed to add plan. Please try again.');
            }
        })
        .catch(error => console.error('Error:', error));
});

