using UnityEngine;

namespace TeamBallGame.Model
{
    //This file contains the public API for player instances which
    //the designer can use to implement gameplay logic.
    public partial class Player
    {
        //Calculated properties.
        public Vector3 DeltaToBall => ballGame.ball.transform.position - transform.position;
        public Vector3 BallPosition => transform.TransformPoint(possessionOffset);
        public Vector3 ReticlePosition => transform.TransformPoint(reticleOffset);
        public Vector3 HeadPosition => transform.TransformPoint(headOffset);
        public bool IsBallOwner => ballGame.playerInPossession == this;
        public bool IsHomeTeam => team.teamType == TeamType.Home;
        public bool IsAI => state == State.AIControl;

        public void SetMovement(bool enabled)
        {
            SetState(State.Tackled);
            move.SetMovement(enabled);
        }

        public Vector3 DeltaToGoal
        {
            get
            {
                switch (team.teamType)
                {
                    case TeamType.Home:
                        return ballGame.homeGoal.transform.position - transform.position;
                    default:
                        return ballGame.awayGoal.transform.position - transform.position;
                }
            }
        }
    }
}