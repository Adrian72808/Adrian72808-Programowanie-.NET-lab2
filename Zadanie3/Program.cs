using System;
using System.Diagnostics;
using System.IO;

namespace SystemMonitor
{
    class Program
    {
        static void Main(string[] args)
        {
            // Liczniki
            var cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            var memoryCounter = new PerformanceCounter("Memory", "Available MBytes");

            // Pętla monitorowania
            Console.WriteLine("Naciśnij dowolny klawisz, aby zakończyć monitorowanie.");
            while (!Console.KeyAvailable)
            {
                // Pobranie wartości
                float cpuUsage = cpuCounter.NextValue();
                float availableMemory = memoryCounter.NextValue();

                // Przegrocznie progu 
                if (availableMemory < 100)
                {
                    LogEvent($"Zaalokowana pamięć jest niska: {availableMemory} MB");
                }

                LogEvent($"Użycie procesora: {cpuUsage}% | Dostępna pamięć: {availableMemory} MB");

                System.Threading.Thread.Sleep(1000);
            }

            Console.WriteLine("Naciśnij dowolny klawisz, aby zakończyć program.");
            Console.ReadKey();
        }

        static void LogEvent(string message)
        {

            string logFile = "monitoring.txt";

            using (StreamWriter writer = new StreamWriter(logFile, true))
            {
                writer.WriteLine($"[{DateTime.Now}] {message}");
            }
        }
    }
}
