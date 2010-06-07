using System;
using System.ComponentModel.Composition;
using System.Threading;
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
            _timer = new Timer(OnTimerTick, null, 0, 1000);
        }

        private void OnTimerTick(object state)
        {
            SynchronizationHelper.Post(OnItemAdded, new TimelineItemEventArgs(new TimelineItemContainer(new PitchifyReview())));
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