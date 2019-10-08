using UnityEngine;

namespace TeamBallGame.Mechanics
{
    /// <summary>
    /// This class is designed to be accessed via Simulation.GetModel.
    /// It contains scene specific configuration items which are not
    /// directly related to gameplay. Gameplay specific data is stored
    /// in the BallGameModel class.
    /// </summary>
    [System.Serializable]
    public class BallGameConfig
    {
        public Sprite aiControlIcon, userControlIcon, refereeControlIcon;

        public Transform reticle, recvReticle;
        public LookIndicator arrowIndicator;
        public DirectionIndicator activeGoalDirectionIndicator;
        public ParticleSystem ballBounceParticles;
        public ParticleSystem goalScoreParticles;
        public GameObject[] enableOnGoal;
        public AudioClip ballBounceAudio;
        public AudioClip headBallCollisionAudio;
        public AudioClip ballKickAudio;
        public AudioClip tackleAudio;
        public AudioClip crowdAudio;
        public AudioClip tackledAudio;
        public AudioClip interceptionAudio;

    }
}