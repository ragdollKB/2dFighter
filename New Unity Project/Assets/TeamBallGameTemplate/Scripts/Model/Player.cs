using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TeamBallGame;
using TeamBallGame.Gameplay;
using TeamBallGame.Mechanics;

namespace TeamBallGame.Model
{
    [RequireComponent(typeof(MovementController))]
    public partial class Player : MonoBehaviour
    {
        //configuration
        public Animator animator;
        public SpriteRenderer icon;
        public Vector3 possessionOffset = new Vector3(0, -1, -1);
        public Vector3 reticleOffset = new Vector3(0, -1, 0);
        public Vector3 headOffset = new Vector3(0, 1, 0);
        public float headSize = 1;

        //These are set when the player prefab is instantiated.
        internal Team team;
        internal FieldPosition fieldPosition;

        ImpactAudio impactAudio;
        MovementController move;
        Vector3 velocity;
        Vector3 lastPosition;
        new Rigidbody rigidbody;
        State state = State.ReturnToPosition;
        BallGameConfig config = Simulation.GetModel<BallGameConfig>();
        Camera mainCamera;
        new AudioSource audio;
        BallGameModel ballGame;

        internal float tackleTimer;
        internal float recoveryTimer;

        internal void SetState(State newState)
        {
            if (state == State.ReturnToPosition)
            {
                if ((transform.position - fieldPosition.transform.position).sqrMagnitude < 1)
                {
                    state = newState;
                }
                else
                {
                    //leave state as is.
                }
            }
            else
            {
                if (newState == State.Tackled)
                {
                    recoveryTimer = ballGame.tackleRecoveryTime;
                }
                state = newState;
            }
            UpdateIcon();
        }

        void UpdateIcon()
        {
            if (icon)
            {
                switch (state)
                {
                    case State.AIControl:
                        icon.sprite = config.aiControlIcon;
                        break;
                    case State.UserControl:
                        icon.sprite = config.userControlIcon;
                        break;
                    case State.ReturnToPosition:
                        icon.sprite = config.refereeControlIcon;
                        break;
                    default:
                        icon.sprite = null;
                        break;
                }
            }
        }

        void Update()
        {
            velocity = transform.InverseTransformDirection(rigidbody.velocity);
            if (animator)
            {
                animator.SetFloat("MovementX", velocity.x);
                animator.SetFloat("MovementZ", velocity.z);
            }
            if (icon)
            {
                icon.transform.LookAt(mainCamera.transform);
            }
            tackleTimer -= Time.deltaTime;
            switch (state)
            {
                case State.UserControl:
                    PerformUserControl();
                    break;
                case State.ReturnToPosition:
                    move.To(fieldPosition.transform.position);
                    move.LookAt(fieldPosition.transform.position);
                    break;
                case State.AIControl:
                    PerformAIControl();
                    break;
                case State.Waiting:
                    CheckForNewState();
                    break;
                case State.Tackled:
                    UpdateTackleState();
                    break;
            }
        }

        void UpdateTackleState()
        {
            recoveryTimer -= Time.deltaTime;
            if (recoveryTimer < 0)
                SetState(State.AIControl);
        }

        void PerformUserControl()
        {

        }

        void CheckForNewState()
        {

        }

        void Awake()
        {
            audio = GetComponent<AudioSource>();
            animator = GetComponentInChildren<Animator>();
            move = GetComponent<MovementController>();
            impactAudio = GetComponent<ImpactAudio>();
            rigidbody = GetComponent<Rigidbody>();
            ballGame = Simulation.GetModel<BallGameModel>();
            mainCamera = Camera.main;
        }

        void Start()
        {
            if (fieldPosition != null)
                transform.position = fieldPosition.transform.position;
            Simulation.Schedule<PlayerMovement>(Fuzzy.Value(2)).player = this;
        }

        void OnCollisionEnter(Collision collision)
        {
            // if I am carrying the ball, any player colliding with me is added
            // to the active contest, which will be resolved later.
            if (IsBallOwner)
            {
                var player = collision.gameObject.GetComponent<Player>();
                if (player != null)
                {
                    var ev = Simulation.Schedule<BallContest>(0);
                    ev.player = player;
                }
            }
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(ReticlePosition, transform.forward);
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(BallPosition, transform.forward);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(HeadPosition, headSize);
        }
    }
}