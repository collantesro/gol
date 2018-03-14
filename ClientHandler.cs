using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

// From http://gunnarpeipman.com/2017/03/aspnet-core-websocket-chat/

namespace gol{
    public class ClientHandler{
        private static ConcurrentDictionary<WebSocket, CancellationToken> allClients = new ConcurrentDictionary<WebSocket, CancellationToken>();
        private readonly RequestDelegate _next;

        public ClientHandler(RequestDelegate next){
            _next = next;
        }

        static ClientHandler(){
            Life.Game.GetInstance().boardChanged = SendBoardToAll;
        }

        public async Task Invoke(HttpContext context){
            if(!context.WebSockets.IsWebSocketRequest){
                await _next.Invoke(context);
                return;
            }
            CancellationToken ct = context.RequestAborted;
            WebSocket connection = await context.WebSockets.AcceptWebSocketAsync();
            allClients.TryAdd(connection, ct);

            // Send the new number of clients:
            SendToAll("clients:" + allClients.Count.ToString());

            try{
                while(true){
                    if (ct.IsCancellationRequested) break;
                    var response = await ReceiveStringAsync(connection, ct);
                    if(string.IsNullOrEmpty(response)){
                        if(connection.State != WebSocketState.Open){
                            break;
                        }
                        continue;
                    }
                    var command = response.Split(":");
                    switch(command[0]){
                        case "randomize":
                            Life.Game.GetInstance().Randomize();
                            SendToAll("info:Randomized Board");
                            break;
                        case "toggle":
                            try{
                                int x = Int32.Parse(command[1]);
                                int y = Int32.Parse(command[2]);

                                Life.Game.GetInstance().ToggleCell(x, y);
                                SendToAll(string.Format("info:Toggled Cell {0},{1}", x, y));
                            } catch(Exception e){
                                await SendStringAsync(connection, "error:" + e.Message, ct);
                            }
                            break;
                        case "faster":
                            Life.Game.GetInstance().MsToSleep -= 250;
                            SendToAll(string.Format("info:Faster Tickrate {0}", Life.Game.GetInstance().MsToSleep));
                            break;
                        case "slower":
                            Life.Game.GetInstance().MsToSleep += 250;
                            SendToAll(string.Format("info:Slower Tickrate {0}", Life.Game.GetInstance().MsToSleep));
                            break;
                        case "normal":
                            Life.Game.GetInstance().MsToSleep = 1_000;
                            SendToAll(string.Format("info:Standard Tickrate {0}", Life.Game.GetInstance().MsToSleep));
                            break;
                        default:
                            continue;
                    }
                }
                await connection.CloseAsync(WebSocketCloseStatus.NormalClosure, "closing", ct);
                connection.Dispose();
            } catch(Exception e){
                Console.Error.WriteLine("Exception in WebSockets ClientHandler: " + e.Message);
            } finally {
                // Socket closed:
                CancellationToken dummy;
                allClients.TryRemove(connection, out dummy);
            }

            // Send the new number of clients:
            SendToAll("clients:" + allClients.Count.ToString());
        }

        private static Task SendStringAsync(WebSocket socket, string data, CancellationToken ct = default(CancellationToken)){
            var buffer = Encoding.UTF8.GetBytes(data);
            var segment = new ArraySegment<byte>(buffer);
            return socket.SendAsync(segment, WebSocketMessageType.Text, true, ct);
        }

        private static async Task<string> ReceiveStringAsync(WebSocket socket, CancellationToken ct = default(CancellationToken)){
            var buffer = new ArraySegment<byte>(new byte[8 * 1024]);
            using(var ms = new MemoryStream()){
                WebSocketReceiveResult result;
                do {
                    ct.ThrowIfCancellationRequested();
                    result = await socket.ReceiveAsync(buffer, ct);
                    ms.Write(buffer.Array, buffer.Offset, result.Count);
                } while(!result.EndOfMessage);

                ms.Seek(0, SeekOrigin.Begin);
                if(result.MessageType != WebSocketMessageType.Text) return null;

                using(var reader = new StreamReader(ms, Encoding.UTF8)){
                    return await reader.ReadToEndAsync();
                }
            }
        }

        private static async void SendToAll(string message){
            if(allClients != null && allClients.Count > 0){
                foreach(var client in allClients.Keys){
                    if(client.State == WebSocketState.Open){
                        await SendStringAsync(client, message, allClients[client]);
                    }
                }
            }
        }

        private static void SendBoardToAll(string board){
            string message = "board:" + board;
            SendToAll(message);
        }
    }
}