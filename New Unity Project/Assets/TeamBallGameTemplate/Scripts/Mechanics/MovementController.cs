using System;
using System.Collections.Generic;
using TeamBallGame.Model;
using UnityEngine;

namespace TeamBallGame.Mechanics
{
    [RequireComponent(typeof(Rigidbody), typeof(Collider))]
    public class MovementController : MonoBehaviour
    {
        new Rigidbody rigidbody;
        public float maxSpeed = 5;
        public float maxTurnSpeed = 360;
        public float backwardsSpeedPenalty = 0.4f;

        float movementScale = 1;
        float targetMovementScale = 1;

        Vector3 targetPosition, bumpDirection;
        Quaternion targetRotation;
        Queue<Vector3> moveCommands = new Queue<Vector3>();
        BallGameModel ballGame = Simulation.GetModel<BallGameModel>();
        float bumpTimer;
        Vector3 currentVelocity;

        public void SetMovement(bool enabled)
        {
            targetMovementScale = enabled ? 1 : 0;
        }

        /// <summary>
        /// Set the destinate position for the controller.
        /// </summary>
        /// <param name="position"></param>
        public void To(Vector3 position)
        {
            targetPosition = position;
        }

        /// <summary>
        /// Set the desired look at position for the controller.
        /// </summary>
        /// <param name="position"></param>
        public void LookAt(Vector3 position)
        {
            var delta = position - transform.position;
            delta.y = 0;
            delta.Normalize();
            if (delta.sqrMagnitude > 0)
                targetRotation = Quaternion.LookRotation(delta);
        }

        /// <summary>
        /// Set the desired look direction for the controller.
        /// </summary>
        /// <param name="position"></param>
        public void LookDirection(Vector3 direction)
        {
            direction.y = 0;
            if (direction.sqrMagnitude > 0)
                targetRotation = Quaternion.LookRotation(direction.normalized);
        }

        /// <summary>
        /// Momentarily push the controller in a direction for a
        /// specified period. Overrides any destination during this time.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="duration"></param>
        public void BumpTowards(Vector3 position, float duration = 1)
        {
            bumpDirection = (position - transform.position);
            bumpTimer = duration;
        }

        void Update()
        {
            var rotation = targetRotation;
            if (bumpTimer > 0)
            {
                LookDirection(bumpDirection);
            }

            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, Time.deltaTime * ballGame.maxTurnSpeed * movementScale);
        }

        void FixedUpdate()
        {
            var delta = targetPosition - transform.position;
            delta.y = 0;

            var forwardRatio =
                Mathf.Lerp(ballGame.backwardsSpeedPenalty, 1,
                    Mathf.InverseLerp(-1, 1, Vector3.Dot(delta.normalized, transform.forward))
                );

            if (bumpTimer > 0)
            {
                delta = bumpDirection;
            }
            bumpTimer -= Time.fixedDeltaTime;
            movementScale = Mathf.Clamp01(Mathf.Lerp(movementScale, targetMovementScale, Time.fixedDeltaTime * 10));
            currentVelocity = Vector3.ClampMagnitude(delta * ballGame.maxSpeed * forwardRatio, ballGame.maxSpeed);

            var ov = rigidbody.velocity;
            ov.x = currentVelocity.x;
            ov.z = currentVelocity.z;

            rigidbody.velocity = ov * movementScale;
        }

        void Awake()
        {
            rigidbody = GetComponent<Rigidbody>();
            targetPosition = transform.position;
        }
    }
}