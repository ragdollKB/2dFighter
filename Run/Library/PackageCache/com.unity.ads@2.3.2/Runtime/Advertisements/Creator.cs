using System;

namespace UnityEngine.Advertisements
{
    static class Creator
    {
#if ASSET_STORE
        static internal IPlatform CreatePlatform()
        {
            try
            {
#if EDITOR
                return new Editor.Platform();
#elif ANDROID
                return new Android.Platform();
#elif IOS
                return new iOS.Platform();
#else
                return new UnsupportedPlatform();
#endif
            }
            catch (Exception exception)
            {
                try
                {
                    Debug.LogError("Initializing Unity Ads.");
                    Debug.LogException(exception);
                }
                catch (MissingMethodException)
                {
                }
                return new UnsupportedPlatform();
            }
        }

#else
        static bool initializeOnStartup
        {
            get
            {
                return UnityAdsSettings.initializeOnStartup;
            }
        }

        internal static bool enabled
        {
            get
            {
                return UnityAdsSettings.enabled;
            }
        }

        static bool testMode
        {
            get
            {
                return UnityAdsSettings.testMode;
            }
        }

        static string gameId
        {
            get
            {
                return UnityAdsSettings.GetGameId(Advertisement.application.platform);
            }
        }

        static internal Type ResolveType()
        {
            if (Advertisement.application.isEditor || 
                Advertisement.application.platform == RuntimePlatform.Android || 
                Advertisement.application.platform == RuntimePlatform.IPhonePlayer)
            {
                return typeof(Platform);
            }

            return typeof(UnsupportedPlatform);
        }

        static internal IPlatform CreatePlatform()
        {
            try
            {
                return (IPlatform)Activator.CreateInstance(ResolveType());
            }
            catch (Exception exception)
            {
                try
                {
                    Debug.LogError("Initializing Unity Ads.");
                    Debug.LogException(exception);
                }
                catch (MissingMethodException)
                {
                }
                return new UnsupportedPlatform();
            }
        }

        //Prevents LoadRuntime from getting called on unsupported platforms or when UnityConnect module is stripped but Ads is not.
        #if UNITY_ADS
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void LoadRuntime()
        {
            if (!Advertisement.isSupported)
            {
                return;
            }

            if (initializeOnStartup)
            {
                Advertisement.Initialize(gameId, testMode);
            }
        }
        #endif

#endif
    }
}
