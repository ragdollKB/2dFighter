using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TeamBallGame.Model;

namespace TeamBallGame.Mechanics
{
    /// <summary>
    /// This behaviour holds configuration information for gameplay,
    /// and stores it in the Simulation BallGameModel instance when
    /// initialised.
    /// </summary>
    public class GameConfiguration : MonoBehaviour
    {
        //This model is public and can be modified in the inspector.
        //The reference is shared where needed, and Unity will deserialize
        //over the shared reference, rather than create a new instance.
        //To preserve this behaviour, this script must be deserialized last.
        public BallGameModel model;
        public BallGameConfig config;

        void OnEnable()
        {
            Simulation.SetModel<BallGameModel>(model);
            Simulation.SetModel<BallGameConfig>(config);
        }
    }
}