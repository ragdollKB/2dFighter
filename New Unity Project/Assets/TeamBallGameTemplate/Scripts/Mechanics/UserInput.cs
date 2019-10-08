using UnityEngine;
using TeamBallGame.Model;
using TeamBallGame;
using TeamBallGame.Gameplay;
using UnityEngine.EventSystems;
using System;

namespace TeamBallGame.Mechanics
{

    public class UserInput : MonoBehaviour
    {
        public Camera mainCamera;

        Vector3 MoveDirection { get; set; }
        Vector3 LookDirection { get; set; }
        float Launch { get; set; }

        bool allowInput = false;

        public Player ActivePlayer
        {
            get => _activePlayer;
            set
            {
                if (_activePlayer != null)
                    _activePlayer.SetState(Player.State.AIControl);
                _activePlayer = value;
                _activePlayer.SetState(Player.State.UserControl);
            }
        }

        Player _activePlayer;
        BallGameModel ballGame = Simulation.GetModel<BallGameModel>();

        Vector2 mousePosition;
        bool useMousePosition = false;

        void OnLookDirectionInput(Vector2 direction, bool isMouse)
        {
            if (!ActivePlayer) return;

            if (direction.sqrMagnitude > 0)
            {
                var delta = Vector3.zero;
                if (isMouse)
                {
                    useMousePosition = true;
                    mousePosition = direction;
                    delta = UpdateLookDeltaFromScreenPosition(mousePosition);
                }
                else
                {
                    delta = new Vector3(direction.x, 0, direction.y).normalized;
                    delta = mainCamera.transform.TransformDirection(delta);
                }
                delta.y = 0;
                LookDirection = delta.normalized;
                {
                    var ev = Simulation.Schedule<LookDirectionChanged>(0);
                    ev.player = ActivePlayer;
                    ev.direction = LookDirection;
                }
            }
        }

        Vector3 UpdateLookDeltaFromScreenPosition(Vector2 position)
        {
            var ray = mainCamera.ScreenPointToRay(position);
            var relativeTransform = ActivePlayer.transform;
            var plane = new Plane(Vector3.up, relativeTransform.position);
            var delta = Vector3.zero;
            if (plane.Raycast(ray, out float enter))
            {
                var hitPoint = ray.origin + ray.direction * enter;
                delta = hitPoint - relativeTransform.position;
                {
                    var ev = Simulation.Schedule<RepositionArrowIndicator>(0);
                    ev.position = hitPoint;
                    ev.direction = delta.normalized;
                }
            }
            return delta;
        }

        void OnTackleInput()
        {
            if (ballGame.ball.Height > 2)
            {
                Simulation.Schedule<PlayerJump>(0).player = ActivePlayer;
            }
            else
            {
                Simulation.Schedule<PlayerTackle>(0).player = ActivePlayer;
            }
        }

        void OnGoalShotInput()
        {
            if (!ActivePlayer) return;
            if (ballGame.IsBallUnderUserControl)
            {
                var ev = Simulation.Schedule<PrepareToLaunchBall>(0);
                var player = ev.player = ballGame.playerInPossession;
                var target = Vector3.zero;
                if (player.team.teamType == TeamType.Home)
                    target = ballGame.homeGoal.transform.position;
                else
                    target = ballGame.awayGoal.transform.position;
                ev.target = target;
            }
        }

        void OnPassInput()
        {
            if (!ActivePlayer) return;
            RaycastHit hit;
            var player = ActivePlayer;
            if (Physics.SphereCast(player.transform.position + player.transform.forward * 6, 2, player.transform.forward, out hit, ballGame.maxKickDistance, player.team.layer.value))
            {
                var ev = Simulation.Schedule<PrepareToPassBall>(0);
                var teammate = hit.collider.GetComponent<Player>();
                ev.player = player;
                ev.target = teammate.transform.position;
                ev.receiver = teammate;
            }
            else
            {
                var ev = Simulation.Schedule<PrepareToLaunchBall>(0);
                ev.player = player;
                ev.target = player.transform.position + player.transform.forward * ballGame.maxKickDistance;
            }
        }

        void OnMoveDirectionInput(Vector2 directionPad)
        {
            if (!ActivePlayer) return;
            var h = directionPad.x;
            var v = directionPad.y;
            var f = mainCamera.transform.forward;
            var r = mainCamera.transform.right;
            f.y = r.y = 0;
            MoveDirection = f.normalized * v + r * h;
        }

        void Reset()
        {
            mainCamera = Camera.main;
        }

        void Update()
        {
            if (!allowInput) return;
            if (ActivePlayer == null) return;
            transform.position = ActivePlayer.transform.position;
            var directionPad = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            OnLookDirectionInput((Vector2)Input.mousePosition, true);
            if (directionPad.sqrMagnitude > 0)
                OnMoveDirectionInput(directionPad);
            if (Input.GetMouseButtonDown(0))
                OnPassInput();
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (ActivePlayer.IsBallOwner)
                    OnGoalShotInput();
                else
                    OnTackleInput();
            }
            if (useMousePosition)
            {
                UpdateLookDeltaFromScreenPosition(mousePosition);
            }
        }

        public void UpdateActivePlayer()
        {
            var player = ActivePlayer;
            if (player != null)
            {
                player.OnUserInput(MoveDirection, LookDirection);
            }
        }

        void Awake()
        {
            DisableUserInput.OnExecute += (e) => allowInput = false;
            EnableUserInput.OnExecute += (e) => allowInput = true;
        }

    }
}