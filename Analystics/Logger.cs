using Aura.Framework.Analystics.Events;
using Aura.Framework.Analystics.Interfaces;
using System;
using System.IO;
using System.Linq;

namespace Aura.Framework.Analystics
{
    /// <summary>
    /// Act likes a logger with some more advanced options.
    /// </summary>
    public class Logger : ILogger, IDisposable
    {
        #region Fields

        /// <summary>
        /// Represents the lock for the log file to prevent wrong, missing or invalid lines.
        /// </summary>
        private static readonly object _Locker = new object();

        /// <summary>
        /// Represents a lock for putting out lines to the console to prevent invalid/unmatching log-lines.
        /// </summary>
        private static readonly object _ConsoleWriterLock = new object();

        /// <summary>
        /// Represents the full file location of the log file.
        /// </summary>
        public string Path { get; private set; }

        /// <summary>
        /// Represents the extension of the file.
        /// </summary>
        public string Extension { get; private set; }

        /// <summary>
        /// Represents a <see cref="bool"/> value whether to log into the console or not.
        /// </summary>
        public bool ConsoleLogging { get; set; } = false;

        /// <summary>
        /// Gets or sets a <see cref="bool"/> value wether the advanced logging mode is enabled or not.
        /// </summary>
        public static bool AdvancedLogging { get; set; } = true;

        #endregion Fields

        #region Events

        /// <summary>
        /// An event that occures when the <see cref="Logger"/> class instance loads a log file.
        /// </summary>
        public event EventHandler<LoggerLoadedEventArgs> FileLoaded;

        /// <summary>
        /// An event that occures when the <see cref="Logger"/> class instance logs a line to the log file.
        /// </summary>
        public event EventHandler<LoggerWritenEventArgs> LineWriten;

        #endregion Events

        #region Contructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Logger"/> class with the default path set to root with the name "Aura.txt".
        /// </summary>
        public Logger()
        {
            Path = ("Aura.txt");
            Extension = "txt";

            if (!Check(Path, Extension, false))
                throw new ArgumentException("Bad file name.");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Logger"/> class.
        /// </summary>
        public Logger(string fullPath)
        {
            Path = fullPath;
            Extension = "txt";

            if (!Check(Path, Extension, false))
                throw new ArgumentException("Bad file name.");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Logger"/> class and clears the log file if it exists.
        /// </summary>
        public Logger(string fullPath, bool clear)
        {
            Path = fullPath;
            Extension = "txt";

            if (!Check(Path, Extension, clear))
                throw new ArgumentException("Bad file name.");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Logger"/> class with a custom file extension.
        /// </summary>
        public Logger(string fullPath, string extension)
        {
            Path = fullPath;
            Extension = extension;

            if (!Check(Path, Extension, false))
                throw new ArgumentException("Bad file name.");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Logger"/> class with a custom file extension and clears the log file if it exists.
        /// </summary>
        public Logger(string fullPath, string extension, bool clear)
        {
            Path = fullPath;
            Extension = extension;

            if (!Check(Path, Extension, clear))
                throw new ArgumentException("Bad file name."); ;
        }

        #endregion Contructors

        #region Methods 

        /// <summary>
        /// Writes a line to a log file and to the console.
        /// </summary>
        public string WriteCustom(string type, object line, ConsoleColor color, Action action = null, bool isAdvanced = false)
        {
            if (isAdvanced && !AdvancedLogging)
                return line.ToString();

            if (!line.ToString().EndsWith("."))
                line = line.ToString() + ".";

            if (action != null)
                action.Invoke();

            var date = DateTime.Now;

            Write(($"[{date}] [{type.ToUpper()}] {line}"));

            lock (_ConsoleWriterLock)
            {
                Console.ForegroundColor = color;
                Console.WriteLine($"[{date}] [{type.ToUpper()}] {line}");
                Console.ResetColor();
            }

            return line.ToString();
        }

        /// <summary>
        /// Logs an info line.
        /// </summary>
        public string WriteInfo(object line)
        {
            return WriteCustom("Info", line, ConsoleColor.Gray);
        }

        /// <summary>
        /// Logs a database line.
        /// </summary>
        public string WriteDatabase(object line)
        {
            return WriteCustom("Database", line, ConsoleColor.Gray);
        }

        /// <summary>
        /// Logs a chatroom line.
        /// </summary>
        public string WriteChatroom(object line)
        {
            return WriteCustom("Chatroom", line, ConsoleColor.White);
        }

        /// <summary>
        /// Logs a private room line.
        /// </summary>
        public string WritePrivateRoom(object line)
        {
            return WriteCustom("Chat", line, ConsoleColor.White);
        }

        /// <summary>
        /// Logs a connection line.
        /// </summary>
        public string WriteConnection(object line)
        {
            return WriteCustom("Connection", line, ConsoleColor.Cyan);
        }

        /// <summary>
        /// Logs an error line.
        /// </summary>
        public string WriteError(object line)
        {
            return WriteCustom("Fatal", line, ConsoleColor.Red);
        }

        /// <summary>
        /// Logs a warning line.
        /// </summary>
        public string WriteWarning(object line)
        {
            return WriteCustom("Warning", line, ConsoleColor.DarkYellow);
        }

        /// <summary>
        /// Logs a command line.
        /// </summary>
        public string WriteCommand(object line)
        {
            return WriteCustom("Command", line, ConsoleColor.White);
        }

        /// <summary>
        /// Logs an incoming line.
        /// </summary>
        public string WriteIncoming(object line)
        {
            if (AdvancedLogging) return WriteCustom("Incoming", line, ConsoleColor.Gray); return line.ToString();
        }

        /// <summary>
        /// Logs an outgoing line.
        /// </summary>
        public string WriteOutgoing(object line)
        {
            if (AdvancedLogging) return WriteCustom("Outgoing", line, ConsoleColor.Gray); return line.ToString();
        }

        /// <summary>
        /// Logs a handled line.
        /// </summary>
        public string WriteHandled(object line)
        {
            if (AdvancedLogging)
                return WriteCustom("Handled", line, ConsoleColor.Gray);
            return line.ToString();
        }

        /// <summary>
        /// Logs an unhandled line.
        /// </summary>
        public string WriteUnhandled(object line)
        {
            return WriteCustom("Unhandled", line, ConsoleColor.Red);
        }

        /// <summary>
        /// Logs an important line.
        /// </summary>
        public string WriteImportant(object line)
        {
            return WriteCustom("Notify", line, ConsoleColor.Magenta);
        }

        /// <summary>
        /// Logs an arguments line.
        /// </summary>
        public string WriteArguments(object line)
        {
            return WriteCustom("Arguments", line, ConsoleColor.Green);
        }

        /// <summary>
        /// Logs an info line.
        /// </summary>
        public string WriteInfo(object line, Action action)
        {
            return WriteCustom("Info", line, ConsoleColor.Gray, action);
        }

        /// <summary>
        /// Logs a database line.
        /// </summary>
        public string WriteDatabase(object line, Action action)
        {
            return WriteCustom("Database", line, ConsoleColor.Gray, action);
        }

        /// <summary>
        /// Logs a chatroom line.
        /// </summary>
        public string WriteChatroom(object line, Action action)
        {
            return WriteCustom("Chatroom", line, ConsoleColor.White, action);
        }

        /// <summary>
        /// Logs a private room line.
        /// </summary>
        public string WritePrivateRoom(object line, Action action)
        {
            return WriteCustom("Chat", line, ConsoleColor.White, action);
        }

        /// <summary>
        /// Logs a connection line.
        /// </summary>
        public string WriteConnection(object line, Action action)
        {
            return WriteCustom("Connection", line, ConsoleColor.Cyan, action);
        }

        /// <summary>
        /// Logs an error line.
        /// </summary>
        public string WriteError(object line, Action action)
        {
            return WriteCustom("Fatal", line, ConsoleColor.Red, action);
        }

        /// <summary>
        /// Logs a warning line.
        /// </summary>
        public string WriteWarning(object line, Action action)
        {
            return WriteCustom("Warning", line, ConsoleColor.DarkYellow, action);
        }

        /// <summary>
        /// Logs a command line.
        /// </summary>
        public string WriteCommand(object line, Action action)
        {
            return WriteCustom("Command", line, ConsoleColor.White, action);
        }

        /// <summary>
        /// Logs an incoming line.
        /// </summary>
        public string WriteIncoming(object line, Action action)
        {
            if (AdvancedLogging) return WriteCustom("Incoming", line, ConsoleColor.Gray, action); return line.ToString();
        }

        /// <summary>
        /// Logs an outgoing line.
        /// </summary>
        public string WriteOutgoing(object line, Action action)
        {
            if (AdvancedLogging) return WriteCustom("Outgoing", line, ConsoleColor.Gray, action); return line.ToString();
        }

        /// <summary>
        /// Logs a handled line.
        /// </summary>
        public string WriteHandled(object line, Action action)
        {
            return WriteCustom("Handled", line, ConsoleColor.Gray, action, true);
        }

        /// <summary>
        /// Logs an unhandled line.
        /// </summary>
        public string WriteUnhandled(object line, Action action)
        {
            return WriteCustom("Unhandled", line, ConsoleColor.Red, action);
        }

        /// <summary>
        /// Logs an important line.
        /// </summary>
        public string WriteImportant(object line, Action action)
        {
            return WriteCustom("Notify", line, ConsoleColor.Magenta, action);
        }

        /// <summary>
        /// Logs an arguments line.
        /// </summary>
        public string WriteArguments(object line, Action action)
        {
            return WriteCustom("Arguments", line, ConsoleColor.Green, action);
        }

        /// <summary>
        /// Reads and returns a <see cref="string"/> array value which contains all the lines of the log file.
        /// </summary>
        public string[] Read()
        {
            return OnFileLoaded(new LoggerLoadedEventArgs(File.ReadAllLines(Path))).Block;
        }

        /// <summary>
        /// Reads and returns a <see cref="string"/> array value which contains all the lines of the log file that has any similarities to the parameter.
        /// </summary>
        public string[] Read(string similarities)
        {
            return OnFileLoaded(new LoggerLoadedEventArgs(File.ReadAllLines(Path).Where(o => o.Contains(similarities)).ToArray())).Block;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
        }

        #endregion Methods

        #region Internal Methods

        /// <summary>
        /// Writes a line to a file and returns itself.
        /// </summary>
        protected string Write(object message, Action action = null)
        {
            if (Check(Path, Extension, false))
                lock (_Locker)
                {
                    using (StreamWriter writer = File.AppendText(Path))
                    {
                        writer.WriteLine(message);
                        if (action == null)
                            OnLineWriten(new LoggerWritenEventArgs(message.ToString()));
                        else
                            OnLineWriten(new LoggerWritenEventArgs(message.ToString(), action.ToString()));
                    }
                }

            if (action != null)
                action.Invoke();

            return message.ToString();
        }

        /// <summary>
        /// Writes a line to a file and returns itself.
        /// </summary>
        protected bool Check(string path, string extension, bool clear)
        {
            if (!path.ToLower().EndsWith($".{extension}"))
                return false;
            
            try
            {
                if (File.Exists(path) && clear)
                    File.WriteAllText(path, string.Empty);
                else
                    File.Create(path).Close();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        #endregion Internal Methods

        #region Virtual Methods

        /// <summary>
        /// Fires the <see cref="FileLoaded"/> event.
        /// </summary>
        protected virtual LoggerLoadedEventArgs OnFileLoaded(LoggerLoadedEventArgs e)
        {
            FileLoaded?.Invoke(this, e);
            return e;
        }

        /// <summary>
        /// Fires the <see cref="LineWriten"/> event.
        /// </summary>
        protected virtual LoggerWritenEventArgs OnLineWriten(LoggerWritenEventArgs e)
        {
            LineWriten?.Invoke(this, e);
            return e;
        }

        #endregion Virtual Methods
    }
}