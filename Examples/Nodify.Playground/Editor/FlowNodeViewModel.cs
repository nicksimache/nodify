using System.Windows.Controls;
using System.Collections.Generic;
using System.Linq;


namespace Nodify.Playground
{
    public enum NodeKind { Logic, Implementation }

    public class ConnectedNodeInfo
    {
        public string Direction { get; set; } = "";
        public NodeViewModel Node { get; set; } = default!;
    }

    public class FlowNodeViewModel : NodeViewModel
    {
        private NodeKind _kind;
        public NodeKind Kind
        {
            get => _kind;
            set => SetProperty(ref _kind, value);
        }

        private bool _detailsExpanded = true;
        public bool DetailsExpanded { get => _detailsExpanded; set => SetProperty(ref _detailsExpanded, value); }

        private string _implementation = "";
        public string Implementation
        {
            get => _implementation;
            set => SetProperty(ref _implementation, value);
        }
        private string _plannedFiles = "";
        public string PlannedFiles { get => _plannedFiles; set => SetProperty(ref _plannedFiles, value); }
        private string _editedFiles = "";
        public string EditedFiles { get => _editedFiles; set => SetProperty(ref _editedFiles, value); }

        public IEnumerable<ConnectedNodeInfo> ConnectedNodes => Graph.Connections
            .Where(c => c.Input.Node == this || c.Output.Node == this)
            .Select(c => new ConnectedNodeInfo
            {
                Direction = c.Input.Node == this ? "From" : "To",
                Node = c.Input.Node == this ? c.Output.Node : c.Input.Node
            });
        internal void RefreshConnections() => OnPropertyChanged(nameof(ConnectedNodes));

        public static FlowNodeViewModel Create(NodeKind kind, string title)
        {
            var node = new FlowNodeViewModel { Kind = kind, Title = title };
            foreach (var shape in new[] { ConnectorShape.Circle, ConnectorShape.Triangle })
            {
                node.Input.Add(new ConnectorViewModel { Shape = shape, MaxConnections = int.MaxValue });
                node.Output.Add(new ConnectorViewModel { Shape = shape, MaxConnections = int.MaxValue });
            }
            return node;
        }

        private string _text = "";
        public string Text
        {
            get => _text;
            set => SetProperty(ref _text, value);
        }

        private string? _title;
        public string? Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public NodifyObservableCollection<ConnectorViewModel> Input { get; } = new NodifyObservableCollection<ConnectorViewModel>();
        public NodifyObservableCollection<ConnectorViewModel> Output { get; } = new NodifyObservableCollection<ConnectorViewModel>();

        public FlowNodeViewModel()
        {
            Orientation = Orientation.Horizontal;
            Input.WhenAdded(c => c.Node = this)
                 .WhenRemoved(c => c.Disconnect());

            Output.WhenAdded(c => c.Node = this)
                 .WhenRemoved(c => c.Disconnect());
        }

        public void Disconnect()
        {
            Input.Clear();
            Output.Clear();
        }
    }
}
