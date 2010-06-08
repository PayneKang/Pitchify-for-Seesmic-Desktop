using System;
using System.Windows;
using Seesmic.Sdp.Extensibility;

namespace PitchifyPlugin
{
    internal class PitchifyReview : ITimelineItem
    {
        public PitchifyReview()
        {
            Text = "Foobar";
        }

        public string Id { get; set; }

        public string Username { get; set; }

        public string Text { get; set; }

        public Uri AvatarUri { get; set; }

        public DataTemplate Template
        {
            get
            {
                PitchifyPlugin.LogInfo("GetTemplate");
                return PitchifyPlugin.TimelineItemTemplate;
            }
        }

        public DataTemplate NotificationTemplate { get { return null; } }

        public DateTimeOffset DateTime { get; set; }

        public GeoLocation GeoLocation { get { return null; } }
    }
}