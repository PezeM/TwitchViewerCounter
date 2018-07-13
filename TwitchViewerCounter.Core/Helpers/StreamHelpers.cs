using System.Collections.Generic;
using TwitchViewerCounter.Core.Models;

namespace TwitchViewerCounter.Core.Helpers
{
    public static class StreamHelpers
    {
        public static bool IsLiveOnline(Stream live)
        {
            return live.StreamType == "live";
        }

        public static FeaturedStreamInfo CheckIfStreamIsFeatured(Stream streamInfo, List<FeaturedStreamInfo> featured)
        {
            if (streamInfo == null || featured == null)
                return null;

            foreach (var featuredInfo in featured)
            {
                if (featuredInfo.Stream.Channel.Id == streamInfo.Channel.Id)
                    return featuredInfo;
            }

            return null;
        }
    }
}
