using Aura.Framework.Core.Client.Models;
using Aura.Framework.Core.Shared.Messages;
using Aura.Framework.Shared.Packets;
using System;

namespace Aura.Framework.Core.Client.Connection.Packets.Handlers.Outgoing
{
    /// <summary>
    /// Represents the connection data packet.
    /// </summary>
    public class ConnectionDataComposer : ServerMessage
    {
        /// <summary>
        /// Sends the outgoing packet to the server.
        /// </summary>
        public ConnectionDataComposer(ConnectionData connectionData) : base(ClientPackets.ConnectionData)
        {
            new DataPackage("USER", Environment.UserName).Write(this);
            new DataPackage("DOMAIN", Environment.UserDomainName).Write(this);
            new DataPackage("MACHINE", Environment.MachineName).Write(this);
            new DataPackage("OSNAME", OSData.Name).Write(this);
            new DataPackage("OSEDITION", OSData.Edition).Write(this);
            new DataPackage("OSVERION", OSData.VersionString).Write(this);
            new DataPackage("ARCHCPU", OSData.ProcessorBits.ToString()).Write(this);
            new DataPackage("ARCHOS", OSData.OSBits.ToString()).Write(this);
            new DataPackage("ARCHPRGRM", OSData.ProgramBits.ToString()).Write(this);
            new DataPackage("OSPACK", OSData.ServicePack).Write(this);
        }
    }
}