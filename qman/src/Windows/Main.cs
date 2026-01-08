using Alternet.UI;
using qman.src.Windows;
using System;
using System.Collections.Generic;
using System.Text;

var application = new Application();
var window = new MainWindow();
application.Run(window);
window.Dispose();
application.Dispose();
return 0;