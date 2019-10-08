using TeamBallGame.Model;
using UnityEngine;

namespace TeamBallGame.Gameplay
{
    /// <summary>
    /// This event is fired when a player is passing the ball.
    /// </summary>
    /// <typeparam name="PrepareToPassBall"></typeparam>
    public class PrepareToPassBall : Simulation.Event<PrepareToPassBall>
    {
        public Vector3 target;
        public float delay = 0.125f;
        public Player player, receiver;

        BallGameModel ballGame = Simulation.GetModel<BallGameModel>();

        internal override bool CheckPrecondition() => player != null && player.IsBallOwner && ballGame.ball.IsInPlay;

        public override void Execute()
        {
            var ev = Simulation.Schedule<PrepareToLaunchBall>(delay);
            ev.target = target;
            ev.player = player;
        }

        internal override void Cleanup()
        {
            player = null;
            receiver = null;
        }
    }
}