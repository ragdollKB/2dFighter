using System;
using TeamBallGame.Model;
using UnityEngine;

namespace TeamBallGame.Gameplay
{
    /// <summary>
    /// This event is triggered when the tackle input is received.
    /// </summary>
    /// <typeparam name="PlayerTackle"></typeparam>
    public class PlayerTackle : Simulation.Event<PlayerTackle>
    {
        public Player player;

        [ThreadStatic] static Collider[] hits = new Collider[12];

        //This event cannot happen if the player has the ball.
        internal override bool CheckPrecondition() => !player.IsBallOwner && player.tackleTimer <= 0;

        public override void Execute()
        {
            player.OnTackle();
            var pos = player.transform.position + (player.transform.forward * 2);
            var hitCount = Physics.OverlapSphereNonAlloc(pos, 1, hits, player.team.opposingTeam.layer.value);
            for (var i = 0; i < hitCount; i++)
            {
                var ev = Simulation.Schedule<PlayerHasBeenTackled>(Fuzzy.Value(0.1f));
                ev.player = hits[i].GetComponent<Player>();
                ev.tackler = player;
                ev.direction = player.transform.forward;
            }
        }

        internal override void Cleanup()
        {
            player = null;
        }
    }
}