using TeamBallGame.Model;
using UnityEngine;

namespace TeamBallGame.Gameplay
{
    /// <summary>
    /// This event is fired when a player is launching the ball at a target position.
    /// </summary>
    /// <typeparam name="PrepareToLaunchBall"></typeparam>
    public class PrepareToLaunchBall : Simulation.Event<PrepareToLaunchBall>
    {
        public Vector3 target;
        public float delay = 0.25f;
        public Player player;

        BallGameModel ballGame = Simulation.GetModel<BallGameModel>();

        internal override bool CheckPrecondition() => player != null && player.IsBallOwner && ballGame.ball.IsInPlay;

        public override void Execute()
        {
            player.OnPrepareToLaunchBall(target);
            var ev = Simulation.Schedule<LaunchBall>(delay);
            ev.target = target;
            ev.player = player;
        }

        internal override void Cleanup()
        {
            player = null;
        }
    }
}