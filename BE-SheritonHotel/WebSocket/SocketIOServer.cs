using Fleck;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace SWD.SheritonHotel.API.WebSocket
{
    public class SocketIOServer
    {
        private readonly ILogger<SocketIOServer> _logger;
        private readonly List<IWebSocketConnection> _sockets = new();
        private readonly WebSocketServer _server;

        public SocketIOServer(ILogger<SocketIOServer> logger)
        {
            _logger = logger;
            _server = new WebSocketServer("ws://0.0.0.0:8181");
        }

        public void Start()
        {
            _server.Start(socket =>
            {
                socket.OnOpen = () =>
                {
                    _sockets.Add(socket);
                    _logger.LogInformation("Connection opened. Client: {0}", socket.ConnectionInfo.ClientIpAddress);
                };
                socket.OnClose = () =>
                {
                    _sockets.Remove(socket);
                    _logger.LogWarning("Connection closed. Client: {0}", socket.ConnectionInfo.ClientIpAddress);
                };
                socket.OnMessage = message =>
                {
                    _logger.LogInformation("Client says: {0}", message);
                    foreach (var s in _sockets)
                    {
                        s.Send($"Client says: {message}");
                    }
                };
                socket.OnError = ex =>
                {
                    _logger.LogError(ex, "WebSocket error occurred.");
                };
            });
        }

        public void SendMessage(string message)
        {
            foreach (var socket in _sockets.ToList())
            {
                socket.Send(message);
            }
        }
    }
}