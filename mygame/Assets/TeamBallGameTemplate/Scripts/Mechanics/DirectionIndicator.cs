using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeamBallGame.Mechanics
{
    /// <summary>
    /// Sets the location of a transform and looks at another transform.
    /// Used for providing visual feedback about directions to the user.
    /// </summary>
    public class DirectionIndicator : MonoBehaviour
    {
        public Transform source, target;
        public Vector3 offset;

        new Renderer renderer;

        void Awake()
        {
            renderer = GetComponent<MeshRenderer>();
        }

        void LateUpdate()
        {
            if (source)
                transform.position = source.position + offset;
            if (target)
            {
                renderer.enabled = true;
                transform.LookAt(target);
            }
            else
            {
                renderer.enabled = false;
            }
        }
    }
}