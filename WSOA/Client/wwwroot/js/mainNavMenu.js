function switchMainNavMenuDisplayStatus() {
    var element = document.getElementById("MainNavMenu");
    var isHide = element.classList.contains("flood");

    if (isHide === true) {
        element.classList.remove("flood");
    }
    else {
        element.classList.add("flood");
    }
}
