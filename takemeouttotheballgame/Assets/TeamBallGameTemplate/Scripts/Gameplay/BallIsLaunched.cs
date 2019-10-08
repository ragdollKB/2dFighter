using TeamBallGame.Model;
using UnityEngine;

namespace TeamBallGame.Gameplay
{
    /// <summary>
    /// This event is fired when the ball has been successfuly launched by a player.
    /// </summary>
    /// <typeparam name="BallIsLaunched"></typeparam>
    public class BallIsLaunched : Simulation.Event<BallIsLaunched>
    {
        public float flightDuration;
        public Player playerThatLaunchedBall;
        public Vector3 targetPosition;
        public Vector3 velocity;

        BallGameModel ballGame = Simulation.GetModel<BallGameModel>();

        internal override bool CheckPrecondition() => ballGame.ball.IsInPlay;

        public override void Execute()
        {
            var A = ballGame.GetClosestPlayer(ballGame.awayTeam.players, targetPosition);
            var B = ballGame.GetClosestPlayer(ballGame.homeTeam.players, targetPosition);
            A.OnBallWillLandNearMe(targetPosition);
            B.OnBallWillLandNearMe(targetPosition);

            //all players look at the player that launched the ball, 
            //except for the player that launched the ball, who looks 
            //at the target position.
            playerThatLaunchedBall.OnLaunchBall(targetPosition);
            foreach (var p in ballGame.players)
            {
                if (p != playerThatLaunchedBall)
                    p.OnOtherPlayerLaunchedBall(playerThatLaunchedBall, targetPosition);
            }
        }

        internal override void Cleanup()
        {
            playerThatLaunchedBall = null;
        }
    }
}