using System;
using System.Collections.Generic;
using TeamBallGame.Mechanics;
using UnityEngine;

namespace TeamBallGame.Model
{
    /// <summary>
    /// This is a general model containing all data required for simulation of a ball game.
    /// It needs to be initialised at the start of a game by the GameController.
    /// It also contains methods for getting useful information from the model.
    /// </summary>
    [System.Serializable]
    public class BallGameModel
    {
        /// <summary>
        /// The ball.
        /// </summary>
        public Ball ball;

        public Team homeTeam, awayTeam;

        public Player homePlayerPrefab, awayPlayerPrefab;

        public float maxSpeed = 5;
        [Range(0, 1)]
        public float backwardsSpeedPenalty = 0.7f;
        public float maxKickDistance = 30;
        public float maxTurnSpeed = 360;


        /// <summary>
        /// The player currently in possession of the ball.
        /// </summary>
        public Player playerInPossession;

        /// <summary>
        /// The list of all players from both teams.
        /// </summary>
        public Player[] players;

        /// <summary>
        /// The list of players involved in an active contest.
        /// </summary>
        public List<Player> activeContest = new List<Player>();

        /// <summary>
        /// Is the ball currently possessed by a player from the home team?
        /// </summary>
        public bool IsBallUnderUserControl => playerInPossession?.team.teamType == TeamType.Home;

        public int homeScore = 0;

        public int awayScore = 0;

        /// <summary>
        /// Goal components for each team.
        /// </summary>
        public Goal homeGoal, awayGoal;

        public float durationFromGoalToBallup = 4;
        public float timeBetweenTackles = 2;
        public float tackleRecoveryTime = 1.3f;


        /// <summary>
        /// Query if the player is one of the closest two players to the ball.
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public bool IsClosest(Player player)
        {
            return player == homeTeam.players[0] || player == awayTeam.players[0];
        }

        /// <summary>
        /// Add a player to the active contest, which will be 
        /// resolved later by an external system.
        /// </summary>
        /// <param name="player"></param>
        public void AddToContest(Player player)
        {
            activeContest.Add(player);
        }

        public Player GetClosestPlayer(Player[] players, Vector3 position)
        {
            var minDelta = float.MaxValue;
            Player closest = null;
            foreach (var p in players)
            {
                var delta = (p.transform.position - position).sqrMagnitude;
                if (delta < minDelta)
                {
                    closest = p;
                    minDelta = delta;
                }
            }
            return closest;
        }
    }
}