using TeamBallGame.Model;
using UnityEngine;

namespace TeamBallGame.Gameplay
{
    /// <summary>
    /// This event is fired when a player collides with the ball.
    /// Generally, this would start a BallContest.
    /// </summary>
    /// <typeparam name="PlayerBallCollision"></typeparam>
    public class PlayerBallCollision : Simulation.Event<PlayerBallCollision>
    {
        public Player player;
        public Collision collision;

        BallGameModel ballGame = Simulation.GetModel<BallGameModel>();

        //This event should only happen to players that are not in control of the ball.
        internal override bool CheckPrecondition() => !player.IsBallOwner && ballGame.ball.IsInPlay;

        public override void Execute()
        {
            var ev = Simulation.Schedule<BallContest>(0);
            ev.player = player;
        }

        internal override void Cleanup()
        {
            collision = null;
            player = null;
        }
    }
}