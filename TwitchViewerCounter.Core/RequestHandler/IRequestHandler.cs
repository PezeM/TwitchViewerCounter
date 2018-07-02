using System.Threading.Tasks;
using TwitchViewerCounter.Core.Models;

namespace TwitchViewerCounter.Core.RequestHandler
{
    public interface IRequestHandler
    {
        TMIRequestResponse GetResponse(string channelName);
        Task<TMIRequestResponse> GetChatterResponseAsync(string channelName);
    }
}
