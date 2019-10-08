using TeamBallGame.Mechanics;
using TeamBallGame.Model;
using UnityEngine;

namespace TeamBallGame.Gameplay
{
    /// <summary>
    /// This event occurs whenever the ball bounces on the field.
    /// </summary>
    /// <typeparam name="BallBounce"></typeparam>
    public class BallBounce : Simulation.Event<BallBounce>
    {
        BallGameModel ballGame = Simulation.GetModel<BallGameModel>();
        BallGameConfig config = Simulation.GetModel<BallGameConfig>();

        public Collision collision;

        public override void Execute()
        {
            ballGame.ball.impactAudio.Play(collision, config.ballBounceAudio);
            if (config.ballBounceParticles)
            {
                var particles = ComponentPool<ParticleSystem>.Take(config.ballBounceParticles);
                particles.transform.position = ballGame.ball.transform.position;
                particles.Play();
                ComponentPool<ParticleSystem>.Return(particles, 1);
            }

            //if the ballgame is not in possession, notifiy closer players towards the ball.
            if (!ballGame.ball.IsPossessed)
            {
                var position = ballGame.ball.transform.position;
                var A = ballGame.GetClosestPlayer(ballGame.awayTeam.players, position);
                var B = ballGame.GetClosestPlayer(ballGame.homeTeam.players, position);
                A.OnBallBounceNearMe(position);
                B.OnBallBounceNearMe(position);
            }
        }

        internal override void Cleanup()
        {
            collision = null;
        }
    }
}