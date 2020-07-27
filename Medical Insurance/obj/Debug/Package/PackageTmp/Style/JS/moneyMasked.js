function leftTrim(sString) {
    while (sString.substring(0, 1) == ' ') {
        sString = sString.substring(1, sString.length);
    }
    return sString;
}

function rightTrim(sString) {
    while (sString.substring(sString.length - 1, sString.length) == ' ') {
        sString = sString.substring(0, sString.length - 1);
    }
    return sString;
}
function trimAll(sString) {
    while (sString.substring(0, 1) == ' ') {
        sString = sString.substring(1, sString.length);
    }
    while (sString.substring(sString.length - 1, sString.length) == ' ') {
        sString = sString.substring(0, sString.length - 1);
    }
    return sString;
}
function onlyNumbers(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode

    if (charCode > 31 && (charCode < 48 || charCode > 57) && charCode != 46)
        return false;
    if (charCode == 13)
        return false;

    return true;

}

var patterns = new Array(
        "###,###,###,###.###",  // US/British
        "###.###.###.###",  // German
        "### ### ### ###",  // French
        "###'###'###'###",  // Swiss
        "#,##,##,##,##,###.###",  // Indian
        "####\u5104 ####\u4E07 ####",  // Japanese/Chinese
        "############" // no formatting
        );


// A little function to take an integer string, strip out current formatting,
// and reformat it according to a pattern string. '#' characters in the pattern
// are substituted with digits from the integer string, other pattern characters
// are output literally into the returned string.
function formatInteger(integer, pattern) {
    var result = '';

    //integer = 500;
    //if (integer < 0) ointeger = 10011;
    //ointeger = 5500;
    var ointeger = integer
    //if (parseInt(integer) =1) ointeger = 99999
    integerIndex = ointeger.length - 1;
    patternIndex = pattern.length - 1;


    while ((integerIndex >= 0) && (patternIndex >= 0)) {
        var digit = ointeger.charAt(integerIndex);
        integerIndex--;


        // Skip non-digits from the source integer (eradicate current formatting).
        if ((digit < '0') || (digit > '9')) continue;


        // Got a digit from the integer, now plug it into the pattern.
        while (patternIndex >= 0) {
            var patternChar = pattern.charAt(patternIndex);
            patternIndex--;


            // Substitute digits for '#' chars, treat other chars literally.
            if (digit == '.')
                break;
            else if (patternChar == '#') {
                result = digit + result;
                break;
            }
            else {
                result = patternChar + result;
            }
        }
    }

    if (integer < 0) {
        result = result.replace(',', '');
        result = result.replace(',', '');
        result = result.replace(',', '');

        result = result * -1

    }
    return result;
}


function appendDollar(id) {
    var amount = document.getElementById(id).value;
    // if (amount.value <=0)    return 0;

    var Doubleval = amount.split(".")[1];

    var Doubleval1
    if (Doubleval == null) Doubleval = ".000"
    else {

        if (Doubleval.length == 3) Doubleval1 = "." + Doubleval;

        else
            if (Doubleval.length == 2) Doubleval1 = "." + Doubleval + "0";
            else
                if (Doubleval.length == 1) Doubleval1 = "." + Doubleval + "00";
                else
                    if (Doubleval.length == 0) Doubleval1 = "." + Doubleval + "000";


        if (Doubleval.length > 3) Doubleval1 = "." + Doubleval.substring(0, 3);


        Doubleval = Doubleval1

        amount = amount.split(".")[0];

    }

    if (trimAll(amount) != "") {
        //            amount = amount.toFixed
        if (amount > 0)
            document.getElementById(id).value = formatInteger(amount, '###,###,###,###') + Doubleval;

        if (Doubleval = 0)
            document.getElementById(id).value = Doubleval * -1;

    }


}
