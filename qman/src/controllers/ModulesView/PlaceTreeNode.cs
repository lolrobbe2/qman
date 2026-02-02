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
        public bool IsPlace { get; set; }
        public Int16 NodeId { get; set; } = -1;
        public string Title { get; init; }
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
                // 1. The Children Selector (Where to find the next level)
                (node, _) => {
                    var panel = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 8 };

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

                    panel.Children.Add(new TextBlock
                    {
                        Text = node.Title,
                        VerticalAlignment = VerticalAlignment.Center
                    });

                    return panel;
                },

                // 2. The Content Builder (The Visuals)
                (item) => item.Children
                
            );
        }
    }
}
