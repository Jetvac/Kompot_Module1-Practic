function switchPassVisibility(id) {
    let passInput = document.getElementById(id);

    if (passInput.getAttribute('type') == "password") {
        passInput.setAttribute('type', 'text');
        passInput.setAttribute('placeholder', 'password');
    } else {
        passInput.setAttribute('type', 'password');
        passInput.setAttribute('placeholder', '········');
    }
}