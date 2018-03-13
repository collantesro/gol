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

//GBTag

function BoardUpdate(flatString) {
    var bttns = document.querySelector("#GBTag"); //document.body.childNodes
    var b; 

    var index = 0 

    for(b in bttns) {
        if(b.tagName == "Button") {
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
