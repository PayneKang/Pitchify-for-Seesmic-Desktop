using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Net;
using System.Threading;
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
            var client = new WebClient();
            var uri = new Uri(@"http://feeds.feedburner.com/Pitchify");

            client.OpenReadAsync(uri);
            client.OpenReadCompleted += FeedRetrieved;
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
                                  Id = GetGuidFromEntryId(entry.Element("guid").Value),
                                  Username = entry.Element("title").Value,
                                  AvatarUri = new Uri(entry.Element(a10 + "link").Attribute("href").Value),
                                  DateTime = DateTime.Parse(entry.Element(a10 + "updated").Value)
                              };

                foreach (var pitchifyReview in entries.First())
                {
                    PitchifyPlugin.LogInfo("Id: " + pitchifyReview.Id);
                    PitchifyPlugin.LogInfo("Username: " + pitchifyReview.Username);
                    PitchifyPlugin.LogInfo("Image: " + pitchifyReview.AvatarUri);
                    PitchifyPlugin.LogInfo("Datetime: " + pitchifyReview.DateTime.ToString());
                    SynchronizationHelper.Post(OnItemAdded, new TimelineItemEventArgs(new TimelineItemContainer(pitchifyReview)));
                }
            }
            catch (Exception ex)
            {
                PitchifyPlugin.LogError(ex);
            }
        }

        private string GetGuidFromEntryId(string entryId)
        {
            var guid = Guid.NewGuid().ToString();
            return new Guid(guid.Substring(0, "bc32900d-a9d0-4ac1-a684-98fb9a4d2c19".Length - entryId.Length) + entryId).ToString();
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