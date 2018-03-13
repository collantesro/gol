function ButtonEventFire(ButtonEvent) {
    var b = ButtonEvent.target;
    var row = b.getAttribute("data-row"); 
    var col = b.getAttribute("data-col");
    console.log("Row: " + row + " Column: " + col);

    ButtonLife(b);
}


function ButtonLife(b) {
    //var b = ButtonEvent.target;
    
    b.classList.toggle("alive");
}

function ButtonRandomize() {
    //connect to server and send the word randomize
    serverConnection.send(randomize);
}

function ButtonFaster() {
    //connect to server and send the word faster
    serverConnection.send(faster);
}

function ButtonNormal() {
    //connect to server and send the word normal
    serverConnection.send(normal);
}

function ButtonSlower() {
    //connect to server and send the word slower
    serverConnection.send(slower);
}