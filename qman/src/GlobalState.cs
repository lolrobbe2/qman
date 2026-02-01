using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input.Platform;
using Avalonia.VisualTree;
using qmanlib.src.storage.models;
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

        public static Module? currentSelected { get; set; }
    }
}
