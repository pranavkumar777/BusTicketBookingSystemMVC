function validate(value,cvv,expirydate) {

    if (value == "") {
        console.log("cn");
        document.getElementById("invalid").innerHTML = "invalid credit card";
        return false;
    }
    if (/[^0-9-\s]+/.test(value))
        return false;

    var nCheck = 0, nDigit = 0, bEven = false;
    value = value.replace(/\D/g, "");

    for (var n = value.length - 1; n >= 0; n--) {
        var cDigit = value.charAt(n),
              nDigit = parseInt(cDigit, 10);

        if (bEven) {
            if ((nDigit *= 2) > 9) nDigit -= 9;
        }

        nCheck += nDigit;
        bEven = !bEven;
    }

    if ((nCheck % 10) != 0 || value.length>16) {
        document.getElementById("invalid").innerHTML = "invalid credit card";
    }
    else {
        document.getElementById("invalid").innerHTML = "";
    }
    if (cvv == "" || cvv.length != 3) {
        document.getElementById("cvvValid").innerHTML = "invalid cvv";
        console.log("cvv validation");
        return false;
    }
    else {
        document.getElementById("cvvValid").innerHTML = "";
    }
    if (expirydate == "") {
        console.log("expirydate validation");
        document.getElementById("expiry").innerHTML = "invalid expiry date";
        return false;
    }
    else {
        document.getElementById("expiry").innerHTML = "";
    }

    return (nCheck % 10) == 0;
}
