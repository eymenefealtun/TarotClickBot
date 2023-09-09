using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Diagnostics;

int _sleepSecond = 2;
int _negativeSecond = 0;
int _positiveSecond = 0;
string _url = "";

ChromeDriver _chromeDriver;
Random _random = new Random();
int _numberOfPull = 0;
int _noSuchElementException = 0;
int _errorOccured = 0;
String _infoString = "";

AppDomain.CurrentDomain.ProcessExit += new EventHandler(CurrentDomain_ProcessExit);



RunApp();

void RunApp()
{
    SetPreferences();

    HandleSelenium();

    Click();
}

void SetPreferences()
{
    try
    {
        Console.Write("Target link:");
        _url = Console.ReadLine();

        Console.Write("Frequency of pull (in every second):");
        _sleepSecond = Convert.ToInt32(Console.ReadLine());

        Console.Write("Random negative number (is going to add to 'frequency of pull'):");
        _negativeSecond = Convert.ToInt32(Console.ReadLine());

        Console.Write("Random positive number (is going to add to 'frequency of pull'):");
        _positiveSecond = Convert.ToInt32(Console.ReadLine());
    }
    catch (Exception)
    {
        Console.Clear();
        Console.WriteLine(@"
                 ────────────────────────────────────────
                 ─────────────▄▄██████████▄▄─────────────
                 ─────────────▀▀▀───██───▀▀▀─────────────
                 ─────▄██▄───▄▄████████████▄▄───▄██▄─────
                 ───▄███▀──▄████▀▀▀────▀▀▀████▄──▀███▄───
                 ──████▄─▄███▀──────────────▀███▄─▄████──
                 ─███▀█████▀▄████▄──────▄████▄▀█████▀███─
                 ─██▀──███▀─██████──────██████─▀███──▀██─
                 ──▀──▄██▀──▀████▀──▄▄──▀████▀──▀██▄──▀──
                 ─────███───────────▀▀───────────███─────
                 ─────██████████████████████████████─────
                 ─▄█──▀██──███───██────██───███──██▀──█▄─
                 ─███──███─███───██────██───███▄███──███─
                 ─▀██▄████████───██────██───████████▄██▀─
                 ──▀███▀─▀████───██────██───████▀─▀███▀──
                 ───▀███▄──▀███████────███████▀──▄███▀───
                 ─────▀███────▀▀██████████▀▀▀───███▀─────
                 ───────▀─────▄▄▄───██───▄▄▄──────▀──────
                 ──────────── ▀▀███████████▀▀ ────────────
                 ────────────────────────────────────────
                           Something gone wrong!
 ");
        Console.WriteLine("Please wait a moment before attempting again.");
        Wait(7, 0, 0);
        Console.Clear();
        SetPreferences();
    }
}

void HandleSelenium()
{
    var chromeOptions = new ChromeOptions();
    chromeOptions.AddArguments("headless");

    var chromerDriverService = ChromeDriverService.CreateDefaultService();
    chromerDriverService.HideCommandPromptWindow = true;

    var chromeDriver = new ChromeDriver(chromerDriverService, chromeOptions);

    _chromeDriver = chromeDriver;
}

void Click()
{
    WriteInfo();

    while ("1" == "1")
    {
        try
        {
            _chromeDriver.Navigate().GoToUrl(_url);
            _numberOfPull++;
            WriteInfo();
            Wait(_sleepSecond, _negativeSecond, _positiveSecond);
        }
        catch (NoSuchElementException)
        {
            _noSuchElementException++;
            WriteInfo();
            Wait(4, 0, 0);
        }
        catch (Exception)
        {
            _errorOccured++;
            WriteInfo();
            Wait(4, 0, 0);
        }

    }
}

void Wait(int second, int negative, int positive)
{
    int random = _random.Next(0, 10) < 4 ? _random.Next(negative * 1000, 0) : _random.Next(0, positive * 1000);
    Thread.Sleep(second * 1000 + random);
}

void WriteInfo()
{
    Console.Clear();
    Console.WriteLine(@"target: {0}
number of pull:                             {1} 
number of NoSuchElementError occured:       {2} 
number of error occured:                    {3} ",
_url.ToString(), _numberOfPull.ToString(), _noSuchElementException.ToString(), _errorOccured.ToString());
}

void CurrentDomain_ProcessExit(object? sender, EventArgs e)
{
    Process[] processes = Process.GetProcessesByName("chromedriver");
    foreach (Process process in processes)
        process.Kill();
}