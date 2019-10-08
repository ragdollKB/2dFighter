using TeamBallGame;
using UnityEngine;

namespace TeamBallGame.Gameplay
{
    /// <summary>
    /// This event occurs when the ball hits a barricade.
    /// Logic for scheduling out-of-bounds events would happen here.
    /// </summary>
    public class BarricadeBallCollision : Simulation.Event
    {
        public Collision collision;
        public Barricade barricade;

        public override void Execute() { }

        internal override void Cleanup()
        {
            collision = null;
            barricade = null;
        }
    }
}