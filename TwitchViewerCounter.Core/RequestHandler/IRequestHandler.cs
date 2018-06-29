using TwitchViewerCounter.Core.Models;

namespace TwitchViewerCounter.Core.RequestHandler
{
    public interface IRequestHandler
    {
        TMIRequestResponse GetChatterResponse(string channelName);
    }
}
