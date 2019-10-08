using System;

namespace UnityEngine.Advertisements
{
    /// <summary>
    /// Collection of options that can be passed to [[Advertisements.Show]] to modify advertisement behaviour.
    /// </summary>
    public class ShowOptions
    {
        /// <summary>
        /// Callback to receive the result of the advertisement.
        /// </summary>
        public Action<ShowResult> resultCallback { get; set; }
        /// <summary>
        /// Add a string to specify an identifier for a specific user in the game.
        /// </summary>
        public string gamerSid { get; set; }
    }
}
