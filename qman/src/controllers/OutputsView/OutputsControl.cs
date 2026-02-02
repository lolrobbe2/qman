using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Reactive;
using qmanlib.src.storage.models;


namespace qman.src.controllers.OutputsView
{

    public class OutputControl : UserControl
    {
        // Styled Properties for Data Binding
        public static readonly StyledProperty<Output?> OutputProperty =
            AvaloniaProperty.Register<OutputControl, Output?>(nameof(Output));

        public static readonly StyledProperty<int> IndexProperty =
            AvaloniaProperty.Register<OutputControl, int>(nameof(Index));

        public Output? Output
        {
            get => GetValue(OutputProperty);
            set => SetValue(OutputProperty, value);
        }

        public int Index
        {
            get => GetValue(IndexProperty);
            set => SetValue(IndexProperty, value);
        }

        // Constructor with parameters
        public OutputControl(int index, Output? output = null)
        {
            this.Index = index;
            this.Output = output;

            InitializeControl();
        }

        // Default constructor for Avalonia compiler/tools
        public OutputControl() => InitializeControl();

        private void InitializeControl()
        {
            HorizontalAlignment = HorizontalAlignment.Stretch;
            VerticalAlignment = VerticalAlignment.Stretch;

            // Set a Minimum height/width so it doesn't disappear if empty
            MinHeight = 100;
            MinWidth = 120;

            Margin = new Thickness(5);

            // We use a property listener to swap the content when Output changes
            this.GetObservable(OutputProperty).Subscribe(new AnonymousObserver<Output?>(_ => UpdateContent()));
        }

        private void UpdateContent()
        {
            if (Output == null)
            {
                Content = CreatePlusButton();
            }
            else
            {
                Content = CreateDataView();
            }
        }

        private Control CreateDataView()
        {
            return new Border
            {
                Background = Brushes.WhiteSmoke,
                CornerRadius = new CornerRadius(8),
                BorderBrush = Brushes.LightGray,
                BorderThickness = new Thickness(1),
                Padding = new Thickness(10),
                Child = new StackPanel
                {
                    HorizontalAlignment = HorizontalAlignment.Stretch,// Stretch panel inside ScrollViewer
                    Spacing = 2,
                    Children =
                    {
                        new TextBlock {
                            [!TextBlock.TextProperty] = new Binding(nameof(Index)) { Source = this, StringFormat = "#{0}" },
                            FontSize = 10,
                            Foreground = Brushes.Gray
                        },
                        new TextBlock {
                            Text = Output?.Name ?? "Unnamed",
                            FontWeight = FontWeight.Bold,
                            TextTrimming = TextTrimming.CharacterEllipsis
                        },
                        new TextBlock {
                            Text = $"{Output?.id} | ID: {Output?.LocationID}",
                            FontSize = 11
                        }
                    }
                }
            };
        }

        private Control CreatePlusButton()
        {
            return new Button
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
                CornerRadius = new CornerRadius(8),
                Content = new PathIcon
                {
                    Width = 24,
                    Height = 24,
                    Data = StreamGeometry.Parse("M19,13H13V19H11V13H5V11H11V5H13V11H19V13Z")
                }
            };
        }
    }
}
