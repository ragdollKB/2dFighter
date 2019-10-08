using TeamBallGame.Model;
using UnityEngine;

namespace TeamBallGame.Gameplay
{
    /// <summary>
    /// This event is continuously scheduled so that pending ball contests can be
    /// resolved and ball possession can be changed accordingly.
    /// </summary>
    public class ResolveBallContest : Simulation.Event
    {
        BallGameModel ballGame = Simulation.GetModel<BallGameModel>();

        public override void Execute()
        {
            //check if there is any contest to resolve.
            ResolvePendingContests();
            Simulation.Reschedule(this, 0.1f);
        }

        private void ResolvePendingContests()
        {
            if (ballGame.activeContest.Count > 0 && ballGame.ball.IsInPlay)
            {
                //if the ball is possesed by a player, add that player to the contest.
                if (ballGame.playerInPossession != null)
                    ballGame.activeContest.Add(ballGame.playerInPossession);

                //pick a random winner.
                var winner = ballGame.activeContest[Random.Range(0, ballGame.activeContest.Count)];
                winner.OnSuccessfulInterception();
                //schedule the receive ball event with the winner as soon as possible (0 seconds).
                var ev = Simulation.Schedule<ReceiveBall>(0);
                ev.player = winner;
                //clear the contest participants, ready for the next contest.
                ballGame.activeContest.Clear();
            }
        }
    }
}