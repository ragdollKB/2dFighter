using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeamBallGame
{
    public abstract class PlayingField
    {
        public abstract bool Contains(Vector3 position);
    }
}