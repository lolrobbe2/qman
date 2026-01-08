using Alternet.UI;
using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;


namespace qman.src.Windows
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public void HelloButton_Click(object? sender, EventArgs e)
        {
            MessageBox.Show("Hello, world!");
        }
    }
}
