function quitBox() {
	if (navigator.userAgent.indexOf("Firefox") != -1 || navigator.userAgent.indexOf("Chrome") != -1) {
		open(location, '_self').close();
		window.location.href = "about:blank";
		window.close();
	} else {
		window.opener = null;
		window.open("", "_self");
		window.close();
		open(location, '_self').close();
	}  
}

function checkMonthDay(month, day) {
    var month2 = parseInt(month);
    switch (month2) {
        case 1:
            if (day < 1 || day > 31) {
                return false;
            }
            break;
        case 2:
            if (day < 1 || day > 31) {
                return false;
            }
            break;
        case 3:
            if (day < 1 || day > 31) {
                return false;
            }
            break;
        case 4:
            if (day < 1 || day > 31) {
                return false;
            }
            break;
        case 5:
            if (day < 1 || day > 31) {
                return false;
            }
            break;
        case 6:
            if (day < 1 || day > 31) {
                return false;
            }
            break;
        case 7:
            if (day < 1 || day > 30) {
                return false;
            }
            break;
        case 8:
            if (day < 1 || day > 30) {
                return false;
            }
            break;
        case 9:
            if (day < 1 || day > 30) {
                return false;
            }
            break;
        case 10:
            if (day < 1 || day > 30) {
                return false;
            }
            break;
        case 11:
            if (day < 1 || day > 30) {
                return false;
            }
            break;
        case 12:
            if (day < 1 || day > 29) {
                return false;
            }
            break;
        default:
            return false;
            break;
    }

    return true;
}