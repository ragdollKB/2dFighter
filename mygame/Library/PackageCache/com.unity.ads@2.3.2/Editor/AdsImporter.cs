using UnityEngine;
using UnityEditor.Advertisements;
using UnityEditor.Build;

namespace UnityEditor
{
    [InitializeOnLoad]
    internal class AdsImporter
    {
        static AdsImporter()
        {
            BuildDefines.getScriptCompilationDefinesDelegates += (target, defines) =>
            {
                if (AdvertisementSettings.enabledForPlatform)
                {
                    defines.Add("UNITY_ADS");
                }
            };
        }
    }
}
