﻿using RestSharp.Deserializers;
using System;

namespace TwitchViewerCounter.Core.Models
{
    public class Stream
    {
        [DeserializeAs(Name = "_id")]
        public long Id { get; set; }

        [DeserializeAs(Name = "game")]
        public string Game { get; set; }

        [DeserializeAs(Name = "viewers")]
        public int Viewers { get; set; }

        [DeserializeAs(Name = "created_at")]
        public DateTimeOffset LiveStartedAt { get; set; }

        [DeserializeAs(Name = "stream_type")]
        public string StreamType { get; set; }

        [DeserializeAs(Name = "channel")]
        public Channel Channel { get; set; }
    }
}
