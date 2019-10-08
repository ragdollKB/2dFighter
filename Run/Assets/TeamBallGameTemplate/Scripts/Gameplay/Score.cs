using TeamBallGame;
using TeamBallGame.Mechanics;
using TeamBallGame.Model;
using UnityEngine;

namespace TeamBallGame.Gameplay
{
    /// <summary>
    /// This event is fired when the ball enters a Goal trigger.
    /// </summary>
    /// <typeparam name="Score"></typeparam>
    public class Score : Simulation.Event<Score>
    {
        //The team that scored the goal.
        public TeamType teamType;
        //The goal instance which activated the event.
        public Goal goal;

        BallGameModel ballGame = Simulation.GetModel<BallGameModel>();
        BallGameConfig config = Simulation.GetModel<BallGameConfig>();

        internal override bool CheckPrecondition() => ballGame.ball.IsInPlay;

        public override void Execute()
        {
            ballGame.ball.IsInPlay = false;
            if (config.goalScoreParticles)
            {
                var particles = ComponentPool<ParticleSystem>.Take(config.goalScoreParticles);
                particles.transform.position = ballGame.ball.transform.position;
                particles.Play();
                ComponentPool<ParticleSystem>.Return(particles, 2);
                foreach (var i in config.enableOnGoal)
                {
                    i.SetActive(true);
                }
            }
            //Add the score to the correct team in the game model.
            switch (teamType)
            {
                case TeamType.Home:
                    ballGame.homeScore += goal.scoreValue;
                    break;
                case TeamType.Away:
                    ballGame.awayScore += goal.scoreValue;
                    break;
            }
            //Next event is to reset gameplay.
            Simulation.Schedule<ResetGamePlay>(1);
        }

        internal override void Cleanup()
        {
            goal = null;
        }
    }
}