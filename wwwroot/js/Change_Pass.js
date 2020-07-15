function checkFields() {
    var Password = document.getElementById("Password");
    var RePassword = document.getElementById("RePassword");

    if (Password.value == "" || RePassword.value == "") {
        Password.style["border"] = "1px solid red";
        RePassword.style["border"] = "1px solid red";
    }
    else if (Password.value.length < 8) {
        Password.style["border"] = "1px solid red";
        var PError = document.getElementById("PError");
        PError.innerHTML = "Password length must be greater than or equal to 6.";
    }
    else if (Password.value != RePassword.value) {
        Password.style["border"] = "1px solid red";
        RePassword.style["border"] = "1px solid red";
        var PError = document.getElementById("PError");
        PError.innerHTML = "Passwords don't match.";
    }
    else{
        return true;
    }
    return false;
}

function HideConfirmMsg() {
    document.getElementById("Confirmation").style.display = "none";
    document.getElementById("del").href = "#";
}