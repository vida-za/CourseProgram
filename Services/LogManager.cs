using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace CourseProgram.Services
{
    public class LogManager
    {
        private readonly string _logFilePath;
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        private string _lastMessage = string.Empty;

        public static LogManager Instance { get; private set; }

        public LogManager(string logFileName = "log.txt")
        {
            string directory = AppDomain.CurrentDomain.BaseDirectory;
            _logFilePath = Path.Combine(directory, logFileName);

            try
            {
                if (!File.Exists(_logFilePath))
                {
                    File.WriteAllText(_logFilePath, string.Empty);
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Error for create logfile: {ex.Message}");
            }
        }

        public static void Initialize(LogManager instance)
        {
            Instance = instance;
        }

        public async Task WriteLogAsync(string message)
        {
            if (_lastMessage != message && !message.Contains("Select"))
            {
                await _semaphore.WaitAsync();
                try
                {
                    string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}";

                    using (var stream = new FileStream(_logFilePath, FileMode.Append, FileAccess.Write, FileShare.Read))
                    using (var writer = new StreamWriter(stream))
                    {
                        writer.WriteLine(logEntry);
                    }

                    _lastMessage = message;
                }
                catch (IOException ex)
                {
                    Console.WriteLine($"Error with write in log: {ex.Message}");
                }
                finally
                {
                    _semaphore.Release();
                }
            }
        }
    }
}