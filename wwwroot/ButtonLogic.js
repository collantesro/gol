function ButtonEventFire(ButtonEvent) {
    var b = ButtonEvent.target;
    var row = b.getAttribute("data-row"); 
    var col = b.getAttribute("data-col");
    console.log("Row: " + row + " Column: " + col);
    serverConnection.send("toggle:" + row + ":" + col);
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
//GBTag

function BoardUpdate(flatString) {
    var bttns = document.querySelector("#GBTag"); //document.body.childNodes
    var b; 

    var index = 0 

<<<<<<< HEAD
    for(b in bttns) {
        if(b.tagName == "BUTTON") {
=======
    for(b = 0; b < bttns.length ; b++ ) {
        if(bttns[b].tagName == "BUTTON") {
>>>>>>> 31cf599025f5b77597b243e7017a32d416345586
            if(flatString.charAt(index) == "X") {
                b.classList.add("alive"); 
            }
            else {
                b.classList.remove("alive"); 
            }
            index++; 
        }
        else {
            continue; 
        }
    }
}
