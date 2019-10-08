using System;
using TeamBallGame.Gameplay;
using UnityEngine;

namespace TeamBallGame.Model
{
    //T his file contains public methods which control the behaviour of a player.
    // Most methods in this file are called from gameplay events.
    // TODO: The logic in these methods could also exist in the event Execute methods,
    // should they be moved?
    public partial class Player
    {
        public void OnTackle()
        {
            if (animator)
                animator.SetTrigger("Slide");
            if (impactAudio)
                impactAudio.Play(1, config.tackleAudio);
            tackleTimer = ballGame.timeBetweenTackles;
            move.BumpTowards(ballGame.ball.transform.position, duration: 0.7f);
        }

        internal void OnHasBeenTackled()
        {
            if (impactAudio)
                impactAudio.Play(1, config.tackledAudio);
        }

        public void OnPlayerJump()
        {
            if (animator) animator.SetTrigger("Jump");
            move.BumpTowards(ballGame.ball.transform.position);
        }

        public void OnPrepareToLaunchBall(Vector3 target)
        {
            move.LookAt(target);
            if (animator) animator.SetTrigger("Kick");
        }

        public void OnSuccessfulInterception()
        {
            if (impactAudio)
                impactAudio.Play(1, config.interceptionAudio);
        }

        public void OnBallWillLandNearMe(Vector3 targetPosition)
        {
            move.LookAt(targetPosition);
            move.To(targetPosition);
        }

        public void OnLaunchBall(Vector3 targetPosition)
        {
            move.LookAt(targetPosition);
        }

        public void OnOtherPlayerLaunchedBall(Player playerThatLaunchedBall, Vector3 targetPosition)
        {
            move.LookAt(playerThatLaunchedBall.transform.position);
        }

        public void OnBallBounceNearMe(Vector3 position)
        {
            if (state != State.Tackled)
            {
                move.LookAt(position);
                move.To(position);
            }
        }

        public void OnUserInput(Vector3 moveDirection, Vector3 lookDirection)
        {
            if (state == State.UserControl)
            {
                move.To(transform.position + moveDirection);
                move.LookAt(transform.position + lookDirection);
            }
        }

        void PerformAIControl()
        {
            var ball = ballGame.ball.transform;

            var playerInPossession = ballGame.playerInPossession;
            var ballIsPossessed = playerInPossession != null;
            var otherTeamHasBall = playerInPossession != null && playerInPossession.team != this.team;
            var iAmClosestToBall = ballGame.IsClosest(this);
            var iAmNearBall = DeltaToBall.sqrMagnitude < 5;

            move.LookAt(ball.position);

            if (!IsBallOwner && iAmClosestToBall)
            {
                move.To(ball.position);
            }
            if (otherTeamHasBall && iAmNearBall)
                Simulation.Schedule<PlayerTackle>(0.1f).player = this;

        }

    }
}