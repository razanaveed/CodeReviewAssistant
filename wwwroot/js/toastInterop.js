window.toastInterop = {
    showToast: function (message, color) {
        const toastEl = document.getElementById('liveToast');
        const toastBody = document.getElementById('toastBody');

        // Update message
        toastBody.textContent = message;

        // Update color (success, danger, warning, info, primary)
        toastEl.className = 'toast align-items-center text-bg-' + color + ' border-0';

        // Show toast using Bootstrap 5 API
        const toast = new bootstrap.Toast(toastEl);
        toast.show();
    }
};