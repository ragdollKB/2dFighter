namespace UnityEngine.Advertisements
{
    /// <summary>
    /// ShowResult is passed to [[ShowOptions.resultCallback]] after the advertisement has completed.
    /// </summary>
    public enum ShowResult
    {
        /// <summary>
        /// Indicates that the advertisement failed to complete.
        /// </summary>
        Failed,
        /// <summary>
        /// Indicates that the advertisement was skipped.
        /// </summary>
        Skipped,
        /// <summary>
        /// Indicates that the advertisement completed successfully.
        /// </summary>
        Finished
    }
}
