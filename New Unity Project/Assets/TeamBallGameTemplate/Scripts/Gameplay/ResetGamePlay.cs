using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TeamBallGame.Model;
using TeamBallGame;
using TeamBallGame.Mechanics;

namespace TeamBallGame.Gameplay
{
    /// <summary>
    /// Reset game play to a ball up or kick off state.
    /// Eg, move ball to center and all players to field positions.
    /// </summary>
    public class ResetGamePlay : Simulation.Event<ResetGamePlay>
    {
        BallGameModel ballGame = Simulation.GetModel<BallGameModel>();
        BallGameConfig config = Simulation.GetModel<BallGameConfig>();

        public override void Execute()
        {
            //The ball is no longer possessed by any player.
            ballGame.playerInPossession = null;
            //move the ball to center position, reset physical forces on the ball.
            var ball = ballGame.ball;
            ball.transform.position = Vector3.zero;
            ball.rigidbody.velocity = Vector3.zero;
            ball.rigidbody.angularVelocity = Vector3.zero;
            ball.IsInPlay = false;

            //move all players back to their starting field positions.
            Simulation.Schedule<DisableUserInput>(0);
            foreach (var p in ballGame.players)
            {
                p.SetState(Player.State.ReturnToPosition);
            }

            //disable all goal event related objects
            foreach (var i in config.enableOnGoal)
            {
                i.SetActive(false);
            }

            //next event is the ball up contest.
            var ev = Simulation.Schedule<BallUp>(ballGame.durationFromGoalToBallup);
            ev.position = Vector3.zero;
        }
    }
}