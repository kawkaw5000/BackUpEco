document.addEventListener('click', function (event) {
if (event.target && event.target.id === 'approveBtn') {
    document.getElementById('modal-approval').style.display = 'block';
    setTimeout(function () {
        document.getElementById('modal-approval').style.opacity = '1';
    }, 10);
}

if (event.target && event.target.id === 'declineBtn') {
    document.getElementById('modal-rejected').style.display = 'block';
    setTimeout(function () {
        document.getElementById('modal-rejected').style.opacity = '1';
    }, 10);
}

if (event.target && event.target.id === 'closeApprovalModalBtn') {
    document.getElementById('modal-approval').style.opacity = '0';
    setTimeout(function () {
        document.getElementById('modal-approval').style.display = 'none';
    }, 300);
}

if (event.target && event.target.id === 'closeDeclineModalBtn') {
    document.getElementById('modal-rejected').style.opacity = '0';
    setTimeout(function () {
        document.getElementById('modal-rejected').style.display = 'none';
    }, 300);
}
});