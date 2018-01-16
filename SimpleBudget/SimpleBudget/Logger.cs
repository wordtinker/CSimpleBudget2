using Prism.Logging;
using System;
using System.IO;

namespace SimpleBudget
{
    public class SimpleLogger : ILoggerFacade
    {
#if DEBUG
        private const string fileName = "DEBUG Log.txt";
#else
        private const string fileName = "Log.txt";
#endif
        private string path;
        public SimpleLogger(string folder)
        {
            path = Path.Combine(folder, fileName);
            Log("Logging session started.", Category.Info, Priority.Medium);
        }
        public void Log(string message, Category category, Priority priority)
        {
            string messageToLog =
                String.Format(System.Globalization.CultureInfo.InvariantCulture,
                    "{1}: {2}. Priority: {3}. Timestamp:{0:u}.\n",
                    DateTime.Now,
                    category.ToString().ToUpperInvariant(),
                    message,
                    priority.ToString());

            if (category == Category.Debug)
            {
#if DEBUG
                File.AppendAllText(path, messageToLog);
#endif
            }
            else
            {
                File.AppendAllText(path, messageToLog);
            }
        }
    }
}