using System;
using System.IO;
using System.Threading.Tasks;

namespace CourseProgram.Services
{
    public class LogManager
    {
        private readonly string _logFilePath;

        public static LogManager Instance { get; private set; }

        public LogManager(string logFileName = "log.txt")
        {
            string directory = AppDomain.CurrentDomain.BaseDirectory;
            _logFilePath = Path.Combine(directory, logFileName);

            if (!File.Exists(_logFilePath))
            {
                using (File.Create(_logFilePath)) { }
            }
        }

        public static void Initialize(LogManager instance)
        {
            Instance = instance;
        }

        public async Task WriteLogAsync(string message)
        {
            string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}";
            await File.AppendAllTextAsync(_logFilePath, logEntry + Environment.NewLine);
        }
    }
}