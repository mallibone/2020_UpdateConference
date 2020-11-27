using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;

namespace SignalR.Function
{
    //public static class OldWay
    //{
    //    [FunctionName("negotiate")]
    //    public static SignalRConnectionInfo Negotiate(
    //        [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req,
    //        [SignalRConnectionInfo(HubName = "SignalR.Azure.Function.UpdateConference")] SignalRConnectionInfo connectionInfo)
    //    {
    //        return connectionInfo;
    //    }
    //}
}