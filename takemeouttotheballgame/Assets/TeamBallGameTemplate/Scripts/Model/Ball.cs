using System;
using System.Collections;
using System.Collections.Generic;
using TeamBallGame.Gameplay;
using TeamBallGame.Mechanics;
using UnityEngine;

namespace TeamBallGame.Model
{
    /// <summary>
    /// This behaviour fires ball events and provides a Ball API to the designer.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class Ball : MonoBehaviour
    {
        new public Rigidbody rigidbody;
        public ImpactAudio impactAudio;

        public bool IsPossessedByHomeTeam => ballGame.playerInPossession == null ? false : ballGame.playerInPossession.team.teamType == TeamType.Home;
        public bool IsPossessedByAwayTeam => ballGame.playerInPossession == null ? false : ballGame.playerInPossession.team.teamType == TeamType.Away;
        public bool IsPossessed => ballGame.playerInPossession != null;
        public float Height => transform.position.y;

        public bool IsInPlay
        {
            get => isInPlay;
            set
            {
                isInPlay = value;
            }
        }

        bool isInPlay;
        BallGameModel ballGame = Simulation.GetModel<BallGameModel>();
        Vector3 velocity;
        Material material;

        void Awake()
        {
            material = GetComponent<Renderer>().material;
        }

        void Update()
        {
            material.color = IsPossessed ? ballGame.playerInPossession.team.teamMaterial.color : Color.white;
        }

        void Reset()
        {
            rigidbody = GetComponent<Rigidbody>();
            impactAudio = GetComponent<ImpactAudio>();
        }

        void OnCollisionEnter(Collision collision)
        {
            Barricade barricade;
            Player player;
            //check the collision components and fire correct event,
            //otherwise just fire the ball bounce event.
            if ((player = collision.gameObject.GetComponent<Player>()) != null)
            {
                var ev = Simulation.Schedule<PlayerBallCollision>(0);
                ev.player = player;
                ev.collision = collision;
            }
            else if ((barricade = collision.gameObject.GetComponent<Barricade>()) != null)
            {
                var ev = Simulation.Schedule<BarricadeBallCollision>(0);
                ev.collision = collision;
                ev.barricade = barricade;
            }
            else
            {
                var ev = Simulation.Schedule<BallBounce>(0);
                ev.collision = collision;
            }
        }

        void FixedUpdate()
        {
            var player = ballGame.playerInPossession;
            var ball = ballGame.ball;
            //If the ball is possessed by a player, then override physics to set the ball to the player's control position.
            if (player != null)
            {
                rigidbody.MovePosition(Vector3.SmoothDamp(ball.transform.position, player.BallPosition, ref velocity, 0.05f));
            }
        }
    }
}