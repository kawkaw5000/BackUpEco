function displayFileName() {
  const fileInput = document.getElementById('myFile');
  const fileNameSpan = document.getElementById('file-name');
  
  if (fileInput.files.length > 0) {
    fileNameSpan.textContent = fileInput.files[0].name;
  } else {
    fileNameSpan.textContent = "No file chosen";
  }
}

function displayPermitFileName() {
  const fileInput = document.getElementById('myPermitFile');
  const fileNameSpan = document.getElementById('Permitfile-name');
  
  if (fileInput.files.length > 0) {
    fileNameSpan.textContent = fileInput.files[0].name;
  } else {
    fileNameSpan.textContent = "No file chosen";
  }
}

