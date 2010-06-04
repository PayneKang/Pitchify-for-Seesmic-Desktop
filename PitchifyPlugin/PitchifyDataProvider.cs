using System;
using System.ComponentModel.Composition;
using Seesmic.Sdp.Extensibility;

namespace PitchifyPlugin
{
    [Export(typeof(IDataProvider))]
    public class PitchifyDataProvider : IDataProvider
    {
        private ISidebarAction _sidebarAction;

        public void OnItemProcessed(ProcessingResult result)
        {

        }

        public event EventHandler<TimelineItemEventArgs> ItemAdded;
        public event EventHandler<TimelineItemEventArgs> ItemRemoved;
        public void Start()
        {

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
            get { return _sidebarAction ?? (_sidebarAction = new PitchifySidebarAction()); }
        }
    }
}