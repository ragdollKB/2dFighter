using System.Collections;
using System.Collections.Generic;
using TeamBallGame.Mechanics;
using UnityEngine;

namespace TeamBallGame.Model
{
    public class Team : MonoBehaviour
    {
        public TeamType teamType;
        public LayerMask layer;
        public Goal goal;
        public Team opposingTeam;
        public FieldPosition[] positions;
        public Player[] players;
        public Material teamMaterial;

        public UserInput UserInput { get; set; }

        BallGameModel ballGame = Simulation.GetModel<BallGameModel>();

        void Reset()
        {
            positions = GetComponentsInChildren<FieldPosition>();
        }

        public void InstantiatePlayers()
        {
            Player playerPrefab = null;
            if (teamType == TeamType.Home) playerPrefab = ballGame.homePlayerPrefab;
            if (teamType == TeamType.Away) playerPrefab = ballGame.awayPlayerPrefab;

            players = new Player[positions.Length];
            for (var i = 0; i < positions.Length; i++)
            {
                var player = Instantiate(playerPrefab);
                player.team = this;
                player.fieldPosition = positions[i];
                player.transform.position = player.fieldPosition.transform.position;
                player.name = $"{teamType}-{player.fieldPosition.name}";
                players[i] = player;
            }
        }
    }
}