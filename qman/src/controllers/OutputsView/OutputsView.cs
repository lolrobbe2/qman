using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.Markup.Xaml.Templates;
using Avalonia.Media;
using qman.src.lib;
using qmanlib.src.storage.models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
            // 1. Handle Null State
            if (current == null || GlobalState.repositories == null)
            {
                var emptyState = new OutputControl(0, null)
                {
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Stretch
                };
                emptyState.PointerPressed += (s, e) => AddModule();

                // Ensure the empty state is docked so it doesn't overlap
                DockPanel.SetDock(emptyState, Dock.Right);
                return emptyState;
            }

            // 2. Fetch Data
            IList<Output?> outputs = GlobalState.repositories!.Outputs.GetModuleOutputs(current).ToArray();

            // 3. Main Container (Grid with Row Definitions)
            var localGrid = new Grid
            {
                // Row 0: Label (Auto), Row 1: Content (*)
                RowDefinitions = new RowDefinitions("Auto, *"),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch
            };

            // Dock this whole grid to the right side of the parent DockPanel
            DockPanel.SetDock(localGrid, Dock.Right);

            // 4. Create Label (Row 0)
            var label = new TextBlock
            {
                Text = current.Location ?? "Unknown Location",
                Margin = new Thickness(10, 5),
                FontSize = 16,
                FontWeight = FontWeight.Bold,
            };
            Grid.SetRow(label, 0);
            localGrid.Children.Add(label);

            // 5. Create the ItemsControl using a UniformGrid (Row 1)
            // No ScrollViewer used here as requested.
            var itemsControl = new ItemsControl
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                ItemsPanel = new FuncTemplate<Panel?>(() => new UniformGrid
                {
                    Rows = 1, // Keep everything in one horizontal row
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Stretch
                }),
                ItemsSource = CreateOutputControls(outputs)
            };
            Grid.SetRow(itemsControl, 1);
            localGrid.Children.Add(itemsControl);

            return localGrid;
        }
        private List<OutputControl> CreateOutputControls(IList<Output?> outputs)
        {
            var controls = new List<OutputControl>();

            for (int i = 0; i < outputs.Count; i++)
            {
                Output? output = outputs[i];
                var control = new OutputControl(i, output);
                if(output != null)
                    control.PointerPressed += (s, e) => AddOutput();

                controls.Add(control);
            }
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
