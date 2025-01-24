
$(document).ready(function() {
  const currentYear = new Date().getFullYear();
  $('#currentYear').text(currentYear);
});
$(document).ready(function() {
  $('[id^="bookButtonID"]').click(function() {
      window.location.href = '/book.html';
  });
});


function scrollToDestination(){
        const dest = document.getElementById("destination");
        dest.scrollIntoView({
            behavior:'smooth'
        });
    }
    $(document).ready(function() {
      const accessToken = localStorage.getItem('accessToken');

      function preventIfNotLoggedIn(event) {
        if (!accessToken) {
          event.preventDefault();
          window.location.href = '/login.html';
        }
      }

      if (accessToken) {
        $('#homeLink, #aboutLink, #destinationLink, #packagesLink, #galleryLink, #contactLink, #bookingsLink').css('display', 'inline-block');
        $('#loginLink').css('display', 'none');
        $('#logoutLink').css('display', 'inline-block');
      } else {
        $('#homeLink, #aboutLink, #destinationLink, #packagesLink, #galleryLink, #contactLink').css('display', 'inline-block');
        $('#bookingsLink').css('display', 'none');
        $('#loginLink').css('display', 'inline-block');
        $('#logoutLink').css('display', 'none');
        $('#bookingsLink').click(preventIfNotLoggedIn);
      }
    });
    
  
  function preventIfNotLoggedIn(event) {
      const accessToken = localStorage.getItem('accessToken');
      if (!accessToken) {
          event.preventDefault(); 
          window.location.href = '/login.html';
      }
  }
  
  function logout() {
      localStorage.removeItem('accessToken');
      window.location.href = '/login.html';
  }
  
  function moveToBook() {
      const accessToken = localStorage.getItem('accessToken');
      if (accessToken != null) {
          window.location.href = 'book.html';
      } else {
          window.location.href = 'login.html';
      }
  }

  document.addEventListener("DOMContentLoaded", function () {
    const openModalButton = document.getElementById("openModalButton");
    const closeModalButton = document.getElementsByClassName("close")[0];
    const modal = document.getElementById("contactModal");
  
    openModalButton.addEventListener("click", function () {
      modal.style.display = "block";
    });
  
    closeModalButton.addEventListener("click", function () {
      modal.style.display = "none";
    });
  
    window.addEventListener("click", function (event) {
      if (event.target === modal) {
        modal.style.display = "none";
      }
    });
  
    const contactForm = document.getElementById("contactForm");
    contactForm.addEventListener("submit", function (event) {
      contactForm.reset(); 
    });
  });
  