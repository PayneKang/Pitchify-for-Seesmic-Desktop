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
            var templates = new ResourceDictionary();
            templates.Source = new Uri("/PitchifyPlugin;component/Templates/DataTemplates.xaml", UriKind.Relative);
            TemplateResources = templates;
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
        private static INotificationService _notify;
        private static ILogService _log;
        private static IStorageService _storage;
        private static IPostingService _posting;

        [Import]
        public IPostingService PostingServiceImport { set { _posting = value; } }

        [Import]
        public IShellService ShellServiceImport { set { _shell = value; } }

        [Import]
        public INotificationService NotificationServiceImport { set { _notify = value; } }

        [Import]
        public ILogService LogServiceImport { set { _log = value; } }

        [Import]
        public IStorageService StorageServiceImport { set { _storage = value; } }

        public static IShellService ShellService { get { return _shell; } }
        public static ILogService LogService { get { return _log; } }
        public static INotificationService NotifyService { get { return _notify; } }
        public static IStorageService StorageService { get { return _storage; } }
        public static IPostingService PostingService { get { return _posting; } }
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
            get
            {
                return (DataTemplate)TemplateResources["TimelineItemTemplate"];
            }
        }

        #endregion
    }
}
