document.getElementById('notificationIcon').addEventListener('click', function() {
  var dropdown = document.getElementById('notificationDropdown');
  dropdown.style.display = (dropdown.style.display === 'block') ? 'none' : 'block';
});

document.addEventListener('click', function(event) {
  var dropdown = document.getElementById('notificationDropdown');
  var icon = document.getElementById('notificationIcon');
  
  if (!icon.contains(event.target) && !dropdown.contains(event.target)) {
    dropdown.style.display = 'none';
  }
});

document.getElementById('profileImg').addEventListener('click', function() {
  var dropdown = document.getElementById('profileDropdown');
  dropdown.style.display = (dropdown.style.display === 'block') ? 'none' : 'block';
});

document.addEventListener('click', function(event) {
  var dropdown = document.getElementById('profileDropdown');
  var profileImg = document.getElementById('profileImg');

  if (!profileImg.contains(event.target) && !dropdown.contains(event.target)) {
    dropdown.style.display = 'none';
  }
});
