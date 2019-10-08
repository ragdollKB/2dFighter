using System.Collections.Generic;

namespace UnityEngine.Advertisements
{
    /// <summary>
    /// Class for sending various metadata to UnityAds.
    /// </summary>
    public sealed class MetaData
    {
        readonly IDictionary<string, object> m_MetaData = new Dictionary<string, object>();

        /// <summary>
        /// Metadata category.
        /// </summary>
        public string category { get; private set; }

        /// <summary>
        /// Constructs an metadata instance that can be passed to the Advertisement class.
        /// </summary>
        public MetaData(string category)
        {
            this.category = category;
        }

        /// <summary>
        /// Sets new metadata fields.
        /// </summary>
        /// <param name="key">Metadata key.</param>
        /// <param name="value">Metadata value. Must be JSON serializable.</param>
        public void Set(string key, object value)
        {
            m_MetaData[key] = value;
        }

        /// <summary>
        /// Returns the stored metadata key.
        /// </summary>
        public object Get(string key)
        {
            return m_MetaData[key];
        }

        /// <summary>
        /// Returns the stored metadata.
        /// </summary>
        public IDictionary<string, object> Values()
        {
            return m_MetaData;
        }

        internal string ToJSON()
        {
            return MiniJSON.Json.Serialize(m_MetaData);
        }
    }
}
