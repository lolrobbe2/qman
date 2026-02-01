using Avalonia.Controls;
using qman.src.lib;
using System;
using System.Collections.Generic;
using System.Text;

namespace qman.src.controllers
{
    internal class ModulesController : ContainerController
    {
        public ModulesController()
        {
            AddController<ModulesView.ModulesView>("modules_view");
        }
    }
}
