# Playground settings

Edit `playground.settings.json` beside the executable and restart the app. The repository copy is copied to the output folder when building (PreserveNewest); edit the repository copy for changes you want to keep in source control. Omitted settings retain the playground defaults. Invalid entries are reported at startup.

Use JSON booleans and numbers, enum names as strings, and points as strings such as `"2000,2000"`. The settings panel and status footer are intentionally absent. Settings still use the original playground models.

All writable properties below are supported, including advanced settings. Their defaults and enum types are defined in `PlaygroundSettings.cs` and `EditorSettings.cs`.

## Playground

| Key | Type |
| --- | --- |
| `EditorGesturesMappings` | `EditorGesturesMappings` |
| `EditorInputMode` | `EditorInputMode` |
| `ShowMinimap` | `bool` |
| `DisableMinimapControls` | `bool` |
| `ResizeToViewport` | `bool` |
| `MinimapMaxViewportOffset` | `PointEditor` |
| `ShouldConnectNodes` | `bool` |
| `AsyncLoading` | `bool` |
| `MinNodes` | `uint` |
| `MaxNodes` | `uint` |
| `MinConnectors` | `uint` |
| `MaxConnectors` | `uint` |
| `PerformanceTestNodes` | `uint` |
| `ShowGridLines` | `bool` |
| `UseCustomConnectors` | `bool` |

## Editor

| Key | Type |
| --- | --- |
| `EnablePendingConnectionSnapping` | `bool` |
| `EnablePendingConnectionPreview` | `bool` |
| `AllowConnectingToConnectorsOnly` | `bool` |
| `EnableRealtimeSelection` | `bool` |
| `DisableAutoPanning` | `bool` |
| `AutoPanningSpeed` | `double` |
| `AutoPanningEdgeDistance` | `double` |
| `DisablePanning` | `bool` |
| `DisableZooming` | `bool` |
| `GridSpacing` | `uint` |
| `MinZoom` | `double` |
| `MaxZoom` | `double` |
| `Zoom` | `double` |
| `Location` | `PointEditor` |
| `SelectableConnections` | `bool` |
| `CanSelectMultipleConnections` | `bool` |
| `DraggableNodes` | `bool` |
| `SelectableNodes` | `bool` |
| `CanSelectMultipleNodes` | `bool` |
| `ConnectionStyle` | `ConnectionStyle` |
| `ConnectionText` | `string?` |
| `CircuitConnectionAngle` | `double` |
| `ConnectionCornerRadius` | `double` |
| `ConnectionSpacing` | `double` |
| `ConnectionSourceOffsetMode` | `ConnectionOffsetMode` |
| `ConnectionTargetOffsetMode` | `ConnectionOffsetMode` |
| `ArrowHeadEnds` | `ArrowHeadEnds` |
| `ArrowHeadShape` | `ArrowHeadShape` |
| `ConnectionSourceOffset` | `PointEditor` |
| `ConnectionTargetOffset` | `PointEditor` |
| `ConnectionStrokeThickness` | `double` |
| `ConnectionOutlineThickness` | `double` |
| `ConnectionFocusVisualPadding` | `double` |
| `DirectionalArrowsCount` | `uint` |
| `DirectionalArrowsOffset` | `double` |
| `IsAnimatingConnections` | `bool` |
| `DirectionalArrowsAnimationDuration` | `double` |
| `ConnectionArrowSize` | `PointEditor` |
| `DisplayConnectionsOnTop` | `bool` |
| `BringIntoViewSpeed` | `double` |
| `BringIntoViewMaxDuration` | `double` |
| `GroupingNodeMovement` | `GroupingMovementMode` |
| `MaxHotKeys` | `uint` |
| `HotKeysDisplayMode` | `HotKeysDisplayMode` |
| `PreserveSelectionOnRightClick` | `bool` |
| `MouseActionSuppressionThreshold` | `double` |
| `AutoPanningTickRate` | `double` |
| `AllowMinimapPanningCancellation` | `bool` |
| `AllowCuttingCancellation` | `bool` |
| `AllowPushItemsCancellation` | `bool` |
| `AllowPanningCancellation` | `bool` |
| `AllowSelectionCancellation` | `bool` |
| `AllowDraggingCancellation` | `bool` |
| `AllowPendingConnectionCancellation` | `bool` |
| `EnableSnappingCorrection` | `bool` |
| `EnableCuttingLinePreview` | `bool` |
| `EnablePendingConnectionHitTesting` | `bool` |
| `EnableConnectorOptimizations` | `bool` |
| `OptimizeSafeZone` | `double` |
| `OptimizeMinimumSelectedItems` | `uint` |
| `EnableRenderingOptimizations` | `bool` |
| `OptimizeRenderingMinimumNodes` | `uint` |
| `OptimizeRenderingZoomOutPercent` | `double` |
| `FitToScreenExtentMargin` | `double` |
| `EnableDraggingOptimizations` | `bool` |
| `EnableStickyConnectors` | `bool` |
| `EnableToggledPanning` | `bool` |
| `EnableToggledCutting` | `bool` |
| `EnableToggledPushingItems` | `bool` |
| `EnableToggledSelecting` | `bool` |
| `EnableToggledDragging` | `bool` |
| `EnableMinimapToggledPanning` | `bool` |
| `AllowPanningWhileSelecting` | `bool` |
| `AllowPanningWhileCutting` | `bool` |
| `AllowPanningWhilePushingItems` | `bool` |
| `AllowZoomingWhileSelecting` | `bool` |
| `AllowZoomingWhileCutting` | `bool` |
| `AllowZoomingWhilePushingItems` | `bool` |
| `AllowZoomingWhilePanning` | `bool` |

## Mouse controls

With the Default mapping, drag empty canvas with the left, middle, or right mouse button to pan. Drag a node header to move it. Shift-drag empty canvas to box-select, or Ctrl+Shift-drag to add to the selection. Scroll to zoom.

Every created node has a circle and triangle input on the left and matching outputs on the right, stacked beneath its text. Connections require matching shapes. The C and Delete canvas shortcuts are ignored while editing text.
