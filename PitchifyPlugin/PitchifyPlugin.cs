using System;
using System.ComponentModel.Composition;
using System.Windows;
using Seesmic.Sdp.Extensibility;

namespace PitchifyPlugin
{
    [Export(typeof(IPlugin))]
    public class PitchifyPlugin : IPlugin
    {
        internal static Guid PluginId = new Guid("bc32900d-a9d0-4ac1-a684-98fb9a4d2c19");
        #region Constructors
        private static readonly ResourceDictionary TemplateResources;

        public PitchifyPlugin() { }
        static PitchifyPlugin()
        {
            TemplateResources = new ResourceDictionary { Source = new Uri("/PitchifyPlugin;component/Templates/DataTemplates.xaml", UriKind.Relative) };
        }
        #endregion

        internal static DataTemplate SmallLogoTemplate
        {
            get
            {
                return (DataTemplate)TemplateResources["SmallLogoTemplate"];
            }
        }

        #region Shell Imports
        private static IShellService _shell;
        private static ILogService _log;
        private static ISessionService _session;

        [Import]
        public IShellService ShellServiceImport { set { _shell = value; } }

        [Import]
        public ISessionService SessionServiceImport { set { _session = value; } }

        [Import]
        public ILogService LogServiceImport { set { _log = value; } }

        public static ISessionService SessionService { get { return _session; } }
        public static IShellService ShellService { get { return _shell; } }
        public static ILogService LogService { get { return _log; } }

        #endregion

        #region IPlugin Implementations
        public void CommitSettings()
        {
            throw new NotImplementedException();
        }

        public Guid Id
        {
            get { return PluginId; }
        }

        public void Initialize()
        {

        }

        public void RevertSettings()
        {
            throw new NotImplementedException();
        }

        public DataTemplate SettingsTemplate
        {
            get { return null; }
        }

        internal static DataTemplate TimelineItemTemplate
        {
            get { return (DataTemplate)TemplateResources["TimelineItemTemplate"]; }
        }

        #endregion

        internal static void LogInfo(string message)
        {
            LogService.Info(string.Format("Plugin: PitchifyPlugin: {0}", message));
        }

        internal static void LogError(Exception ex)
        {
            LogService.Error("Plugin: PitchifyPlugin: ", ex);
        }
    }
}
