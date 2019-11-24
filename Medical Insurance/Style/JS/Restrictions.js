// لغة عربية
function isAlphabetKeyAR(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if ((charCode <= 46 && charCode >= 8) || (charCode <= 1791 && charCode >= 1536)) {

        return true;
    }

    return false;

}
// لغة إنجليزية
function isAlphabetKeyEN(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if ((charCode <= 47 && charCode >= 0) || (charCode <= 255 && charCode >= 58)) {

        return true;
    }

    return false;

}
//ارقام وكسر
function isAlphabetKeyEU(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if ((charCode <= 57 && charCode >= 48) || (charCode <= 46 && charCode >= 46)|| (charCode <= 32 && charCode >=0)) {

        return true;
    }

    return false;

}
//ارقام وكسر + لغة العربية
function isAlphabetKeyEUAR(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if ((charCode <= 57 && charCode >= 48) || (charCode <= 46 && charCode >= 8) || (charCode <= 1791 && charCode >= 1536)) {

        return true;
    }

    return false;

}
//ارقام وكسر +ولغة انجليزية
function isAlphabetKeyEUEN(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if ((charCode <= 255 && charCode >= 0)) {

        return true;
    }

    return false;

}
//ارقام صحيحه
function isAlphabetKeyEUIN(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if ((charCode <= 57 && charCode >= 48) || (charCode <= 31 && charCode >= 0) ) {

        return true;
    }

    return false;

}