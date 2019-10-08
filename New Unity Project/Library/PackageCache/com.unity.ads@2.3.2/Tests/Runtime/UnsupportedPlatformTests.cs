using NUnit.Framework;

namespace UnityEngine.Advertisements.Tests
{
    [TestFixture]
    class UnsupportedPlatformTests
    {
        private UnsupportedPlatform m_Platform;

        [SetUp]
        public void SetUp()
        {
            m_Platform = new UnsupportedPlatform();
        }

        [Test]
        public void InitialState()
        {
            Assert.That(m_Platform.debugMode, Is.False);
            Assert.That(m_Platform.isInitialized, Is.False);
            Assert.That(m_Platform.isSupported, Is.False);
            Assert.That(m_Platform.version, Is.Null);
        }

        [TestCase("")]
        [TestCase(null)]
        [TestCase("test")]
        [TestCase("ads")]
        [TestCase("1")]
        public void IsReadyAndGetPlacementState(string placementId)
        {
            Assert.That(m_Platform.IsReady(placementId), Is.False);
            Assert.That(m_Platform.GetPlacementState(placementId), Is.EqualTo(PlacementState.NotAvailable));
        }

        [TestCase("")]
        [TestCase(null)]
        [TestCase("test")]
        [TestCase("ads")]
        [TestCase("1")]
        public void Show(string placementId)
        {
            Assert.DoesNotThrow(() => m_Platform.Show(placementId));
        }

        [TestCase("")]
        [TestCase(null)]
        [TestCase("test")]
        [TestCase("ads")]
        [TestCase("1")]
        public void ShowWithEvent(string placementId)
        {
            int count = 0;
            object sender = null;
            FinishEventArgs eventArgs = null;
            m_Platform.OnFinish += (s, a) =>
            {
                count++;
                sender = s;
                eventArgs = a;
            };

            Assert.DoesNotThrow(() => m_Platform.Show(placementId));

            Assert.That(count, Is.EqualTo(1));
            Assert.That(sender, Is.SameAs(m_Platform));
            Assert.That(eventArgs, Is.Not.Null);
            Assert.That(eventArgs.placementId, Is.EqualTo(placementId));
            Assert.That(eventArgs.showResult, Is.EqualTo(ShowResult.Failed));
        }

        [TestCase(null, false)]
        [TestCase("", false)]
        [TestCase("test", false)]
        [TestCase("123435", false)]
        [TestCase(null, true)]
        [TestCase("", true)]
        [TestCase("test", true)]
        [TestCase("123435", true)]
        public void Initialize(string gameId, bool testMode)
        {
            Assert.DoesNotThrow(() => m_Platform.Initialize(gameId, testMode));
        }

        [Test]
        public void SetMetaData()
        {
            Assert.DoesNotThrow(() => m_Platform.SetMetaData(new MetaData("test")));
        }

        [Test]
        public void Events()
        {
            Assert.DoesNotThrow(() => m_Platform.OnReady += (X, y) => {});
            Assert.DoesNotThrow(() => m_Platform.OnStart += (X, y) => {});
            Assert.DoesNotThrow(() => m_Platform.OnError += (X, y) => {});

            Assert.DoesNotThrow(() => m_Platform.OnReady -= (X, y) => {});
            Assert.DoesNotThrow(() => m_Platform.OnStart -= (X, y) => {});
            Assert.DoesNotThrow(() => m_Platform.OnError -= (X, y) => {});
        }
    }
}
