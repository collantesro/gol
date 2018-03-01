function ButtonEventFire(ButtonEvent) {
    var b = ButtonEvent.target;
    var x = b.getAttribute("data-x"); 
    var y = b.getAttribute("data-y");
    console.log("x-coordinate: " + x + " y-coordinate: " + y);
}


function ButtonLogic(ButtonEvent) {
    var b = ButtonEvent.target;
    
    if(b.style.background='white') {
        b.style.background='black';
    }
    else if (b.style.background='black') {
        b.style.background='white';
    }
    
    
}