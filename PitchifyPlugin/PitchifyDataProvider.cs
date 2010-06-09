using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Browser;
using System.Xml.Linq;
using Seesmic.Sdp.Extensibility;
using Seesmic.Sdp.Utils;

namespace PitchifyPlugin
{
    [Export(typeof(IDataProvider))]
    public class PitchifyDataProvider : IDataProvider
    {
        private ISidebarAction _sidebarAction;
        private Timeline _timeline;
        private Timer _timer;

        public event EventHandler<TimelineItemEventArgs> ItemAdded;
        public event EventHandler<TimelineItemEventArgs> ItemRemoved;
        public void Start()
        {
            _timeline = new Timeline();
            _timer = new Timer(OnTimerTick, null, 0, 1800000);
        }

        private void OnTimerTick(object state)
        {
            try
            {
                var client = new WebClient();
                var uri = new Uri(@"http://feeds.feedburner.com/Pitchify");

                client.OpenReadAsync(uri);
                client.OpenReadCompleted += FeedRetrieved;
            }
            catch (Exception ex)
            {
                PitchifyPlugin.LogError(ex);
            }
        }

        private void FeedRetrieved(object sender, OpenReadCompletedEventArgs e)
        {
            var stream = e.Result;
            var xml = XDocument.Load(stream);

            XNamespace a10 = "http://www.w3.org/2005/Atom";

            try
            {
                var entries = from entry in xml.Descendants("item")
                              select new PitchifyReview
                              {
                                  Id = "pitchify-" + entry.Element("guid").Value,
                                  Username = entry.Element("title").Value,
                                  Text = GetTextFromEntry(entry.Element("description").Value),
                                  SpotifyUri = GetSpotifyLink(entry.Element("description").Value),
                                  AvatarUri = new Uri(entry.Element(a10 + "link").Attribute("href").Value),
                                  DateTime = DateTime.Parse(entry.Element(a10 + "updated").Value)
                              };

                foreach (var pitchifyReview in entries)
                {
                    SynchronizationHelper.Post(OnItemAdded, new TimelineItemEventArgs(new TimelineItemContainer(pitchifyReview)));
                }
            }
            catch (Exception ex)
            {
                PitchifyPlugin.LogError(ex);
            }
        }

        private string GetTextFromEntry(string description)
        {
            description = StripHtmlTags(description);

            var text = new StringBuilder();
            text.AppendFormat("{0}\n", GetAverageRating(description));
            text.AppendFormat("Available in: \n{0}", GetAvailability(description));
            return text.ToString();
        }

        private Regex spotifyLinkRegex = new Regex(@"http://open.spotify.com/\w+/\w+");
        private Uri GetSpotifyLink(string description)
        {
            Match link = spotifyLinkRegex.Match(description);
            var uri = new Uri(link.Success ? link.Value : string.Empty);
            return uri;
        }

        private Regex availabilityRegex = new Regex(@"\(.*\)");
        private string GetAvailability(string description)
        {
            string availability = "All countries";
            if (description.Contains("Restricted availability"))
            {
                Match match = availabilityRegex.Match(description);
                availability = match.Success ? match.Value.Substring(1, match.Value.Length - 2) : "Failed to parse restricted availability";
            }
            return availability;
        }

        private Regex averageRatingRegex = new Regex("Average rating: [0-9.]+");
        private string GetAverageRating(string description)
        {
            Match averageRating = averageRatingRegex.Match(description);

            return averageRating.Success
                ? averageRating.Value
                : "Failed to parse average rating";
        }

        private string StripHtmlTags(string value)
        {
            int length = 0;
            int.TryParse(value, out length);
            string formattedValue = Regex.Replace(value as string, "<.*?>", "");
            formattedValue = Regex.Replace(formattedValue, @"\n+\s+", "\n\n");
            formattedValue = formattedValue.TrimStart(' ');
            formattedValue = HttpUtility.HtmlDecode(formattedValue);
            if (length > 0 && formattedValue.Length >= length)
                formattedValue = formattedValue.Substring(0, length - 1);
            return formattedValue;
        }

        private void OnItemAdded(TimelineItemEventArgs e)
        {
            var handler = ItemAdded;

            if (handler != null)
                handler(this, e);
        }

        public void OnItemProcessed(ProcessingResult result)
        {
            if (result.Blocked == false && result.Error == null)
            {
                _timeline.Add(result.TimelineItemContainer);
            }
        }

        public void Stop()
        {

        }

        public Guid Id
        {
            get { return PitchifyPlugin.PluginId; }
        }

        public ISidebarAction SidebarAction
        {
            get { return _sidebarAction ?? (_sidebarAction = new PitchifySidebarAction(_timeline)); }
        }
    }
}