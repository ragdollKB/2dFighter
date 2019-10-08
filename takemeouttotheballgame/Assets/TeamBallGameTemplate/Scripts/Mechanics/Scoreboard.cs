using System;
using System.Collections;
using System.Collections.Generic;
using TeamBallGame.Gameplay;
using TeamBallGame.Model;
using UnityEngine;

namespace TeamBallGame.Mechanics
{
    /// <summary>
    /// Scoreboard waits for ScoreEvents and updates text widgets with the 
    /// new score values.
    /// </summary>
    public class Scoreboard : MonoBehaviour
    {
        public TMPro.TMP_Text home, away;

        BallGameModel ballGame = Simulation.GetModel<BallGameModel>();

        void Awake()
        {
            //Subscribe the listener method to all Score events.
            Score.OnExecute += OnScoreEvent;
            //Set initial text on scoreboard to initial score values (0).
            home.text = $"{ballGame.homeScore}";
            away.text = $"{ballGame.awayScore}";
        }

        void OnScoreEvent(Score score)
        {
            //Update text values on scoreboard with home and away scores.
            home.text = $"{ballGame.homeScore}";
            away.text = $"{ballGame.awayScore}";
        }
    }
}