using TeamBallGame.Model;
using UnityEngine;

namespace TeamBallGame.Gameplay
{
    public class PlayerMovement : Simulation.Event<PlayerMovement>
    {
        public Player player;

        public override void Execute()
        {
            Simulation.Reschedule(this, Fuzzy.Value(1));
        }
    }
}