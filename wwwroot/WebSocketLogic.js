"use strict";
let serverConnection;

function connect(){
    let protocol = window.location.protocol === "https:" ? "wss:" : "ws:";
    let uri = protocol + "//" + window.location.host + "/ws";
    
    serverConnection = new WebSocket(uri);

    DisplayLog("WebSocket connecting...");

    serverConnection.onopen = socketConnected;
    serverConnection.onclose = socketClosed;
    serverConnection.onmessage = socketMessage;
    serverConnection.onerror = socketError;
}

function socketClosed(e){
    DisplayLog("WebSocket connection was closed.");
}

function socketConnected(e){
    DisplayLog("WebSocket connected!");
}

function socketMessage(e){
    let response = e.data.split(":");
    switch(response[0]){
        case "board":
            BoardUpdate(response[1]);
            break;
        case "clients":
            DisplayCount(response[1]);
            break;
        case "info":
            DisplayLog(response[1]);
            break;
        default:
            DisplayLog(e.data);
    }
}

function socketError(e){
    DisplayLog("WebSocket error: " + e.data);
}

connect();