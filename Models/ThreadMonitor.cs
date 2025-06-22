using System.Diagnostics;

namespace TetrisConsole.Models;

public static class ThreadMonitor
{
    public class ThreadInfo
    {
        public int TotalThreads { get; set; }
        public int ActiveThreads { get; set; }
        public int InactiveThreads { get; set; }
        public int WaitingThreads { get; set; }
        public int RunningThreads { get; set; }
        public long MemoryUsageMB { get; set; }
        public TimeSpan CpuTime { get; set; }
    }

    /// <summary>
    /// Obtém informações detalhadas sobre threads do processo
    /// </summary>
    public static ThreadInfo GetThreadInfo()
    {
        using (var processo = Process.GetCurrentProcess())
        {
            var info = new ThreadInfo
            {
                TotalThreads = processo.Threads.Count,
                MemoryUsageMB = processo.WorkingSet64 / 1024 / 1024,
                CpuTime = processo.TotalProcessorTime
            };

            int running = 0;
            int waiting = 0;
            int active = 0;

            // Analisa o estado de cada thread
            foreach (ProcessThread thread in processo.Threads)
            {
                switch (thread.ThreadState)
                {
                    case System.Diagnostics.ThreadState.Running:
                        running++;
                        active++;
                        break;
                    case System.Diagnostics.ThreadState.Ready:
                        active++;
                        break;
                    case System.Diagnostics.ThreadState.Standby:
                        active++;
                        break;
                    case System.Diagnostics.ThreadState.Wait:
                        waiting++;
                        break;
                    case System.Diagnostics.ThreadState.Transition:
                        waiting++;
                        break;
                    default:
                        waiting++;
                        break;
                }
            }

            info.RunningThreads = running;
            info.ActiveThreads = active;
            info.WaitingThreads = waiting;
            info.InactiveThreads = info.TotalThreads - info.ActiveThreads;

            return info;
        }
    }

    /// <summary>
    /// Obtém lista detalhada de threads com seus estados
    /// </summary>
    public static List<string> GetThreadDetails()
    {
        var details = new List<string>();
        
        using (var processo = Process.GetCurrentProcess())
        {
            foreach (ProcessThread thread in processo.Threads)
            {
                string estado = thread.ThreadState switch
                {
                    System.Diagnostics.ThreadState.Running => "🟢 Executando",
                    System.Diagnostics.ThreadState.Ready => "🟡 Pronta",
                    System.Diagnostics.ThreadState.Standby => "🟠 Standby",
                    System.Diagnostics.ThreadState.Wait => "🔴 Aguardando",
                    System.Diagnostics.ThreadState.Transition => "🔵 Transição",
                    _ => "⚪ Desconhecido"
                };

                details.Add($"Thread {thread.Id}: {estado} (CPU: {thread.TotalProcessorTime.TotalMilliseconds:F1}ms)");
            }
        }

        return details;
    }
} 