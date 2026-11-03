using System;
using System.Linq;
using System.Windows;

namespace Nodify.Playground
{
    /// <summary>A planning example; file paths are suggestions, not files changed by the app.</summary>
    internal static class DeckBuilderExample
    {
        public static FlowNodeViewModel AddTo(NodifyEditorViewModel graph)
        {
            double origin = graph.Nodes.Count == 0 ? 0 : graph.Nodes.Max(n => n.Location.X) + 400;
            FlowNodeViewModel Add(string title, NodeKind kind, int column, double row, string summary, string instructions, string files)
            {
                var node = FlowNodeViewModel.Create(kind, title);
                node.Location = new Point(origin + column * 370, row * 280);
                node.Text = summary;
                node.Implementation = instructions;
                node.PlannedFiles = files.Replace("|", Environment.NewLine);
                graph.Nodes.Add(node);
                return node;
            }
            void Link(FlowNodeViewModel parent, FlowNodeViewModel child)
            {
                var shape = child.Kind == NodeKind.Implementation ? ConnectorShape.Triangle : ConnectorShape.Circle;
                graph.Schema.TryAddConnection(parent.Output.First(p => p.Shape == shape), child.Input.First(p => p.Shape == shape));
            }

            var root = Add("Create Deck builder menu", NodeKind.Logic, 0, 2,
                "Build a menu where players browse cards and assemble a valid deck.",
                "Split the feature into the menu UI and card definitions. Keep card data independent of the UI. Complete when players can inspect cards, add or remove them, validate the deck, and save their selection.",
                "Assets/Scripts/Decks/DeckDefinition.cs|Assets/Scripts/Decks/DeckService.cs");
            var ui = Add("Create the UI", NodeKind.Logic, 1, 0,
                "Card collection, deck slots, filters, and card details.",
                "Create a reusable card tile, a collection grid, deck slots, and a detail panel. Show selection, deck capacity, and validation errors. Bind to the deck service. Verify keyboard navigation and small-screen layouts.",
                "Assets/UI/DeckBuilder/DeckBuilder.uxml|Assets/UI/DeckBuilder/DeckBuilder.uss|Assets/Scripts/UI/DeckBuilderPresenter.cs");
            var cards = Add("Create the cards", NodeKind.Logic, 1, 3,
                "Shared card definitions and the buildings, units, and spells categories.",
                "Define stable card IDs, display names, costs, category, artwork references, and behavior references. Provide a catalog that the UI can query without instantiating gameplay objects.",
                "Assets/Scripts/Cards/CardDefinition.cs|Assets/Scripts/Cards/CardCatalog.cs");
            var buildings = Add("Create buildings", NodeKind.Logic, 2, 0,
                "Stationary cards with placement rules and a lifetime.",
                "Build the common building lifecycle: validate placement, spawn, acquire targets, update lifetime, and clean up. Expose shared stats through card data.",
                "Assets/Scripts/Buildings/BuildingController.cs");
            var units = Add("Create units", NodeKind.Logic, 2, 3,
                "Moving combat units with targeting, attacks, and health.",
                "Provide reusable movement, targeting, health, and attack components. Individual unit definitions configure those components. Keep presentation separate from combat state.",
                "Assets/Scripts/Units/UnitController.cs|Assets/Scripts/Units/UnitDefinition.cs");
            var spells = Add("Create spells", NodeKind.Logic, 2, 6,
                "Targeted effects with range, area, and duration rules.",
                "Create targeting previews and validate casts before spending resources. Resolve effects through shared damage and status-effect services. Keep effect strength configurable.",
                "Assets/Scripts/Spells/SpellDefinition.cs|Assets/Scripts/Spells/SpellResolver.cs");
            Link(root, ui); Link(root, cards);
            Link(cards, buildings); Link(cards, units); Link(cards, spells);

            var cannon = Add("Cannon", NodeKind.Implementation, 3, 0,
                "A stationary defensive building.",
                "1. Define cost, health, lifetime, range, and attack interval in card data.\n2. Create a building prefab using the shared lifecycle.\n3. Acquire valid ground targets and fire through the damage service.\n4. Release targets and destroy the object on death or expiry.\nAcceptance: invalid placements fail; the cannon does not chase targets or attack outside its range.",
                "Assets/Cards/Buildings/Cannon.asset|Assets/Prefabs/Buildings/Cannon.prefab|Assets/Scripts/Buildings/CannonAttack.cs");
            var wizard = Add("Ice Wizard", NodeKind.Implementation, 3, 2,
                "A ranged unit whose attacks apply a slowing effect.",
                "1. Create a unit definition with configurable health, range, attack cadence, and movement speed.\n2. Add a projectile attack with a configurable impact radius.\n3. Apply damage and a timed slow through shared effect services.\n4. Define refresh and stacking rules explicitly.\nAcceptance: slow expires correctly and repeated hits do not leave a permanent modifier.",
                "Assets/Cards/Units/IceWizard.asset|Assets/Prefabs/Units/IceWizard.prefab|Assets/Scripts/Combat/SlowEffect.cs");
            var giant = Add("Giant", NodeKind.Implementation, 3, 3,
                "A durable melee unit focused on buildings.",
                "1. Configure the unit's health, movement, attack range, and damage.\n2. Filter target selection to buildings.\n3. Navigate to a valid attack position and stop to attack.\n4. Reacquire a target when the current building is destroyed.\nAcceptance: nearby units do not divert its target selection; destroyed targets never stall movement.",
                "Assets/Cards/Units/Giant.asset|Assets/Prefabs/Units/Giant.prefab|Assets/Scripts/Units/BuildingTargetSelector.cs");
            var skeletons = Add("Skeleton Army", NodeKind.Implementation, 3, 4,
                "A card that deploys a group of small melee units.",
                "1. Configure group size and formation offsets in card data.\n2. Find valid spawn positions around the selected point.\n3. Spawn individual skeletons with independent health and targeting.\n4. Use pooled instances and clean up each unit independently.\nAcceptance: the card spends its cost once, spawn positions avoid blocked space, and all spawned units can be removed cleanly.",
                "Assets/Cards/Units/SkeletonArmy.asset|Assets/Prefabs/Units/Skeleton.prefab|Assets/Scripts/Units/FormationSpawner.cs");
            var fireball = Add("Fireball", NodeKind.Implementation, 3, 6,
                "An area-damage spell with a targeting preview.",
                "1. Configure cost, travel time, radius, and damage in spell data.\n2. Display the target radius before casting.\n3. Resolve damage once on impact and spawn presentation effects.\n4. Return pooled effects after playback.\nAcceptance: canceled casts spend nothing and each target in the area receives only one impact.",
                "Assets/Cards/Spells/Fireball.asset|Assets/Prefabs/Spells/Fireball.prefab|Assets/Scripts/Spells/AreaDamageEffect.cs");
            Link(buildings, cannon); Link(units, wizard); Link(units, giant); Link(units, skeletons); Link(spells, fireball);
            return root;
        }
    }
}
