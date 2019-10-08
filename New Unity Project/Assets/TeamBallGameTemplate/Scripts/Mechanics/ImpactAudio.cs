using UnityEngine;

namespace TeamBallGame.Mechanics
{
    [RequireComponent(typeof(AudioSource))]
    /// <summary>
    /// Plays impact audio, taking into account distance from audio listener and the speed of sound in air at 20C.
    /// </summary>
    public class ImpactAudio : MonoBehaviour
    {
        const float SPEED_OF_SOUND_IN_AIR = 343;

        public float estimatedMaxImpactVelocity = 16;
        public new AudioSource audio;

        AudioListener audioListener;

        public void Play(float magnitude, AudioClip clip)
        {
            audio.volume = Mathf.InverseLerp(0f, estimatedMaxImpactVelocity, magnitude);
            var distanceToListener = (transform.position - audioListener.transform.position).magnitude;
            var timeToPlay = distanceToListener / SPEED_OF_SOUND_IN_AIR;
            audio.clip = clip;
            audio.PlayScheduled(AudioSettings.dspTime + timeToPlay);
        }

        public void Play(Collision collision, AudioClip clip)
        {
            Play(collision.relativeVelocity.magnitude, clip);
        }


        void OnEnable()
        {
            audio = GetComponent<AudioSource>();
            audioListener = GameObject.FindObjectOfType<AudioListener>();
        }

    }
}