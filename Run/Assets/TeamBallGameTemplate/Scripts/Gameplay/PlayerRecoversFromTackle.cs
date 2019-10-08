using TeamBallGame.Model;

namespace TeamBallGame.Gameplay
{
    public class PlayerRecoversFromTackle : Simulation.Event<PlayerRecoversFromTackle>
    {
        public Player player;

        public override void Execute()
        {
            player.SetMovement(enabled: true);
        }
    }
}