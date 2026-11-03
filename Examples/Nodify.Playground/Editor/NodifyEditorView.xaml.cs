using Nodify.Events;
using Nodify.Interactivity;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Data;

namespace Nodify.Playground
{
    public partial class NodifyEditorView : UserControl
    {
        public NodifyEditor EditorInstance => Editor;

        public NodifyEditorView()
        {
            InitializeComponent();
            AddCanvasShortcut(Key.C, nameof(NodifyEditorViewModel.CommentSelectionCommand));
            AddCanvasShortcut(Key.Delete, nameof(NodifyEditorViewModel.DeleteSelectionCommand));

            EditorInstance.ActiveNavigationLayerChanged += DisplayActiveNavigationLayer;
        }

        private void AddCanvasShortcut(Key key, string command)
        {
            var binding = new InputBinding(ApplicationCommands.NotACommand, new CanvasKeyGesture(key));
            BindingOperations.SetBinding(binding, InputBinding.CommandProperty,
                new Binding("DataContext." + command) { Source = EditorInstance });
            EditorInstance.InputBindings.Add(binding);
        }

        // Let text controls receive ordinary typing and editing keys without invoking graph actions.
        private sealed class CanvasKeyGesture : InputGesture
        {
            private readonly Key _key;
            public CanvasKeyGesture(Key key) { _key = key; }

            public override bool Matches(object targetElement, InputEventArgs inputEventArgs)
            {
                if (Keyboard.FocusedElement is TextBoxBase || Keyboard.FocusedElement is PasswordBox)
                    return false;
                return inputEventArgs is KeyEventArgs keyEvent && keyEvent.Key == _key && Keyboard.Modifiers == ModifierKeys.None;
            }
        }

        static NodifyEditorView()
        {
            InputProcessor.Shared<Connector>.ReplaceHandlerFactory<ConnectorState.Connecting>(elem => new CustomConnecting(elem));
            InputProcessor.Shared<Connector>.RegisterHandlerFactory(elem => new RetargetConnections(elem));
        }

        private void SelectFlowNode(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is System.Windows.FrameworkElement element && element.DataContext is FlowNodeViewModel node)
                node.Graph.SelectedNode = node;
        }

        private void Minimap_Zoom(object sender, ZoomEventArgs e)
        {
            EditorInstance.ZoomAtPosition(e.Zoom, e.Location);
        }

        private void DisplayActiveNavigationLayer(KeyboardNavigationLayerId layerId)
        {
            var editorVm = (NodifyEditorViewModel)EditorInstance.DataContext;

            if (layerId == KeyboardNavigationLayerId.Nodes)
            {
                editorVm.KeyboardNavigationLayer = nameof(KeyboardNavigationLayerId.Nodes);
            }
            else if (layerId == KeyboardNavigationLayerId.Connections)
            {
                editorVm.KeyboardNavigationLayer = nameof(KeyboardNavigationLayerId.Connections);
            }
            else if (layerId == KeyboardNavigationLayerId.Decorators)
            {
                editorVm.KeyboardNavigationLayer = nameof(KeyboardNavigationLayerId.Decorators);
            }
            else
            {
                editorVm.KeyboardNavigationLayer = "Custom";
            }
        }
    }
}
