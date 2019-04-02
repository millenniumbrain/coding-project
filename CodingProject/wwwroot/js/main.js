var $form = $("#form");
var $retrieve = $("#retrieveButton");

$form.submit(function (event) {
    event.preventDefault();
    var formData = new FormData(document.getElementById("form"));

    // date validation
    var dateFormat = /^\d{2}\/\d{2}\/\d{4}$/;
    if (!dateFormat.test($("#dob").val())) {
        $("#dob").get(0).setCustomValidity("Wrong date format! Use MM/MM/YYYY, ex: 01/01/1969");
        return false;
    } else {
        $("#dob").get(0).setCustomValidity("");
        // end function and prevent ajax trigger
    }

    if ($form.get(0).checkValidity()) {
        $.ajax({
            url: "/person",
            data: formData,
            type: "POST",
            processData: false,
            contentType: false
        }).done(function (data) {
            var $formMsg = $("#formMsg");
            $formMsg.text(data["msg"]);
            $formMsg.show();
            setTimeout(function () {
                $formMsg.hide();
            }, 5000);
        });
    } 

});

$retrieve.click(function () {
    $.get("/person", function (data) {
        var table = document.getElementById("tableOwn");

        if (table.rows.length < data.length) {
            table.deleteRow(0);
            for (var i = 0; i < data.length; i++) {
                var row = document.createElement("tr");
                var firstName = document.createElement("td");
                firstName.textContent = data[i]["FirstName"];
                var lastName = document.createElement("td");
                lastName.textContent = data[i]["LastName"];
                var dob = document.createElement("td");
                dob.textContent = data[i]["DOB"];
                var address = document.createElement("td");
                address.textContent = data[i]["Addresses"][0]["StreetOne"] + " " + data[i]["Addresses"][0]["StreetTwo"];
                var zipCode = document.createElement("td");
                zipCode.textContent = data[i]["Addresses"][0]["ZipCode"];
                var city = document.createElement("td");
                city.textContent = data[i]["Addresses"][0]["City"];
                var state = document.createElement("td");
                state.textContent = data[i]["Addresses"][0]["State"];
                row.appendChild(firstName);
                row.appendChild(lastName);
                row.appendChild(address);
                row.appendChild(dob);
                row.appendChild(city);
                row.appendChild(state);
                row.appendChild(zipCode);
                console.log("Moosse")
                table.appendChild(row);
            }
        }
    });
});
