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

<<<<<<< HEAD
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
=======
//GBTag

function BoardUpdate(flatString) {
    var bttns = document.querySelector("#GBTag").childNodes(); //document.body.childNodes
    var b; 

    var index = 0 

    for(b = 0; b < bttns.length ; b++ ) {
        if(b.tagName == "BUTTON") {
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
>>>>>>> 802e666007192888c1db8eef5ef36f1c254c39fd
