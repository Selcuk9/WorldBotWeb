const inputPass = document.getElementById('password');
const inputConfPassword = document.getElementById('confirm_password');

inputConfPassword.addEventListener('change', function () {
    if (inputPass.value === this.value) {
        if (this.classList.contains('input-failed')) {
            this.classList.remove('input-failed');
        }
        this.classList.add('input-success');
    }
    else {

        if (this.classList.contains('input-success')) {
            this.classList.remove('input-success');
        }
        this.classList.add('input-failed');
    }

});