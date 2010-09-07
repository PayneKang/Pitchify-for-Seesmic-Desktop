using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using Seesmic.Sdp.Extensibility;
using Seesmic.Sdp.Utils;

namespace PitchifyPlugin
{
    public class PitchifyReview : ITimelineItem
    {
        public PitchifyReview()
        {
            Text = "Foobar";
        }

        public string Id { get; set; }

        public string Username { get; set; }

        public string Text { get; set; }

        public Uri SpotifyUri { get; set; }

        public Uri DetailsUri { get; set; }

        public Uri AvatarUri { get; set; }

        public DataTemplate Template { get { return PitchifyPlugin.TimelineItemTemplate; } }

        public DataTemplate NotificationTemplate { get { return null; } }

        public DateTimeOffset DateTime { get; set; }

        public string DateTimeText
        {
            get { return "Added " + MetadataControl.DateTimeToFriendlyString(DateTime.DateTime, true); }
        }

        public GeoLocation GeoLocation { get { return null; } }
    }
}