// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// SweetAlert notification helper
function showNotification(type, title, message) {
    Swal.fire({
        icon: type,
        title: title,
        text: message,
        toast: true,
        position: 'top-end',
        showConfirmButton: false,
        timer: 3000,
        timerProgressBar: true
    });
}

// Confirmation dialog helper
function showConfirmDialog(title, text, confirmCallback) {
    Swal.fire({
        title: title,
        text: text,
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes',
        cancelButtonText: 'Cancel'
    }).then((result) => {
        if (result.isConfirmed && confirmCallback) {
            confirmCallback();
        }
    });
}

// Date picker initialization
$(document).ready(function() {
    // Set minimum date for date inputs
    $('input[type="date"]').each(function() {
        if ($(this).attr('min') === undefined) {
            var today = new Date().toISOString().split('T')[0];
            $(this).attr('min', today);
        }
    });
    
    // Form validation feedback
    $('form').on('submit', function(e) {
        if (!this.checkValidity()) {
            e.preventDefault();
            e.stopPropagation();
            showNotification('error', 'Validation Error', 'Please fill in all required fields.');
        }
        $(this).addClass('was-validated');
    });
});
