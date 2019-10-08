using TeamBallGame.Mechanics;
using TeamBallGame.Model;
using UnityEngine;

namespace TeamBallGame.Gameplay
{
    public class RepositionArrowIndicator : Simulation.Event<RepositionArrowIndicator>
    {
        public Vector3 position;
        public Vector3 direction;
        BallGameConfig config = Simulation.GetModel<BallGameConfig>();
        BallGameModel model = Simulation.GetModel<BallGameModel>();

        public override void Execute()
        {
            var arrow = config.arrowIndicator;
            if (arrow)
            {
                arrow.transform.position = position;
                arrow.transform.forward = direction;
                arrow.SetPositions(model.homeTeam.players[0].transform.position, position);
            }
        }
    }
}