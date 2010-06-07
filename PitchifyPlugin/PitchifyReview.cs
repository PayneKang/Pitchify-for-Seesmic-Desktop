using System;
using System.Windows;
using Seesmic.Sdp.Extensibility;

namespace PitchifyPlugin
{
    internal class PitchifyReview : ITimelineItem
    {
        public PitchifyReview()
        {
            Id = Guid.NewGuid().ToString();
            Text = "Pitchify";
        }

        public string Id { get; set; }

        public string Username { get; set; }

        public string Text { get; set; }

        public Uri AvatarUri { get { return null; } }

        public DataTemplate Template { get { return PitchifyPlugin.TimelineItemTemplate; } }

        public DataTemplate NotificationTemplate { get { return null; } }

        public DateTimeOffset DateTime { get; set; }

        public GeoLocation GeoLocation { get { return null; } }
    }
}