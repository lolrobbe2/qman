using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input.Platform;
using Avalonia.VisualTree;
using qmanlib.src.storage.qdb;
using qmanlib.src.storage.repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace qman.src
{
    /// <summary>
    /// class containing all the globaly accesable objects such as the current loaded QbusDatabase
    /// </summary>
    public class GlobalState
    {
        public static CommonRepostories? repositories { get; set; }
    }
    public class Clipboard {
    public static IClipboard Get() {

        //Desktop
        if (App.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime { MainWindow: { } window }) {
            return window.Clipboard!;

        }
        //Android (and iOS?)
        else if (App.Current?.ApplicationLifetime is ISingleViewApplicationLifetime { MainView: { } mainView }) {
            var visualRoot = mainView.GetVisualRoot();
            if (visualRoot is TopLevel topLevel) {
                return topLevel.Clipboard!;
            }
        }

        return null!;
    }
}
}
