function DisplayLog(message) {
    document.querySelector("#statusMessages").textContent += message + "\r\n";
}

function DisplayCount(count) {
    document.querySelector("#statusMessages").textContent += "Clients Connected: " + count + "\r\n"; 
}