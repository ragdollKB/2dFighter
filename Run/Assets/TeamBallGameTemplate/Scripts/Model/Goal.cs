using System.Collections;
using System.Collections.Generic;
using TeamBallGame.Gameplay;
using UnityEngine;

namespace TeamBallGame.Model
{
    /// <summary>
    /// This behaviour is attached to goal trigger volumes, and will
    /// fire a Score event when a Ball component enters the trigger.
    /// </summary>
    public class Goal : MonoBehaviour
    {
        public TeamType teamType;

        public int scoreValue = 1;

        new AudioSource audio;

        void Awake()
        {
            audio = GetComponent<AudioSource>();
        }

        void OnTriggerEnter(Collider collider)
        {
            // if the ball entered this trigger, then fire a score event.
            var ball = collider.GetComponent<Ball>();
            if (ball != null)
            {
                var ev = Simulation.Schedule<Score>(0);
                ev.teamType = teamType;
                ev.goal = this;
                if (audio != null) audio.Play();
            }
        }
    }
}