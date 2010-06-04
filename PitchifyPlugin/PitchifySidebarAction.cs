using Seesmic.Sdp.Extensibility;
using Seesmic.Sdp.Utils;

namespace PitchifyPlugin
{
    public class PitchifySidebarAction : SidebarAction
    {
        private ITimeline _timeline;

        public PitchifySidebarAction(ITimeline timeline)
        {
            _timeline = timeline;
            Text = "Pitchify";
        }

        public override void Invoke(SidebarActionContext context)
        {
            context.Space.AddTimelineColumn(this, _timeline, "Pitchify", null);
        }
    }
}