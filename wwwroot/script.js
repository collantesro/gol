function DisplayLog(message) {
    var entry = new Date().toLocaleString() + ": " + message + "<br/>";
    document.querySelector("#statusMessages").insertAdjacentHTML("afterbegin", entry);
}

function DisplayCount(count) {
    DisplayLog("Clients Connected: " + count);
}
