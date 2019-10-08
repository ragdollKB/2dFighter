using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace UnityEngine.Advertisements.Tests
{
    [TestFixture]
    public class AdvertisementTests
    {
        [Test]
        public void DebugMode()
        {
            Advertisement.debugMode = false;
            Assert.That(Advertisement.debugMode, Is.False);

            Advertisement.debugMode = true;
            Assert.That(Advertisement.debugMode, Is.True);
        }

        [Test]
        [UnityPlatform(RuntimePlatform.Android, RuntimePlatform.IPhonePlayer)]
        public void IsSupported()
        {
            var isSupported = Application.platform == RuntimePlatform.Android
                              || Application.platform == RuntimePlatform.IPhonePlayer
                              || Application.platform == RuntimePlatform.OSXEditor;

            Assert.That(Advertisement.isSupported, Is.EqualTo(isSupported));
        }

        [Test]
        public void Version()
        {
            Assert.That(Advertisement.version, Does.Match(@"[0-9]+\.[0-9]+\.[0-9]+"));
        }

        [UnityTest, Order(0), Timeout(60000)]
        [UnityPlatform(RuntimePlatform.Android, RuntimePlatform.IPhonePlayer)]
        public IEnumerator Initialize()
        {
            Advertisement.debugMode = false;

            MetaData metaData = new MetaData("test");
            metaData.Set("serverUrl", "https://fake-ads-backend.applifier.info");
            metaData.Set("autoClose", "true");
            Advertisement.SetMetaData(metaData);

            Advertisement.Initialize("457");

            Assert.That(Advertisement.isInitialized, Is.True);
            Assert.That(Advertisement.isShowing, Is.False);

            while (!Advertisement.IsReady("defaultVideoAndPictureZone"))
            {
                yield return null;
            }

            Assert.That(Advertisement.GetPlacementState("defaultVideoAndPictureZone"),
                Is.EqualTo(PlacementState.Ready));
        }

        [UnityTest, Timeout(120000), Order(1)]
        [UnityPlatform(RuntimePlatform.Android, RuntimePlatform.IPhonePlayer)]
        public IEnumerator CallbackIsReceivedWhenAdFinishes()
        {
            Assert.That(Advertisement.isInitialized, Is.True);
            Assert.That(Advertisement.isShowing, Is.False);

            yield return new MonoBehaviourTest<UnityAdsScript>();
        }
    }
}