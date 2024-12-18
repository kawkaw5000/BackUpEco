document.addEventListener("DOMContentLoaded", function() {
    var createModal = document.getElementById("myModal");
    var addBtn = document.getElementById("add-btn");
    var closeCreateModalBtn = document.getElementById("closeModalBtn");

    if (addBtn) {
      addBtn.onclick = function() {
        createModal.style.display = "flex";
        setTimeout(function() {
          createModal.classList.add("show");
        }, 10);
      };
    }

    if (closeCreateModalBtn) {
      closeCreateModalBtn.onclick = function() {
        createModal.classList.remove("show"); 
        setTimeout(function() {
          createModal.style.display = "none";
        }, 300);
      };
    }

    var deleteModal = document.getElementById("deleteModal");
    var cancelDeleteBtn = document.getElementById("cancelDeleteBtn");
    var confirmDeleteBtn = document.getElementById("confirmDeleteBtn");

    document.querySelectorAll("#del-btn").forEach(function(deleteBtn) {
        deleteBtn.addEventListener("click", function() {
            deleteModal.style.display = "flex";
            setTimeout(function() {
                deleteModal.classList.add("show");
            }, 10);
        });
    });

    if (cancelDeleteBtn) {
      cancelDeleteBtn.onclick = function() {
        deleteModal.classList.remove("show");
        setTimeout(function() {
          deleteModal.style.display = "none";
        }, 300);
      };
    }

    if (confirmDeleteBtn) {
      confirmDeleteBtn.onclick = function() {
        deleteModal.classList.remove("show");
        setTimeout(function() {
          deleteModal.style.display = "none";
        }, 300);
      };
    }

    window.onclick = function(event) {
      if (event.target == createModal) {
        createModal.classList.remove("show");
        setTimeout(function() {
          createModal.style.display = "none"; 
        }, 300);
      } else if (event.target == deleteModal) {
        deleteModal.classList.remove("show");
        setTimeout(function() {
          deleteModal.style.display = "none"; 
        }, 300);
      }
    };

    document.querySelectorAll('.action-btn').forEach(button => {
      button.addEventListener('click', function(event) {
        event.stopPropagation();

        var dropdownContent = this.parentElement.querySelector('.dropdown-content');
        var isDropdownVisible = dropdownContent.classList.contains('show');

        document.querySelectorAll('.dropdown-content.show').forEach(dropdown => {
          dropdown.classList.remove('show');
        });
        if (!isDropdownVisible) {
          dropdownContent.classList.add('show');
        }
      });
    });

    window.addEventListener('click', function() {
      document.querySelectorAll('.dropdown-content.show').forEach(dropdown => {
        dropdown.classList.remove('show');
      });
    });
});
