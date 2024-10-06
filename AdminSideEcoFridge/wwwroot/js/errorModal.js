function showErrorModal(message) {
    document.getElementById('modalBody').innerText = message;
document.getElementById('errorModal').style.display = 'block';
    }

function hideErrorModal() {
    document.getElementById('errorModal').style.display = 'none';
    }

document.querySelector('.close-btn').onclick = function () {
    hideErrorModal();
    }

window.onclick = function (event) {
        if (event.target === document.getElementById('errorModal')) {
    hideErrorModal();
        }
    }

document.addEventListener('DOMContentLoaded', function () {
        var errorMessage = '@Html.Raw(ViewData["ErrorMessage"])';
if (errorMessage) {
    showErrorModal(errorMessage);
        }
    });
