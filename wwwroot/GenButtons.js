function GenButtons(){
    let container = document.querySelector("#GBTag");
    let size = 10;
    if(window.UniverseSize){ // In case we decide to change the size, just set window.UniverseSize to a different value.
        size = window.UniverseSize;
    }
    for(let i = 0; i < size; i++){ // For each row:
        for(let j = 0; j < size; j++){ //For each column:
            // Create Button:
            let newButton = document.createElement("button");
            // For now, set the text of the button as an underscore:
            newButton.innerText = "_";
            // Set its data-* attributes
            newButton.setAttribute("data-x", i);
            newButton.setAttribute("data-y", j);
            // Add the event listener for the click:
            newButton.addEventListener("click", ButtonEventFire);
            // Add it to the container:
            container.appendChild(newButton);
        }
        container.appendChild(document.createElement("br")); // newline break for the next row of buttons.
    }
}

GenButtons();