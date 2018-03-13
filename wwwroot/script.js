function DisplayLog(message) {
    document.querySelector("#statusMessages").textContent += message + "<br/>";
}

function DisplayCount(count) {
    document.querySelector("#statusMessages").textContent += "Clients Connected: " + count + "<br/>"; 
}