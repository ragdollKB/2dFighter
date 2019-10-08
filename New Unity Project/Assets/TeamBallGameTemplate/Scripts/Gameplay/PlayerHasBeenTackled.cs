using TeamBallGame.Model;
using UnityEngine;
using static TeamBallGame.Simulation;

namespace TeamBallGame.Gameplay
{

    /// <summary>
    /// This event is fired when a player has been tackled.
    /// </summary>
    /// <typeparam name="PlayerHasBeenTackled"></typeparam>
    public class PlayerHasBeenTackled : Simulation.Event<PlayerHasBeenTackled>
    {
        public Player player;
        public Player tackler;
        public Vector3 direction;
        public float tacklePower = 5;

        BallGameModel ballGame = Simulation.GetModel<BallGameModel>();

        public override void Execute()
        {
            player.animator.SetTrigger("Tackled");
            player.SetMovement(enabled: false);
            player.OnHasBeenTackled();
            Schedule<PlayerRecoversFromTackle>(ballGame.tackleRecoveryTime).player = player;
            if (player.IsBallOwner && ballGame.ball.IsInPlay)
            {
                //apply velocity to rigidbody
                ballGame.ball.rigidbody.velocity = direction * tacklePower;
                //once launched, the ball is no longer possessed by the player.
                ballGame.playerInPossession = null;
            }
        }

        internal override void Cleanup()
        {
            player = null;
            tackler = null;
        }
    }
}