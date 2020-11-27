using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace SignalR.Function
{
    public class SignalRChatDemo : ServerlessHub
    {
        [FunctionName("negotiate")]
        public SignalRConnectionInfo Negotiate([HttpTrigger(AuthorizationLevel.Anonymous)] HttpRequest req)
        {
            var userId = req.Headers.ContainsKey("x-ms-signalr-user-id") ? req.Headers["x-ms-signalr-user-id"] : default;
            var claims = req.Headers.ContainsKey("Authorization") ? GetClaims(req.Headers["Authorization"]) : default;
            return Negotiate(userId, claims);
        }

        [FunctionName(nameof(SendMessage))]
        public async Task SendMessage([SignalRTrigger] InvocationContext invocationContext, string message, ILogger logger)
        {
            logger.LogInformation($"{invocationContext.ConnectionId} sent message: {message}");
            await Clients.All.SendAsync("NewMessage", message);
        }
    }
}
