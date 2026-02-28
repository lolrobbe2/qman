using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Markup.Xaml.Templates;
using Avalonia.Media;
using qmanlib.src.storage.models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Xml.Linq;

namespace qman.src.controllers.ModulesView
{
    internal class PlaceTreeNode : Control
    {
        public static readonly StyledProperty<bool> IsEditingProperty =
        AvaloniaProperty.Register<PlaceTreeNode, bool>(nameof(IsEditing));

        public bool IsEditing
        {
            get => GetValue(IsEditingProperty);
            set => SetValue(IsEditingProperty, value);
        }
        public bool IsPlace { get; set; }
        public Int16 NodeId { get; set; } = -1;
        public string Title { get; set; }
        List<PlaceTreeNode> Children { get; set; } = new List<PlaceTreeNode>();

        public PlaceTreeNode(Place place, string filter)
        {
            Title = place.Name;
            IsPlace = true;
            NodeId = place.ID;
            var childPlaces = GlobalState.repositories!.Places.GetChildren(place);
            foreach (Place item in childPlaces)
            {
                Children.Add(new PlaceTreeNode(item,filter));
            }

            var childModules = GlobalState.repositories!.Modules.GetModulesByPlace(place);
            foreach (Module item in childModules)
            {
                if (filter == ""  || item.Location.Contains(filter, StringComparison.OrdinalIgnoreCase))
                    Children.Add(new PlaceTreeNode(item));
            }
        }
        public PlaceTreeNode(Module module)
        {
            IsPlace = false;
            Title = module.Location;
            NodeId = module.Id;
        }


        public static IDataTemplate GetTemplate()
        {
            return new FuncTreeDataTemplate<PlaceTreeNode>(
                (node, _) => {
                    var panel = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 8 };

                    // Keep your icon exactly as it was
                    string path = !node.IsPlace
                        ? "M12,2L4.5,20.29L5.21,21L12,18L18.79,21L19.5,20.29L12,2Z"
                        : "M10,20V14H14V20H19V12H22L12,3L2,12H5V20H10Z";

                    panel.Children.Add(new PathIcon
                    {
                        Data = StreamGeometry.Parse(path),
                        Width = 14,
                        Height = 14,
                        Foreground = !node.IsPlace ? Brushes.Gray : Brushes.SteelBlue
                    });

                    var textDisplay = new TextBlock
                    {
                        Text = node.Title,
                        VerticalAlignment = VerticalAlignment.Center
                    };

                    var editInput = new TextBox
                    {
                        Text = node.Title,
                        VerticalAlignment = VerticalAlignment.Center,
                        IsVisible = false
                    };

                    // Logic to swap visibility based on the StyledProperty
                    // This replaces the .Subscribe that was giving you the error
                    node.PropertyChanged += (s, e) =>
                    {
                        if (e.Property == PlaceTreeNode.IsEditingProperty)
                        {
                            bool isEditing = node.IsEditing;
                            textDisplay.IsVisible = !isEditing;
                            editInput.IsVisible = isEditing;

                            if (isEditing)
                            {
                                editInput.Text = node.Title; // Sync text when starting edit
                                editInput.Focus();
                            } else
                            {
                                if (!node.IsPlace) GlobalState.repositories?.Modules.SetModuleName(node.NodeId, editInput.Text);

                            }
                        } 
                    };

                    editInput.LostFocus += (s, e) =>
                    {
                        // If the user clicks away, we stop editing.
                        // You can decide here if you want to save or cancel.
                        node.Title = editInput.Text;
                        textDisplay.Text = editInput.Text;
                        node.IsEditing = false;
                    };

                    editInput.KeyDown += (s, e) => {
                        if (e.Key == Key.Enter)
                        {
                            node.Title = editInput.Text;
                            textDisplay.Text = editInput.Text;
                            node.IsEditing = false;
                        }
                        else if (e.Key == Key.Escape)
                        {
                            node.IsEditing = false;

                        }
                    };

                    panel.Children.Add(textDisplay);
                    panel.Children.Add(editInput);

                    return panel;
                },
                (item) => item.Children
            );
        }
    }
}
