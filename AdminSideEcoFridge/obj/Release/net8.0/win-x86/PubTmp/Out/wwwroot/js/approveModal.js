document.querySelector('.Orgaction-btn').addEventListener('click', function() {
  const dropdown = document.querySelector('.dropdown-menu');
  dropdown.style.display = dropdown.style.display === 'block' ? 'none' : 'block';
});

document.getElementById('approve-btn').addEventListener('click', function() {
  showModal('Organization has been approved');
});

document.getElementById('reject-btn').addEventListener('click', function() {
  showModal('Organization has been rejected');
});

document.getElementById('close-modal').addEventListener('click', function() {
  closeModal();
});

function showModal(message) {
  document.getElementById('modal-message').textContent = message;
  document.getElementById('org-modal').style.display = 'block';
}

function closeModal() {
  document.getElementById('org-modal').style.display = 'none';
}

window.addEventListener('click', function(e) {
  const dropdown = document.querySelector('.dropdown-menu');
  if (!e.target.matches('.Orgaction-btn') && !e.target.matches('.Orgaction-btn *')) {
      dropdown.style.display = 'none';
  }
});
