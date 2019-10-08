using TeamBallGame;
using TeamBallGame.Mechanics;
using TeamBallGame.Model;
using UnityEngine;

namespace TeamBallGame.Gameplay
{
    /// <summary>
    /// This event occurs when a player launches the ball.
    /// It is usually followed by a BallIsLaunched event.
    /// </summary>
    public class LaunchBall : Simulation.Event<LaunchBall>
    {
        public Vector3 target;
        public Player player;

        BallGameModel ballGame = Simulation.GetModel<BallGameModel>();
        BallGameConfig config = Simulation.GetModel<BallGameConfig>();

        //The launch event is only valid if the ball is currently possessed by a player.
        internal override bool CheckPrecondition() => player != null && player.IsBallOwner && ballGame.ball.IsInPlay;

        public override void Execute()
        {
            //prevent any existing rolling motion from effecting the ball.
            ballGame.ball.rigidbody.angularVelocity = Vector3.zero;

            //calculate initial velocity and duration of ball flight, create event.
            CalculateVelocity(target, out Vector3 velocity, out float duration);
            var ev = Simulation.Schedule<BallIsLaunched>(0);
            ev.playerThatLaunchedBall = player;
            ev.flightDuration = duration;
            ev.targetPosition = target;
            ev.velocity = velocity;

            //apply velocity to rigidbody
            ballGame.ball.rigidbody.velocity = velocity;

            ballGame.ball.impactAudio.Play(velocity.magnitude, config.ballKickAudio);

            Debug.DrawLine(ballGame.ball.transform.position, target, Color.blue, 2);

            //once launched, the ball is no longer possessed by the player.
            ballGame.playerInPossession = null;
        }

        public void CalculateVelocity(Vector3 target, out Vector3 velocity, out float duration)
        {
            var delta = target - ballGame.ball.transform.position;
            delta = Vector3.ClampMagnitude(delta, ballGame.maxKickDistance);
            var dir = delta.normalized;
            var angle = 10 + 35 * Mathf.InverseLerp(0, ballGame.maxKickDistance, delta.magnitude);
            dir = Vector3.Slerp(dir, Vector3.up, angle / 90f);
            var sinAngle = Mathf.Sin(2 * angle * Mathf.Deg2Rad);
            var g = Physics.gravity.magnitude;
            var v = Mathf.Sqrt(delta.magnitude * g / sinAngle);
            duration = (2 * v * sinAngle) / g;
            velocity = dir * v;
        }

        internal override void Cleanup()
        {
            player = null;
        }
    }
}