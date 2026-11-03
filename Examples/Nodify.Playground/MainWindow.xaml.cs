using System;
using System.Windows;

namespace Nodify.Playground
{
    public partial class MainWindow : Window
    {
        private readonly Random _rand = new Random();

        public MainWindow()
        {
            InitializeComponent();

        }

        private int _nextNodeId = 1;

        private void AddNode_Click(object sender, RoutedEventArgs e) => AddNode(NodeKind.Logic);
        private void AddImplementation_Click(object sender, RoutedEventArgs e) => AddNode(NodeKind.Implementation);

        private void AddNode(NodeKind kind)
        {
            var editor = EditorView.EditorInstance;
            var graph = ((PlaygroundViewModel)DataContext).GraphViewModel;
            var offset = graph.Nodes.Count % 5 * 30;
            var node = FlowNodeViewModel.Create(kind, $"{kind} {_nextNodeId++}");
            node.Location = new Point(editor.ViewportLocation.X + editor.ViewportSize.Width * 0.4 - 120 + offset,
                editor.ViewportLocation.Y + editor.ViewportSize.Height * 0.5 - 90 + offset);
            graph.Nodes.Add(node);
            graph.SelectedNode = node;
        }

        private void DeckExample_Click(object sender, RoutedEventArgs e)
        {
            var graph = ((PlaygroundViewModel)DataContext).GraphViewModel;
            var root = DeckBuilderExample.AddTo(graph);
            graph.SelectedNode = root;
            EditorCommands.FitToScreen.Execute(null, EditorView.EditorInstance);
        }

        private void BringIntoView_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is PlaygroundViewModel model)
            {
                NodifyObservableCollection<NodeViewModel> nodes = model.GraphViewModel.Nodes;
                if (nodes.Count == 0) return;
                int index = _rand.Next(nodes.Count);

                if (nodes.Count > index)
                {
                    NodeViewModel node = nodes[index];
                    EditorCommands.BringIntoView.Execute(node.Location, EditorView.Editor);
                }
            }
        }

        private void AnimateConnections_Click(object sender, RoutedEventArgs e)
        {
            EditorSettings.Instance.IsAnimatingConnections = !EditorSettings.Instance.IsAnimatingConnections;
        }
    }
}
