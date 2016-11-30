using Aura.Framework.Analystics.Events;
using Aura.Framework.Analystics.Interfaces;
using Aura.Framework.Enumerators;
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
        private static readonly object _WriteLocker = new object();

        /// <summary>
        /// Represents the lock for the console log to prevent wrong, missing or invalid lines.
        /// </summary>
        private static readonly object _ConsoleLocker = new object();

        /// <summary>
        /// Represents the full file location of the log file.
        /// </summary>
        public string Path { get; private set; }

        /// <summary>
        /// Represents the extension of the file.
        /// </summary>
        public string Extension { get; private set; }

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
        /// Initializes a new instance of the <see cref="Logger"/> class.
        /// </summary>
        public Logger()
        {
            Extension = "alog";
            Path = ($"/Analystics/Aura.{Extension}");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Logger"/> class.
        /// </summary>
        public Logger(object name)
        {
            Extension = "alog";
            Path = ($"/Analystics/{name}.{Extension}");
        }

        #endregion Contructors

        #region Methods

        /// <summary>
        /// Writes a line to a log file and to the console.
        /// </summary>
        public string Write(LoggerType type, object line)
        {
            lock (_ConsoleLocker)
            {
                Console.WriteLine(Write(($"[{DateTime.Now}] [{type.ToString().ToUpper()}] {line}")));
            }

            return line.ToString();
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
        protected string Write(object message)
        {
            if (!File.Exists(Path))
                File.Create(Path).Close();

            lock (_WriteLocker)
            {
                using (StreamWriter writer = File.AppendText(Path))
                {
                    writer.WriteLine(message);
                    OnLineWriten(new LoggerWritenEventArgs(message.ToString()));
                }
            }

            return message.ToString();
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