using TeamBallGame;
using TeamBallGame.Model;

namespace TeamBallGame.Gameplay
{
    /// <summary>
    /// This event is scheduled when players come into contact with the
    /// ball or the player in possession of the ball. The actual contest
    /// (with all particpants) is resolved later.
    /// </summary>
    public class BallContest : Simulation.Event
    {
        public Player player;
        BallGameModel ballGame = Simulation.GetModel<BallGameModel>();

        internal override bool CheckPrecondition() => ballGame.ball.IsInPlay;

        public override void Execute()
        {
            ballGame.AddToContest(player);
        }

        internal override void Cleanup()
        {
            player = null;
        }
    }
}