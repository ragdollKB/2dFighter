#if UNITY_EDITOR
    using System;
    using System.IO;
    using System.Net;
#elif UNITY_IOS
    using System;
    using System.Runtime.InteropServices;
    using AOT;
#elif UNITY_ANDROID
    using System;
    using System.Collections.Generic;
#endif


namespace UnityEngine.Advertisements
{
#if UNITY_EDITOR 
    sealed internal class Platform : IPlatform
    {
        static string s_BaseUrl = "http://adserver.unityads.unity3d.com/games";
        static string s_Version = "2.3.0";

        bool m_DebugMode;
        Configuration m_Configuration;
        Placeholder m_Placeholder;

        public event EventHandler<ReadyEventArgs> OnReady { add {} remove {} }
        public event EventHandler<StartEventArgs> OnStart;
        public event EventHandler<FinishEventArgs> OnFinish;
        public event EventHandler<ErrorEventArgs> OnError { add {} remove {} }

        public bool isInitialized
        {
            get
            {
                return m_Configuration != null;
            }
        }

        public bool isSupported
        {
            get
            {
                return Application.isEditor;
            }
        }

        public string version
        {
            get
            {
                return s_Version;
            }
        }

        public bool debugMode
        {
            get
            {
                return m_DebugMode;
            }
            set
            {
                m_DebugMode = value;
            }
        }

        public void Initialize(string gameId, bool testMode)
        {
            if (debugMode)
            {
                Debug.Log("UnityAdsEditor: Initialize(" + gameId + ", " + testMode + ");");
            }

            var placeHolderGameObject = new GameObject("UnityAdsEditorPlaceHolderObject")
            {
                hideFlags = HideFlags.HideAndDontSave | HideFlags.HideInInspector
            };
            m_Placeholder = placeHolderGameObject.AddComponent<Placeholder>();
            m_Placeholder.OnFinish += (object sender, FinishEventArgs e) =>
            {
                var handler = OnFinish;
                if (handler != null)
                {
                    handler(sender, new FinishEventArgs(e.placementId, e.showResult));
                }
            };

            string configurationUrl = string.Join("/", new string[] {
                s_BaseUrl,
                gameId,
                string.Join("&", new string[] {
                    "configuration?platform=editor",
                    "unityVersion=" + Uri.EscapeDataString(Application.unityVersion),
                    "sdkVersionName=" + Uri.EscapeDataString(version)
                })
            });
            WebRequest request = WebRequest.Create(configurationUrl);
            request.BeginGetResponse(result =>
            {
                WebResponse response = request.EndGetResponse(result);
                var reader = new StreamReader(response.GetResponseStream());
                string responseBody = reader.ReadToEnd();
                try
                {
                    m_Configuration = new Configuration(responseBody);
                    if (!m_Configuration.enabled)
                    {
                        Debug.LogWarning("gameId " + gameId + " is not enabled");
                    }
                }
                catch (Exception exception)
                {
                    Debug.LogError("Failed to parse configuration for gameId: " + gameId);
                    Debug.Log(responseBody);
                    Debug.LogException(exception);
                }
                reader.Close();
                response.Close();
            }, null);
        }

        public bool IsReady(string placementId)
        {
            if (placementId == null)
            {
                return isInitialized;
            }
            return isInitialized && m_Configuration.placements.ContainsKey(placementId);
        }

        public PlacementState GetPlacementState(string placementId)
        {
            if (IsReady(placementId))
            {
                return PlacementState.Ready;
            }
            return PlacementState.NotAvailable;
        }

        public void Show(string placementId)
        {
            // If placementId is null, use explicit defaultPlacement to match native behaviour
            if (isInitialized && placementId == null)
            {
                placementId = m_Configuration.defaultPlacement;
            }
            if (IsReady(placementId))
            {
                var handler = OnStart;
                if (handler != null)
                {
                    handler(this, new StartEventArgs(placementId));
                }
                m_Placeholder.Show(placementId, m_Configuration.placements[placementId]);
            }
            else
            {
                var handler = OnFinish;
                if (handler != null)
                {
                    handler(this, new FinishEventArgs(placementId, ShowResult.Failed));
                }
            }
        }

        public void SetMetaData(MetaData metaData)
        {
        }
    }
#elif UNITY_IOS
    sealed internal class Platform : IPlatform, IPurchasingEventSender
    {       
        static Platform s_Instance;
        
        static CallbackExecutor s_CallbackExecutor;

        delegate void unityAdsReady(string placementId);
        delegate void unityAdsDidError(long rawError, string message);
        delegate void unityAdsDidStart(string placementId);
        delegate void unityAdsDidFinish(string placementId, long rawShowResult);
        delegate void unityAdsPurchasingDidInitiatePurchasingCommand(string eventString);
        delegate void unityAdsPurchasingGetProductCatalog();
        delegate void unityAdsPurchasingGetPurchasingVersion();
        delegate void unityAdsPurchasingInitialize();

        [DllImport("__Internal")]
        static extern void UnityAdsInitialize(string gameId, bool testMode);
        
        [DllImport("__Internal")]
        static extern void UnityAdsPurchasingDispatchReturnEvent(long eventType, string payload);

        [DllImport("__Internal")]
        static extern void UnityAdsShow(string placementId);
        
        [DllImport("__Internal")]
        static extern bool UnityAdsGetDebugMode();
        
        [DllImport("__Internal")]
        static extern void UnityAdsSetDebugMode(bool debugMode);
        
        [DllImport("__Internal")]
        static extern bool UnityAdsIsSupported();
        
        [DllImport("__Internal")]
        static extern bool UnityAdsIsReady(string placementId);
        
        [DllImport("__Internal")]
        static extern long UnityAdsGetPlacementState(string placementId);
        
        [DllImport("__Internal")]
        static extern string UnityAdsGetVersion();
        
        [DllImport("__Internal")]
        static extern bool UnityAdsIsInitialized();
        
        [DllImport("__Internal")]
        static extern void UnityAdsSetMetaData(string category, string data);

        [DllImport("__Internal")]
        static extern void UnityAdsSetReadyCallback(unityAdsReady callback);
        
        [DllImport("__Internal")]
        static extern void UnityAdsSetDidErrorCallback(unityAdsDidError callback);
        
        [DllImport("__Internal")]
        static extern void UnityAdsSetDidStartCallback(unityAdsDidStart callback);
        
        [DllImport("__Internal")]
        static extern void UnityAdsSetDidFinishCallback(unityAdsDidFinish callback);
        
        [DllImport("__Internal")]
        static extern void UnityAdsSetDidInitiatePurchasingCommandCallback(unityAdsPurchasingDidInitiatePurchasingCommand callback);
        
        [DllImport("__Internal")]
        static extern void UnityAdsSetGetProductCatalogCallback(unityAdsPurchasingGetProductCatalog callback);
        
        [DllImport("__Internal")]
        static extern void UnityAdsSetGetVersionCallback(unityAdsPurchasingGetPurchasingVersion callback);
        
        [DllImport("__Internal")]
        static extern void UnityAdsSetInitializePurchasingCallback(unityAdsPurchasingInitialize callback);
        
        [MonoPInvokeCallback(typeof(unityAdsReady))]
        static void UnityAdsReady(string placementId)
        {
            s_CallbackExecutor.Post((executor) =>
            {
                var handler = s_Instance.OnReady;
                if (handler != null)
                {
                    handler(s_Instance, new ReadyEventArgs(placementId));
                }
            });
        }
        
        [MonoPInvokeCallback(typeof(unityAdsDidError))]
        static void UnityAdsDidError(long rawError, string message)
        {
            s_CallbackExecutor.Post((executor) =>
            {
                var handler = s_Instance.OnError;
                if (handler != null)
                {
                    handler(s_Instance, new ErrorEventArgs(rawError, message));
                }
            });
        }
        
        [MonoPInvokeCallback(typeof(unityAdsDidStart))]
        static void UnityAdsDidStart(string placementId)
        {
            s_CallbackExecutor.Post((executor) =>
            {
                var handler = s_Instance.OnStart;
                if (handler != null)
                {
                    handler(s_Instance, new StartEventArgs(placementId));
                }
            });
        }
        
        [MonoPInvokeCallback(typeof(unityAdsDidFinish))]
        static void UnityAdsDidFinish(string placementId, long rawShowResult)
        {
            s_CallbackExecutor.Post((executor) =>
            {
                var handler = s_Instance.OnFinish;
                if (handler != null)
                {
                    handler(s_Instance, new FinishEventArgs(placementId, (ShowResult)rawShowResult));
                }
            });
        }
        
        [MonoPInvokeCallback(typeof(unityAdsPurchasingDidInitiatePurchasingCommand))]
        static void UnityAdsDidInitiatePurchasingCommand(string eventString)
        {
            string result = Purchasing.InitiatePurchasingCommand(eventString).ToString();
            UnityAdsPurchasingDispatchReturnEvent((long)PurchasingEvent.COMMAND, result);
        }
        
        [MonoPInvokeCallback(typeof(unityAdsPurchasingGetProductCatalog))]
        static void UnityAdsPurchasingGetProductCatalog()
        {
            string result = Purchasing.GetPurchasingCatalog();
            UnityAdsPurchasingDispatchReturnEvent((long)PurchasingEvent.CATALOG, result);
        }
        
        [MonoPInvokeCallback(typeof(unityAdsPurchasingGetPurchasingVersion))]
        static void UnityAdsPurchasingGetPurchasingVersion()
        {
            string result = Purchasing.GetPromoVersion();
            UnityAdsPurchasingDispatchReturnEvent((long)PurchasingEvent.VERSION, result);
        }
        
        [MonoPInvokeCallback(typeof(unityAdsPurchasingInitialize))]
        static void UnityAdsPurchasingInitialize()
        {
            string result = Purchasing.Initialize(s_Instance).ToString();
            UnityAdsPurchasingDispatchReturnEvent((long)PurchasingEvent.INITIALIZATION, result);
        }

        public event EventHandler<ReadyEventArgs> OnReady;
        public event EventHandler<StartEventArgs> OnStart;
        public event EventHandler<FinishEventArgs> OnFinish;
        public event EventHandler<ErrorEventArgs> OnError;

        public Platform()
        {
            s_Instance = this;

            var callbackExecutorGameObject = new GameObject("UnityAdsCallbackExecutorObject")
            {
                hideFlags = HideFlags.HideAndDontSave | HideFlags.HideInInspector
            };
            s_CallbackExecutor = callbackExecutorGameObject.AddComponent<CallbackExecutor>();
            Object.DontDestroyOnLoad(callbackExecutorGameObject);

            UnityAdsSetReadyCallback(UnityAdsReady);
            UnityAdsSetDidErrorCallback(UnityAdsDidError);
            UnityAdsSetDidStartCallback(UnityAdsDidStart);
            UnityAdsSetDidFinishCallback(UnityAdsDidFinish);
            UnityAdsSetDidInitiatePurchasingCommandCallback(UnityAdsDidInitiatePurchasingCommand);
            UnityAdsSetGetProductCatalogCallback(UnityAdsPurchasingGetProductCatalog);
            UnityAdsSetGetVersionCallback(UnityAdsPurchasingGetPurchasingVersion);
            UnityAdsSetInitializePurchasingCallback(UnityAdsPurchasingInitialize);
        }
        
        public bool isInitialized
        {
            get
            {
                return UnityAdsIsInitialized();
            }
        }

        public bool isSupported
        {
            get
            {
                return UnityAdsIsSupported();
            }
        }
        
        public string version
        {
            get
            {
                return UnityAdsGetVersion();
            }
        }
        
        public bool debugMode
        {
            get
            {
                return UnityAdsGetDebugMode();
            }
            set
            {
                UnityAdsSetDebugMode(value);
            }
        }
        
        public void Initialize(string gameId, bool testMode)
        {
            UnityAdsInitialize(gameId, testMode);
        }
        
        public bool IsReady(string placementId)
        {
            return UnityAdsIsReady(placementId);
        }
        
        public PlacementState GetPlacementState(string placementId)
        {
            return (PlacementState)UnityAdsGetPlacementState(placementId);
        }
        
        public void Show(string placementId)
        {
            UnityAdsShow(placementId);
        }
        
        public void SetMetaData(MetaData metaData)
        {
            UnityAdsSetMetaData(metaData.category, metaData.ToJSON());
        }
        
        void IPurchasingEventSender.SendPurchasingEvent(string payload)
        {
            UnityAdsPurchasingDispatchReturnEvent((long)PurchasingEvent.EVENT, payload);
        }
    }
#elif UNITY_ANDROID
    sealed internal class Platform : AndroidJavaProxy, IPlatform,  IPurchasingEventSender
    {
        readonly AndroidJavaObject m_CurrentActivity;
        readonly AndroidJavaClass m_UnityAds;
        readonly Purchase m_UnityAdsPurchase;
        readonly CallbackExecutor m_CallbackExecutor;

        void onUnityAdsReady(string placementId)
        {
            m_CallbackExecutor.Post((executor) =>
            {
                var handler = OnReady;
                if (handler != null)
                {
                    handler(this, new ReadyEventArgs(placementId));
                }
            });
        }

        void onUnityAdsStart(string placementId)
        {
            m_CallbackExecutor.Post((executor) =>
            {
                var handler = OnStart;
                if (handler != null)
                {
                    handler(this, new StartEventArgs(placementId));
                }
            });
        }

        void onUnityAdsFinish(string placementId, AndroidJavaObject rawShowResult)
        {
            var showResult = (ShowResult)rawShowResult.Call<int>("ordinal");
            m_CallbackExecutor.Post((executor) =>
            {
                var handler = OnFinish;
                if (handler != null)
                {
                    handler(this, new FinishEventArgs(placementId, showResult));
                }
            });
        }

        void onUnityAdsError(AndroidJavaObject rawError, string message)
        {
            var error = (long)rawError.Call<int>("ordinal");
            m_CallbackExecutor.Post((executor) =>
            {
                var handler = OnError;
                if (handler != null)
                {
                    handler(this, new ErrorEventArgs(error, message));
                }
            });
        }

        public event EventHandler<ReadyEventArgs> OnReady;
        public event EventHandler<StartEventArgs> OnStart;
        public event EventHandler<FinishEventArgs> OnFinish;
        public event EventHandler<ErrorEventArgs> OnError;

        public Platform() : base("com.unity3d.ads.IUnityAdsListener")
        {
            var unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            m_CurrentActivity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity");
            m_UnityAds = new AndroidJavaClass("com.unity3d.ads.UnityAds");
            m_UnityAdsPurchase = new Purchase();

            var callbackExecutorGameObject = new GameObject("UnityAdsCallbackExecutorObject")
            {
                hideFlags = HideFlags.HideAndDontSave | HideFlags.HideInInspector
            };
            m_CallbackExecutor = callbackExecutorGameObject.AddComponent<CallbackExecutor>();
            Object.DontDestroyOnLoad(callbackExecutorGameObject);
        }

        public bool isInitialized
        {
            get
            {
                return m_UnityAds.CallStatic<bool>("isInitialized");
            }
        }

        public bool isSupported
        {
            get
            {
                return m_UnityAds.CallStatic<bool>("isSupported");
            }
        }

        public string version
        {
            get
            {
                return m_UnityAds.CallStatic<string>("getVersion");
            }
        }

        public bool debugMode
        {
            get
            {
                return m_UnityAds.CallStatic<bool>("getDebugMode");
            }
            set
            {
                m_UnityAds.CallStatic("setDebugMode", value);
            }
        }

        public void Initialize(string gameId, bool testMode)
        {
            m_UnityAds.CallStatic("initialize", m_CurrentActivity, gameId, this, testMode);
            m_UnityAdsPurchase.Initialize(this);
        }

        public bool IsReady(string placementId)
        {
            if (placementId == null)
            {
                return m_UnityAds.CallStatic<bool>("isReady");
            }
            return m_UnityAds.CallStatic<bool>("isReady", placementId);
        }

        public PlacementState GetPlacementState(string placementId)
        {
            AndroidJavaObject rawPlacementState;
            if (placementId == null)
            {
                rawPlacementState = m_UnityAds.CallStatic<AndroidJavaObject>("getPlacementState");
            }
            else
            {
                rawPlacementState = m_UnityAds.CallStatic<AndroidJavaObject>("getPlacementState", placementId);
            }
            return (PlacementState)rawPlacementState.Call<int>("ordinal");
        }

        public void Show(string placementId)
        {
            if (placementId == null)
            {
                m_UnityAds.CallStatic("show", m_CurrentActivity);
            }
            else
            {
                m_UnityAds.CallStatic("show", m_CurrentActivity, placementId);
            }
        }

        public void SetMetaData(MetaData metaData)
        {
            var metaDataObject = new AndroidJavaObject("com.unity3d.ads.metadata.MetaData", m_CurrentActivity);
            metaDataObject.Call("setCategory", metaData.category);
            foreach (KeyValuePair<string, object> entry in metaData.Values())
            {
                metaDataObject.Call<bool>("set", entry.Key, entry.Value);
            }
            metaDataObject.Call("commit");
        }

        void IPurchasingEventSender.SendPurchasingEvent(string payload)
        {
            m_UnityAdsPurchase.SendEvent(payload);
        }
    }
    #endif
}