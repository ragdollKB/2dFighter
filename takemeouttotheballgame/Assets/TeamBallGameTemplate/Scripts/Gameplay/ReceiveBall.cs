using TeamBallGame.Model;
using TeamBallGame;
using UnityEngine;
using TeamBallGame.Mechanics;

namespace TeamBallGame.Gameplay
{
    /// <summary>
    /// This event is fired when a player receives control of the ball.
    /// </summary>
    public class ReceiveBall : Simulation.Event<ReceiveBall>
    {
        public Player player;

        BallGameModel ballGame = Simulation.GetModel<BallGameModel>();
        BallGameConfig config = Simulation.GetModel<BallGameConfig>();

        internal override bool CheckPrecondition() => player != null && !player.IsBallOwner && ballGame.ball.IsInPlay;

        public override void Execute()
        {
            //give receiving player possession of the ball.
            ballGame.playerInPossession = player;
            config.activeGoalDirectionIndicator.source = player.transform;
            config.activeGoalDirectionIndicator.target = player.team.goal.transform;
            //if this player's team is not being controlled by a user
            if (player.IsAI)
            {
                Vector3 target = player.team.goal.transform.position;
                //if goal is more than 20 units away, pass to closer player.
                if ((target - player.transform.position).magnitude > 20)
                {
                    var other = ballGame.GetClosestPlayer(player.team.players, Vector3.Lerp(target, player.transform.position, 0.5f));
                    if (other != player)
                    {
                        var nv = Simulation.Schedule<PrepareToPassBall>(Fuzzy.Value(0.5f));
                        nv.target = other.transform.position;
                        nv.player = player;
                        nv.receiver = other;
                    }
                }
                else
                {
                    var nv = Simulation.Schedule<PrepareToLaunchBall>(0);
                    nv.target = target;
                    nv.player = player;
                }
            }
        }

        internal override void Cleanup()
        {
            player = null;
        }

    }
}
