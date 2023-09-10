using System.Diagnostics;

int _sleepSecond = 2;
int _negativeSecond = 0;
int _positiveSecond = 0;
string _url = "";

Random _random = new Random();
int _numberOfPull = 0;
int _errorOccured = 0;
HttpClient _httpClient = new HttpClient();

AppDomain.CurrentDomain.ProcessExit += new EventHandler(CurrentDomain_ProcessExit);

RunRawHtml();   // 100 pull in 20 second - result: 75 pull

Console.ReadLine();
void RunRawHtml()
{
    SetPreferences();

    WriteInfo();

    while ("1" == "1")
    {
        Thread.Sleep(255);
        try
        {
            _httpClient.GetAsync(_url,
           HttpCompletionOption.ResponseContentRead);
            _numberOfPull++;
            WriteInfo();
            Wait(_sleepSecond, _negativeSecond, _positiveSecond);
        }
        catch (Exception)
        {
            _errorOccured++;
            WriteInfo();
            Wait(4, 0, 0);
        }

    }
}

void SetPreferences()
{
    try
    {
        Console.Write("Target link:");
        _url = Console.ReadLine();

        Console.Write("Frequency of pull (in every second):");
        _sleepSecond = Convert.ToInt32(Console.ReadLine());

        Console.Write("Random minimum negative number (is going to add to 'frequency of pull'):");
        _negativeSecond = Convert.ToInt32(Console.ReadLine());

        Console.Write("Random maximum positive number (is going to add to 'frequency of pull'):");
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
number of error occured:                    {2} ",
_url.ToString(), _numberOfPull.ToString(), _errorOccured.ToString());
}

void CurrentDomain_ProcessExit(object? sender, EventArgs e)
{
    Process[] processes = Process.GetProcessesByName("chromedriver");
    foreach (Process process in processes)
        process.Kill();
}
