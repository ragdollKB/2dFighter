using System;

namespace UnityEngine.Advertisements
{
    interface IPlatform
    {
        event EventHandler<ReadyEventArgs> OnReady;
        event EventHandler<StartEventArgs> OnStart;
        event EventHandler<FinishEventArgs> OnFinish;
        event EventHandler<ErrorEventArgs> OnError;

        bool isInitialized { get; }
        bool isSupported { get; }
        string version { get; }
        bool debugMode { get; set; }

        void Initialize(string gameId, bool testMode);
        bool IsReady(string placementId);
        PlacementState GetPlacementState(string placementId);
        void Show(string placementId);
        void SetMetaData(MetaData metaData);
    }
}
