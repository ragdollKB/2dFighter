using UnityEngine;

namespace TeamBallGame
{
    public class Quadrangle : PlayingField
    {
        public Vector3 size;

        public override bool Contains(Vector3 position)
        {
            var xSize = size.x / 2;
            var zSize = size.z / 2;
            return Mathf.Abs(position.x) <= xSize && Mathf.Abs(position.z) <= zSize;
        }
    }
}