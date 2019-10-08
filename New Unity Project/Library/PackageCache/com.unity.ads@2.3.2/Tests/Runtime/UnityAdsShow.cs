using UnityEngine.TestTools;

namespace UnityEngine.Advertisements.Tests
{
    public class UnityAdsScript : MonoBehaviour, IMonoBehaviourTest
    {
        public const float kSecondsToWait = 30f;

        private const string kPlacementId = "defaultVideoAndPictureZone";

        private bool m_ShowHasBeenCalled;
        private float m_ElapsedTime;
        private bool m_TestFinished = false;

        public bool IsTestFinished
        {
            get { return m_TestFinished; }
        }

        void Update()
        {
            m_ElapsedTime += Time.deltaTime;

            if (!Advertisement.IsReady(kPlacementId))
            {
                if (m_ElapsedTime > kSecondsToWait)
                {
                    Debug.LogError(string.Format("Expected advertisement to be ready after max. {0} seconds",
                        kSecondsToWait));
                    m_TestFinished = true;
                }

                return;
            }

            if (!m_ShowHasBeenCalled)
            {
                Advertisement.debugMode = false;
                Advertisement.Show(kPlacementId, new ShowOptions
                {
                    resultCallback = showResult =>
                    {
                        Debug.Log(string.Format("=== UnityAds result: {0} (after {1:n0} seconds) ===", showResult,
                            m_ElapsedTime));
                        m_TestFinished = true;
                    }
                });
                m_ShowHasBeenCalled = true;
            }
        }
    }
}