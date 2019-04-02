var $form = $("#form");
var $retrieve = $("#retrieveButton");

$form.submit(function (event) {
    event.preventDefault();
    var formData = new FormData(document.getElementById("form"));
    console.log(formData);
    $.ajax({
        url: "/person",
        data: formData,
        type: "POST",
        processData: false,
        contentType: false
    })
});

$retrieve.click(function () {
    $.get("/person", function (data) {
        var table = document.getElementById("tableOwn");
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
            row.appendChild(zipCode);
            row.appendChild(city);
            row.appendChild(state);

            table.appendChild(row);
        }
    });
})