using TeamBallGame.Mechanics;
using TeamBallGame.Model;
using UnityEngine;

namespace TeamBallGame.Gameplay
{
    /// <summary>
    /// This event is fired when the head collider of a player makes contact
    /// with the ball.
    /// </summary>
    /// <typeparam name="HeadBallCollision"></typeparam>
    public class HeadBallCollision : Simulation.Event<HeadBallCollision>
    {
        public Player player;
        public Vector3 deltaToBall;

        BallGameModel ballGame = Simulation.GetModel<BallGameModel>();
        BallGameConfig config = Simulation.GetModel<BallGameConfig>();

        public override void Execute()
        {
            ballGame.ball.impactAudio.Play(deltaToBall.magnitude, config.headBallCollisionAudio);
            ballGame.ball.rigidbody.AddForce(Vector3.Reflect(deltaToBall, player.transform.up));
        }

        internal override void Cleanup()
        {
            player = null;
        }
    }
}