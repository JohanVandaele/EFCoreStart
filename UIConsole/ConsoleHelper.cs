// Johan Vandaele - Versie: 20250101

using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace UIConsole;

public enum OptionMode { Optional = 0, Mandatory = 1 }
public enum SelectionMode { None = 0, Single = 1, Multiple = 2 }
public enum ReturnNullOrEmpty { Null = 0, Empty = 1 }

// Structure used by GetWindowRect
internal struct Rect
{
	public int Left;
	public int Top;
	public int Right;
	public int Bottom;
}

// -------
// Program
// -------
public static partial class Program
{
	// -------------------------------
	// Window Size and Window Position
	// -------------------------------
	// Import the necessary functions from user32.dll
	[DllImport("user32.dll")]
	static extern IntPtr GetForegroundWindow();

	[DllImport("user32.dll")]
	static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

	[DllImport("user32.dll")]
	static extern bool GetWindowRect(IntPtr hWnd, out Rect lpRect);

	[DllImport("user32.dll")]
	static extern bool MoveWindow(IntPtr hWnd, int x, int y, int nWidth, int nHeight, bool bRepaint);

	// Constants for the ShowWindow function
	private const int SW_HIDE = 0;
	private const int SW_MAXIMIZE = 3;
	private const int SW_MINIMIZE = 6;
	private const int SW_RESTORE = 9;

	private static void Maximize()
	{
		Process p = Process.GetCurrentProcess();
		ShowWindow(p.MainWindowHandle, 3); //SW_MAXIMIZE = 3
	}

	// --------------
	// Window Buttons
	// --------------
	// Import the necessary functions from user32.dll
	[DllImport("user32.dll")]
	public static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);

	[DllImport("user32.dll")]
	private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

	[DllImport("kernel32.dll", ExactSpelling = true)]
	private static extern IntPtr GetConsoleWindow();

	// Constants
	private const int MF_BYCOMMAND = 0x00000000;
	private const int SC_CLOSE = 0xF060;
	private const int SC_MINIMIZE = 0xF020;
	private const int SC_MAXIMIZE = 0xF030;
	private const int SC_SIZE = 0xF000; // resize

	// - - - - - - - - - - - - - - - - - - - - 

	private const int LengthInputLabel = 32;

	private static bool darkMode = true;    // Start in darkmode
	public static string ConsBGC => darkMode ? Ansi.BLBLACK : Ansi.BDWHITE;
	public static string ConsFGC => darkMode ? Ansi.FLWHITE : Ansi.FDBLACK;

	private static readonly string InputBGC = Ansi.BgRgb(197, 195, 246);
	private static readonly string InputFGC = Ansi.FgRgb(0, 0, 0);

	private static readonly TimeSpan paswoordDuurtijd = new(10, 0, 0, 1);

	// appSettings.json
	private static bool Beep;
	private static bool AlternateRows;
	private static bool AlternateRowsOrig;
	private static int Showwindow = 3;      // HIDE = 0, MAXIMIZE = 3, MINIMIZE = 6, RESTORE = 9
	private static int ListPageSize = 0;    // 0 = no paging, >0 = number of lines per page

	// ===========================================
	// - - - - - - - - C O N S O L E - - - - - - - 
	// ===========================================

	// ------------
	// InitConsole
	// ------------
	public static void StartConsole()
	{
		// - - - - - - - - - - - -
		// Configuration settings
		// - - - - - - - - - - - -
		IConfigurationRoot configuration = null!;

		// Zoek de naam in de connectionStrings section - appsettings.json
		configuration = new ConfigurationBuilder()
				.SetBasePath(Directory.GetParent(AppContext.BaseDirectory)!.FullName)
				.AddJsonFile("appSettings.json", false)
				.Build();

		var mySettings = configuration.GetSection("ConsoleSettings");

		if (!mySettings.GetChildren().Any())
		{
			ToonInfoBoodschap($"\nGeen ConsoleSettings gevonden in appsettings.json");

			Console.WriteLine(@"
{
	. . . . . . . . . 

	""ConsoleSettings"": {
		""AlternateRows"": ""true"",    // Default=false
		""Beep"": ""True"",             // Default=false
		""ListPageSize"": ""19"",       // Default=0 (No paging)
		""ShowWindow"": ""3""           // HIDE = 0, MAXIMIZE = 3 (default), MINIMIZE = 6, RESTORE = 9
		}
}
"
				);
		}

		AlternateRows = AlternateRowsOrig = mySettings["AlternateRows"] != null && mySettings["AlternateRows"]!.Equals("true", StringComparison.CurrentCultureIgnoreCase);
		Beep = mySettings["Beep"] != null && mySettings["Beep"]!.Equals("True", StringComparison.CurrentCultureIgnoreCase);
		int.TryParse(mySettings["ListPageSize"], out ListPageSize);
		int.TryParse(mySettings["ShowWindow"], out Program.Showwindow);
		if (Program.Showwindow == 0) Program.Showwindow = 3;    // Default

		// - - - - - 
		// DarkMode
		// - - - - - 
		Console.Write($"{ConsBGC}{ConsFGC}{Ansi.EraseScreen}{Ansi.CURSORHOME}");

		darkMode = ((string)LeesKeuzeUitLijst("Dark Mode", ["J", "N", "j", "n"], OptionMode.Mandatory)!).Equals("J", StringComparison.CurrentCultureIgnoreCase);

		// ----
		// Ansi
		// ----
		Ansi.EnableVirtualConsole();    // If needed

		ResetConsole();
	}

	// ------------
	// ResetConsole
	// ------------
	public static void ResetConsole()
	{
		// -------------------------------
		// Window Size and Window Position
		// -------------------------------
		// Get the handle of the console window
		var consoleWindowHandle = GetForegroundWindow();

		// Maximize the console window
		ShowWindow(consoleWindowHandle, Program.Showwindow);

		// Get the screen size
		GetWindowRect(consoleWindowHandle, out Rect screenRect);

		// Resize and reposition the console window to fill the screen
		var width = screenRect.Right - screenRect.Left;
		var height = screenRect.Bottom - screenRect.Top;
		MoveWindow(consoleWindowHandle, screenRect.Left, screenRect.Top, width, height, true);

		// --------------
		// Window Buttons
		// --------------
		var handle = GetConsoleWindow();
		var sysMenu = GetSystemMenu(handle, false);

		if (handle != IntPtr.Zero)
		{
			//DeleteMenu(sysMenu, SC_CLOSE, MF_BYCOMMAND);
			//DeleteMenu(sysMenu, SC_MINIMIZE, MF_BYCOMMAND);
			//DeleteMenu(sysMenu, SC_MAXIMIZE, MF_BYCOMMAND);
			//_ = DeleteMenu(sysMenu, SC_SIZE, MF_BYCOMMAND);	// Disable Resize of window
		}

        // -------
        // Console
        // -------
#pragma warning disable CA1416 // Validate platform compatibility (Only Windows)
		Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
#pragma warning disable CA1416 // Validate platform compatibility (Only Windows)
        Console.SetBufferSize(Console.LargestWindowWidth, 5000 /*Console.LargestWindowHeight*/);
		Console.CursorSize = 10;            // Only on Windows
		Console.SetWindowPosition(0, 0);
#pragma warning restore CA1416 // Validate platform compatibility (Only Windows)
        Console.CursorVisible = true;
		Console.SetCursorPosition(0, 0);

		Console.OutputEncoding = Encoding.Unicode;// UTF8;
		Console.Write($"{ConsBGC}{ConsFGC}{Ansi.EraseScreen}{Ansi.CURSORHOME}");

		Thread.Sleep(1000); // Wait for Finish running Threats
	}

	// =======================================
	// S E T T I N G S
	// =======================================
	public static void SetBeep(bool beep) => Beep = beep;
	public static void SetListPageSize(int listPageSize) => ListPageSize = listPageSize;

	// =======================================
	// - - - - - - - - I N P U T - - - - - - - 
	// =======================================
	// ---------------------
	// Show Input bounderies
	// ---------------------
	private static void StartInput(int length)
	{
		var (left, top) = Console.GetCursorPosition();
		Console.Write($"[{InputBGC}{new string(' ', length)}{ConsFGC}{ConsBGC}]{InputBGC}{InputFGC}");
		Console.SetCursorPosition(left + 1, top);
	}

	private static void EndInput() => Console.Write($"{ConsFGC}{ConsBGC}");

	// ---------
	// DrukToets
	// ---------
	public static void DrukToets()
	{
		Console.Write("\nDruk een toets");
		Console.ReadKey();
	}

	// ----------
	// LeesString
	// ----------
	public static string? LeesString(string label = "", int minLength = 0, int maxLength = 100, OptionMode optionMode = OptionMode.Optional, ReturnNullOrEmpty returnNullEmpty = ReturnNullOrEmpty.Empty)
	{
		var isInsert = false;
		var cursorPos = 0;

		// Correction on Parameter values
		if (minLength < 0) minLength = 0;
		else if (minLength == 0 && optionMode == OptionMode.Mandatory) minLength = 1;
		else if (minLength > 0) optionMode = OptionMode.Mandatory;

		if (maxLength < 1) maxLength = 1;

		// Input
		string input;
		var (left, top) = Console.GetCursorPosition();

		while (true)
		{
			// Init
			input = string.Empty;

			// Label
			Console.SetCursorPosition(left, top);
			Console.Write($"{label + (optionMode == OptionMode.Mandatory ? "* : " : " : "),LengthInputLabel}");

			// Input
			{
				// Input bounderies
				StartInput(maxLength);

				// Read Input
				ConsoleKeyInfo keyInfo;

				do
				{
					keyInfo = Console.ReadKey(true);

					if (keyInfo.Key == ConsoleKey.Insert) isInsert = !isInsert;

					if (keyInfo.Key == ConsoleKey.LeftArrow || keyInfo.Key == ConsoleKey.RightArrow)
					{
						if (keyInfo.Key == ConsoleKey.LeftArrow)
						{
							if (cursorPos > 0) cursorPos--;
						}

						if (keyInfo.Key == ConsoleKey.RightArrow)
						{
							if (cursorPos < input.Length) cursorPos++;
						}

						//Console.SetCursorPosition(inputCursorPos.Left+cursorPos+1, inputCursorPos.Top);
					}

					//if (keyInfo.Key != ConsoleKey.Backspace && keyInfo.Key != ConsoleKey.Enter && input.Length < maxLength)
					//if ((char.IsLetterOrDigit(keyInfo.KeyChar) || char.IsLetterOrDigit(keyInfo.KeyChar)) && input.Length < maxLength)
					if (!char.IsControl(keyInfo.KeyChar) && input.Length < maxLength)
					{
						input += keyInfo.KeyChar;
						//cursorPos++;        //!!!
						Console.Write(keyInfo.KeyChar);
					}
					else
					{
						if (keyInfo.Key == ConsoleKey.Backspace && input.Length > 0)
						{
							input = input[..^1];        //input = input.Substring(0, (input.Length - 1));
							Console.Write("\b \b");
						}
					}
				}
				while (keyInfo.Key != ConsoleKey.Enter);

				EndInput();
			}

			Console.WriteLine();

			// Validations
			if (optionMode == OptionMode.Mandatory && string.IsNullOrWhiteSpace(input))
			{
				ToonFoutBoodschap("Verplichte ingave...");
				Console.SetCursorPosition(left, top);
				continue;
			}
			else if (input.Length < minLength)
			{
				ToonFoutBoodschap($"De ingave is te kort (min {minLength} teken{(minLength > 1 ? "s" : "")})...");
				Console.SetCursorPosition(left, top);
				continue;
			}
			else if (input.Length > maxLength)
			{
				ToonFoutBoodschap($"De ingave is te lang (max {maxLength} teken{(maxLength > 1 ? "s" : "")})...");
				Console.SetCursorPosition(left, top);
				continue;
			}

			break;
		}

		// Finish
		if (string.IsNullOrWhiteSpace(input))
		{
			if (returnNullEmpty == ReturnNullOrEmpty.Null) return null;
			else return string.Empty;
		}

		return input;
	}

	// -------------
	// LeesDatumTijd
	// -------------
	public static DateTime? LeesDatumTijd(string label, DateTime min, DateTime max, OptionMode optionMode = OptionMode.Optional)
	{
		// Parameters
		if (min > max) throw new ArgumentException("Parameterfout: minDateTime moet vóór maxDateTime liggen.");

		// Input
		string input;
		var inpParsed = DateTime.MinValue;
		const string formaat = "DD/MM/YYYY UU:MM:SS";

		var (left, top) = Console.GetCursorPosition();

		while (true)
		{
			// Label
			Console.SetCursorPosition(left, top);
			Console.Write($"{label + (optionMode == OptionMode.Mandatory ? "* : " : " : "),LengthInputLabel}");

			// Input bounderies
			StartInput(formaat.Length);

			// Input
			input = Console.ReadLine()!;

			EndInput();

			if (optionMode == OptionMode.Mandatory && string.IsNullOrWhiteSpace(input))
			{
				ToonFoutBoodschap("Verplichte ingave...");
				continue;
			}

			if (!string.IsNullOrWhiteSpace(input))
			{
				if (!DateTime.TryParse(input, out inpParsed))
				{
					ToonFoutBoodschap($"Ongeldige DatumTijd ({formaat})...");
					continue;
				}

				if (inpParsed < min || inpParsed > max)
				{
					//ToonFoutBoodschap($"De DatumTijd moet liggen tussen {min.ToString("dd-MMMM-yyyy hh:mm:ss")} en {max.ToString("dd-MMMM-yyyy hh:mm:ss")}...");
					ToonFoutBoodschap($"De DatumTijd moet liggen tussen {min:dd-MMMM-yyyy hh:mm:ss} en {max:dd-MMMM-yyyy hh:mm:ss}...");
					continue;
				}
			}

			break;
		}

		return string.IsNullOrWhiteSpace(input) ? null : inpParsed;
	}

	// ---------
	// LeesDatum
	// ---------
	public static DateOnly? LeesDatum(string label, DateOnly min, DateOnly max, OptionMode optionMode = OptionMode.Optional)
	{
		// Parameters
		if (min > max) throw new ArgumentException("Parameterfout: minDate moet vóór maxDate liggen.");

		// Input
		string input;
		var inpParsed = DateOnly.MinValue;
		const string formaat = "DD/MM/YYYY";

		var (left, top) = Console.GetCursorPosition();

		while (true)
		{
			Console.SetCursorPosition(left, top);
			Console.Write($"{label + (optionMode == OptionMode.Mandatory ? "* : " : " : "),LengthInputLabel}");

			// Input bounderies
			StartInput(formaat.Length);

			// Input
			input = Console.ReadLine()!;

			EndInput();

			if (optionMode == OptionMode.Mandatory && string.IsNullOrWhiteSpace(input))
			{
				ToonFoutBoodschap("Verplichte ingave...");
				continue;
			}

			if (!string.IsNullOrWhiteSpace(input))
			{
				if (!DateOnly.TryParse(input, out inpParsed))
				{
					ToonFoutBoodschap($"Ongeldige Datum ({formaat})...");
					continue;
				}

				if (inpParsed < min || inpParsed > max)
				{
					ToonFoutBoodschap($"De datum moet liggen tussen {min} en {max}...");
					continue;
				}
			}

			break;
		}

		return string.IsNullOrWhiteSpace(input) ? null : inpParsed;
	}

	// --------
	// LeesTijd
	// --------
	public static TimeOnly? LeesTijd(string label, TimeOnly? min = null, TimeOnly? max = null, OptionMode optionMode = OptionMode.Optional)
	{
		// Parameters
		if (min != null && max == null || min == null && max != null) throw new ArgumentException("Parameterfout: minTime of maxTime niet ingevuld.");
		if (min != null && max != null) if (min > max) throw new ArgumentException("Parameterfout: minTime moet vóór maxTime liggen.");

		// Input
		string input;
		var inpParsed = TimeOnly.MinValue;
		const string formaat = "UU:MM:SS";

		var (left, top) = Console.GetCursorPosition();

		while (true)
		{
			Console.SetCursorPosition(left, top);
			Console.Write($"{label + (optionMode == OptionMode.Mandatory ? "* : " : " : "),LengthInputLabel}");

			// Input bounderies
			StartInput(formaat.Length);

			// Input
			input = Console.ReadLine()!;

			EndInput();

			if (optionMode == OptionMode.Mandatory && string.IsNullOrWhiteSpace(input))
			{
				ToonFoutBoodschap("Verplichte ingave...");
				continue;
			}

			if (!string.IsNullOrWhiteSpace(input))
			{
				if (!TimeOnly.TryParse(input, out inpParsed))
				{
					ToonFoutBoodschap($"Ongeldige Tijd ({formaat})...");
					continue;
				}

				if (min != null && max != null)
				{
					if (inpParsed < min || inpParsed > max)
					{
						ToonFoutBoodschap($"De Tijd moet liggen tussen {FormatTime(min)} en {FormatTime(max)}...");
						continue;
					}
				}
			}

			break;
		}

		return string.IsNullOrWhiteSpace(input) ? null : inpParsed;

		string FormatTime(TimeOnly? time) => time == null ? "" : $"{time.Value.Hour}:{time.Value.Minute}:{time.Value.Second}";
	}

	public static string FormatTime(this TimeOnly t) => $"{t.Hour}:{t.Minute}:{t.Second}";

	// ---------
	// LeesGetal
	// ---------
	public static T? LeesGetal<T>(string label, T? min, T? max, OptionMode optionMode = OptionMode.Optional) where T : INumber<T>, IMinMaxValue<T>
	{
		// Parameters
		min ??= T.MinValue;
		max ??= T.MaxValue;

		if (min > max) throw new ArgumentException("Parameterfout: minimum moet vóór maximum liggen.");

		// RegEx
		string strRegEx;
		var length = 15;

		switch (Type.GetTypeCode(typeof(T)))
		{
			case TypeCode.Byte:
				strRegEx = null!;
				length = 3;
				break;
			case TypeCode.Char:
				strRegEx = null!;
				length = 1;
				break;
			case TypeCode.DateTime:
				strRegEx = null!;
				length = 19;
				break;
			case TypeCode.Decimal:
				strRegEx = @$"^[+-]?(\d*{CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator})?\d+$";
				length = 10;
				break;
			case TypeCode.Double:
				strRegEx = @$"^[+-]?(\d*{CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator})?\d+$";
				length = 10;
				break;
			case TypeCode.Int16:
				strRegEx = @"^[+-]?\d+$";
				length = 6;
				break;
			case TypeCode.Int32:
				strRegEx = @"^[+-]?\d+$";
				length = 11;
				break;
			case TypeCode.Int64:
				strRegEx = @"^[+-]?\d+$";
				length = 20;
				break;
			case TypeCode.SByte:
				strRegEx = null!;
				length = 3;
				break;
			case TypeCode.Single:
				strRegEx = @$"^[+-]?(\d*{CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator})?\d+$";
				length = 10;
				break;
			case TypeCode.UInt16:
				strRegEx = @"^\d+$";
				length = 5;
				break;
			case TypeCode.UInt32:
				strRegEx = @"^\d+$";
				length = 10;
				break;
			case TypeCode.UInt64:
				strRegEx = @"^\d+$";
				length = 20;
				break;
			default:
				strRegEx = @"^$";
				break;
		}

		var regEx = new Regex(strRegEx);

		// Input
		string? input;
		var inpParsed = T.Zero;

		var (left, top) = Console.GetCursorPosition();

		while (true)
		{
			// Read Input
			input = LeesRegex(label, regEx, length, optionMode);

			if (!string.IsNullOrWhiteSpace(input))
			{
				if (!T.TryParse(input, null, out inpParsed))
				{
					ToonFoutBoodschap("Ongeldig getal...");
					Console.SetCursorPosition(left, top);
					continue;
				}

				if (inpParsed < min || inpParsed > max)
				{
					ToonFoutBoodschap($"Het getal moet liggen tussen {min} en {max}...");
					Console.SetCursorPosition(left, top);
					continue;
				}
			}

			break;
		}

		return string.IsNullOrWhiteSpace(input) ? default(T) : inpParsed;
	}

	// -------
	// LeesInt
	// -------
	public static int? LeesInt(string label = "", int min = int.MinValue, int max = int.MaxValue, OptionMode optionMode = OptionMode.Optional)
	{
		// Parameters
		if (min > max) throw new ArgumentException("Parameterfout: minimum moet vóór maximum liggen.");

		// Input
		string? input;
		var inpParsed = 0;

		var (left, top) = Console.GetCursorPosition();

		while (true)
		{
			// Read Input
			input = LeesRegex(label, new Regex(@"^[+-]?\d+$"), 11, optionMode);

			if (!string.IsNullOrWhiteSpace(input))
			{
				if (!int.TryParse(input, out inpParsed))
				{
					ToonFoutBoodschap("Ongeldig getal...");
					Console.SetCursorPosition(left, top);
					continue;
				}

				if (inpParsed < min || inpParsed > max)
				{
					ToonFoutBoodschap($"Het getal moet liggen tussen {min} en {max}...");
					Console.SetCursorPosition(left, top);
					continue;
				}
			}

			break;
		}

		return string.IsNullOrWhiteSpace(input) ? null : inpParsed;
	}

	// -----------
	// LeesDecimal
	// -----------
	public static decimal? LeesDecimal(string label = "", decimal min = decimal.MinValue, decimal max = decimal.MaxValue, OptionMode optionMode = OptionMode.Optional)
	{
		// Parameters
		if (min > max) throw new ArgumentException("Parameterfout: minimum moet vóór maximum liggen.");

		// Input
		string? input;
		var inpParsed = 0m;

		var (left, top) = Console.GetCursorPosition();

		while (true)
		{
			// Read Input
			input = LeesRegex(label, new Regex(@$"^[+-]?(\d*{CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator})?\d+$"), 20, optionMode);

			if (!string.IsNullOrWhiteSpace(input))
			{
				if (!decimal.TryParse(input, out inpParsed))
				{
					ToonFoutBoodschap("Ongeldig getal...");
					Console.SetCursorPosition(left, top);
					continue;
				}

				if (inpParsed < min || inpParsed > max)
				{
					ToonFoutBoodschap($"Het getal moet liggen tussen {min} en {max}...");
					Console.SetCursorPosition(left, top);
					continue;
				}
			}

			break;
		}

		return string.IsNullOrWhiteSpace(input) ? null : inpParsed;
	}

	// ---------
	// LeesFloat
	// ---------
	public static float? LeesFloat(string label = "", float min = float.MinValue, float max = float.MaxValue, OptionMode optionMode = OptionMode.Optional)
	{
		// Parameters
		if (min > max) throw new ArgumentException("Parameterfout: minimum moet vóór maximum liggen.");

		// Input
		string? input;
		var inpParsed = 0f;

		var (left, top) = Console.GetCursorPosition();

		while (true)
		{
			// Read Input
			input = LeesRegex(label, new Regex(@$"^[+-]?(\d*{CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator})?\d+$"), 20, optionMode);

			if (!string.IsNullOrWhiteSpace(input))
			{
				if (!float.TryParse(input, out inpParsed))
				{
					ToonFoutBoodschap("Ongeldig getal...");
					Console.SetCursorPosition(left, top);
					continue;
				}

				if (inpParsed < min || inpParsed > max)
				{
					ToonFoutBoodschap($"Het getal moet liggen tussen {min} en {max}...");
					Console.SetCursorPosition(left, top);
					continue;
				}
			}

			break;
		}

		return string.IsNullOrWhiteSpace(input) ? null : inpParsed;
	}

	// --------
	// LeesBool
	// --------
	public static bool? LeesBool(string label = "(+/-)", OptionMode optionMode = OptionMode.Optional)
	{
		string input;

		var (left, top) = Console.GetCursorPosition();

		while (true)
		{
			input = string.Empty;

			// Label
			Console.SetCursorPosition(left, top);
			Console.Write($"{label + (label == "" ? "" : " ") + "(+/-)" + (optionMode == OptionMode.Mandatory ? "* : " : " : "),LengthInputLabel}");

			// Input bounderies
			StartInput(1);

			// Read Input
			ConsoleKeyInfo keyInfo;

			do
			{
				keyInfo = Console.ReadKey(true);

				if (keyInfo.Key != ConsoleKey.Backspace && keyInfo.Key != ConsoleKey.Enter && input.Length < 1)
				{
					if (keyInfo.KeyChar == '+' || keyInfo.KeyChar == '-')
					{
						input += keyInfo.KeyChar;
						Console.Write(keyInfo.KeyChar);
					}
				}
				else
				{
					if (keyInfo.Key == ConsoleKey.Backspace && input.Length > 0)
					{
						input = input[..^1];        //input = input.Substring(0, (input.Length - 1));
						Console.Write("\b \b");
					}
				}
			}
			while (keyInfo.Key != ConsoleKey.Enter);

			EndInput();

			Console.WriteLine();

			// Validations
			if (optionMode == OptionMode.Mandatory && string.IsNullOrWhiteSpace(input))
			{
				ToonFoutBoodschap("Verplichte ingave...");
				Console.SetCursorPosition(left, top);
				continue;
			}

			//if (!(input == "+" || input == "-"))
			//{
			//    ToonFoutBoodschap("Ongeldige keuze (+/-)...");
			//    Console.SetCursorPosition(labelCursorPos.Left, labelCursorPos.Top);
			//    continue;
			//}

			// Finish
			if (input == null) break;	// ???

			input = input.ToUpper();

			break;
		}

		return input == "+" ? true : (input == "-" ? false : null);
	}

	// ---------
	// LeesLijst
	// ---------
	// Show List with/without pagination and with/without alternating colors
	public static void DisplayList(List<string> displayValues, SelectionMode selectionMode)
	{
		// Save CursorPosition of starting page
		var cursorLeftSave = Console.CursorLeft;
		var cursorTopSave = Console.CursorTop;

		// Init for Paging
		var pageCount = 1;  // In case no paging is used
		var lineCount = displayValues.Count;
		var totEinde = false;

		if (ListPageSize > 0) pageCount = ((lineCount + ListPageSize - 1) / ListPageSize);

		// - - - - - - - - - - - - - -
		// For every line in the list
		// - - - - - - - - - - - - - -
		for (var seq = 0; seq < lineCount; seq++)
		{
			// - - - - - - - - - 
			// Display the Line
			// - - - - - - - - - 
			// Text to display
			var displayText = string.Empty;

			if (selectionMode != SelectionMode.None) displayText = $"{string.Format("{0:0000}", seq + 1),-8}";

			//displayText = $"{string.Format("{0:0000}", seq + 1),-8}";   // !!!Test!!!

			displayText = $"{displayText}{displayValues.ElementAt(seq)} ";

			// Display with/without alternating color
			var altBGC = ConsBGC;
			var altFGC = ConsFGC;

			if (AlternateRows)
				if (seq % 2 == 0)
				{
					altBGC = darkMode ? Ansi.BDBLUE : Ansi.BDGRAY;
					altFGC = darkMode ? Ansi.FDGRAY : Ansi.FDBLUE;
				}

			// Write text
			Console.WriteLine($"{altFGC}{altBGC}{displayText}{Ansi.EraseFromCursorToEndLine}{ConsFGC}{ConsBGC}");

			// - - - -
			// Paging
			// - - - -
			var isPaging = ListPageSize > 0 && !totEinde;

			if (isPaging)
			{
				// End & PgUp
				var currentPage = (seq / ListPageSize) + 1;
				var isFirstPage = currentPage == 1;
				var isLastPage = currentPage == pageCount;
				var isLastLineOfList = seq + 1 == lineCount;
				var isLastLineOfPage = ((seq + 1) % ListPageSize == 0) || (isLastPage && isLastLineOfList);
				var isOnePage = ListPageSize >= lineCount;

				// Show PagingMenu
				if (!isOnePage)
					if (isLastLineOfPage)
					{
						// Init
						var choice = ConsoleKey.A;   // Init on not-used value

						// Last line of list: fill the rest of the page with empty lines
						if (isLastLineOfList)
							for (var i = lineCount; i < currentPage * ListPageSize; i++)
								Console.Write($"-{Ansi.EraseFromCursorToEndLine}\n");

						// Show/Read PagingMenu
						var keuzes = new List<string>();

						if (!isFirstPage) keuzes.Add($"{ConsFGC}<{Ansi.FDRED}Home{ConsFGC}>=Eerste");           // ←
						if (!isLastLineOfList) keuzes.Add($"{ConsFGC}<{Ansi.FDRED}PgDwn{ConsFGC}>=Volgende");   // ↓
						if (!isFirstPage) keuzes.Add($"{ConsFGC}<{Ansi.FDRED}PgUp{ConsFGC}>=Vorige");           // ↑
						if (!isLastLineOfList) keuzes.Add($"{ConsFGC}<{Ansi.FDRED}End{ConsFGC}>=Laatste");      // →
						if (!(isOnePage || isLastPage)) keuzes.Add($"{ConsFGC}Scroll <{Ansi.FDRED}E{ConsFGC}>inde");

						keuzes.Add($"{ConsFGC}<{Ansi.FDRED}S{ConsFGC}>top lijst");
						keuzes.Add($"{ConsFGC}Aantal: {lineCount}");
						keuzes.Add($"{ConsFGC}Blz: {currentPage}/{pageCount}");

						Console.WriteLine($"\n{ConsBGC}{ConsFGC}{string.Join($" {Ansi.FDYELLOW}|{Ansi.FNORMAL} ", keuzes)}{ConsBGC}{ConsFGC}{Ansi.EraseFromCursorToEndLine}");
						Ansi.CursorHide(true);

						// Input for Paging
						while (!(
							   (choice == ConsoleKey.PageUp && !isFirstPage)
							|| (choice == ConsoleKey.PageDown && !isLastLineOfList)
							|| choice == ConsoleKey.S
							|| (choice == ConsoleKey.E && !(isOnePage || isLastPage))
							|| (choice == ConsoleKey.Home && !isFirstPage)
							|| (choice == ConsoleKey.End && !isLastLineOfList)
							))
							choice = Console.ReadKey(true).Key;

						Ansi.CursorHide(false);

						// Perform Paging
						// Stop List
						if (choice == ConsoleKey.S) return;

						// Scroll till the end of the list
						if (choice == ConsoleKey.E) totEinde = true;

						// Previous page
						//if (choice == ConsoleKey.UpArrow)
						if (choice == ConsoleKey.PageUp)
						{
							if (isLastPage)
							{
								if (lineCount % ListPageSize == 0) seq = lineCount - ListPageSize * 2 - 1;
								else seq -= ListPageSize + lineCount % ListPageSize;
							}
							else seq -= (ListPageSize * 2);

							if (seq < 0) seq = -1;  // Should not be possible
						}

						// Default behaviour: next page
						//if (choice == ConsoleKey.PageDown)

						// Position on First page
						if (choice == ConsoleKey.Home) seq = -1;

						// Position on Last page
						if (choice == ConsoleKey.End)
						{
							if (lineCount % ListPageSize == 0) seq = lineCount - ListPageSize - 1;
							else seq = lineCount - lineCount % ListPageSize - 1;
						}

						// Position cursor for choosen page
						if (choice == ConsoleKey.PageUp || choice == ConsoleKey.PageDown || choice == ConsoleKey.Home || choice == ConsoleKey.End)
							Console.SetCursorPosition(cursorLeftSave, cursorTopSave);

						if (choice == ConsoleKey.E) Console.WriteLine("\n");
					}
			}
		}
	}

	// LeesLijst
	public static List<object> LeesLijst(string titel, IEnumerable<object> l, List<string> displayValues, SelectionMode selectionMode = SelectionMode.None, OptionMode optionMode = OptionMode.Optional)
	{
		var lijst = l;
		var lineCount = displayValues.Count;
		var okLijst = true;

		ToonTitel($"{titel}");

		List<object> gekozenObjecten = [];      //List<object> gekozenObjecten = new List<object>();

		//var labelCursorPos = Console.GetCursorPosition();

		while (true)
		{
			if (!displayValues.Any())
			{
				Console.WriteLine();
				ToonFoutBoodschap("Lege lijst");
				break;
			}

			if (okLijst)    // Only First time
			{
				Console.WriteLine();
				DisplayList(displayValues, selectionMode);
				Console.WriteLine();
			}

			// Multiple selection
			if (selectionMode == SelectionMode.Multiple)
			{
				var (left, top) = Console.GetCursorPosition();

				var seqKeuzes = LeesString($"Geef de volgnummers uit de lijst (gescheiden door een comma)", 0, 1000, optionMode)!;

				okLijst = true;

				if (optionMode == OptionMode.Optional && string.IsNullOrWhiteSpace(seqKeuzes)) break;

				var keuzes = seqKeuzes.Split(',').Distinct().ToArray();

				gekozenObjecten.Clear();

				// Validate
				foreach (var keuze in keuzes)
				{
					if (int.TryParse(keuze, out int intKeuze))
					{
						if (intKeuze > 0 & intKeuze <= lineCount)
							gekozenObjecten.Add(lijst.ElementAt(intKeuze - 1));
						else
						{
							ToonFoutBoodschap($"Ongeldige keuze.  Keuze tussen 1 en {lineCount}.  Probeer opnieuw...");
							Console.SetCursorPosition(left, top);
							okLijst = false;
							break;
						}
					}
					else
					{
						ToonFoutBoodschap("De lijst mag enkel cijfers bevatten...");
						Console.SetCursorPosition(left, top);
						okLijst = false;
						break;
					}
				}

				if (okLijst) break;
			}

			// Single Selection
			if (selectionMode == SelectionMode.Single)
			{
				var leesInt = LeesInt($"Geef het volgnummer uit de lijst", 1, lineCount, optionMode);

				if (leesInt == null) break; // Only for optional

				gekozenObjecten.Add(lijst.ElementAt((int)leesInt - 1));
				break;
			}

			// No Selection (display only)
			if (selectionMode == SelectionMode.None) break;
		}

		return gekozenObjecten;
	}

	// ------------------------
	// LeesKeuzeUitSimpeleLijst
	// ------------------------
	public static object? LeesKeuzeUitLijst(string label, List<object> keuzeLijst, OptionMode optionMode = OptionMode.Optional)
	{
		string input;
		var keuzes = " (" + string.Join(", ", keuzeLijst) + ")";

		var (left, top) = Console.GetCursorPosition();

		while (true)
		{
			Console.SetCursorPosition(left, top);
			input = LeesString(label + keuzes, 0, keuzeLijst.Max(i => i == null ? 0 : i.ToString()!.Length), optionMode)!;

			if (string.IsNullOrWhiteSpace(input)) break;    // Only for Optional

			if (!keuzeLijst.Contains(input))
			{
				ToonFoutBoodschap("Verkeerde keuze...");
				Console.SetCursorPosition(left, top);
				continue;
			}

			break;
		}

		return string.IsNullOrWhiteSpace(input) ? null : keuzeLijst.ElementAt(keuzeLijst.IndexOf(input));
	}

	// ---------
	// LeesRegEx
	// ---------
	public static string? LeesRegex(string label, Regex regex, int length = 100, OptionMode optionMode = OptionMode.Optional)
	{
		string input;

		var (left, top) = Console.GetCursorPosition();

		while (true)
		{
			Console.SetCursorPosition(left, top);
			//input = LeesString(label, 0, 100, optionMode)!;
			input = LeesString(label, 0, length, optionMode)!;

			if (string.IsNullOrWhiteSpace(input)) break;    // Only for Optional

			if (!regex.Match(input).Success)
			{
				//Console.WriteLine();
				ToonFoutBoodschap("Verkeerd formaat...");
				Console.SetCursorPosition(left, top);
				continue;
			}

			break;
		}

		return string.IsNullOrWhiteSpace(input) ? null : input;
	}

	// ------------------
	// LeesTelefoonnummer
	// ------------------
	public static string? LeesTelefoonNummer(string label = "Telefoonnummer", OptionMode optionMode = OptionMode.Optional)
		=> LeesRegex(label, new Regex(@"^((\+|00(\s|\s?\-\s?)?)32(\s|\s?\-\s?)?(\(0\)[\-\s]?)?|0)[1-9]((\s|\s?\-\s?)?[0-9])((\s|\s?-\s?)?[0-9])((\s|\s?-\s?)?[0-9])\s?[0-9]\s?[0-9]\s?[0-9]\s?[0-9]\s?[0-9]$"), 15, optionMode);    // 0479 97 60 44

	// --------------
	// LeesEmailAdres
	// --------------
	public static string? LeesEmailAdres(string label = "Emailadres", OptionMode optionMode = OptionMode.Optional)
		=> LeesRegex(label, new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"), 50, optionMode); // j.v@a.com
																									//@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";

	// --------------
	// LeesWebsiteUrl
	// --------------
	public static string? LeesWebsiteUrl(string label = "Website", OptionMode optionMode = OptionMode.Optional)
		=> LeesRegex(label, new Regex(@"(?<Protocol>\w+):\/\/(?<Domain>[\w@][\w.:@]+)\/?[\w\.?=%&=\-@/$,]*"), 50, optionMode);
	//Regex rx = new Regex(@"http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?", RegexOptions.Compiled);

	// --------
	// LeesEnum
	// --------
	public static List<object> LeesEnum<TEnum>(string label, SelectionMode selectionMode = SelectionMode.Single, OptionMode optionMode = OptionMode.Optional) where TEnum : struct, IConvertible, IComparable, IFormattable
	{
		var type = typeof(TEnum);

		if (!type.IsEnum) throw new ArgumentException("TEnum moet een enumerated type zijn.");

		var lijst = new List<string>();

		foreach (var item in Enum.GetValues(type)) lijst.Add(Enum.GetName(type, item)!);

		var input = LeesLijst(label, lijst, lijst, selectionMode, optionMode);

		//return string.IsNullOrWhiteSpace(input) ? null : input;
		return input;
	}

	// ------------
	// LeesPaswoord
	// ------------
	public static string? LeesPaswoord(string label = "Paswoord", int minlengte = 8, int maxLengte = 64, OptionMode optionMode = OptionMode.Optional)
	{
		//var input = new SecureString();
		string input;

		var (left, top) = Console.GetCursorPosition();

		while (true)
		{
			// - - - 
			// Input
			// - - - 
			//input = LeesString(label, 0, maxLengte, optionMode)!;

			Console.Write($"{label + (optionMode == OptionMode.Mandatory ? "* : " : " : "),LengthInputLabel}");

			// Input bounderies
			StartInput(maxLengte);

			input = string.Empty;

			ConsoleKeyInfo keyInfo;

			do
			{
				keyInfo = Console.ReadKey(true);

				//if (keyInfo.Key != ConsoleKey.Backspace && keyInfo.Key != ConsoleKey.Enter && input.Length < maxLengte)
				if (!char.IsControl(keyInfo.KeyChar) && keyInfo.KeyChar != ' ' && input.Length < maxLengte)
				{
					input += keyInfo.KeyChar;
					Console.Write("*");
				}
				else
				{
					if (keyInfo.Key == ConsoleKey.Backspace && input.Length > 0)
					{
						input = input[..^1];        //input = input.Substring(0, (input.Length - 1));
						Console.Write("\b \b");
					}
				}
			}
			while (keyInfo.Key != ConsoleKey.Enter);

			EndInput();

			Console.WriteLine();

			// - - - - - -
			// Validation
			// - - - - - -
			if (optionMode == OptionMode.Mandatory && string.IsNullOrWhiteSpace(input))
			{
				ToonFoutBoodschap($"Verplichte ingave...");
				Console.SetCursorPosition(left, top);
				continue;
			}

			if (string.IsNullOrWhiteSpace(input)) break;    // Only for Optional

			// Minimum eight characters, one uppercase letter, one lowercase letter, one number and one special character:
			var regex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&_])[A-Za-z\d@$!%*?&_]{" + minlengte + "," + maxLengte + "}$");

			if (!regex.Match(input).Success)
			{
				ToonFoutBoodschap($"Het paswoord moet minstens 1 kleine letter, 1 hoofdletter, 1 cijfer bevatten en één speciaal teken.\nHet paswoord moet minstens {minlengte} en maximum {maxLengte} tekens lang zijn.");
				Console.SetCursorPosition(left, top);
				continue;
			}

			break;
		}

		return string.IsNullOrWhiteSpace(input) ? null : input;
	}

	// ==========================================
	// - - - - - - - - O U T P U T - - - - - - - 
	// ==========================================
	// ----------------
	// ToonBoodschappen
	// ----------------
	private enum TextSoort { Fout, Info, Warning, Success }
	public static void ToonFoutBoodschap(string tekst) => ToonTekst($"{ConvertToonText(tekst, TextSoort.Fout)}", $"{Ansi.FgRgb(252, 3, 28)}");
	public static void ToonInfoBoodschap(string tekst) => ToonTekst($"{ConvertToonText(tekst, TextSoort.Info)}", $"{Ansi.FgRgb(32, 3, 252)}");
	public static void ToonWarningBoodschap(string tekst) => ToonTekst($"{ConvertToonText(tekst, TextSoort.Warning)}", $"{Ansi.FgRgb(252, 161, 3)}");
	public static void ToonSuccessBoodschap(string tekst) => ToonTekst($"{ConvertToonText(tekst, TextSoort.Success)}", $"{Ansi.FgRgb(3, 252, 7)}");
	public static void ToonTekst(string tekst) => Console.WriteLine($"{ConsFGC}{ConsBGC}{tekst}");
	public static void ToonTekst(string tekst, string fgColor) => Console.WriteLine($"{fgColor}{ConsBGC}{tekst}{ConsFGC}");
	public static void ToonTekst(string tekst, string fgColor, string bgColor) => Console.WriteLine($"{fgColor}{bgColor}{tekst}{ConsFGC}{ConsBGC}");

	private static string ConvertToonText(string text, TextSoort textSoort)
	{
		string newText = string.Empty, preText = string.Empty;
		var endPre = false;

		foreach (var c in text)
			// BS, HT, LF, VT, FF, CR
			if (!endPre && c > 7 && c < 14) preText += c;
			else
			{
				newText += c;
				endPre = true;
			}

		return $"{preText}{(textSoort == TextSoort.Fout ? "❌ " : textSoort == TextSoort.Info ? "✋ " : textSoort == TextSoort.Warning ? "❗❗" : textSoort == TextSoort.Success ? "✅ " : "")}{newText}";
	}

	// ---------
	// ToonTitel
	// ---------
	public static void ToonTitel(string titel, OptionMode optionMode = OptionMode.Optional)
		=> Console.Write($"\n{titel + (optionMode == OptionMode.Mandatory ? "*" : "")}");

	//// ------------------
	//// WriteLineWithColor
	//// ------------------
	//public static void WriteLine(string tekst, ConsoleColor? color = null)
	//{
	//	if (color.HasValue)
	//	{
	//		var oldColor = System.Console.ForegroundColor;

	//		if (color == oldColor) Console.WriteLine(tekst);
	//		else
	//		{
	//			Console.ForegroundColor = color.Value;
	//			Console.WriteLine(tekst);
	//			Console.ForegroundColor = oldColor;
	//		}
	//	}
	//	else Console.WriteLine(tekst);
	//}

	//public static void WriteLine(string tekst, string color)
	//{
	//	if (string.IsNullOrEmpty(color))
	//	{
	//		WriteLine(tekst);
	//		return;
	//	}

	//	if (!Enum.TryParse(color, true, out ConsoleColor col)) WriteLine(tekst);
	//	else WriteLine(tekst, col);
	//}

	//public static void Write(string tekst, ConsoleColor? color = null)
	//{
	//	if (color.HasValue)
	//	{
	//		var oldColor = System.Console.ForegroundColor;

	//		if (color == oldColor) Console.Write(tekst);
	//		else
	//		{
	//			Console.ForegroundColor = color.Value;
	//			Console.Write(tekst);
	//			Console.ForegroundColor = oldColor;
	//		}
	//	}
	//	else Console.Write(tekst);
	//}

	//public static void Write(string tekst, string color)
	//{
	//	if (string.IsNullOrEmpty(color))
	//	{
	//		Write(tekst);
	//		return;
	//	}

	//	//if (!ConsoleColor.TryParse(color, true, out ConsoleColor col)) Write(tekst);
	//	if (!Enum.TryParse(color, true, out ConsoleColor col)) Write(tekst);
	//	else Write(tekst, col);
	//}

	//private static readonly Lazy<Regex> ColorBlockRegEx = new(() => new Regex("\\[(?<color>.*?)\\](?<text>[^[]*)\\[/\\k<color>\\]", RegexOptions.IgnoreCase), isThreadSafe: true);

	//public static void WriteWithColor(string text, ConsoleColor? baseTextColor = null)  // Red, Green, Yellow, Blue, Magenta, Cyan, Grey
	//{
	//	// Default color
	//	baseTextColor ??= Console.ForegroundColor;

	//	// Nothing to write : \n
	//	if (string.IsNullOrEmpty(text))
	//	{
	//		WriteLine(string.Empty);    // Console.WriteLine();
	//		return;
	//	}

	//	// Check if any color in line
	//	var at1 = text.IndexOf("[");
	//	var at2 = text.IndexOf("]");

	//	if (at1 == -1 || at2 <= at1)
	//	{
	//		//WriteLine(text, baseTextColor);
	//		Write(text, baseTextColor);
	//		return;
	//	}

	//	// For all color parts
	//	while (true)
	//	{
	//		var match = ColorBlockRegEx.Value.Match(text);

	//		if (match.Length < 1)
	//		{
	//			Write(text, baseTextColor);
	//			break;
	//		}

	//		// write up to expression
	//		Write(text[..match.Index], baseTextColor);      //Write(text.Substring(0, match.Index), baseTextColor);

	//		// strip out the expression
	//		var highlightText = match.Groups["text"].Value;
	//		var colorVal = match.Groups["color"].Value;

	//		Write(highlightText, colorVal);

	//		// remainder of string
	//		text = text[(match.Index + match.Value.Length)..];      //text = text.Substring(match.Index + match.Value.Length);
	//	}
	//}

	//public static void WriteLineWithColor(string text, ConsoleColor? baseTextColor = null) => WriteWithColor($"{text}\n", baseTextColor);

	public static string Bool2PlusMin(bool? b) => b == null ? " " : (bool)b ? $"+" : $"-";
	public static string Bool2Icon(bool? b) => b == null ? "  " : (bool)b ? $"✅" : $"❌";  // 2 bytes

	/// public static string Bool2Color(bool b) => b ? $"[green]{b}[/green]" : $"[red]{b}[/red]";
	/// public static string Bool2Color(bool b) => b ? $"[green]{b}[/green]" : $"[red]{b}[/red]";
	public static string Bool2Color(bool b) => b ? $"{Ansi.FDGREEN}{b}{ConsFGC}" : $"{Ansi.FDRED}{b}{ConsFGC}";

	/// public static string Bool2ColorIcon(bool? b) => b == null ? "  " : (bool)b ? $"[green]✅[/green]" : $"[red]❌[/red]";  // 2 bytes
	/// public static string Bool2ColorIcon(bool? b) => b == null ? "  " : (bool)b ? $"{AC.FDGREEN}✅{AC.FNORMAL}" : $"{AC.FDRED}❌{AC.FNORMAL}";  // 2 bytes
	public static string Bool2ColorIcon(bool? b) => b == null ? "  " : (bool)b ? $"{Ansi.FDGREEN}✅{ConsFGC}" : $"{Ansi.FDRED}❌{ConsFGC}";  // 2 bytes
}

// -------------------------
// Ansi Console Escape Codes (XTerm)
// -------------------------
public static class Ansi
{
	// https://en.wikipedia.org/wiki/ANSI_escape_code
	// https://learn.microsoft.com/en-us/windows/console/console-virtual-terminal-sequences

	// - - - - - - - - - 
	// Windows registery
	// - - - - - - - - - 
	//reg add HKEY_CURRENT_USER\Console /v VirtualTerminalLevel /t REG_DWORD /d 0x00000001 /f
	//reg add HKEY_CURRENT_USER\Console /v VirtualTerminalLevel /t REG_DWORD /d 0x00000000 /f

	// ---------------------
	// Enable Ansi Sequences (if needed)
	// ---------------------
	[DllImport("kernel32.dll", SetLastError = true)]
	private static extern IntPtr GetStdHandle(int nStdHandle);

	[DllImport("kernel32.dll")]
	private static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

	[DllImport("kernel32.dll")]
	private static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);

	[DllImport("kernel32.dll")]
	private static extern uint GetLastError();

	// P/Invoke declarations
	private const int STD_OUTPUT_HANDLE = -11;
	private const uint ENABLE_VIRTUAL_TERMINAL_PROCESSING = 4;
	private const uint DISABLE_NEWLINE_AUTO_RETURN = 0x0008;

	// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
	// C0 - single byte command (7bit control codes, byte range
	// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

	// Mnemonic		Name					Sequence	ShortDescription										Support		decimal	octal	hex		C-escape	Ctrl-Key
	// NUL			Null					\0, \x00	NUL is ignored.											✓			0		000		0x00
	// BEL			Bell					\a, \x07	Ring the bell.more										✓			7		007		0x07	\a			^G
	// BS			Backspace				\b, \x08	Move the cursor one position to the left.more			✓			8		010		0x08	\b			^H
	// HT			Horizontal Tabulation	\t, \x09	Move the cursor to the next character tab stop.			✓			9		011		0x09	\t			^I
	// LF			Line Feed				\n, \x0A	Move the cursor one row down, scrolling if needed.more	✓			10		012		0x0A	\n			^J
	// VT			Vertical Tabulation		\v, \x0B	Treated as LF.											✓			11		013		0x0B	\v			^K
	// FF			Form Feed				\f, \x0C	Treated as LF.											✓			12		014		0x0C	\f			^L
	// CR			Carriage Return			\r, \x0D	Move the cursor to the beginning of the row.			✓			13		015		0x0D	\r			^M
	// SO			Shift Out				    \x0E	Switch to an alternative character set.					Partial
	// SI			Shift In				    \x0F	Return to regular character set after Shift Out.		✓
	// ESC			Escape					\e, \x1B	Start of a sequence.Cancels any other sequence.			✓
	// DEL	127		177		0x7F	<none>	<none>			Delete character

	public static readonly string NUL = "\x00";
	public static readonly string BEL = "\a";
	public static readonly string BS = "\b";
	public static readonly string HT = "\t";
	public static readonly string LF = "\n";
	public static readonly string VT = "\v";
	public static readonly string FF = "\f";
	public static readonly string CR = "\r";
	public static readonly string SO = "\x0E";
	public static readonly string SI = "\x0F";
	public static readonly string ESC = "\e"; // \x1b";
	public static readonly string DEL = "\x7f"; // 

	// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
	// C1 - single byte command (8bit control codes, byte range 
	// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

	// Mnemonic		Name							Sequence	ShortDescription									Support
	// IND			Index							\x84		Move the cursor one line down scrolling if needed.	✓
	// NEL			Next Line						\x85		Move the cursor to the beginning of the next row.	✓
	// HTS			Horizontal Tabulation Set		\x88		Places a tab stop at the current cursor position.	✓
	// DCS			Device Control String			\x90		Start of a DCS sequence.							✓
	// CSI			Control Sequence Introducer		\x9B		Start of a CSI sequence.							✓
	// ST			String Terminator				\x9C		Terminator used for string type sequences.			✓
	// OSC			Operating System Command		\x9D		Start of an OSC sequence.							✓
	// PM			Privacy Message					\x9E		Start of a privacy message.							✓
	// APC			Application Program Command		\x9F		Start of an APC sequence.							✓

	public static readonly string IND = "\x84";
	public static readonly string NEL = "\x85";
	public static readonly string HTS = "\x88";
	public static readonly string DCS = "\x90"; // "\x1bP"
												//public static readonly string CSI = "\x9B"; // "\x1b["
	public static readonly string ST = "\x9C";
	public static readonly string OSC = "\x9D"; // "\x1b]"
	public static readonly string PM = "\x9E";
	public static readonly string APC = "\x9F";

	// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
	// CSI - Control Sequence Introducer: sequence starting with ESC[(7bit) or CSI(\x9B, 8bit)
	// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

	// Mnemonic		Name									Sequence		ShortDescription																	Support
	// ICH			Insert Characters						CSI Ps @		Insert Ps(blank) characters(default = 1). more										✓
	// SL			Scroll Left								CSI Ps SP @		Scroll viewport Ps times to the left.more											✓
	// CUU			Cursor Up								CSI Ps A		Move cursor Ps times up(default=1). more											✓
	// SR			Scroll Right							CSI Ps SP A		Scroll viewport Ps times to the right.more											✓
	// CUD			Cursor Down								CSI Ps B		Move cursor Ps times down (default=1). more											✓
	// CUF			Cursor Forward							CSI Ps C		Move cursor Ps times forward(default=1).											✓
	// CUB			Cursor Backward							CSI Ps D		Move cursor Ps times backward(default=1).											✓
	// CNL			Cursor Next Line						CSI Ps E		Move cursor Ps times down(default=1) and to the first column.more					✓
	// CPL			Cursor Backward							CSI Ps F		Move cursor Ps times up(default=1) and to the first column.more						✓
	// CHA			Cursor Horizontal Absolute				CSI Ps G		Move cursor to Ps-th column of the active row(default=1).							✓
	// CUP			Cursor Position							CSI Ps ; Ps H	Set cursor to position[Ps, Ps] (default = [1, 1]). more								✓
	// CHT			Cursor Horizontal Tabulation			CSI Ps I		Move cursor Ps times tabs forward(default=1).										✓
	// ED			Erase In Display						CSI Ps J		Erase various parts of the viewport.more											✓
	// DECSED		Selective Erase In Display				CSI ? Ps J		Same as ED with respecting protection flag.											✓
	// EL			Erase In Line							CSI Ps K		Erase various parts of the active row.more											✓
	// DECSEL		Selective Erase In Line					CSI ? Ps K		Same as EL with respecting protecting flag.											✓
	// IL			Insert Line								CSI Ps L		Insert Ps blank lines at active row (default=1). more								✓
	// DL			Delete Line								CSI Ps M		Delete Ps lines at active row(default=1). more										✓
	// DCH			Delete Character						CSI Ps P		Delete Ps characters(default=1). more												✓
	// SU			Scroll Up								CSI Ps S		Scroll Ps lines up(default=1).														✓
	// SD			Scroll Down								CSI Ps T		Scroll Ps lines down(default=1).													✓
	// ECH			Erase Character							CSI Ps X		Erase Ps characters from current cursor position to the right(default=1). more		✓
	// CBT			Cursor Backward Tabulation				CSI Ps Z		Move cursor Ps tabs backward(default=1).											✓
	// HPA			Horizontal Position Absolute			CSI Ps `		Same as CHA.																		✓
	// HPR			Horizontal Position Relative			CSI Ps a		Same as CUF.																		✓
	// REP			Repeat Preceding Character				CSI Ps b		Repeat preceding character Ps times(default=1). more								✓
	// DA1			Primary Device Attributes				CSI c			Send primary device attributes.														✓
	// DA2			Secondary Device Attributes				CSI > c			Send primary device attributes.														✓
	// VPA			Vertical Position Absolute				CSI Ps d		Move cursor to Ps-th row (default=1).												✓
	// VPR			Vertical Position Relative				CSI Ps e		Move cursor Ps times down(default=1).												✓
	// HVP			Horizontal and Vertical Position		CSI Ps ; Ps f	Same as CUP.																		✓
	// TBC			Tab Clear								CSI Ps g		Clear tab stops at current position(0) or all(3) (default=0). more					✓
	// SM			Set Mode								CSI Pm h		Set various terminal modes.more														Partial
	// DECSET		DEC Private Set Mode					CSI? Pm h		Set various terminal attributes.more												Partial
	// RM			Reset Mode								CSI Pm l		Set various terminal attributes. more												Partial
	// DECRST		DEC Private Reset Mode					CSI ? Pm l		Reset various terminal attributes. more												Partial
	// SGR			Select Graphic Rendition				CSI Pm m		Set/Reset various text attributes. more												Partial
	// DSR			Device Status Report					CSI Ps n		Request cursor position (CPR) with Ps = 6.											✓
	// DECDSR		DEC Device Status Report				CSI ? Ps n		Only CPR is supported (same as DSR).												Partial
	// DECRQM		Request Mode							CSI Ps $p		Request mode state.more																✓
	// DECSTR		Soft Terminal Reset						CSI ! p			Reset several terminal attributes to initial state. more							✓
	// DECSCA		Select Character Protection Attribute	CSI Ps " q		Whether DECSED and DECSEL can erase (0=default, 2) or not (1).						✓
	// DECSCUSR		Set Cursor Style						CSI Ps SP q		Set cursor style.more																✓
	// DECSTBM		Set Top and Bottom Margin				CSI Ps ; Ps r	Set top and bottom margins of the viewport[top; bottom] (default = viewport size).	✓
	// SCOSC		Save Cursor								CSI s			Save cursor position, charmap and text attributes.									Partial
	// SCORC		Restore Cursor							CSI u			Restore cursor position, charmap and text attributes.								Partial
	// DECIC		Insert Columns							CSI Ps ' }		Insert Ps columns at cursor position. more											✓
	// DECDC		Delete Columns							CSI Ps ' ~		Delete Ps columns at cursor position. more											✓

	// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
	// DCS - Device Control String: sequence starting with ESC P(7bit) or DCS(\x90, 8bit)
	// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

	// Mnemonic		Name							Sequence						ShortDescription						Support
	// SIXEL		SIXEL Graphics					DCS Ps ; Ps ; Ps ; q Pt ST		Draw SIXEL image.						External
	// DECUDK		User Defined Keys				DCS Ps ; Ps \| Pt ST			Definitions for user-defined keys.		✗
	// XTGETTCAP	Request Terminfo String			DCS + q Pt ST					Request Terminfo String.				✗
	// XTSETTCAP	Set Terminfo Data				DCS + p Pt ST					Set Terminfo Data.						✗
	// DECRQSS		Request Selection or Setting	DCS $ q Pt ST					Request several terminal settings.more	Partial

	// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
	// OSC - Operating System Command: sequence starting with ESC ] (7bit) or OSC(\x9D, 8bit)
	// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

	// Identifier	Sequence					ShortDescription											Support
	// 0			OSC 0 ; Pt BEL				Set window title and icon name.more							Partial
	// 1			OSC 1 ; Pt BEL				Set icon name.												✗
	// 2			OSC 2 ; Pt BEL				Set window title.more										✓
	// 4			OSC 4 ; c ; spec BEL		Change color number c to the color specified by spec.more	✓
	// 8			OSC 8 ; params ; uri BEL	Create a hyperlink to uri using params. more				✓
	// 10			OSC 10 ; Pt BEL				Set or query default foreground color.more					✓
	// 11			OSC 11 ; Pt BEL				Same as OSC 10, but for default background.					✓
	// 12			OSC 12 ; Pt BEL				Same as OSC 10, but for default cursor color.				✓
	// 104			OSC 104 ; c BEL				Reset color number c to themed color.more					✓
	// 110			OSC 110 BEL					Restore default foreground to themed color.					✓
	// 111			OSC 111 BEL					Restore default background to themed color.					✓
	// 112			OSC 112 BEL					Restore default cursor to themed color.						✓

	// - - - - - - - - - - - - - - - - - - - 
	// ESC: sequence starting with ESC(\x1B)
	// - - - - - - - - - - - - - - - - - - - 

	// Mnemonic	Name							Sequence	ShortDescription										Support
	// SC		Save Cursor						ESC 7		Save cursor position, charmap and text attributes.		✓
	// RC		Restore Cursor					ESC 8		Restore cursor position, charmap and text attributes.	✓
	// DECALN	Screen Alignment Pattern		ESC # 8		Fill viewport with a test pattern (E).					✓
	// IND		Index							ESC D		Move the cursor one line down scrolling if needed.		✓
	// NEL		Next Line						ESC E		Move the cursor to the beginning of the next row.		✓
	// HTS		Horizontal Tabulation Set		ESC H		Places a tab stop at the current cursor position.		✓
	// IR		Reverse Index					ESC M		Move the cursor one line up scrolling if needed.		✓
	// DCS		Device Control String			ESC P		Start of a DCS sequence.								✓
	// CSI		Control Sequence Introducer		ESC[		Start of a CSI sequence.								✓
	// ST		String Terminator				ESC \		Terminator used for string type sequences.				✓
	// OSC		Operating System Command		ESC]		Start of an OSC sequence.								✓
	// PM		Privacy Message					ESC ^		Start of a privacy message.								✓
	// APC		Application Program Command		ESC _		Start of an APC sequence.								✓

	public static readonly string SC = $"{ESC}7";
	public static readonly string RC = $"{ESC}8";
	public static readonly string DECALN = $"{ESC}#8";
	//public static readonly string IND = $"{ESC}D";
	//public static readonly string NEL = $"{ESC}E";
	//public static readonly string HTS = $"{ESC}H";
	public static readonly string IR = $"{ESC}M";
	//public static readonly string DCS = $"{ESC}P";
	public static readonly string Csi = $"{ESC}[";
	//public static readonly string ST = $"{ESC}\";
	//public static readonly string OSC = $"{ESC}]";
	//public static readonly string PM = $"{ESC}^";
	//public static readonly string APC = $"{ESC}_";

	// - - - -
	// Colors
	// - - - -

	// 8-16 Colors
	// ColorName		    ForegroundColorCode	BackgroundColorCodeBright
	// Black		    	30					40
	// Red				    31					41
	// Green			    32					42
	// Yellow		    	33					43
	// Blue				    34					44
	// Magenta	    		35					45
	// Cyan			    	36					46
	// White			    37					47
	// Foreground Extended  38                  48  Applies extended color value to the foreground/background
	// Foreground Default   39                  49  Applies only the foreground/background portion of the defaults(see 0)
	//
	// Bright Black		    90					100
	// Bright Red		    91					101
	// Bright Green	    	92					102
	// Bright Yellow	    93					103
	// Bright Blue	    	94					104
	// Bright Magenta   	95					105
	// Bright Cyan		    96					106
	// Bright White		    97					107
	//
	// Default			    39					49				Use Default color to reset colors only
	// Reset			    0					0				The Reset color is the reset code that resets all colors and text effects

	// ForeGround
	public static readonly string FNORMAL = Console.IsOutputRedirected ? "" : $"{Csi}39m";    // foreground color at startup

	// ForeGround - Light - 0; (NotBold)
	public static readonly string FLGRAY = Console.IsOutputRedirected ? "" : $"{Csi}90m";       // $"{CSI}30;1m"
	public static readonly string FLRED = Console.IsOutputRedirected ? "" : $"{Csi}91m";
	public static readonly string FLGREEN = Console.IsOutputRedirected ? "" : $"{Csi}92m";
	public static readonly string FLYELLOW = Console.IsOutputRedirected ? "" : $"{Csi}93m";
	public static readonly string FLBLUE = Console.IsOutputRedirected ? "" : $"{Csi}94m";
	public static readonly string FLMAGENTA = Console.IsOutputRedirected ? "" : $"{Csi}95m";
	public static readonly string FLCYAN = Console.IsOutputRedirected ? "" : $"{Csi}96m";
	public static readonly string FLWHITE = Console.IsOutputRedirected ? "" : $"{Csi}97m";

	// ForeGround - Dark - 1; (Bold)
	public static readonly string FDBLACK = Console.IsOutputRedirected ? "" : $"{Csi}30m";
	public static readonly string FDRED = Console.IsOutputRedirected ? "" : $"{Csi}31m";
	public static readonly string FDGREEN = Console.IsOutputRedirected ? "" : $"{Csi}32m";
	public static readonly string FDYELLOW = Console.IsOutputRedirected ? "" : $"{Csi}33m";
	public static readonly string FDBLUE = Console.IsOutputRedirected ? "" : $"{Csi}34m";
	public static readonly string FDMAGENTA = Console.IsOutputRedirected ? "" : $"{Csi}35m";
	public static readonly string FDCYAN = Console.IsOutputRedirected ? "" : $"{Csi}36m";
	public static readonly string FDGRAY = Console.IsOutputRedirected ? "" : $"{Csi}37m";

	// BackGround
	public static readonly string BNORMAL = Console.IsOutputRedirected ? "" : $"{Csi}49m";    // background color at startup

	// BackGround - Light - 1;
	public static readonly string BLBLACK = Console.IsOutputRedirected ? "" : $"{Csi}40m";
	public static readonly string BLRED = Console.IsOutputRedirected ? "" : $"{Csi}41m";
	public static readonly string BLGREEN = Console.IsOutputRedirected ? "" : $"{Csi}42m";
	public static readonly string BLYELLOW = Console.IsOutputRedirected ? "" : $"{Csi}43m";
	public static readonly string BLBLUE = Console.IsOutputRedirected ? "" : $"{Csi}44m";
	public static readonly string BLMAGENTA = Console.IsOutputRedirected ? "" : $"{Csi}45m";
	public static readonly string BLCYAN = Console.IsOutputRedirected ? "" : $"{Csi}46m";
	public static readonly string BLGRAY = Console.IsOutputRedirected ? "" : $"{Csi}47m";

	// BackGround - Dark - 0;
	public static readonly string BDGRAY = Console.IsOutputRedirected ? "" : $"{Csi}100m";      // $"{CSI}40;1m"
	public static readonly string BDRED = Console.IsOutputRedirected ? "" : $"{Csi}101m";
	public static readonly string BDGREEN = Console.IsOutputRedirected ? "" : $"{Csi}102m";
	public static readonly string BDYELLOW = Console.IsOutputRedirected ? "" : $"{Csi}103m";
	public static readonly string BDBLUE = Console.IsOutputRedirected ? "" : $"{Csi}104m";
	public static readonly string BDMAGENTA = Console.IsOutputRedirected ? "" : $"{Csi}105m";
	public static readonly string BDCYAN = Console.IsOutputRedirected ? "" : $"{Csi}106m";
	public static readonly string BDWHITE = Console.IsOutputRedirected ? "" : $"{Csi}107m";

	// 256 Colors
	// The following escape codes tells the terminal to use the given color ID:
	//		ESCCodeSequence	Description
	//		ESC[38;5;{ID}m	Set foreground color.
	//		ESC[48;5;{ID}m	Set background color.
	// Where {ID} should be replaced with the color index from 0 to 255.
	// The table starts with the original 16 colors(0-15).
	// The proceeding 216 colors(16-231) or formed by a 3bpc RGB value offset by 16, packed into a single value.
	// The final 24 colors(232-255) are grayscale starting from a shade slighly lighter than black, ranging up to shade slightly darker than white.
	// Some emulators interpret these steps as linear increments(256 / 24) on all three channels, although some emulators may explicitly define these values.

	public static string FColor(int id) => (id >= 0 && id <= 255) ? $"{Csi}38;5;{id}m" : "";
	public static string BColor(int id) => (id >= 0 && id <= 255) ? $"{Csi}48;5;{id}m" : "";

	// RGB Colors (24-bit)
	// More modern terminals supports Truecolor(24-bit RGB), which allows you to set foreground and background colors using RGB.
	// These escape sequences are usually not well documented.
	//		ESCCodeSequence			Description
	//		ESC[38;2;{r};{g};{b}m	Set foreground color as RGB.
	//		ESC[48;2;{r};{g};{b}m	Set background color as RGB.
	// Note that ;38 and ;48 corresponds to the 16 color sequence and is interpreted by the terminal to set the foreground and background color respectively. Where as ;2 and ;5 sets the color format.
	public static string FgRgb(int r, int g, int b) => $"{Csi}38;2;{(r >= 0 && r <= 255 ? r : 0)};{(g >= 0 && g <= 255 ? g : 0)};{(b >= 0 && b <= 255 ? b : 0)}m";
	public static string BgRgb(int r, int g, int b) => $"{Csi}48;2;{(r >= 0 && r <= 255 ? r : 0)};{(g >= 0 && g <= 255 ? g : 0)};{(b >= 0 && b <= 255 ? b : 0)}m";

	// - - - - - - 
	// Decorations
	// - - - - - - 

	// ESCCodeSequence	ResetSequence	Description
	// ESC[1;34;{...}m					Set graphics modes for cell, separated by semicolon(;).
	// ESC[0m							reset all modes(styles and colors)
	// ESC[1m			ESC[22m			set bold mode.
	// ESC[2m			ESC[22m			set dim / faint mode.
	// ESC[3m			ESC[23m			set italic mode.
	// ESC[4m			ESC[24m			set underline mode.
	// ESC[5m			ESC[25m			set blinking mode
	// ESC[7m			ESC[27m			set inverse / reverse mode
	// ESC[8m			ESC[28m			set hidden / invisible mode
	// ESC[9m			ESC[29m			set strikethrough mode.

	public static readonly string Default = Console.IsOutputRedirected ? "" : $"{Csi}0m";    // All attributes off(color at startup)
	public static readonly string HiCol = $"{Csi}1m";
	public static readonly string LoCol = $"{Csi}2m";   // 22

	public static readonly string BoldOn = Console.IsOutputRedirected ? "" : $"{Csi}1m";
	public static readonly string DimOn = Console.IsOutputRedirected ? "" : $"{Csi}2m";
	public static readonly string ItalicOn = Console.IsOutputRedirected ? "" : $"{Csi}3m";
	public static readonly string UnderlineOn = Console.IsOutputRedirected ? "" : $"{Csi}4m";
	public static readonly string BlinkOn = Console.IsOutputRedirected ? "" : $"{Csi}5m";
	public static readonly string ReverseOn = Console.IsOutputRedirected ? "" : $"{Csi}7m";
	public static readonly string HiddenOn = Console.IsOutputRedirected ? "" : $"{Csi}8m";
	public static readonly string StrikeThroughOn = Console.IsOutputRedirected ? "" : $"{Csi}[9m";
	public static readonly string OverLinedOn = Console.IsOutputRedirected ? "" : $"{Csi}[53m";

	public static readonly string BoldOf = Console.IsOutputRedirected ? "" : $"{Csi}21m";
	public static readonly string DimOf = Console.IsOutputRedirected ? "" : $"{Csi}22m";
	public static readonly string ItalicOf = Console.IsOutputRedirected ? "" : $"{Csi}23m";
	public static readonly string UnderlineOf = Console.IsOutputRedirected ? "" : $"{Csi}24m";
	public static readonly string BlinkOf = Console.IsOutputRedirected ? "" : $"{Csi}25m";
	public static readonly string ReverseOf = Console.IsOutputRedirected ? "" : $"{Csi}27m";
	public static readonly string HiddenOf = Console.IsOutputRedirected ? "" : $"{Csi}28m";
	public static readonly string StrikeThroughOf = Console.IsOutputRedirected ? "" : $"{Csi}29m";

	// - - -
	// Erase
	// - - -

	// ESC[J   erase in display(same as ESC[0J)
	// ESC[0J  erase from cursor until end of screen
	// ESC[1J  erase from cursor to beginning of screen
	// ESC[2J  erase entire screen
	// ESC[3J  erase saved lines
	// ESC[K   erase in line(same as ESC[0K)
	// ESC[0K  erase from cursor to end of line
	// ESC[1K  erase start of line to the cursor
	// ESC[2K  erase the entire line
	//
	// Note: Erasing the line won't move the cursor, meaning that the cursor will stay at the last position it was at before the line was erased. You can use \r after erasing the line, to return the cursor to the start of the current line.

	// ClearScreen clears the screen
	public static void ClearScreen()
	{
		Console.Write($"{Csi}H\r");      // moves cursor to home position (0, 0)
		Console.Write($"{Csi}2J\r");     // Clear screen
	}

	public static string EraseInDisplay = $"{Csi}J";
	public static string EraseFromCursorToToEndScreen = $"{Csi}0J";
	public static string EraseFromCursorToToBeginScreen = $"{Csi}1J";
	public static string EraseScreen = $"{Csi}2J";
	public static string EraseSavedLines = $"{Csi}3J";
	public static string EraseInLine = $"{Csi}K";
	public static string EraseFromCursorToEndLine = $"{Csi}0K";
	public static string EraseFromStartLineToCursor = $"{Csi}1K";
	public static string EraseEntireLine = $"{Csi}2K";

	// - - - - - - - -
	// Cursor control
	// - - - - - - - -

	// ESCCodeSequence			Description
	// ESC[H					moves cursor to home position(0, 0)
	// ESC[{line};{column}H		moves cursor to line #, column #
	// ESC[{line};{column}f		moves cursor to line #, column #
	// ESC[#A					moves cursor up # lines
	// ESC[#B					moves cursor down # lines
	// ESC[#C					moves cursor right # columns
	// ESC[#D					moves cursor left # columns
	// ESC[#E					moves cursor to beginning of next line, # lines down
	// ESC[#F					moves cursor to beginning of previous line, # lines up
	// ESC[#G					moves cursor to column #
	// ESC[6n					request cursor position(reports as ESC[#;#R)				// !!!
	// ESC M					moves cursor one line up, scrolling if needed				// !!!
	// ESC 7					save cursor position(DEC)
	// ESC 8					restores the cursor to the last saved position(DEC)
	// ESC[s					save cursor position(SCO)
	// ESC[u					restores the cursor to the last saved position(SCO)
	// Note: Some sequences, like saving and restoring cursors, are private sequences and are not standardized. While some terminal emulators (i.e. xterm and derived) support both SCO and DEC sequences, they are likely to have different functionality. It is therefore recommended to use DEC sequences.

	//ESC[<n>d				VPA		Vertical Line Position Absolute     Cursor moves to the < n > th position vertically in the current column
	//ESC[<y>;<x>H			CUP		Cursor Position 	*Cursor moves to<x>; <y> coordinate within the viewport, where <x> is the column of the<y> line
	//ESC[<y>;<x>f			HVP		Horizontal Vertical Position* Cursor moves to<x>; <y> coordinate within the viewport, where <x> is the column of the<y> line

	public static readonly string CURSORHOME = Console.IsOutputRedirected ? "" : $"{Csi}H";
	public static readonly string CURSORUP = Console.IsOutputRedirected ? "" : $"{ESC} M";              // RI : Reverse Index – Performs the reverse operation of \n, moves cursor up one line, maintains horizontal position, scrolls buffer if necessary*
	public static readonly string CURSORSAVEDEC = Console.IsOutputRedirected ? "" : $"{ESC} 7";         // DECSC
	public static readonly string CURSORRESTOREDEC = Console.IsOutputRedirected ? "" : $"{ESC}8";       // DECSR
	public static readonly string CURSORSAVE = Console.IsOutputRedirected ? "" : $"{Csi}s";
	public static readonly string CURSORRESTORE = Console.IsOutputRedirected ? "" : $"{Csi}u";

	public static string CursorMoveTo(int row, int col) => $"{Csi}{row};{col}H";
	public static string CursorMoveTo2(int row, int col) => $"{Csi}{row};{col}f";
	public static string CursorUp(int n) => $"{Csi}{n}A";
	public static string CursorDown(int n) => $"{Csi}{n}B";
	public static string CursorRight(int n) => $"{Csi}{n}C";
	public static string CursorLeft(int n) => $"{Csi}{n}D";
	public static string CursorDownBeginLine(int n) => $"{Csi}{n}E";
	public static string CursorUpBeginLine(int n) => $"{Csi}{n}F";
	public static string CursorToCol(int n) => $"{Csi}{n}G";

	// - - - - - - - - -
	// Cursor Visiblity
	// - - - - - - - - -

	// ESC[?12h    ATT160  Text Cursor Enable Blinking     Start the cursor blinking
	// ESC[?12l    ATT160  Text Cursor Disable Blinking    Stop blinking the cursor
	// ESC[?25h    DECTCEM     Text Cursor Enable Mode Show    Show the cursor
	// ESC[?25l    DECTCEM     Text Cursor Enable Mode Hide    Hide the cursor

	// - - - - - - - 
	// Cursor Chape
	// - - - - - - - 

	// SP is a literal space character (0x20)
	// ESC[0 SP q    DECSCUSR    User Shape  Default cursor shape configured by the user
	// ESC[1 SP q    DECSCUSR    Blinking Block  Blinking block cursor shape
	// ESC[2 SP q    DECSCUSR    Steady Block    Steady block cursor shape
	// ESC[3 SP q    DECSCUSR    Blinking Underline  Blinking underline cursor shape
	// ESC[4 SP q    DECSCUSR    Steady Underline    Steady underline cursor shape
	// ESC[5 SP q    DECSCUSR    Blinking Bar    Blinking bar cursor shape
	// ESC[6 SP q    DECSCUSR    Steady Bar  Steady bar cursor shape

	// - - - - - - - - - - - 
	// Viewport Positioning
	// - - - - - - - - - - - 

	// ESC[<n>S     SU  Scroll Up   Scroll text up by<n>.Also known as pan down, new lines fill in from the bottom of the screen
	// ESC[<n>T     SD  Scroll Down     Scroll down by<n>.Also known as pan up, new lines fill in from the top of the screen

	// - - - - - - - - - 
	// Text Modification
	// - - - - - - - - - 

	// ESC[<n>@ 	ICH     Insert Character    Insert < n > spaces at the current cursor position, shifting all existing text to the right.Text exiting the screen to the right is removed.
	// ESC[<n>P     DCH     Delete Character    Delete < n > characters at the current cursor position, shifting in space characters from the right edge of the screen.
	// ESC[<n>X     ECH     Erase Character     Erase < n > characters from the current cursor position by overwriting them with a space character.
	// ESC[<n>L     IL  Insert Line     Inserts < n > lines into the buffer at the cursor position.The line the cursor is on, and lines below it, will be shifted downwards.
	// ESC[<n>M     DL  Delete Line     Deletes < n > lines from the buffer, starting with the row the cursor is on.

	// - - - 
	// Erase
	// - - - 

	// For the following commands, the parameter<n> has 3 valid values:
	//   0 erases from the current cursor position(inclusive) to the end of the line/display
	//   1 erases from the beginning of the line/display up to and including the current cursor position
	//   2 erases the entire line/display
	// ESC[<n>J     ED  Erase in Display    Replace all text in the current viewport / screen specified by < n > with space characters
	// ESC[<n>K     EL  Erase in Line   Replace all text on the line with the cursor specified by < n > with space characters

	// - - - - - - -
	// Screen Modes
	// - - - - - - -
	// Set Mode
	// ESCCodeSequence		Description
	// ESC[={value}h		Changes the screen width or type to the mode specified by value.
	// ESC[= 0h				40 x 25 monochrome(text)
	// ESC[= 1h				40 x 25 color(text)
	// ESC[= 2h				80 x 25 monochrome(text)
	// ESC[= 3h				80 x 25 color(text)
	// ESC[= 4h				320 x 200 4 - color(graphics)
	// ESC[= 5h				320 x 200 monochrome(graphics)
	// ESC[= 6h				640 x 200 monochrome(graphics)
	// ESC[= 7h				Enables line wrapping
	// ESC[= 13h			320 x 200 color(graphics)
	// ESC[= 14h			640 x 200 color(16 - color graphics)
	// ESC[= 15h			640 x 350 monochrome(2 - color graphics)
	// ESC[= 16h			640 x 350 color(16 - color graphics)
	// ESC[= 17h			640 x 480 monochrome(2 - color graphics)
	// ESC[= 18h			640 x 480 color(16 - color graphics)
	// ESC[= 19h			320 x 200 color(256 - color graphics)
	// ESC[={value}l		Resets the mode by using the same values that Set Mode uses, except for 7, which disables line wrapping.The last character in this escape sequence is a lowercase L.

	public static readonly string SETSCREENMODEMONOTXT40x25 = Console.IsOutputRedirected ? "" : $"{Csi}0h";
	public static readonly string SETSCREENMODECOLORTXT40x25 = Console.IsOutputRedirected ? "" : $"{Csi}1h";
	public static readonly string SETSCREENMODEMONOTXT80x25 = Console.IsOutputRedirected ? "" : $"{Csi}2h";
	public static readonly string SETSCREENMODECOLORTXT80x25 = Console.IsOutputRedirected ? "" : $"{Csi}3h";
	public static readonly string SETSCREENMODE4COLOR320x200 = Console.IsOutputRedirected ? "" : $"{Csi}4h";
	public static readonly string SETSCREENMODEMONO320x200 = Console.IsOutputRedirected ? "" : $"{Csi}5h";
	public static readonly string SETSCREENMODEMONO640x200 = Console.IsOutputRedirected ? "" : $"{Csi}6h";
	public static readonly string SETSCREENMODEENABLEWRAP = Console.IsOutputRedirected ? "" : $"{Csi}7h";
	public static readonly string SETSCREENMODECOLOR320x200 = Console.IsOutputRedirected ? "" : $"{Csi}13h";
	public static readonly string SETSCREENMODECOLOR640x200 = Console.IsOutputRedirected ? "" : $"{Csi}14h";
	public static readonly string SETSCREENMODEMONO640x350 = Console.IsOutputRedirected ? "" : $"{Csi}15h";
	public static readonly string SETSCREENMODECOLOR640x350 = Console.IsOutputRedirected ? "" : $"{Csi}16h";
	public static readonly string SETSCREENMODEMONO640x480 = Console.IsOutputRedirected ? "" : $"{Csi}17h";
	public static readonly string SETSCREENMODECOLOR640x480 = Console.IsOutputRedirected ? "" : $"{Csi}18h";
	public static readonly string SETSCREENMODE256COLOR320x200 = Console.IsOutputRedirected ? "" : $"{Csi}19h";

	public static readonly string RESETSCREENMODEMONOTXT40x25 = Console.IsOutputRedirected ? "" : $"{Csi}0l";
	public static readonly string RESETSCREENMODECOLORTXT40x25 = Console.IsOutputRedirected ? "" : $"{Csi}1l";
	public static readonly string RESETSCREENMODEMONOTXT80x25 = Console.IsOutputRedirected ? "" : $"{Csi}2l";
	public static readonly string RESETSCREENMODECOLORTXT80x25 = Console.IsOutputRedirected ? "" : $"{Csi}3l";
	public static readonly string RESETSCREENMODE4COLOR320x200 = Console.IsOutputRedirected ? "" : $"{Csi}4l";
	public static readonly string RESETSCREENMODEMONO320x200 = Console.IsOutputRedirected ? "" : $"{Csi}5l";
	public static readonly string RESETSCREENMODEMONO640x200 = Console.IsOutputRedirected ? "" : $"{Csi}6l";
	//public static readonly string RESETSCREENMODEENABLEWRAP = Console.IsOutputRedirected ? "" : $"{CSI}7l";
	public static readonly string RESETSCREENMODECOLOR320x200 = Console.IsOutputRedirected ? "" : $"{Csi}13l";
	public static readonly string RESETSCREENMODECOLOR640x200 = Console.IsOutputRedirected ? "" : $"{Csi}14l";
	public static readonly string RESETSCREENMODEMONO640x350 = Console.IsOutputRedirected ? "" : $"{Csi}15l";
	public static readonly string RESETSCREENMODECOLOR640x350 = Console.IsOutputRedirected ? "" : $"{Csi}16l";
	public static readonly string RESETSCREENMODEMONO640x480 = Console.IsOutputRedirected ? "" : $"{Csi}17l";
	public static readonly string RESETSCREENMODECOLOR640x480 = Console.IsOutputRedirected ? "" : $"{Csi}18l";
	public static readonly string RESETSCREENMODE256COLOR320x200 = Console.IsOutputRedirected ? "" : $"{Csi}19l";

	// - - - - - - - - - - -
	// Common Private Modes
	// - - - - - - - - - - -
	// These are some examples of private modes, which are not defined by the specification, but are implemented in most terminals.
	// ESCCodeSequence		Description
	// ESC[?25l				make cursor invisible
	// ESC[?25h				make cursor visible
	// ESC[?47l				restore screen
	// ESC[?47h				save screen
	// ESC[?1049h			enables the alternative buffer
	// ESC[?1049l			disables the alternative buffer
	// Refer to the XTerm Control Sequences for a more in-depth list of private modes defined by XTerm.
	// Note: While these modes may be supported by the most terminals, some may not work in multiplexers like tmux.

	public static readonly string CURSORINVISIBLE = Console.IsOutputRedirected ? "" : $"{Csi}25l";
	public static readonly string CURSORVISIBLE = Console.IsOutputRedirected ? "" : $"{Csi}25h";
	public static readonly string SCREENRESTORE = Console.IsOutputRedirected ? "" : $"{Csi}47l";
	public static readonly string SCREENSAVE = Console.IsOutputRedirected ? "" : $"{Csi}47h";
	public static readonly string ALTBUFFERENABLE = Console.IsOutputRedirected ? "" : $"{Csi}1049h";
	public static readonly string ALTBUFFERDISABLE = Console.IsOutputRedirected ? "" : $"{Csi}1049l";
	public static void CursorHide(bool hide) => Console.Write($"{Csi}?25{(hide ? "l" : "h")}");

	// - - - - - - - 
	// Line drawing
	// - - - - - - - 
	// Sequence Description                                             Behavior
	// ESC(0    Designate Character Set – DEC Line Drawing Enables DEC  Line Drawing Mode
	// ESC(B    Designate Character Set – US ASCII                      Enables ASCII Mode (Default)

	// Hex      ASCIIDEC    LineDrawing
	// 0x6a     j           ┘
	// 0x6b 	k           ┐
	// 0x6c 	l           ┌
	// 0x6d 	m           └
	// 0x6e 	n           ┼
	// 0x71 	q           ─
	// 0x74 	t           ├
	// 0x75 	u           ┤
	// 0x76 	v           ┴
	// 0x77 	w           ┬
	// 0x78 	x           │

	// - - - - - - - - - 
	// Keyboard Strings
	// - - - - - - - - - 
	// ESC[{ code};{string};{...}p
	// Redefines a keyboard key to a specified string.
	// The parameters for this escape sequence are defined as follows:
	// • code is one or more of the values listed in the following table.These values represent keyboard keys and key combinations. When using these values in a command, you must type the semicolons shown in this table in addition to the semicolons required by the escape sequence.The codes in parentheses are not available on some keyboards.ANSI.SYS will not interpret the codes in parentheses for those keyboards unless you specify the /X switch in the DEVICE command for ANSI.SYS.
	// • string is either the ASCII code for a single character or a string contained in quotation marks. For example, both 65 and "A" can be used to represent an uppercase A.
	// IMPORTANT: Some of the values in the following table are not valid for all computers. Check your computer's documentation for values that are different.
	// List of keyboard strings
	// Key						Code		SHIFT+code	CTRL+code	ALT+code
	// F1						0;59		0;84		0;94		0;104
	// F2						0;60		0;85		0;95		0;105
	// F3						0;61		0;86		0;96		0;106
	// F4						0;62		0;87		0;97		0;107
	// F5						0;63		0;88		0;98		0;108
	// F6						0;64		0;89		0;99		0;109
	// F7						0;65		0;90		0;100		0;110
	// F8						0;66		0;91		0;101		0;111
	// F9						0;67		0;92		0;102		0;112
	// F10						0;68		0;93		0;103		0;113
	// F11						0;133		0;135		0;137		0;139
	// F12						0;134		0;136		0;138		0;140
	// HOME (num keypad)		0;71		55			0;119		--
	// UP ARROW (num keypad)	0;72		56			(0;141)		--
	// PAGE UP (num keypad)		0;73		57			0;132		--
	// LEFT ARROW (num keypad)	0;75		52			0;115		--
	// RIGHT ARROW (num keypad)	0;77		54			0;116		--
	// END (num keypad)			0;79		49			0;117		--
	// DOWN ARROW (num keypad)	0;80		50			(0;145)		--
	// PAGE DOWN (num keypad)	0;81		51			0;118		--
	// INSERT (num keypad)		0;82		48			(0;146)		--
	// DELETE (num keypad)		0;83		46			(0;147)		--
	// HOME						(224;71)	(224;71)	(224;119)	(224;151)
	// UP ARROW					(224;72)	(224;72)	(224;141)	(224;152)
	// PAGE UP					(224;73)	(224;73)	(224;132)	(224;153)
	// LEFT ARROW				(224;75)	(224;75)	(224;115)	(224;155)
	// RIGHT ARROW				(224;77)	(224;77)	(224;116)	(224;157)
	// END						(224;79)	(224;79)	(224;117)	(224;159)
	// DOWN ARROW				(224;80)	(224;80)	(224;145)	(224;154)
	// PAGE DOWN				(224;81)	(224;81)	(224;118)	(224;161)
	// INSERT					(224;82)	(224;82)	(224;146)	(224;162)
	// DELETE					(224;83)	(224;83)	(224;147)	(224;163)
	// PRINT SCREEN				--			--			0;114		--
	// PAUSE/BREAK				--			--			0;0			--
	// BACKSPACE				8			8			127			(0)
	// ENTER					13			--			10			(0
	// TAB						9			0;15		(0;148)		(0;165)
	// NULL						0;3			--			--			--
	// A						97			65			1			0;30
	// B						98			66			2			0;48
	// C						99			66			3			0;46
	// D						100			68			4			0;32
	// E						101			69			5			0;18
	// F						102			70			6			0;33
	// G						103			71			7			0;34
	// H						104			72			8			0;35
	// I						105			73			9			0;23
	// J						106			74			10			0;36
	// K						107			75			11			0;37
	// L						108			76			12			0;38
	// M						109			77			13			0;50
	// N						110			78			14			0;49
	// O						111			79			15			0;24
	// P						112			80			16			0;25
	// Q						113			81			17			0;16
	// R						114			82			18			0;19
	// S						115			83			19			0;31
	// T						116			84			20			0;20
	// U						117			85			21			0;22
	// V						118			86			22			0;47
	// W						119			87			23			0;17
	// X						120			88			24			0;45
	// Y						121			89			25			0;21
	// Z						122			90			26			0;44
	// 1						49			33			--			0;120
	// 2						50			64			0			0;121
	// 3						51			35			--			0;122
	// 4						52			36			--			0;123
	// 5						53			37			--			0;124
	// 6						54			94			30			0;125
	// 7						55			38			--			0;126
	// 8						56			42			--			0;126
	// 9						57			40			--			0;127
	// 0						48			41			--			0;129
	// -						45			95			31			0;130
	// =						61			43			---			0;131
	// [						91			123			27			0;26
	// ]						93			125			29			0;27
	// 							92			124			28			0;43
	// ;						59			58			--			0;39
	// '						39			34			--			0;40
	// ,						44			60			--			0;51
	// .						46			62			--			0;52
	// /						47			63			--			0;53
	// `						96			126			--			(0;41)
	// ENTER (keypad)			13			--			10			(0;166)
	// / (keypad)				47			47			(0;142)		(0;74)
	// * (keypad)				42			(0;144)		(0;78)		--
	// - (keypad)				45			45			(0;149)		(0;164)
	// + (keypad)				43			43			(0;150)		(0;55)
	// 5 (keypad)				(0;76)		53			(0;143)		--

	// - - - - - - 

	//// GetPos gets the current x, y position of the cursor
	//public static (int row, int col) GetPos()
	//{
	//	int x = 0, y = 0;
	//	Console.Write($"{CSI}6n");
	//	//.Scan("\x1b[{row};{col}R");
	//	return (x, y);
	//}

	//// SetScrollRegion sets the start and end lines that the screen will scroll within
	//public static void SetScrollRegion(int start, int end)
	//{
	//	Console.Write($"{CSI}{start};{end}r");
	//}

	//// ResetScrollRegion resets the scroll region
	//public static void ResetScrollRegion()
	//{
	//	Console.Write($"{CSI}r");
	//}

	//// TermSize returns the size of the terminal
	//public static (int row, int col) TermSize()
	//{
	//	//		x, y, _:= term.GetSize(0)
	//	//return x, y
	//	return (0, 0);
	//}

	public static void EnableVirtualConsole()
	{
		// ---------------------
		// Enable Ansi Sequences (if needed)
		// ---------------------
		// Get the handle to the standard output stream
		var stdHandle = GetStdHandle(STD_OUTPUT_HANDLE);

		// Get the current console mode
		//uint mode;

		if (!GetConsoleMode(stdHandle, out uint mode))
		{
			Console.Error.WriteLine("Failed to get console mode");
			return;
		}

		// Enable the virtual terminal processing mode
		mode |= ENABLE_VIRTUAL_TERMINAL_PROCESSING; //| DISABLE_NEWLINE_AUTO_RETURN;

		if (!SetConsoleMode(stdHandle, mode))
		{
			Console.Error.WriteLine("Failed to set console mode");
			return;
		}
	}

	public static void AnsiCodeTest()
	{
		Console.WriteLine("\nDecorations:\n");

        Console.WriteLine($"{BoldOn}Bold{BoldOf}, {UnderlineOn}Underline{UnderlineOf}, {ReverseOn}Reverse{ReverseOf}, {BlinkOn}Blink{BlinkOf} , {ItalicOn}Italic{ItalicOf}");	// ???
        Console.WriteLine($"{BoldOn}Bold{FNORMAL}, {BoldOf}DubbelUnderline{UnderlineOf}, {UnderlineOn}Underline{UnderlineOf}, {ReverseOn}Reverse{ReverseOf}, {BlinkOn}Blink{BlinkOf} , {ItalicOn}Italic{ItalicOf}");	// ???

        Console.WriteLine($"{Program.ConsBGC}{Program.ConsFGC}");
		Program.DrukToets();

		// - - - - - - 

		Program.ResetConsole();
		Console.WriteLine("\n8-bit colors:\n");

		Console.WriteLine($"ForeGroundLight {FLGRAY}Gray{FNORMAL}, {FLRED}Red{FNORMAL}, {FLGREEN}Green{FNORMAL}, {FLYELLOW}Yellow{FNORMAL}, {FLBLUE}Blue{FNORMAL}, {FLMAGENTA}Magenta{FNORMAL}, {FLCYAN}Cyan{FNORMAL}");
		Console.WriteLine($"ForeGroundDark  {FDGRAY}Gray{FNORMAL}, {FDRED}Red{FNORMAL}, {FDGREEN}Green{FNORMAL}, {FDYELLOW}Yellow{FNORMAL}, {FDBLUE}Blue{FNORMAL}, {FDMAGENTA}Magenta{FNORMAL}, {FDCYAN}Cyan{FNORMAL}");
		Console.WriteLine($"BackGroundLight {BLGRAY}Gray{BNORMAL}, {BLRED}Red{BNORMAL}, {BLGREEN}Green{BNORMAL}, {BLYELLOW}Yellow{BNORMAL}, {BLBLUE}Blue{BNORMAL}, {BLMAGENTA}Magenta{BNORMAL}, {BLCYAN}Cyan{BNORMAL}");
		Console.WriteLine($"BackGroundDark  {BDGRAY}Gray{BNORMAL}, {BDRED}Red{BNORMAL}, {BDGREEN}Green{BNORMAL}, {BDYELLOW}Yellow{BNORMAL}, {BDBLUE}Blue{BNORMAL}, {BDMAGENTA}Magenta{BNORMAL}, {BDCYAN}Cyan{BNORMAL}");

		Console.Write($"{Csi}0m");
		Console.WriteLine($"{Program.ConsBGC}{Program.ConsFGC}");
		Console.Write($"Foreground normal : ");
		for (var c = 30; c <= 37; c++) Console.Write($"{Csi}{c}m{c,-4}");
		Console.Write($"{Csi}0m");

		Console.WriteLine($"{Program.ConsBGC}{Program.ConsFGC}");
		Console.Write($"Foreground Bold   : ");
		for (var c = 30; c <= 37; c++) Console.Write($"{Csi}{c};1m{c,-4}");
		Console.Write($"{Csi}0m");

		Console.WriteLine($"{Program.ConsBGC}{Program.ConsFGC}");
		Console.Write($"Foreground Bold   : ");
		for (var c = 90; c <= 97; c++) Console.Write($"{Csi}{c}m{c,-4}");
		Console.Write($"{Csi}0m");

		Console.WriteLine($"{Program.ConsBGC}{Program.ConsFGC}");
		Console.Write($"Background normal : ");
		for (var c = 40; c <= 47; c++) Console.Write($"{Csi}{c}m{c,-4}");
		Console.Write($"{Csi}0m");

		Console.WriteLine($"{Program.ConsBGC}{Program.ConsFGC}");
		Console.Write($"Background Bold   : ");
		for (var c = 40; c <= 47; c++) Console.Write($"{Csi}{c};1m{c,-4}");
		Console.Write($"{Csi}0m");

		Console.WriteLine($"{Program.ConsBGC}{Program.ConsFGC}");
		Console.Write($"Background Bold   : ");
		for (var c = 100; c <= 107; c++) Console.Write($"{Csi}{c}m{c,-4}");
		Console.Write($"{Csi}0m");

		Console.WriteLine($"{Program.ConsBGC}{Program.ConsFGC}");
		Program.DrukToets();

		// - - - - - - 

		Program.ResetConsole();
		Console.WriteLine("\n256 colors:");

		for (var i = 0; i <= 255; i++) Console.Write($"{(i % 32 == 0 ? "\n" : "")}{FColor(i)} {string.Format("{0:000}", i),-3} {FNORMAL}");
		Console.WriteLine($"{Program.ConsBGC} {Program.ConsFGC}\n");
		for (var i = 0; i <= 255; i++) Console.Write($"{(i % 32 == 0 ? "\n" : "")}{BColor(i)} {string.Format("{0:000}", i),-3} {BNORMAL}");

		Console.WriteLine($"{Program.ConsBGC}{Program.ConsFGC}");
		Program.DrukToets();

		// - - - - - - 

		Program.ResetConsole();
		Console.WriteLine($"{Program.ConsBGC}{Program.ConsFGC}\n\nRGB colors:");

		const int step = 64;

		for (var r = 0; r <= 255; r += step)
			for (var g = 0; g <= 255; g += step)
				for (var b = 0; b <= 255; b += step)
					Console.Write($"{(b % 256 == 0 ? "\n" : "")}{FgRgb(r, g, b)}{'*',-3}{FNORMAL}");

		Console.WriteLine($"{Program.ConsBGC}{Program.ConsFGC}\n");

		for (var r = 0; r <= 255; r += step)
			for (var g = 0; g <= 255; g += step)
				for (var b = 0; b <= 255; b += step)
					Console.Write($"{(b % 256 == 0 ? "\n" : "")}{BgRgb(r, g, b)}{'*',-3}{BNORMAL}");

		Console.WriteLine($"{Program.ConsBGC}{Program.ConsFGC}");
		Program.DrukToets();

		// -------- --------------------------------------------------------------- --------------------------------------------------------------------------------------------------------
		// n		Name															Note
		// -------- --------------------------------------------------------------- --------------------------------------------------------------------------------------------------------
		// 0		Reset or normal													All attributes become turned off
		// 1		Bold or increased intensity										As with faint, the color change is a PC (SCO / CGA) invention.[25][better source needed]
		// 2		Faint, decreased intensity, or dim								May be implemented as a light font weight like bold.[26]
		// 3		Italic															Not widely supported. Sometimes treated as inverse or blink.[25]
		// 4		Underline														Style extensions exist for Kitty, VTE, mintty, iTerm2 and Konsole.[27][28][29]
		// 5		Slow blink														Sets blinking to less than 150 times per minute
		// 6		Rapid blink														MS-DOS ANSI.SYS, 150 + per minute; not widely supported
		// 7		Reverse video or invert											Swap foreground and background colors; inconsistent emulation[30][dubious – discuss]
		// 8		Conceal or hide													Not widely supported.
		// 9		Crossed -out, or strike											Characters legible but marked as if for deletion.Not supported in Terminal.app.
		// 10		Primary(default) font
		// 11–19	Alternative font												Select alternative font n − 10
		// 20		Fraktur(Gothic)													Rarely supported
		// 21		Doubly underlined; or: not bold									Double - underline per ECMA-48,[5]: 8.3.117 but instead disables bold intensity on several terminals, including in the Linux kernel's console before version 4.17.[31]
		// 22		Normal intensity												Neither bold nor faint; color changes where intensity is implemented as such.
		// 23		Neither italic, nor blackletter
		// 24		Not underlined													Neither singly nor doubly underlined
		// 25		Not blinking													Turn blinking off
		// 26		Proportional spacing											ITU T.61 and T.416, not known to be used on terminals
		// 27		Not reversed
		// 28		Reveal															Not concealed
		// 29		Not crossed out 	
		// 30–37	Set foreground color
		// 38		Set foreground color											Next arguments are 5; n or 2; r; g; b
		// 39		Default foreground color										Implementation defined(according to standard)
		// 40–47	Set background color
		// 48		Set background color											Next arguments are 5; n or 2; r; g; b
		// 49		Default background color										Implementation defined(according to standard)
		// 50		Disable proportional spacing									T.61 and T.416
		// 51		Framed															Implemented as "emoji variation selector" in mintty.[32]
		// 52		Encircled														Implemented as "emoji variation selector" in mintty.[32]
		// 53		Overlined														Not supported in Terminal.app
		// 54		Neither framed nor encircled
		// 55		Not overlined
		// 58		Set underline color												Not in standard; implemented in Kitty, VTE, mintty, and iTerm2.[27][28] Next arguments are 5; n or 2; r; g; b.
		// 59		Default underline color											Not in standard; implemented in Kitty, VTE, mintty, and iTerm2.[27][28]
		// 60		Ideogram underline or right side line							Rarely supported
		// 61		Ideogram double underline, or double line on the right side		Rarely supported
		// 62		Ideogram overline or left side line								Rarely supported
		// 63		Ideogram double overline, or double line on the left side		Rarely supported
		// 64		Ideogram stress marking											Rarely supported
		// 65		No ideogram attributes											Reset the effects of all of 60–64
		// 73		Superscript														Implemented only in mintty[32]
		// 74		Subscript														Implemented only in mintty[32]
		// 75		Neither superscript nor subscript								Implemented only in mintty[32]
		// 90–97	Set bright foreground color										Not in standard; originally implemented by aixterm[16]
		// 100–107	Set bright background color										Not in standard; originally implemented by aixterm[16]
		// -------- --------------------------------------------------------------- --------------------------------------------------------------------------------------------------------

		Program.ResetConsole();
		Console.WriteLine("\n\nTest:\n");

		for (var x = 0; x <= 108; x++) Console.Write($"{(x % 10 == 0 ? "\n" : "")}{Csi}{x}m {x,-3}{Csi}m");

		// - - - - - - 

		Console.WriteLine($"{Program.ConsBGC}{Program.ConsFGC}\nTest:\n");

		for (var i = 0; i <= 15; i++)
		{
			for (var j = 0; j <= 15; j++)
			{
				var code = i * 16 + j;
				Console.Write($"{Csi}38;5;{code}m {code,-3} ");
			}

			Console.Write($"\n{Csi}0m");
		}

		Console.WriteLine($"{Program.ConsBGC}{Program.ConsFGC}");
		Program.DrukToets();

		// - - - - - - 

		Program.ResetConsole();

		Console.WriteLine("Progress Indicator\n");

		Console.WriteLine("Loading...");

		for (var i = 1; i <= 100; i++)
		{
			Thread.Sleep(150);
			Console.Write($"{CursorToCol(0)}{i}%");
		}

		Console.WriteLine();
		Program.DrukToets();

		// - - - - - - 

		Program.ResetConsole();

		Console.WriteLine("ASCII Bar Progress Indicator\n");

		Console.WriteLine("Loading...");

		for (var i = 1; i <= 100; i++)
		{
			Thread.Sleep(150);
			var width = (i + 1) / 4;
			Console.Write($"{CursorToCol(0)}[{new string('*', width)}{new string(' ', 25 - width)}]");
		}

		Console.WriteLine($"{Program.ConsBGC}{Program.ConsFGC}");
		Program.DrukToets();

		// - - - - - - 

		Program.ResetConsole();

		Console.WriteLine("Graphic Mode");

		Console.WriteLine($"{ESC}(0");      // Enter Graphic mode
		Console.WriteLine("lqwqk");         // ┌─┬─┐
		Console.WriteLine("tqnqu");         // ├─┼─┤
		Console.WriteLine("tqvqu");         // ├─┴─┤
		Console.WriteLine("x   x");         // │   │
		Console.WriteLine("mqqqj");         // └───┘
		Console.WriteLine($"{ESC}(B");      // Enter Text mode

		Console.WriteLine($"{Program.ConsBGC}{Program.ConsFGC}");
		Console.WriteLine();

		Console.WriteLine($"{ESC}(0");
		for (var i = 'a'; i <= 'z'; i++) Console.Write($"{i}");
		Console.WriteLine($"{Program.ConsBGC}{Program.ConsFGC}");
		for (var i = 0; i <= 255; i++) Console.Write($"{(char)i}");
		Console.WriteLine($"{ESC}(B");

		// - - - - - - 

		Console.WriteLine($"{Program.ConsBGC}{Program.ConsFGC}");
		Program.DrukToets();
	}
}

//https://github.com/microsoft/terminal/issues/14622

//using System.Runtime.InteropServices;
//using static System.Console;

//DoTest();

//void DoTest()
//{
//    // Ensure that VT100 code support is enabled - Windows only.
//    WindowsVt.Enable();

//    BufferHeight = 60;
//    SetCursorPosition(0, WindowHeight - 1);
//    Write("This is a status line at the bottom of the console.");
//    WriteLine($"\u001b[0;{Console.WindowHeight - 2}r");
//    SetCursorPosition(0, 0);

//    for (var i = 0; i < 50; i++)
//    {
//        WriteLine(i.ToString());
//        //Thread.Sleep(100);  // Work around for Windows Terminal
//    }

//    ReadKey();
//}

//public static class WindowsVt
//{

//    private const int STD_OUTPUT_HANDLE = -11;

//    private const uint ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x0004;

//    private const uint DISABLE_NEWLINE_AUTO_RETURN = 0x0008;

//    public static bool Enable()
//    {
//        var iStdOut = GetStdHandle(STD_OUTPUT_HANDLE);

//        if (!GetConsoleMode(iStdOut, out var outConsoleMode))
//        {
//            return false;
//        }

//        outConsoleMode |= ENABLE_VIRTUAL_TERMINAL_PROCESSING | DISABLE_NEWLINE_AUTO_RETURN;

//        return SetConsoleMode(iStdOut, outConsoleMode);
//    }

//    [DllImport("kernel32.dll", SetLastError = true)]
//    private static extern bool GetConsoleMode(nint hConsoleHandle, out uint lpMode);

//    [DllImport("kernel32.dll", SetLastError = true)]
//    private static extern bool SetConsoleMode(nint hConsoleHandle, uint dwMode);

//    [DllImport("kernel32.dll", SetLastError = true)]
//    private static extern nint GetStdHandle(int nStdHandle);
//}
