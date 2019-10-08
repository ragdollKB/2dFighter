using UnityEngine;

namespace TeamBallGame.Mechanics
{
    /// <summary>
    /// A specialised Directional Indicator used to indicate
    /// the current pass direction, and if the pass direction
    /// will intercept a team mate.
    /// </summary>
    public class LookIndicator : MonoBehaviour
    {
        public LineRenderer lineRenderer;
        public Material normalMaterial;
        public Material canPassMaterial;

        void Awake()
        {
            lineRenderer.material = normalMaterial;
        }

        public void SetPassIndicator(bool passable)
        {
            if (passable)
                lineRenderer.material = canPassMaterial;
            else
                lineRenderer.material = normalMaterial;
        }

        public void SetPositions(Vector3 start, Vector3 end)
        {
            end.y = start.y = 0.1f;
            lineRenderer.SetPosition(0, start);
            lineRenderer.SetPosition(1, end);
        }


    }
}