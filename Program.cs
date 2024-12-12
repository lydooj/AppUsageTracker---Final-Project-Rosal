using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Timers;
using System.IO;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;

namespace AppUsageTracker
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Clyde Justine B. Rosal
            // note to self, start doing the improvements or enhancements during free time or if really bored

            ReadMe instructions = new ReadMe();
            instructions.Instructions();
            Console.SetWindowSize(width: 120, height: 30);
            Console.SetBufferSize(width: 120, height: 30); 

            do
            {
                int appNumber = 0;
                string appName;
                int appTime;
                int choice = 0;

                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("App Usage Tracker");
                Console.WriteLine("==================");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Choose a Number");
                Console.WriteLine("1. Start");
                Console.WriteLine("2. Read Previous Sessions");
                Console.WriteLine("3. Help");
                Console.WriteLine("0. Exit");
                Console.ResetColor();
                Console.Write("\nChoice: ");
                Console.ForegroundColor = ConsoleColor.Magenta;
                string chc = Console.ReadLine();
                Console.ResetColor();
                if (string.IsNullOrWhiteSpace(chc))

                {
                    Console.Clear();
                    continue;
                }

                try
                {
                    choice = int.Parse(chc);
                }
                catch (FormatException)
                {
                    Console.Beep(350, 125);
                    Console.Beep(350, 150);
                    Console.WriteLine("Please enter a valid given numerical choice.");
                    Thread.Sleep(1500);
                    continue;
                }

                // case 1 - enter the numbers
                // case 2 - read previous sessions
                // case 3 - help
                // case 0 - exit
                // default - error
                switch (choice)
                {
                    case 1:
                        Console.Clear();

                        while (true)
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write("Enter Number of Applications to be Monitored (Input 'Enter' to go back): ");
                            Console.ResetColor();
                            string input = Console.ReadLine();

                            if (string.IsNullOrWhiteSpace(input))
                            {
                                Console.Beep(1000, 100);
                                break;
                            }

                            try
                            {
                                appNumber = int.Parse(input);
                                if (appNumber <= 0)
                                {
                                    Console.Beep(350, 125);
                                    Console.Beep(350, 150);
                                    Console.WriteLine("Please enter a VALID number. (+)");
                                    Thread.Sleep(1000);
                                    Console.Clear();
                                    continue;
                                }
                                break;
                            }
                            catch (FormatException e)
                            {
                                Console.Beep(350, 125);
                                Console.Beep(350, 150);
                                Console.WriteLine("Please enter a NUMBER.");
                                Console.ReadLine();
                                Console.Clear();
                            }

                        };

                        if (appNumber == 0)
                        {
                            break;
                        }

                        AppUsage[] app = new AppUsage[appNumber];

                        Console.WriteLine("\nEnter Applications to be Monitored: ");
                        for (int i = 0; i < appNumber; i++)
                        {
                            Console.Write("{0}. App Name: ", i + 1);
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            appName = Console.ReadLine();
                            Console.ResetColor();

                            Console.Write("Allocated time in Minutes for App No. {0}: ", i + 1);
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            appTime = int.Parse(Console.ReadLine());
                            Console.ResetColor();

                            app[i] = new AppUsage(appName, appTime); //set the appname and apptime within an array, might change depending on how it goes
                        }

                        for (int i = 0; i < app.Length; i++) //displays all of the apps, their allocated times and starts the tracking process
                        {
                            app[i].DisplayAppInfo();

                        }

                        Console.WriteLine("Press 'Enter' to start the tracking or type 'BACK' to return to the menu.");
                        string start = Console.ReadLine();
                        Console.Clear();

                        Console.WriteLine("TRACKING THE FOLLOWING PROCESSES: ");
                        if (string.IsNullOrWhiteSpace(start))
                        {
                            for (int i = 0; i < app.Length; i++) //displays all of the apps, their allocated times and starts the tracking process
                            {
                                app[i].DisplayAppInfo();
                                app[i].StartAppTracking();
                            }

                            while (true)
                            {

                                Console.Write("\nTo check for the remaining time, input '1' otherwise just ignore: \n");
                                string goback = Console.ReadLine();

                                if (string.IsNullOrWhiteSpace(goback))
                                {
                                    break;
                                }
                                else if (int.Parse(goback) == 1)
                                {
                                    for (int ctr = 0; ctr < app.Length; ctr++)
                                    {
                                        Console.WriteLine("");
                                        app[ctr].RemainingTime();
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Invalid Input");
                                    Thread.Sleep(1000);

                                    continue;
                                }
                            }

                        }
                        else if (start == "BACK")
                        {
                            break;
                        }
                        else
                        {
                            Console.Beep(350, 125);
                            Console.Beep(350, 150);
                            Console.WriteLine("Input is CASE-SENSITIVE, returning you to the MENU...");
                            Thread.Sleep(1000);
                            break;
                        }

                        break;

                    case 2:
                        Console.Clear();
                        int logchoice = 0;
                        string data;
                        StreamReader reader = null;

                        do
                        {
                            try
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("Enter a Corresponding Number for your choice:");
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.WriteLine("1. Read Sessions Log");
                                Console.WriteLine("2. View Usage Statistics");
                                Console.WriteLine("3. Clear Sessions Log");
                                Console.ResetColor();

                                Console.Write("\nChoice: ");
                                logchoice = int.Parse(Console.ReadLine());
                                break;
                            }
                            catch (Exception)
                            {
                                Console.Beep(350, 125);
                                Console.Beep(350, 150);
                                Console.WriteLine("Please Enter a Valid Choice.");
                                continue;
                            }
                        } while (true);

                        switch (logchoice)
                        {
                            case 1:
                                try
                                {
                                    Console.Clear();
                                    char yes;
                                    reader = new StreamReader("app_usage_log.txt");
                                    data = reader.ReadLine();

                                    do
                                    {
                                        Console.ForegroundColor = ConsoleColor.Green;
                                        Console.WriteLine("Previous Sessions:\n");
                                        Console.ResetColor();
                                        for (int i = 0; i != 10;)
                                        {
                                            if (i < 10)
                                            {
                                                Console.WriteLine(data);
                                                data = reader.ReadLine();
                                                i++;
                                            }
                                        }

                                        Console.Write("\nNext (y) or Close (any key)?: ");
                                        yes = char.Parse(Console.ReadLine());
                                        if (yes == 'y')
                                        {
                                            Console.Clear();
                                            continue;
                                        }
                                        else
                                        {
                                            Console.Clear();
                                            break;
                                        }
                                    } while (true);
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e.Message);
                                }
                                finally
                                {
                                    reader?.Close();
                                }
                                break;

                            case 2:
                                Console.Clear();
                                var statistics = new ProcessTimeStatistics();
                                statistics.CompileStatistics();

                                Console.WriteLine("\nPress any key to continue...");
                                Console.ReadKey();
                                break;

                            case 3:
                                try
                                {
                                    File.WriteAllText("app_usage_log.txt", string.Empty);
                                    Console.Clear();
                                    Console.WriteLine("Logging File (app_usage_log.txt) has been cleared!");
                                    Console.WriteLine("Going back to home page");
                                    Thread.Sleep(2000);
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine("Error Clearing Log: " + e.Message);
                                }
                                break;

                            default:
                                Console.WriteLine("Invalid Number Choice. Returning you to the Menu...");
                                Thread.Sleep(1000);
                                break;
                        }
                        break;

                    case 3:
                        Console.Clear();
                        instructions.InstructionReading();
                        Console.Write("\n\nPress 'Enter' to go back ");
                        Console.ReadLine();
                        break;

                    case 0:
                        Console.Clear();
                        Console.Beep(262, 200);
                        Console.Beep(294, 200);
                        Console.Beep(330, 200);
                        Console.WriteLine("Exiting...");
                        Console.Beep(262, 150);
                        Console.Beep(294, 150);
                        Console.Beep(330, 150);
                        Thread.Sleep(500);
                        Console.WriteLine($"Thank you for using the App-Usage Tracker !\nSession Ended @ {DateTime.Now}");
                        return;

                    default:
                        Console.Beep(350, 125);
                        Console.Beep(350, 150);
                        Console.WriteLine("INVALID CHOICE. PLEASE 'Enter' TO RESET.");
                        Console.ReadLine();
                        Console.Clear();
                        break;
                }
            } while (true);

        }
    }

    public interface ITrack
    {
        void StartAppTracking();
        void DisplayAppInfo();
    }

    public abstract class Base : ITrack
    {
        protected string AppName { get; private set; }
        protected int AllocatedTimeInMinutes { get; set; }
        protected Process Process { get; set; }
        protected UsageLogger UsageLogger { get; private set; } = new UsageLogger();

        public Base(string appName, int allocatedTimeInMinutes)
        {
            AppName = appName;
            AllocatedTimeInMinutes = allocatedTimeInMinutes;
        }

        public virtual void StartAppTracking()
        {
            Process[] processes = Process.GetProcessesByName(AppName);
            if (processes.Length > 0)
            {
                Process = processes[0];
                UsageLogger.Log($"Started Tracking: {AppName} @ {DateTime.Now}, Allocated Time: {AllocatedTimeInMinutes} minutes.");
            }
            else
            {
                Console.WriteLine($"No running process found for {AppName}");
            }
        }

        public virtual void DisplayAppInfo()
        {
            Console.Write("App Name: ");
            Console.ForegroundColor = ConsoleColor.Cyan; // Set color for AppName
            Console.Write(AppName);
            Console.ResetColor(); // Reset color to default

            Console.Write(", Allocated Time: ");
            Console.ForegroundColor = ConsoleColor.Yellow; // Set color for AllocatedTimeInMinutes
            Console.Write($"{AllocatedTimeInMinutes} Minute(s)\n");
            Console.ResetColor(); // Reset color to default
        }
    }

    public class AppUsage : Base
    {
        private static List<AppUsage> monitoredApps = new List<AppUsage>();
        private static System.Timers.Timer usageTimer;
        private bool isProcessExited = false;
        private DateTime lastWarning = DateTime.MinValue;
        private DateTime lastReminder = DateTime.MinValue;
        private const int warningHz = 800;
        private const int warningDuration = 300;
        private const int warningInterval = 10;

        // Fields to handle multiple instances
        private List<Process> processes = new List<Process>();
        private Dictionary<int, DateTime> processStartTimes = new Dictionary<int, DateTime>();

        public AppUsage(string appName, int allocatedTimeInMinutes) : base(appName, allocatedTimeInMinutes)
        {
        }

        public override void StartAppTracking()
        {
            Process[] foundProcesses = Process.GetProcessesByName(AppName);
            if (foundProcesses.Length > 0)
            {
                foreach (var proc in foundProcesses)
                {
                    try
                    {
                        proc.EnableRaisingEvents = true;
                        proc.Exited += Process_Exited;
                        processes.Add(proc);
                        processStartTimes[proc.Id] = DateTime.Now;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error setting up process {proc.ProcessName} (PID: {proc.Id}): {ex.Message}");
                        continue;
                    }
                }

                if (processes.Count > 0)
                {
                    if (!monitoredApps.Contains(this))
                    {
                        monitoredApps.Add(this);
                    }

                    if (usageTimer == null)
                    {
                        usageTimer = new System.Timers.Timer(1000);
                        usageTimer.Elapsed += CheckUsage;
                        usageTimer.Start();
                    }

                    foreach (var proc in processes)
                    {
                        UsageLogger.Log($"Started tracking process: {AppName} (PID: {proc.Id}) @ {processStartTimes[proc.Id]}");
                        Console.WriteLine($"Started tracking process: {AppName} (PID: {proc.Id}) @ {processStartTimes[proc.Id]}");
                    }
                }
            }
            else
            {
                Console.WriteLine($"No running process found for {AppName}");
            }
        }
        private void Process_Exited(object sender, EventArgs e)
        {
            if (sender is Process exitedProcess)
            {
                if (processStartTimes.TryGetValue(exitedProcess.Id, out DateTime startTime))
                {
                    var runtime = (DateTime.Now - startTime).TotalMinutes;
                    string message = $"Process: {AppName} (PID: {exitedProcess.Id}) - Runtime: {runtime:F2} minutes @ {DateTime.Now}";
                    UsageLogger.Log(message);
                    Console.WriteLine(message);

                    processes.RemoveAll(p => p.Id == exitedProcess.Id);
                    processStartTimes.Remove(exitedProcess.Id);

                    if (!processes.Any())
                    {
                        isProcessExited = true;
                    }
                }
            }
        }

        private void WarningSystem(bool isWarning = false)
        {
            if (isWarning)
            {
                if ((DateTime.Now - lastWarning).TotalSeconds >= warningInterval)
                {
                    Console.Beep(warningHz, warningDuration);
                    Thread.Sleep(100);
                    Console.Beep(warningHz, warningDuration);
                    lastWarning = DateTime.Now;
                }
            }
        }

        private void CheckUsage(object sender, ElapsedEventArgs e)
        {
            List<AppUsage> appsToRemove = new List<AppUsage>();

            foreach (var app in monitoredApps)
            {
                try
                {
                    List<Process> processesToRemove = new List<Process>();
                    bool allProcessesExited = true;

                    foreach (var proc in app.processes.ToList())
                    {
                        try
                        {
                            bool processExists = !proc.HasExited && Process.GetProcesses().Any(p => p.Id == proc.Id);
                            if (!processExists)
                            {
                                if (app.processStartTimes.TryGetValue(proc.Id, out DateTime startTime))
                                {
                                    double elapsedRuntime = (DateTime.Now - startTime).TotalMinutes;
                                    string logMessage = $"Process: {app.AppName} (PID: {proc.Id}) - Runtime: {elapsedRuntime:F2} minutes @ {DateTime.Now}";
                                    app.UsageLogger.Log(logMessage);
                                    Console.Write("Process: ");
                                    Console.ForegroundColor = ConsoleColor.Cyan;
                                    Console.Write(app.AppName);
                                    Console.ResetColor();
                                    Console.Write($" (PID: {proc.Id}) - Runtime: ");
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.Write(TimeFormatter.FormatTimeRemaining(elapsedRuntime));
                                    Console.ResetColor();
                                    Console.WriteLine($" @ {DateTime.Now}");
                                }
                                processesToRemove.Add(proc);
                                continue;
                            }

                            allProcessesExited = false;
                            proc.Refresh();

                            if (app.processStartTimes.TryGetValue(proc.Id, out DateTime processStartTime))
                            {
                                TimeSpan elapsedTime = DateTime.Now - processStartTime;
                                double remainingTime = app.AllocatedTimeInMinutes - elapsedTime.TotalMinutes;

                                if (remainingTime <= 0)
                                {
                                    double totalRuntime = elapsedTime.TotalMinutes;
                                    string logMessage = $"Process: {app.AppName} (PID: {proc.Id}) - Runtime: {totalRuntime:F2} minutes @ {DateTime.Now}";
                                    app.UsageLogger.Log(logMessage);
                                    Console.Write("Process: ");
                                    Console.ForegroundColor = ConsoleColor.Cyan;
                                    Console.Write(app.AppName);
                                    Console.ResetColor();
                                    Console.Write($" (PID: {proc.Id}) - Runtime: ");
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.Write(TimeFormatter.FormatTimeRemaining(totalRuntime));
                                    Console.ResetColor();
                                    Console.WriteLine($" @ {DateTime.Now}");

                                    try
                                    {
                                        proc.Kill();
                                    }

                                    catch (Exception ex)
                                    {
                                        Console.WriteLine($"Error killing process {proc.ProcessName} (PID: {proc.Id}): {ex.Message}");
                                    }

                                    processesToRemove.Add(proc);
                                }
                                else if (remainingTime <= 1 && remainingTime > 0)
                                {
                                    if ((DateTime.Now - app.lastWarning).TotalSeconds >= warningInterval)
                                    {
                                        Console.Write("\nWarning: '");
                                        Console.ForegroundColor = ConsoleColor.Cyan;
                                        Console.Write(app.AppName);
                                        Console.ResetColor();
                                        Console.Write($"' (PID: {proc.Id}) has ");
                                        Console.ForegroundColor = ConsoleColor.Yellow;
                                        Console.Write(TimeFormatter.FormatTimeRemaining(remainingTime));
                                        Console.ResetColor();
                                        Console.WriteLine(" left.");
                                        app.WarningSystem(true);
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            processesToRemove.Add(proc);
                        }
                    }

                    // Remove terminated processes
                    foreach (var proc in processesToRemove)
                    {
                        app.processes.Remove(proc);
                        app.processStartTimes.Remove(proc.Id);
                    }

                    // If all processes for this app have been terminated, remove the app from monitoring
                    if (allProcessesExited || !app.processes.Any())
                    {
                        appsToRemove.Add(app);
                    }
                }
                catch
                {
                    appsToRemove.Add(app);
                }
            }

            foreach (var app in appsToRemove)
            {
                monitoredApps.Remove(app);
            }
        }


        public void RemainingTime()
        {
            if (!processes.Any())
            {
                Console.Write("No active processes being monitored for ");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write(AppName);
                Console.ResetColor();
                Console.WriteLine(".");
                return;
            }

            var activeProcesses = processes
                .Where(p => !p.HasExited && processStartTimes.ContainsKey(p.Id))
                .ToList();

            if (!activeProcesses.Any())
            {
                Console.Write("No active processes being monitored for ");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write(AppName);
                Console.ResetColor();
                Console.WriteLine(".");
                return;
            }

            Console.Write("\nRemaining Time for ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(AppName);
            Console.ResetColor();
            Console.WriteLine(":");

            foreach (var proc in activeProcesses.OrderBy(p => processStartTimes[p.Id]))
            {
                try
                {
                    if (processStartTimes.TryGetValue(proc.Id, out DateTime startTime))
                    {
                        double remaining = AllocatedTimeInMinutes - (DateTime.Now - startTime).TotalMinutes;

                        string additionalInfo = "";
                        try
                        {
                            var parent = ParentProcessUtilities.GetParentProcess(proc.Id);
                            if (parent != null)
                            {
                                additionalInfo = $" (Parent: {parent.ProcessName})";
                            }
                        }
                        catch
                        {
                            // Ignore parent process errors
                        }

                        Console.Write($"Instance (PID: {proc.Id}){additionalInfo}: ");
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write($"{TimeFormatter.FormatTimeRemaining(Math.Max(0, remaining))}");
                        Console.ResetColor();
                        Console.Write(" out of ");
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write($"{AllocatedTimeInMinutes:F0} minutes");
                        Console.ResetColor();
                        Console.WriteLine($" left (Started: {startTime:HH:mm:ss})");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error getting remaining time for {AppName} (PID: {proc.Id}): {ex.Message}");
                }
            }
        }
    }

    public static class ParentProcessUtilities
    {
        public static Process GetParentProcess(int pid)
        {
            try
            {
                using var query = new System.Management.ManagementObjectSearcher("SELECT ParentProcessId FROM Win32_Process WHERE ProcessId=" + pid);

                foreach (var mo in query.Get())
                {
                    int parentPid = Convert.ToInt32(mo["ParentProcessId"]);
                    return Process.GetProcessById(parentPid);
                }
            }
            catch
            {

            }
            return null;
        }
    }
    public class TimeFormatter
    {
        public static string FormatTimeRemaining(double totalMinutes)
        {
            if (totalMinutes <= 0)
            {
                return "0 minutes 0 seconds";
            }

            int minutes = (int)Math.Floor(totalMinutes);
            int seconds = (int)Math.Round((totalMinutes - minutes) * 60);

            // Handle case where seconds round up to 60
            if (seconds == 60)
            {
                minutes++;
                seconds = 0;
            }

            return $"{minutes} minute{(minutes != 1 ? "s" : "")} {seconds} second{(seconds != 1 ? "s" : "")}";
        }
    }

    public class ProcessTimeStatistics
    {
        private readonly Dictionary<string, List<(DateTime start, DateTime end)>> processTimePeriods =
        new Dictionary<string, List<(DateTime start, DateTime end)>>();

        public void CompileStatistics()
        {
            try
            {
                if (!File.Exists("app_usage_log.txt"))
                {
                    Console.WriteLine("No usage log file found.");
                    return;
                }

                processTimePeriods.Clear();
                string[] logLines = File.ReadAllLines("app_usage_log.txt");

                foreach (string line in logLines)
                {
                    if (line.Contains("Started tracking process:"))
                    {
                        // Extract process name and start time
                        string processName = ExtractBetween(line, "Started tracking process: ", " (PID:");
                        if (string.IsNullOrEmpty(processName)) continue;

                        DateTime startTime = ExtractDateTime(line, "@");
                        if (startTime == DateTime.MinValue) continue;

                        if (!processTimePeriods.ContainsKey(processName))
                        {
                            processTimePeriods[processName] = new List<(DateTime start, DateTime end)>();
                        }
                        processTimePeriods[processName].Add((startTime, DateTime.MinValue));
                    }
                    else if (line.Contains("Process:") && line.Contains("Runtime:"))
                    {
                        // Extract process name and end time
                        string processName = ExtractBetween(line, "Process: ", " (PID:");
                        if (string.IsNullOrEmpty(processName)) continue;

                        DateTime endTime = ExtractDateTime(line, "@");
                        if (endTime == DateTime.MinValue) continue;

                        if (processTimePeriods.ContainsKey(processName))
                        {
                            var openPeriod = processTimePeriods[processName].FirstOrDefault(p => p.end == DateTime.MinValue);

                            if (openPeriod != default)
                            {
                                int index = processTimePeriods[processName].IndexOf(openPeriod);
                                processTimePeriods[processName][index] = (openPeriod.start, endTime);
                            }
                        }
                    }
                }

                // Calculate and display merged time periods
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Process Usage Statistics:\n");
                Console.ResetColor();
                
                foreach (var kvp in processTimePeriods)
                {
                    double totalMinutes = CalculateNonOverlappingTime(kvp.Value);
                    Console.WriteLine($"{kvp.Key}: {totalMinutes:F2} minutes");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error compiling statistics: {ex.Message}");
            }
        }

        private double CalculateNonOverlappingTime(List<(DateTime start, DateTime end)> periods)
        {
            // Remove incomplete periods
            periods = periods.Where(p => p.end != DateTime.MinValue).ToList();
            if (!periods.Any()) return 0;

            // Sort periods by start time
            periods = periods.OrderBy(p => p.start).ToList();

            // Merge overlapping periods
            var mergedPeriods = new List<(DateTime start, DateTime end)>();
            var currentPeriod = periods[0];

            foreach (var period in periods.Skip(1))
            {
                if (period.start <= currentPeriod.end)
                {
                    // Periods overlap, extend current period if necessary
                    currentPeriod = (currentPeriod.start,
                        period.end > currentPeriod.end ? period.end : currentPeriod.end);
                }
                else
                {
                    // No overlap, add current period and start new one
                    mergedPeriods.Add(currentPeriod);
                    currentPeriod = period;
                }
            }
            mergedPeriods.Add(currentPeriod);

            // Calculate total time from merged periods
            return mergedPeriods.Sum(p => (p.end - p.start).TotalMinutes);
        }

        private DateTime ExtractDateTime(string line, string identifier)
        {
            try
            {
                int index = line.LastIndexOf(identifier);
                if (index < 0) return DateTime.MinValue;

                string dateStr = line.Substring(index + identifier.Length).Trim();
                if (DateTime.TryParse(dateStr, out DateTime result))
                {
                    return result;
                }
            }
            catch { }
            return DateTime.MinValue;
        }

        private string ExtractBetween(string source, string start, string end)
        {
            int startIndex = source.IndexOf(start);
            if (startIndex < 0) return null;
            startIndex += start.Length;

            int endIndex = source.IndexOf(end, startIndex);
            if (endIndex < 0) return null;

            return source.Substring(startIndex, endIndex - startIndex).Trim();
        }
    }

    public class UsageLogger
    {
        private const string LogFilePath = "app_usage_log.txt";
        private const long MaxSize = 5 * 1024 * 1024; // 5 mb file limit, idk may change or smth
        private static readonly object _lockObj = new object();

        public void Log(string message)
        {
            int retryCount = 0;
            const int maxRetries = 3;
            const int retryDelay = 100;

            while (retryCount < maxRetries)
            {
                try
                {
                    lock (_lockObj)
                    {
                        if (File.Exists(LogFilePath))
                        {
                            var fileInfo = new FileInfo(LogFilePath);
                            if (fileInfo.Length > MaxSize)
                            {
                                // Create backup of old log before clearing
                                string backupPath = $"{LogFilePath}.bak";
                                if (File.Exists(backupPath))
                                {
                                    File.Delete(backupPath);
                                }
                                File.Move(LogFilePath, backupPath);
                                File.WriteAllText(LogFilePath, "");
                            }
                        }

                        File.AppendAllText(LogFilePath, $"{DateTime.Now}: {message}{Environment.NewLine}");
                        return;
                    }
                }
                catch (IOException ex)
                {
                    retryCount++;
                    if (retryCount == maxRetries)
                    {
                        Console.WriteLine($"Failed to write to log after {maxRetries} attempts: {ex.Message}");
                        return;
                    }
                    Thread.Sleep(retryDelay);
                }
            }
        }
    }

    public class ReadMe
    {
        private const string ReadMePath = "read_me.txt";


        public void Instructions()
        {
            using (StreamWriter write = new StreamWriter(ReadMePath))
            {
                write.WriteLine("Welcome to a Quick Guide on how to use the application.");
                write.WriteLine("\nCommand Terminal: ");
                write.WriteLine("1. Open any process you want to track and terminate after reaching the allocated time.");
                write.WriteLine("2. Simultaneously press the windows key + r to open the 'run' window.");
                write.WriteLine("3. Type in 'cmd' and press 'enter'.");
                write.WriteLine("4. Input 'tasklist' into the command terminal.");
                write.WriteLine("5. Find the process that you want to track. (e.g. winword, chrome)");
                write.WriteLine("6. Proceed to App Usage Tracker and start.");

                write.WriteLine("\nWindows Powershell: ");
                write.WriteLine("1. Open any process you want to track and terminate after reaching the allocated time.");
                write.WriteLine("2. Simultaneously press the windows key + r to open the 'run' window.");
                write.WriteLine("3. Type in 'powershell' and press 'enter'.");
                write.WriteLine("4. Input 'Get-process'.");
                write.WriteLine("5. Find the process that you want to track. (e.g. winword, chrome)");
                write.WriteLine("6. Proceed to App Usage Tracker and start.");
            }
        }

        public void InstructionReading()
        {
            if (File.Exists(ReadMePath))
            {
                using (StreamReader read = new StreamReader(ReadMePath))
                {
                    string line;
                    while ((line = read.ReadLine()) != null)
                    {
                        Console.WriteLine(line);
                    }
                }
            }
            else
            {
                Console.WriteLine("The instructions file does not exist.");
            }
        }

    }

}