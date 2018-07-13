using System.Threading.Tasks;
using TwitchViewerCounter.Core.Models;

namespace TwitchViewerCounter.Core.RequestHandler
{
    public interface IRequestHandler
    {
        ChattersInfo GetResponse(string channelName);
        Task<ChattersInfo> GetChatterResponseAsync(string channelName);
    }
}
