using System;

namespace Aura.Framework.Connectivity.Server
{
    /// <summary>
    /// Represents a class used for colored logging.
    /// </summary>
    public static class ServerLogger
    {
        #region Properties

        /// <summary>
        /// Gets or sets a <see cref="bool"/> value wether the advanced logging mode is enabled or not.
        /// </summary>
        public static bool AdvancedLogging { get; set; } = true;

        /// <summary>
        /// Represents a lock for putting out lines to the console to prevent invalid/unmatching log-lines.
        /// </summary>
        private static readonly object _ConsoleWriterLock = new object();

        #endregion Properties

        #region Public Methods

        /// <summary>
        /// Logs a custom line.
        /// </summary>
        public static void Custom(string type, object line, ConsoleColor color, bool isAdvanced = false)
        {
            if (isAdvanced && !AdvancedLogging)
                return;

            if (!line.ToString().EndsWith("."))
                line = line.ToString() + ".";

            lock (_ConsoleWriterLock)
            {
                Console.ForegroundColor = color;
                Console.WriteLine($"[{DateTime.Now}] [{type.ToUpper()}] {line}");
                Console.ResetColor();
            }
        }

        /// <summary>
        /// Logs an info line.
        /// </summary>
        public static void Info(object line) { Custom("Info", line, ConsoleColor.Gray); }

        /// <summary>
        /// Logs a database line.
        /// </summary>
        public static void Database(object line) { Custom("Database", line, ConsoleColor.Gray); }

        /// <summary>
        /// Logs a chatroom line.
        /// </summary>
        public static void Chatroom(object line) { Custom("Chatroom", line, ConsoleColor.White); }

        /// <summary>
        /// Logs a private room line.
        /// </summary>
        public static void PrivateRoom(object line) { Custom("Chat", line, ConsoleColor.White); }

        /// <summary>
        /// Logs a connection line.
        /// </summary>
        public static void Connection(object line) { Custom("Connection", line, ConsoleColor.Cyan); }

        /// <summary>
        /// Logs an error line.
        /// </summary>
        public static void Error(object line) { Custom("Fatal", line, ConsoleColor.Red); }

        /// <summary>
        /// Logs a warning line.
        /// </summary>
        public static void Warning(object line) { Custom("Warning", line, ConsoleColor.DarkYellow); }

        /// <summary>
        /// Logs a command line.
        /// </summary>
        public static void Command(object line) { Custom("Command", line, ConsoleColor.White); }

        /// <summary>
        /// Logs an incoming line.
        /// </summary>
        public static void Incoming(object line) { if (AdvancedLogging) Custom("Incoming", line, ConsoleColor.Gray); }

        /// <summary>
        /// Logs an outgoing line.
        /// </summary>
        public static void Outgoing(object line) { if (AdvancedLogging) Custom("Outgoing", line, ConsoleColor.Gray); }

        /// <summary>
        /// Logs a handled line.
        /// </summary>
        public static void Handled(object line) { if (AdvancedLogging) Custom("Handled", line, ConsoleColor.Gray); }

        /// <summary>
        /// Logs an unhandled line.
        /// </summary>
        public static void Unhandled(object line) { Custom("Unhandled", line, ConsoleColor.Red); }

        /// <summary>
        /// Logs an important line.
        /// </summary>
        public static void Important(object line) { Custom("Notify", line, ConsoleColor.Magenta); }

        /// <summary>
        /// Logs an arguments line.
        /// </summary>
        public static void Arguments(object line) { Custom("Arguments", line, ConsoleColor.Green); }

        #endregion Public Methods
    }
}