using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TeamBallGame.Model;
using TeamBallGame.Gameplay;
using System;

namespace TeamBallGame.Mechanics
{
    public class Umpire : MonoBehaviour
    {
        public MovementController Move { get; private set; }

        BallGameModel ballGame = Simulation.GetModel<BallGameModel>();
        new AudioSource audio;
        new Rigidbody rigidbody;
        Animator animator;

        void Awake()
        {
            audio = GetComponent<AudioSource>();
            rigidbody = GetComponent<Rigidbody>();
            animator = GetComponentInChildren<Animator>();
            Move = GetComponent<MovementController>();
        }

        void Update()
        {
            var p = ballGame.ball.transform.position;
            var targetPos = p;
            targetPos.y = 0;
            targetPos += Vector3.left * 15;
            targetPos.x *= p.x > 0 ? 1 : -1;
            Move.To(targetPos);
            Move.LookAt(p);

            var velocity = transform.InverseTransformDirection(rigidbody.velocity);
            if (animator)
            {
                animator.SetFloat("MovementX", velocity.x);
                animator.SetFloat("MovementZ", velocity.z);
            }
        }

        void OnEnable()
        {
            ResetGamePlay.OnExecute += OnResetGamePlay;
            Score.OnExecute += OnScore;
            BallUp.OnExecute += OnBallUp;
        }

        void OnDisable()
        {
            ResetGamePlay.OnExecute -= OnResetGamePlay;
            Score.OnExecute -= OnScore;
            BallUp.OnExecute -= OnBallUp;
        }

        void OnBallUp(BallUp ev)
        {
            audio.Play();
            Move.To(Vector3.left * 10);
        }

        void OnScore(Score ev)
        {
            audio.Play();
        }

        void OnResetGamePlay(ResetGamePlay ev)
        {
            Move.To(Vector3.zero + Vector3.left * 5);
        }
    }
}