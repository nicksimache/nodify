using System.Windows;

namespace Nodify.Playground
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            SettingsFile.Load();
            Nodify.Interactivity.EditorGestures.Mappings.Apply(PlaygroundSettings.Instance.EditorInputMode);
            base.OnStartup(e);
        }
    }
}
