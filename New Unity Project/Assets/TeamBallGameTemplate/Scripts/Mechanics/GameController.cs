using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TeamBallGame.Gameplay;
using TeamBallGame.Model;
using TeamBallGame;

using System;

namespace TeamBallGame.Mechanics
{

    public class GameController : MonoBehaviour
    {
        public UserInput homeUserInput, awayUserInput;

        BallGameModel ballGame = Simulation.GetModel<BallGameModel>();
        BallGameConfig config = Simulation.GetModel<BallGameConfig>();

        void Start()
        {
            ballGame.homeTeam.UserInput = homeUserInput;
            SetupField();
            Simulation.Schedule<ResetGamePlay>(1);
            Simulation.Schedule<ResolveBallContest>(1f);
        }

        void Update()
        {
            UpdatePlayerControl();
            SortByNearestToBall(ballGame.homeTeam.players);
            SortByNearestToBall(ballGame.awayTeam.players);
            Simulation.Tick();
        }

        void SortByNearestToBall(Player[] players)
        {
            //actually, we only care about index 0 being the closest to ball!
            var ballPosition = ballGame.ball.transform.position;
            var closest = 0;
            var distance = (players[closest].transform.position - ballPosition).sqrMagnitude;
            for (int i = 1, n = players.Length; i < n; i++)
            {
                var d = (players[i].transform.position - ballPosition).sqrMagnitude;
                if (d < distance)
                {
                    distance = d;
                    closest = i;
                }
            }
            if (closest != 0)
            {
                var t = players[0];
                players[0] = players[closest];
                players[closest] = t;
            }
        }

        void UpdatePlayerControl()
        {
            homeUserInput.ActivePlayer = ballGame.homeTeam.players[0];
            if (homeUserInput.enabled)
            {
                homeUserInput.UpdateActivePlayer();
                config.reticle.transform.position = homeUserInput.ActivePlayer.ReticlePosition;
            }
        }

        void SetupField()
        {
            ballGame.homeTeam.InstantiatePlayers();
            ballGame.awayTeam.InstantiatePlayers();
            var size = ballGame.homeTeam.players.Length + ballGame.awayTeam.players.Length;
            ballGame.players = new Player[size];
            ballGame.homeTeam.players.CopyTo(ballGame.players, 0);
            ballGame.awayTeam.players.CopyTo(ballGame.players, ballGame.homeTeam.players.Length);
        }

        void OnDestroy()
        {
            Simulation.DestroyModel<BallGameModel>();
        }

    }
}