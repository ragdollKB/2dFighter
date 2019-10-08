using System.Collections;
using System.Collections.Generic;
using TeamBallGame.Model;
using UnityEngine;

namespace TeamBallGame.Mechanics
{
    public class CrowdController : MonoBehaviour
    {

        public AnimationCurve intensity;
        public Transform homeGoal, awayGoal;

        BallGameModel ballGame = Simulation.GetModel<BallGameModel>();
        BallGameConfig config = Simulation.GetModel<BallGameConfig>();

        new AudioSource audio;
        float distanceBetweenGoals;
        float volumeVelocity;

        void Awake()
        {
            audio = GetComponent<AudioSource>();
            distanceBetweenGoals = (homeGoal.position - awayGoal.position).magnitude;
        }


        void Update()
        {
            if (audio)
            {
                var distanceToHome = (ballGame.ball.transform.position - homeGoal.position).magnitude;
                var normalizedDistance = distanceToHome / distanceBetweenGoals;
                var crowdVolume = intensity.Evaluate(1f - normalizedDistance);
                audio.volume = Mathf.SmoothDamp(audio.volume, crowdVolume, ref volumeVelocity, 1, float.MaxValue);
            }
        }
    }
}