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

})