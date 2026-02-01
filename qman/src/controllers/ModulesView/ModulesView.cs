using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Styling;
using qman.src.lib;
using qmanlib.src.storage.models;
using qmanlib.src.storage.qdb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace qman.src.controllers.ModulesView
{
    internal class ModulesView : Controller
    {
        private DataTransfer dataTransfer = new DataTransfer();

        private static string filter { get; set; } = "";
        private DockPanel sidebar = new DockPanel();
        public override Control? GetContent(ref DockPanel panel)
        {

            // 2. Initialize the Grid with your 10% / 90% split
            var mainGrid = new Grid
            {
                ColumnDefinitions = new ColumnDefinitions("10*, 90*")
            };

            // 3. Create the components using helper functions
            sidebar = new DockPanel();
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
                Height = 30,
                Text = filter
            };
           

            searchBar.TextChanged += (s, e) =>
            {
                if (filter != (searchBar.Text?.ToLower() ?? ""))
                { 
                    filter = searchBar.Text?.ToLower() ?? "";
                    CreateModuleTree(ref sidebar);
                }


            };

            // Dock search bar to the top of the sidebar
            DockPanel.SetDock(searchBar, Dock.Top);
            parent.Children.Add(searchBar);
        }
        public void DropHandler(object? sender, DragEventArgs drag)
        {
            if(sender is TreeView treeView)
            {
                if (drag.Source is ContentPresenter presenter)
                {
                    if (presenter.Content is PlaceTreeNode node)
                    {
                        Console.WriteLine("hello");
                    }
                }
            }
        }
 
        private async void OnPointerPressed(object? sender, PointerPressedEventArgs e)
        {
            // Check if it's the left mouse button
            if (e.GetCurrentPoint(sender as Visual).Properties.IsLeftButtonPressed)
            {
                // Try to find the node that was clicked

                if (e.Source is Visual visual && sender is TreeView treeView)
                {
                    if (treeView.SelectedItem is PlaceTreeNode node)
                    {
                        dataTransfer.Add(DataTransferItem.CreateText(node.Title));
                        var result = await DragDrop.DoDragDropAsync(e, dataTransfer, DragDropEffects.Move);
                        Console.WriteLine("dragged");
                    }
                }

            }
        }
        private void CreateModuleTree(ref DockPanel parent)
        {
            var treeView = parent.Children.OfType<TreeView>().FirstOrDefault();
            var treeDataSource = new List<PlaceTreeNode> { };

            if (GlobalState.repositories != null)
            {
                Place root = GlobalState.repositories!.Places.GetRoot();
                treeDataSource.Add(new PlaceTreeNode(root, filter));
            }
            if (treeView == null)
            {
                treeView = new TreeView
                {
                    ItemsSource = treeDataSource,
                    // Set the template we built in the previous step
                    ItemTemplate = PlaceTreeNode.GetTemplate()
                   
                };
                // 3. Optional: Auto-expand the root node via a Style
                // This tells every TreeViewItem to bind its expansion to the node's state
                treeView.Styles.Add(new Style(x => x.OfType<TreeViewItem>())
                {
                    Setters = { new Setter(TreeViewItem.IsExpandedProperty, true) }
                });
                // 4. Add it to the DockPanel
                parent.Children.Add(treeView);
            }
            else
            {
                treeView.ItemsSource = treeDataSource;
            }

            // 1. Point to the static Event Definition, then the Method Name
            treeView.AddHandler(InputElement.PointerPressedEvent, OnPointerPressed, RoutingStrategies.Tunnel);
            DragDrop.AddDropHandler(treeView, DropHandler);
            DragDrop.SetAllowDrop(treeView, true);
        }
    }
}
