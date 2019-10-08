using UnityEngine;
using TeamBallGame.Model;

namespace TeamBallGame.Gameplay
{
    /// <summary>
    /// This event is triggered when gameplay should start, eg at the 
    /// start of a game period or after the ball is returned to the center.
    /// </summary>
    /// <typeparam name="StartGameplay"></typeparam>
    public class StartGameplay : Simulation.Event<StartGameplay>
    {
        BallGameModel ballGame = Simulation.GetModel<BallGameModel>();

        public override void Execute()
        {
            // //Get two closest players
            // var A = ballGame.homeTeam.players[0];
            // var B = ballGame.awayTeam.players[0];
            // //Choose a random winner
            // var winner = Random.value > 0.5f ? A : B;
            // //Next event is for the winner to receive possession.
            // var ev = Simulation.Schedule<ReceiveBall>(0);
            // ev.player = winner;
            Simulation.Schedule<EnableUserInput>(0);
            foreach (var p in ballGame.players)
            {
                p.SetState(Player.State.AIControl);
            }
        }
    }
}