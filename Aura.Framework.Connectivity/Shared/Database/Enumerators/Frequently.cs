﻿namespace Aura.Framework.Connectivity.Shared.Database.Enumerators
{
    /// <summary>
    /// Specifies how often the 'events' are done.
    /// </summary>
    public enum Frequently
    {
        /// <summary>
        /// Used if the 'event' is done on daily basis.
        /// </summary>
        Daily,

        /// <summary>
        /// Used if the 'event' is done on weekly basis.
        /// </summary>
        Weekly,

        /// <summary>
        /// Used if the 'event' is done on monthly basis.
        /// </summary>
        Monthly,

        /// <summary>
        /// Used if the 'event' is done on yearly basis.
        /// </summary>
        Yearly
    }
}