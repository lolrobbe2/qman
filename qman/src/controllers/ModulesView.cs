using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.Media;
using qman.src.lib;
using qmanlib.src.storage.qdb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace qman.src.controllers
{
    internal class ModulesView : Controller
    {
        private IList<QdbModule>? Modules => GlobalState.currentDb?.Modules;
        private AvaloniaList<QdbModule> FilteredModules = new();
        public override Control? GetContent(ref DockPanel panel)
        {
            // 1. Setup Data
            FilteredModules.Clear();
            FilteredModules.AddRange(Modules ?? []);

            // 2. Initialize the Grid with your 10% / 90% split
            var mainGrid = new Grid
            {
                ColumnDefinitions = new ColumnDefinitions("10*, 90*")
            };

            // 3. Create the components using helper functions
            var sidebar = new DockPanel();
            CreateSearchBar(ref sidebar);
            CreateModuleTree(ref sidebar);

            // 4. Position the sidebar in the Grid
            Grid.SetColumn(sidebar, 0);
            mainGrid.Children.Add(sidebar);

            // 5. Add a placeholder for the right-side content
            var contentArea = new Border { Background = Brushes.Transparent };
            Grid.SetColumn(contentArea, 1);
            mainGrid.Children.Add(contentArea);

            return mainGrid;
        }



        private void CreateSearchBar(ref DockPanel parent)
        {
            var searchBar = new TextBox
            {
                Watermark = "Search modules...",
                Margin = new Thickness(5),
                Height = 30
            };

            searchBar.TextChanged += (s, e) =>
            {
                var text = searchBar.Text?.ToLower() ?? "";
                var results = Modules?.Where(m => m.Location.ToLower().Contains(text));

                FilteredModules.Clear();
                FilteredModules.AddRange(results ?? []);
            };

            // Dock search bar to the top of the sidebar
            DockPanel.SetDock(searchBar, Dock.Top);
            parent.Children.Add(searchBar);
        }

        private void CreateModuleTree(ref DockPanel parent)
        {
            var treeView = new TreeView
            {
                ItemsSource = FilteredModules,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                ItemTemplate = new FuncTreeDataTemplate<QdbModule>(
                    (data, _) => new TextBlock
                    {
                        [!TextBlock.TextProperty] = new Binding(nameof(QdbModule.Location))
                    },
                    data => null)
            };

            // TreeView fills the remaining space in the sidebar
            parent.Children.Add(treeView);
        }
    }
}
