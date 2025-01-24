$(document).ready(function () {
   
    $('#loginForm').submit(function (event) {
      event.preventDefault();

      var email = $('#emailInput').val();
      var password = $('#passwordInput').val();

      $.ajax({
        url: 'http://localhost:3000/api/user/login', 
        method: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({ email: email, password: password }),
        success: function (response) {
          localStorage.setItem('accessToken', response.accessToken);
          localStorage.setItem('role', response.role);
          localStorage.setItem('id',response.id);
          alert('Login successful');
          window.location.href = '/index.html';
        },
        error: function (xhr, status, error) {
          console.error('Login failed:', error);
          alert('Login failed. Check your email and password.');
        }
      });
    });
  });
  var accessToken = localStorage.getItem('accessToken');
  document.addEventListener('DOMContentLoaded', function () { 
    const homeLink = document.getElementById('homeLink');
    const bookFlightLink = document.getElementById('bookFlightLink');
    const bookingsLink = document.getElementById('bookingsLink');
    const loginLink = document.getElementById('loginLink');
    const logoutLink = document.getElementById('logoutLink');

    if (accessToken!=null) {
      homeLink.style.display = 'inline-block';
      bookFlightLink.style.display = 'inline-block';
      bookingsLink.style.display = 'inline-block';
      loginLink.style.display = 'none';
      logoutLink.style.display = 'inline-block';
    } else {
      homeLink.style.display = 'inline-block';
      bookFlightLink.style.display = 'none';
      bookingsLink.style.display = 'none';
      loginLink.style.display = 'inline-block';
      logoutLink.style.display = 'none';
    }
}); 

  function logout() {
    localStorage.removeItem('accessToken');

    window.location.href = '/login.html';

  }

  document.addEventListener('DOMContentLoaded', function () {
    const logo = document.querySelector('.input-icon2');
    const passwordInput = document.getElementById('passwordInput');
    logo.addEventListener('click', function () {
        passwordInput.type = passwordInput.type === 'password' ? 'text' : 'password';
        logo.src = passwordInput.type === 'password' ? '/assets/password-hide.png' : '/assets/password-icon.png';
    });
    const registerButton = document.getElementById('registerButton');
    registerButton.addEventListener('click', function () {
        redirectToRegister();
    });
});

function redirectToRegister() {
    window.location.href = './register.html';
}