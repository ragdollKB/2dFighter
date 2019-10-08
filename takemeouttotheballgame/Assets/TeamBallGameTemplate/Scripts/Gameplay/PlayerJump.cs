using TeamBallGame;
using TeamBallGame.Model;

namespace TeamBallGame.Gameplay
{
    /// <summary>
    /// This event is fired when the jump input is activated.
    /// </summary>
    /// <typeparam name="PlayerJump"></typeparam>
    public class PlayerJump : Simulation.Event<PlayerJump>
    {
        public Player player;

        BallGameModel ballGame = Simulation.GetModel<BallGameModel>();

        public override void Execute()
        {
            //This will be overridden by UserInput if the player is under user control.
            player.OnPlayerJump();
            var deltaToBall = player.HeadPosition - ballGame.ball.transform.position;
            if (deltaToBall.sqrMagnitude < player.headSize * player.headSize)
            {
                var ev = Simulation.Schedule<HeadBallCollision>(0);
                ev.player = player;
                ev.deltaToBall = deltaToBall;
            }
        }

        internal override void Cleanup()
        {
            player = null;
        }
    }
}