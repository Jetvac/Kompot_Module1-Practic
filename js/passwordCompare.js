function passwordCompare() {
    password = document.getElementById('password-data__input');
    confirmationPassword = document.getElementById('password-data__input__confirmation');

    if(password.value != confirmationPassword.value){
        return false;
    }
    return true;
}