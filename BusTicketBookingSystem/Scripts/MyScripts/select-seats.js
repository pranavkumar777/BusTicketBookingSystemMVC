


function submit() {

    var cardnumber = document.getElementById("cardnumber").value;
    console.log(cardnumber);
    var cvv = document.getElementById("cvv").value;
    var expirydate = document.getElementById("expiry").value;
    var result = validate(cardnumber, cvv, expirydate);

    console.log(result);
    var seatDetails = {

        seats: selectedSeats,
        BusID: document.getElementById("BusID").value,
        Cost: document.getElementById("Cost").value,
        CustomerEmail: document.getElementById("CustomerEmail").value,
        BusSource: document.getElementById("source").value,
        BusDestination: document.getElementById("destination").value,

    };

    console.log(seatDetails);
    if (result) {
        var xhr = $.ajax({
            type: "POST",
            url: "Payment",

            data: seatDetails,
            traditional: true,
            success: function () {
                window.location = "/Customer/Payment";
            }
        }
            );

    }



}
