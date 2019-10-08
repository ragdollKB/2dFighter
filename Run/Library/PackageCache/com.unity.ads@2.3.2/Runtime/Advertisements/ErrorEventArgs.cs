using System;

namespace UnityEngine.Advertisements
{
    public class ErrorEventArgs : EventArgs
    {
        // error is not mapped to an enum as it is not exposed to public API, can be changed later
        public long error { get; private set; }
        public string message { get; private set; }

        public ErrorEventArgs(long error, string message)
        {
            this.error = error;
            this.message = message;
        }
    }
}
