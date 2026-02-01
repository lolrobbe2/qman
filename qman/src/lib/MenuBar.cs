using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using System.Collections.Generic;

namespace qman.src.lib
{
    internal class MenuBarBuilder
    {
        private readonly List<MenuItem> _topMenus = new();

        private MenuItem? _currentMenu;

        private MenuBarBuilder() { }

        public static MenuBarBuilder Create()
        {
            return new MenuBarBuilder();
        }

        public MenuBarBuilder AddMenu(string header)
        {
            var menu = new MenuItem { Header = header };
            _topMenus.Add(menu);
            _currentMenu = menu;
            return this;
        }

        public MenuBarBuilder AddItem(string header, EventHandler<RoutedEventArgs>?  onClick = null)
        {
            if (_currentMenu == null)
                throw new InvalidOperationException("Call AddMenu before adding items.");

            var item = new MenuItem { Header = header };
            if (onClick != null)
                item.Click += onClick;

            _currentMenu.Items.Add(item);
            return this;
        }
        public MenuBarBuilder AddSeperator()
        {
            return AddItem("-");
        }
        public MenuBarBuilder EndMenu()
        {
            _currentMenu = null;
            return this;
        }

        public Menu Build()
        {
            var menu = new Menu();
            foreach (var topMenu in _topMenus)
                menu.Items.Add(topMenu);
            return menu;
        }
    }
}
