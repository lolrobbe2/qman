using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Layout;
using Avalonia.Markup.Xaml.Templates;
using Avalonia.Media;
using qman.src.lib;
using qmanlib.src.storage.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace qman.src.controllers.OutputsView
{
    internal class OutputsView : Controller
    {
        private qmanlib.src.storage.models.Module? current => GlobalState.currentSelected;

        public override Control? GetContent(ref DockPanel panel)
        {
            // 1. Handle Null State (No Module Selected)
            if (current == null || GlobalState.repositories == null)
            {
                var emptyState = new OutputControl(0, null);
                emptyState.PointerPressed += (s, e) => AddModule();

                // Return centered in a single-cell grid
                return new Grid { Children = { emptyState } };
            }

            // 2. Fetch Data
            IList<Output> outputs = GlobalState.repositories!.Outputs.GetModuleOutputs(current).ToArray();

            // 3. Create the Main Grid with Row Definitions
            var mainGrid = new Grid
            {
                // Row 0: Auto (takes size of label)
                // Row 1: * (takes all remaining vertical space)
                RowDefinitions = new RowDefinitions("Auto, *"),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch
            };

            // Row 0: Module Label
            var label = new TextBlock
            {
                Text = current.Location ?? "Unknown Location", // Handle potential nulls
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(10, 5),
                FontSize = 16,
                FontWeight = FontWeight.Bold
            };
            
            Grid.SetRow(label, 0);

            // Row 1: The Horizontal Outputs List
            var scrollViewer = new ScrollViewer
            {
                HorizontalScrollBarVisibility = ScrollBarVisibility.Auto,
                VerticalScrollBarVisibility = ScrollBarVisibility.Disabled,
                // Ensure the scrollviewer itself stretches to fill the Grid cell
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Content = new ItemsControl
                {
                    // VerticalAlignment Stretch ensures the cards can be as tall as the row
                    VerticalAlignment = VerticalAlignment.Stretch,
                    ItemsPanel = new FuncTemplate<Panel?>(() => new StackPanel
                    {
                        Orientation = Orientation.Horizontal,
                        Spacing = 8,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Stretch // Allow items to fill height
                    }),
                    ItemsSource = CreateOutputControls(outputs)
                }
            };
            Grid.SetRow(scrollViewer, 1);

            // Add children to grid
            mainGrid.Children.Add(label);
            mainGrid.Children.Add(scrollViewer);

            return mainGrid;
        }
        private List<OutputControl> CreateOutputControls(IList<Output> outputs)
        {
            var controls = new List<OutputControl>();

            for (int i = 0; i < outputs.Count; i++)
            {
                controls.Add(new OutputControl(i + 1, outputs[i]));
            }

            // The "Add Output" button (last in the list)
            var addButton = new OutputControl(outputs.Count + 1, null);
            addButton.PointerPressed += (s, e) => AddOutput();
            controls.Add(addButton);

            return controls;
        }

        private void AddModule()
        {
            // Your logic for adding a new module
            Console.WriteLine("Callback: AddModule triggered");
        }

        private void AddOutput()
        {
            // Your logic for adding a new output to the current module
            Console.WriteLine("Callback: AddOutput triggered");
        }
    }
}
