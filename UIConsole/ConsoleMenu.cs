// Johan Vandaele - Versie: 20250101

namespace UIConsole;

public partial class Program
{
    // -----
    // Enums
    // -----
    public enum MenuDirection { Horizontal, Vertical }
    public enum MenuItemActive { Enabled, Disabled }
    public enum MenuItemVisible { Visible, Hidden }

    // --------
    // MenuItem
    // --------
    public class MenuItem
    {
        public MenuItem() { }
        public MenuItem(int id) => (Id) = (id);

        public MenuItem(int id, string label, MenuItemActive active, MenuItemVisible visible)
            => (Id, Label, Active, Visible, LabelOrg, ActiveOrg, VisibleOrg) = (id, label, active, visible, label, active, visible);

        public MenuItem(int id, string label, string titel, MenuItemActive active, MenuItemVisible visible)
            => (Id, Label, SubMenuTitel, Active, Visible, LabelOrg, ActiveOrg, VisibleOrg) = (id, label, titel, active, visible, label, active, visible);

        // ----------
        // Properties
        // ----------
        private string label { get; set; } = null!;

        public int Id { get; set; }         // NotUnique : To enable/disable menuitem

        public string Label
        {
            get { return label; }
            set
            {
                label = value;
                if (this is SubMenu && label != null) label += "...";
            }
        }

        public string ActualLabel { get; set; } = null!;
        public string SubMenuTitel { get; set; } = null!;
        public MenuItemActive Active { get; set; }
        public MenuItemVisible Visible { get; set; }
        public string LabelOrg { get; set; } = null!;
        public MenuItemActive ActiveOrg { get; set; }
        public MenuItemVisible VisibleOrg { get; set; }
    }

    // --------
    // MenuLijn
    // --------
    public class MenuLijn : MenuItem
    {
        // -----------
        // Constructor
        // -----------
        public MenuLijn() { }
        public MenuLijn(int id) : base(id) { }

        // ----------
        // Properties
        // ----------
    }

    // -------
    // SubMenu
    // -------
    public class SubMenu : MenuItem
    {
        // -----------
        // Constructor
        // -----------
        public SubMenu(int id, string? label, string titel, MenuItemActive active, MenuItemVisible visible, List<MenuItem> menuItems) : base(id, label!, active, visible)
            => (SubMenuTitel, Richting, MenuItems) = (titel, MenuDirection.Vertical, menuItems);

        public SubMenu(int id, string? label, string titel, MenuItemActive active, MenuItemVisible visible, MenuDirection richting, List<MenuItem> menuItems) : base(id, label!, active, visible)
            => (SubMenuTitel, Richting, MenuItems) = (titel, richting, menuItems);

        // ----------
        // Properties
        // ----------
        public List<MenuItem> MenuItems { get; set; }
        public MenuDirection Richting { get; set; }
        public string Input { get; set; } = null!;
    }

    public class MenuAction(int id, string label, string titel, MenuItemActive active, MenuItemVisible visible, Action menuItemAction00) : MenuItem(id, label, titel, active, visible)
    {
        // ----------
        // Properties
        // ----------
        public Action MenuItemAction00 { get; set; } = menuItemAction00;
        public Action<object> MenuItemAction01 { get; set; } = null!;
        public object Par01 { get; set; } = null!;
    }

    // -----------------------
    // Menu Instance variables
    // -----------------------
    public static string MenuGegevens = $"";
    private static bool FirstTime = true;
    private static string menuTextForegroundActive = ConsFGC;
    private static string menuTextForegroundNotActive = Ansi.FgRgb(40, 40, 40);  // Gray
    private static string menuTextBackground = Ansi.BgRgb(65, 105, 225);

    private static string MenuPath = string.Empty;
    private static string AppTitel = string.Empty;

    private const string ExitTextSubMenu = "E<x>it";
    private const string ExitTextHoofdMenu = "E<x>it toepassing";
    private const string Versie = " - Johan Vandaele - Versie 20250101";

    // =======================================
    // S E T T I N G S
    // =======================================
    public static void SetAlternateRows(bool alternateRows) => AlternateRows = alternateRows;

    // ---------
    // SetActive
    // ---------
    public static void SetActive(SubMenu sm, List<int> menuIds, MenuItemActive active)
        => menuIds.ForEach(i => SetActive(sm, i, active));

    private static void SetActive(MenuItem menuItem, int id, MenuItemActive active)
    {
        if (menuItem is SubMenu menu)
            foreach (var item in menu.MenuItems)
            {
                if (item.Id == id) item.Active = active;
                SetActive(item, id, active);
            }
    }

    // ---------
    // Set Label
    // ---------
    public static void SetLabel(SubMenu sm, List<int> menuIds, string label)
        => menuIds.ForEach(i => SetLabel(sm, i, label));

    private static void SetLabel(MenuItem menuItem, int id, string label)
    {
        if (menuItem is SubMenu menu)
            foreach (var item in menu.MenuItems)
            {
                if (item.Id == id) item.Label = label;
                SetLabel(item, id, label);
            }
    }

    // ----------
    // SetVisible
    // ----------
    public static void SetVisible(SubMenu sm, List<int> menuIds, MenuItemVisible visible)
        => menuIds.ForEach(i => SetVisible(sm, i, visible));

    private static void SetVisible(MenuItem menuItem, int id, MenuItemVisible visible)
    {
        if (menuItem is SubMenu menu)
            foreach (var item in menu.MenuItems)
            {
                if (item.Id == id) item.Visible = visible;
                SetVisible(item, id, visible);
            }
    }

    // -----
    // Reset
    // -----
    public static void ResetMenu(SubMenu subMenu)
    {
        foreach (var item in subMenu.MenuItems)
        {
            item.Active = item.ActiveOrg;
            item.Visible = item.VisibleOrg;
            item.Label = item.LabelOrg;

            if (item is SubMenu menu) ResetMenu(menu);
        }
    }

    // ------------------
    // ToonMenuHorizontal
    // ------------------
    private static void ToonMenuHorizontal(SubMenu menuItem)
    {
        char? keuze = null;

        menuItem.Input = string.Empty;
        var menuHorizontal = string.Empty;

        foreach (var item in menuItem.MenuItems)
        {
            var label = item.Label;

            if (item is not MenuLijn)
            {
                var idx1 = label.IndexOf('<');
                var idx2 = label.IndexOf('>');

                if (idx1 >= 0 && idx2 >= 0 && idx2 == idx1 + 2)
                    if (item.Active == MenuItemActive.Enabled)
                        menuItem.Input += label.Substring(idx1 + 1, 1).ToUpper();
                    else
                    {
                        label = label.Remove(idx1, 1);
                        label = label.Remove(idx2 - 1, 1);
                    }

                menuHorizontal += $"{label} - ";
            }
        }

        Console.WriteLine($"{menuHorizontal}{ExitTextSubMenu}");
        menuItem.Input += "X";

        keuze = LeesString($"Geef uw keuze ({menuItem.Input})", 1, 1, OptionMode.Mandatory)!.ToUpper().ToCharArray()[0];

        while (!menuItem.Input.Contains((char)keuze))
        {
            ToonFoutBoodschap($"Verkeerde keuze ({menuItem.Input}): ");
            keuze = LeesString($"Geef uw keuze ({menuItem.Input})", 1, 1, OptionMode.Mandatory)!.ToUpper().ToCharArray()[0];
        }

        var selectedItem = menuItem.MenuItems.Where(i => !(i is MenuLijn) && i.Label.Contains($"<{keuze}>")).FirstOrDefault();

        if (selectedItem is MenuAction action)                     // Execute method
        {
            Console.Write(menuTextBackground);

            if (action.MenuItemAction00 != null)
            {
                if (selectedItem.SubMenuTitel != string.Empty)
                    ToonMenuHoofding(selectedItem.SubMenuTitel, '─', false);

                action.MenuItemAction00();
            }

            Console.Write($"{ConsFGC}");
        }
    }

    // --------
    // ToonMenu
    // --------
    public static void ToonMenu(string appTitel, SubMenu menuItem)
    {
        AppTitel = appTitel;
        Console.Title = $"{AppTitel}{Versie}";

        if (FirstTime)      // Eerste oproep van een Menu (Start application)
        {
            FirstTime = false;
            StartConsole();

            Console.Clear();

            ToonMenuHoofding(appTitel, '=', false, 0);
            Console.WriteLine();

            // AsciiArt

            //Console.WriteLine(@" .oooooo..o                                  oooooooooo.                      oooo");
            //Console.WriteLine(@"d8P'    `Y8                                  `888'   `Y8b                     `888");
            //Console.WriteLine(@"Y88bo.       .ooooo.  ooo. .oo.    .oooooooo  888     888  .ooooo.   .ooooo.   888  oooo");
            //Console.WriteLine(@" `""Y8888o.  d88' `88b `888P""Y88b  888' `88b   888oooo888' d88' `88b d88' `88b  888 .8P'");
            //Console.WriteLine(@"     `""Y88b 888   888  888   888  888   888   888    `88b 888   888 888   888  888888.");
            //Console.WriteLine(@"oo     .d8P 888   888  888   888  `88bod8P'   888    .88P 888   888 888   888  888 `88b.");
            //Console.WriteLine(@"8""88888P'  `Y8bod8P' o888o o888o `8oooooo.  o888bood8P'  `Y8bod8P' `Y8bod8P' o888o o888o");
            //Console.WriteLine(@"                                  d""     YD");
            //Console.WriteLine(@"                                  ""Y88888P'");
            //Console.WriteLine(@"");
            //Console.WriteLine(@"   oooo           oooo                                   oooooo     oooo                            ooooo   ooooo                     oooo");
            //Console.WriteLine(@"   `888           `888                                    `888.     .8'                             `888'   `888'                     `888");
            //Console.WriteLine(@"    888  .ooooo.   888 .oo.    .oooo.   ooo. .oo.          `888.   .8'    .oooo.   ooo. .oo.         888     888   .ooooo.   .ooooo.   888  oooo   .ooooo.");
            //Console.WriteLine(@"    888 d88' `88b  888P""Y88b  `P  )88b  `888P""Y88b          `888. .8'    `P  )88b  `888P""Y88b        888ooooo888  d88' `88b d88' `""Y8  888 .8P'   d88' `88b");
            //Console.WriteLine(@"    888 888   888  888   888   .oP""888   888   888           `888.8'      .oP""888   888   888        888     888  888ooo888 888        888888.    888ooo888");
            //Console.WriteLine(@"    888 888   888  888   888  d8(  888   888   888            `888'      d8(  888   888   888        888     888  888    .o 888   .o8  888 `88b.  888    .o");
            //Console.WriteLine(@".o. 88P `Y8bod8P' o888o o888o `Y888""8o  o888o o888o            `8'       `Y888""8o  o888o o888o      o888o   o888o `Y8bod8P' `Y8bod8P' o888o o888o `Y8bod8P'");
            //Console.WriteLine(@"`Y888P");
            //Console.WriteLine(@"");

            //Console.WriteLine(@"oooooo    oooo  oooooooooo.         .o.       oooooooooo.");
            //Console.WriteLine(@"`888.     .8'  `888'   `Y8b       .888.      `888'   `Y8b ");
            //Console.WriteLine(@"`888.   .8'    888      888     .8`888.      888     888");
            //Console.WriteLine(@"`888. .8'     888      888    .8' `888.     888oooo888' ");
            //Console.WriteLine(@"`888.8'      888      888   .88ooo8888.    888    `88b ");
            //Console.WriteLine(@"`888'       888     d88'  .8'     `888.   888    .88P ");
            //Console.WriteLine(@"`8'        o888bood8P'  o88o     o8888o o888bood8P'  ");

            Console.WriteLine(@"  ____                      _      _   _      _                 ");
            Console.WriteLine(@" / ___|___  _ __  ___  ___ | | ___| | | | ___| |_ __   ___ _ __ ");
            Console.WriteLine(@"| |   / _ \| '_ \/ __|/ _ \| |/ _ \ |_| |/ _ \ | '_ \ / _ \ '__|");
            Console.WriteLine(@"| |__| (_) | | | \__ \ (_) | |  __/  _  |  __/ | |_) |  __/ |   ");
            Console.WriteLine(@" \____\___/|_| |_|___/\___/|_|\___|_| |_|\___|_| .__/ \___|_|   ");
            Console.WriteLine(@"                                               |_|              ");
            Console.WriteLine();
            Console.WriteLine(@"     _       _                  __     __              _            _      ");
            Console.WriteLine(@"    | | ___ | |__   __ _ _ __   \ \   / /_ _ _ __   __| | __ _  ___| | ___ ");
            Console.WriteLine(@" _  | |/ _ \| '_ \ / _` | '_ \   \ \ / / _` | '_ \ / _` |/ _` |/ _ \ |/ _ \");
            Console.WriteLine(@"| |_| | (_) | | | | (_| | | | |   \ V / (_| | | | | (_| | (_| |  __/ |  __/");
            Console.WriteLine(@" \___/ \___/|_| |_|\__,_|_| |_|    \_/ \__,_|_| |_|\__,_|\__,_|\___|_|\___|");

            Console.WriteLine($"\n\n.NET Versie: {Environment.Version}");

            Console.ReadKey();

            ToonMenu(menuItem);

            Console.Write($"{ConsBGC}{ConsFGC}");
            Console.WriteLine("\nWij danken u voor onze prettige samenwerking. Tot de volgend keer....");
            Console.ReadKey();
        }
        else
        {
            if (menuItem.Richting == MenuDirection.Vertical) ToonMenu(menuItem);
            else ToonMenuHorizontal(menuItem);
        }
    }

    // --------
    // ToonMenu
    // --------
    private static void ToonMenu(SubMenu menuItem)
    {
        // I n i t i a l i s a t i e s
        MenuPath += $"\\{menuItem.SubMenuTitel}";
        char? keuze = null;

        while (keuze != 'X')
        {
            // Bepaal minimum lengte van een menuitemlabel
            var labelLength = ExitTextSubMenu.Length;

            if (string.IsNullOrWhiteSpace(menuItem.Label)) labelLength = ExitTextHoofdMenu.Length;

            menuItem.Input = string.Empty;

            // Bepaal de geactualiseerde label en verzamel de input tekens
            foreach (var item in menuItem.MenuItems)
                if (item is not MenuLijn)
                {
                    var label = item.Label;

                    // Bepaal het input character van een menuitem
                    var idx1 = label.IndexOf('<');
                    var idx2 = label.IndexOf('>');

                    if (idx1 >= 0 && idx2 >= 0 && idx2 == idx1 + 2)
                        if (item.Active == MenuItemActive.Enabled && item.Visible == MenuItemVisible.Visible)
                            // Indien menuitem enabled : verzamel de mogelijke tekens voor input
                            menuItem.Input += label.Substring(idx1 + 1, 1).ToUpper();
                        else
                        {
                            // Indien menuitem disabled verwijder de '<' en '>' : kan niet gekozen worden
                            label = label.Remove(idx2, 1);
                            label = label.Remove(idx1, 1);
                        }

                    // Bepaal de lengte van het langste menuitem
                    if (!(item is MenuLijn) && item.Visible == MenuItemVisible.Visible)
                        if (item.Label.Length > labelLength) labelLength = item.Label.Length;

                    // Bewaar de geactualiseerde label
                    item.ActualLabel = label;
                }

            // Voeg E<X>it toe
            menuItem.Input += "X";

            // C o n s o l e
            ResetConsole();

            // T o o n   M e n u
            ToonSubMenu(menuItem, labelLength);

            // M e n u I n p u t
            if (Beep) Console.Beep();

            var cursorPos = Console.GetCursorPosition();

            keuze = LeesString($"Geef uw keuze ({menuItem.Input})", 1, 1, OptionMode.Mandatory)!.ToUpper().ToCharArray()[0];

            while (!menuItem.Input.Contains((char)keuze))
            {
                ToonFoutBoodschap($"Verkeerde keuze ({menuItem.Input}): ");
                Console.Beep();
                Console.SetCursorPosition(cursorPos.Left, cursorPos.Top);
                keuze = LeesString($"Geef uw keuze ({menuItem.Input})", 1, 1, OptionMode.Mandatory)!.ToUpper().ToCharArray()[0];
            }

            if (keuze != 'X')
            {
                var selectedItem = menuItem.MenuItems.Where(i => !(i is MenuLijn) && i.Label.ToUpper().Contains($"<{keuze}>")).FirstOrDefault();

                if (selectedItem is MenuAction action)                     // Execute method
                {
                    if (action.MenuItemAction00 != null)
                    {
                        if (selectedItem.SubMenuTitel != string.Empty)
                        {
                            var line = new string('─', selectedItem.SubMenuTitel.Length * 2);

                            ResetConsole();

                            // Action Title
                            Console.WriteLine($"\t┌{line}┐");
                            Console.Write($"\t│");
                            foreach (var c in selectedItem.SubMenuTitel) Console.Write(c.ToString().ToUpper() + " ");
                            Console.Write("│");
                            Console.WriteLine($"\n\t└{line}┘");
                        }

                        action.MenuItemAction00();
                    }

                    //else if (((MenuAction)selectedItem).MenuItemAction01 != null) ((MenuAction)selectedItem).MenuItemAction01(((MenuAction)selectedItem).Par01);

                    ToonInfoBoodschap("\nDruk een toets om verder te gaan.");
                    Console.ReadKey();
                }
                else if (selectedItem is SubMenu menu1)                   // Toon Submenu
                    ToonMenu(menu1);
            }
            else
            {
                if (string.IsNullOrWhiteSpace(menuItem.Label))
                {
                    if (!(bool)LeesBool("Weet u zeker dat u de toepassing wenst te verlaten ?", OptionMode.Mandatory)!) keuze = null;
                }
                else
                {
                    var idx = MenuPath.LastIndexOf('\\');
                    MenuPath = MenuPath.Remove(idx, MenuPath.Length - idx);
                }
            }
        }
    }

    // ------------
    // Toon SubMenu
    // ------------
    private static void ToonSubMenu(SubMenu subMenu, int labelLength)
    {
        //Console.Clear();

        ToonMenuHoofding(subMenu.SubMenuTitel, '─', true);

        foreach (var item in subMenu.MenuItems)
            if (item is MenuLijn) ToonMenuLijn(labelLength, 0);
            else if (item.Visible == MenuItemVisible.Visible) WriteMenuItem(item.ActualLabel, item.Active, labelLength);

        ToonMenuLijn(labelLength, 0);
        WriteMenuItem(string.IsNullOrWhiteSpace(subMenu.Label) ? ExitTextHoofdMenu : ExitTextSubMenu, MenuItemActive.Enabled, labelLength);
        ToonMenuLijn(labelLength, 0, true);       // Last Menu line
    }

    // -------------
    // Toon MenuLijn
    // -------------
    private static bool lijn = false;

    private static void ToonMenuLijn(int lengte, int level = 0, bool lastMenuLine = false)
    {
        if (!lijn) Console.Write($"{new string('\t', level + 1)}");

        Console.Write(menuTextBackground);

        if (lastMenuLine)
        {
            if (!lijn) Console.WriteLine($"└{new string('─', lengte)}┘");
        }
        else
            if (!lijn) Console.WriteLine($"├{new string('─', lengte)}┤");

        Console.Write($"{ConsBGC}");

        lijn = true;
    }

    // -------------
    // ToonMenuTitel
    // -------------
    //private static void ToonMenuHoofding(string text, char lineChar, bool toonGegevens, int level = 0)
    private static void ToonMenuHoofding(string text, char lineChar, bool toonGegevens, int level = 0)
    {
        lineChar = '─';

        if (toonGegevens)
        {
            //Console.WriteLine($"MenuPath: {MenuPath}");
            Console.Title = $"{AppTitel} - {MenuPath}{Versie}";
            Console.WriteLine($"    Info: {MenuGegevens}");
        }

        // Init
        var line = new string(lineChar, text.Length * 2);
        var tabs = new string('\t', level + 1);

        // Line 1
        Console.Write($"\n{tabs}{menuTextBackground}┌{line}┐\n");

        // Line 2
        Console.Write($"{menuTextBackground}{tabs}│");
        foreach (var c in text) Console.Write(c.ToString().ToUpper() + " ");
        Console.Write("│\n");

        // Line 3
        Console.Write($"{menuTextBackground}{tabs}└{line}┘\n");

        // Restore colors
        Console.Write($"{ConsBGC}");
    }

    // -------------
    // WriteMenuItem
    // -------------
    private static void WriteMenuItem(string text, MenuItemActive isActive, int lengte, int level = 0)
    {
        lijn = false;

        // tabs
        Console.Write($"{new string('\t', level + 1)}");

        // Text
        Console.Write(isActive == MenuItemActive.Enabled ? menuTextForegroundActive : menuTextForegroundNotActive);

        var textLength = text.Length;

        text = text.Replace("<", $"<{Ansi.FDYELLOW}{Ansi.UnderlineOn}").Replace(">", $"{ConsFGC}{Ansi.UnderlineOf}>");

        Console.Write(menuTextBackground);
        Console.WriteLine($"{ConsFGC}│{text}{new string(' ', lengte - textLength)}│");

        // Restore colors
        Console.Write($"{ConsBGC}{ConsFGC}");
    }

    // ---------
    // PrintMenu
    // ---------
    public static void PrintMenu(string appTitel, SubMenu menuItem)
    {
        if (FirstTime)      // Eerste oproep van een Menu (Start application)
        {
            FirstTime = false;
            StartConsole();

            menuTextBackground = darkMode ? Ansi.BgRgb(0, 0, 0) : Ansi.BgRgb(255, 255, 255);

            ToonMenuHoofding(appTitel, '=', false);
        }

        if (menuItem.Richting == MenuDirection.Vertical) PrintMenu(menuItem);
        else PrintMenuHorizontal(menuItem, 0);
    }

    // ---------
    // PrintMenu
    // ---------
    private static int PrintMenu(SubMenu menuItem, int level = 0)
    {
        level++;

        ToonMenuHoofding(menuItem.SubMenuTitel, '─', false, level);

        // Bepaal minimum lengte van een menuitemlabel
        var labelLength = ExitTextSubMenu.Length;

        if (string.IsNullOrWhiteSpace(menuItem.Label)) labelLength = ExitTextHoofdMenu.Length;

        foreach (var mi in menuItem.MenuItems)
        {
            foreach (var item in menuItem.MenuItems)
                if (item is not MenuLijn)
                    if (item.Label.Length > labelLength) labelLength = item.Label.Length;

            if (mi is MenuLijn) ToonMenuLijn(labelLength, level);
            else
            {
                WriteMenuItem($"{mi.Label}", MenuItemActive.Enabled, labelLength, level);

                if (mi is SubMenu menu) PrintMenu(menu, level);
            }
        }

        ToonMenuLijn(labelLength, level);
        WriteMenuItem(menuItem.Label is null ? ExitTextHoofdMenu : ExitTextSubMenu, MenuItemActive.Enabled, labelLength, level);
        ToonMenuLijn(labelLength, level, true);
        Console.WriteLine();

        return level;
    }

    // -------------------
    // PrintMenuHorizontal
    // -------------------
    private static int PrintMenuHorizontal(SubMenu menuItem, int level)
    {
        level++;

        ToonMenuHoofding(menuItem.SubMenuTitel, '─', false, level);

        // Bepaal minimum lengte van een menuitemlabel
        var labelLength = ExitTextSubMenu.Length;
        if (string.IsNullOrWhiteSpace(menuItem.Label)) labelLength = ExitTextHoofdMenu.Length;

        foreach (var mi in menuItem.MenuItems)
        {
            foreach (var item in menuItem.MenuItems)
                if (item is not MenuLijn)
                    if (item.Label.Length > labelLength) labelLength = item.Label.Length;

            if (mi is MenuLijn) ToonMenuLijn(labelLength, level);
            else
            {
                WriteMenuItem($"{mi.Label}", MenuItemActive.Enabled, labelLength, level);

                if (mi is SubMenu menu) PrintMenu(menu, level);
            }
        }

        ToonMenuLijn(labelLength, level);
        WriteMenuItem(menuItem.Label is null ? ExitTextHoofdMenu : ExitTextSubMenu, MenuItemActive.Enabled, labelLength, level);
        ToonMenuLijn(labelLength, level);
        Console.WriteLine();

        return level;
    }
}
