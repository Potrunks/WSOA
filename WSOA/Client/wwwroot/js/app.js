function blurring(classNames) {
    var elements = document.getElementsByClassName(classNames);

    for (var i = 0; i < elements.length; i++) {
        var element = elements[i];
        var isHide = element.classList.contains("flood");

        if (isHide === true) {
            element.classList.remove("flood");
        }
        else {
            element.classList.add("flood");
        }
    }
}