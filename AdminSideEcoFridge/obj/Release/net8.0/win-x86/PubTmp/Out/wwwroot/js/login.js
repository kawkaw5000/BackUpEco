function toggleCurrentPasswordVisibility() {
    const passwordInput = document.getElementById("current-password");
    const eyeIcon = document.getElementById("current-eye-icon");

    if (passwordInput.type === "password") {
        passwordInput.type = "text";
        eyeIcon.src = "/images/LoginImg/eye-password-hide-svgrepo-com.svg";
    } else {
        passwordInput.type = "password";
        eyeIcon.src = "/images/LoginImg/eye-close-up-svgrepo-com.svg";
    }
}

function togglePasswordVisibility() {
  const passwordInput = document.getElementById("password");
  const eyeIcon = document.getElementById("eye-icon");
  
  if (passwordInput.type === "password") {
    passwordInput.type = "text";
      eyeIcon.src = "/images/LoginImg/eye-password-hide-svgrepo-com.svg";
  } else {
    passwordInput.type = "password";
    eyeIcon.src = "/images/LoginImg/eye-close-up-svgrepo-com.svg";
  }
}


function toggleConfirmPasswordVisibility() {
  const passwordInput = document.getElementById("confirm-password");
  const eyeIcon = document.getElementById("eyeconfirm-icon");
  
  if (passwordInput.type === "password") {
    passwordInput.type = "text";
      eyeIcon.src = "/images/LoginImg/eye-password-hide-svgrepo-com.svg";
  } else {
    passwordInput.type = "password";
      eyeIcon.src = "/images/LoginImg/eye-close-up-svgrepo-com.svg";
  }
}


function toggleCurrentPasswordVisibilityLogin() {
    const passwordInput = document.getElementById("current-password-login");
    const eyeIcon = document.getElementById("current-eye-icon");

    if (passwordInput.type === "password") {
        passwordInput.type = "text";
        eyeIcon.src = "/images/LoginImg/eye-password-hide-svgrepo-com.svg";
    } else {
        passwordInput.type = "password";
        eyeIcon.src = "/images/LoginImg/eye-close-up-svgrepo-com.svg";
    }
}

function togglePasswordVisibilityLogin() {
  const passwordInput = document.getElementById("password-login");
  const eyeIcon = document.getElementById("eye-icon");
  
  if (passwordInput.type === "password") {
    passwordInput.type = "text";
      eyeIcon.src = "/images/LoginImg/eye-password-hide-svgrepo-com.svg";
  } else {
    passwordInput.type = "password";
    eyeIcon.src = "/images/LoginImg/eye-close-up-svgrepo-com.svg";
  }
}


function toggleConfirmPasswordVisibilityLogin() {
  const passwordInput = document.getElementById("confirm-password-login");
  const eyeIcon = document.getElementById("eyeconfirm-icon");
  
  if (passwordInput.type === "password") {
    passwordInput.type = "text";
      eyeIcon.src = "/images/LoginImg/eye-password-hide-svgrepo-com.svg";
  } else {
    passwordInput.type = "password";
      eyeIcon.src = "/images/LoginImg/eye-close-up-svgrepo-com.svg";
  }
}

