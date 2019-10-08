using System;

namespace UnityEngine.Advertisements
{
    sealed class UnsupportedPlatform : IPlatform
    {
        public event EventHandler<ReadyEventArgs> OnReady { add {} remove {} }
        public event EventHandler<StartEventArgs> OnStart { add {} remove {} }
        public event EventHandler<FinishEventArgs> OnFinish;
        public event EventHandler<ErrorEventArgs> OnError { add {} remove {} }

        public bool isInitialized
        {
            get
            {
                return false;
            }
        }

        public bool isSupported
        {
            get
            {
                return false;
            }
        }

        public string version
        {
            get
            {
                return null;
            }
        }

        public bool debugMode
        {
            get
            {
                return false;
            }
            set
            {
            }
        }

        public void Initialize(string gameId, bool testMode)
        {
        }

        public bool IsReady(string placementId)
        {
            return false;
        }

        public PlacementState GetPlacementState(string placementId)
        {
            return PlacementState.NotAvailable;
        }

        public void Show(string placementId)
        {
            var handler = OnFinish;
            if (handler != null)
            {
                handler(this, new FinishEventArgs(placementId, ShowResult.Failed));
            }
        }

        public void SetMetaData(MetaData metaData)
        {
        }
    }
}
