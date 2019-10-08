using UnityEngine;

namespace TeamBallGame
{
    public class Oval : PlayingField
    {
        public Vector3 size;

        public override bool Contains(Vector3 position)
        {
            var xRadius = size.x / 2;
            var zRadius = size.z / 2;
            return ((position.x * position.x) / (xRadius * xRadius)) + ((position.z * position.z) / (zRadius * zRadius)) <= 1.0f;
        }
    }
}