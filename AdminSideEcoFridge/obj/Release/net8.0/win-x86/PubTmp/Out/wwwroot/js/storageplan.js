document.addEventListener("DOMContentLoaded", function() {
  document.querySelectorAll('.dropdown-item').forEach(item => {
    item.addEventListener('click', function(event) {
      if(this.textContent === 'Update') {
        event.stopPropagation();

        const selectedRow = this.closest('tr');
        showUpdatePlan(selectedRow);

        const dropdown = this.closest('.dropdown-content');
        dropdown.classList.remove('show');
      }

      if(this.textContent === 'Disable') {
        event.stopPropagation();

        const selectedRow = this.closest('tr');
        showDisableModal(selectedRow);
        
        const dropdown = this.closest('.dropdown-content');
        dropdown.classList.remove('show');
      }

      if(this.textContent === 'Delete') {
        event.stopPropagation();

        const selectedRow = this.closest('tr');
        showDeleteModal(selectedRow);

        const dropdown = this.closest('.dropdown-content');
        dropdown.classList.remove('show');
      }
    });
  });

  document.getElementById('add-new-plan').addEventListener('click', function(event) {
    event.stopPropagation();
    document.querySelector('.add-plan').style.display = 'flex';
    document.querySelector('.update-plan').style.display = 'none';
  });

  document.getElementById('ok-disable').addEventListener('click', function (event) {
    event.stopPropagation();
    document.querySelector('.disable-modal').style.display = 'none';
  });

  document.getElementById('cancel-disable').addEventListener('click', function(event) {
    event.stopPropagation();
    document.querySelector('.disable-modal').style.display = 'none';
  });

  document.getElementById('ok-delete').addEventListener('click', function (event) {
    event.stopPropagation();
    document.querySelector('.delete-modal').style.display = 'none';
  });

  document.getElementById('ok-activate').addEventListener('click', function (event) {
    event.stopPropagation();
      document.querySelector('.activate-modal').style.display = 'none';
  });

  document.getElementById('cancel-delete').addEventListener('click', function(event) {
    event.stopPropagation();
    document.querySelector('.delete-modal').style.display = 'none';
  });

});

function showDeleteModal() {
  document.querySelector('.delete-modal').style.display = 'flex';
}

function showDisableModal() {
  document.querySelector('.disable-modal').style.display = 'flex';
}

function showUpdatePlan() {
  document.querySelector('.add-plan').style.display = 'none';
  document.querySelector('.update-plan').style.display = 'flex';
}