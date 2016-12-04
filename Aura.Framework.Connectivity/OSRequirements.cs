using System;
using System.Net;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace Aura.Framework.Connectivity
{
    /// <summary>
    /// Contains the functionality to check if the system meets the required soft-/hardware to run.
    /// </summary>
    public static class OSRequirements
    {
        #region Public Static Methods

        /// <summary>
        /// Returns a <see cref="bool"/> value based on whether the currently used framework is compatible with the server.
        /// </summary>
        public static bool Framework()
        {
            // Class "ReflectionContext" exists from .NET 4.5 onwards.
            return Type.GetType("System.Reflection.ReflectionContext", false) != null;
        }

        /// <summary>
        /// Returns a <see cref="string"/> value which represents the public/external ip address.
        /// </summary>
        public static async Task<string> GetIPAddress()
        {
            Task<string> downloader = new Task<string>(() =>
            {
                return new WebClient().DownloadString("https://api.ipify.org");
            });
            downloader.Start();
            return await downloader;
        }

        /// <summary>
        /// Returns a <see cref="bool"/> value based on whether the service is running.
        /// </summary>
        public static bool Service(string service)
        {
            ServiceController controller = new ServiceController(service);
            try
            {
                switch (controller.Status)
                {
                    case ServiceControllerStatus.Running:
                        return true;

                    default:
                        return false;
                }
            }
            catch (InvalidOperationException) { }
            return false;
        }

        #endregion Public Static Methods
    }
}