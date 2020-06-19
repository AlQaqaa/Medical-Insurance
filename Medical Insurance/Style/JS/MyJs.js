$(document).ready(function () {
    $('.chosen-select').select2();
});

function pageLoad(sender, args) {
    $(".chosen-select").select2();
}

$(document).ajaxStart(function () {
    $('#wait').show();
}).ajaxStop(function () {
    $('#wait').hide();
});




