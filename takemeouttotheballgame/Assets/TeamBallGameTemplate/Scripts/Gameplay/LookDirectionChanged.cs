using TeamBallGame.Mechanics;
using TeamBallGame.Model;
using UnityEngine;

namespace TeamBallGame.Gameplay
{
    /// <summary>
    /// This event is fired when the user input has changed the look direction
    /// of the active player.
    /// </summary>
    /// <typeparam name="LookDirectionChanged"></typeparam>
    public class LookDirectionChanged : Simulation.Event<LookDirectionChanged>
    {
        public Player player;
        public Vector3 direction;

        BallGameModel ballGame = Simulation.GetModel<BallGameModel>();
        BallGameConfig config = Simulation.GetModel<BallGameConfig>();

        public override void Execute()
        {
            RaycastHit hit;
            if (Physics.SphereCast(player.transform.position + player.transform.forward * 6, 2, player.transform.forward, out hit, ballGame.maxKickDistance, player.team.layer.value))
            {
                var teammate = hit.collider.GetComponent<Player>();
                config.recvReticle.transform.position = teammate.ReticlePosition;
                config.arrowIndicator.SetPassIndicator(true);
            }
            else
            {
                config.recvReticle.transform.position = Vector3.up * 5000;
                config.arrowIndicator.SetPassIndicator(false);
            }
        }

        internal override void Cleanup()
        {
            player = null;
        }
    }
}