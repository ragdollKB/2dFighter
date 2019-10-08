using UnityEngine;
using TeamBallGame.Model;
using TeamBallGame;

namespace TeamBallGame.Gameplay
{
    /// <summary>
    /// This event occurs when the ball is stationary, or thrown in / bounced by an umpire
    /// and the two nearest players will compete for possession.
    /// </summary>
    public class BallUp : Simulation.Event<BallUp>
    {
        public Vector3 position;
        BallGameModel ballGame = Simulation.GetModel<BallGameModel>();

        internal override bool CheckPrecondition() => !ballGame.ball.IsInPlay;

        public override void Execute()
        {
            ballGame.ball.rigidbody.MovePosition(position);
            ballGame.ball.rigidbody.velocity = Vector3.up * 9;
            ballGame.ball.IsInPlay = true;
            Simulation.Schedule<StartGameplay>(1);
        }
    }
}