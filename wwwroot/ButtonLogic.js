function ButtonEventFire(ButtonEvent) {
    var b = ButtonEvent.target;
    var row = b.getAttribute("data-row"); 
    var col = b.getAttribute("data-col");
    console.log("Row: " + row + " Column: " + col);

    ButtonLife(b);
}


function ButtonLife(b) {
    //var b = ButtonEvent.target;
    
    if(b.style.background=='white') {
        b.style.backgroundColor='black';
    }
    else  {
        b.style.backgroundColor='white';
    }
}