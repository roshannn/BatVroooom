using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class BallPathPredictor : MonoBehaviour
{
    private LineRenderer lineRenderer;
    [SerializeField] private int resolution = 30;
    [SerializeField] private float timeStep = 0.1f;
    [SerializeField] private float groundLevel = -4.5f; // Matches BallController logic

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    public void VisualizePath(Vector2 startPos, Vector2 velocity, PhysicsMaterial2D pitchMaterial)
    {
        lineRenderer.positionCount = resolution;
        Vector3[] points = new Vector3[resolution];
        
        Vector2 currentPos = startPos;
        Vector2 currentVelocity = velocity;
        float gravity = Physics2D.gravity.y;

        float bounciness = pitchMaterial != null ? pitchMaterial.bounciness : 0.5f;
        float friction = pitchMaterial != null ? pitchMaterial.friction : 0.4f;

        for (int i = 0; i < resolution; i++)
        {
            points[i] = currentPos;

            // Physics simulation step
            float t = timeStep;
            Vector2 displacement = currentVelocity * t + 0.5f * new Vector2(0, gravity) * t * t;
            Vector2 nextPos = currentPos + displacement;
            currentVelocity += new Vector2(0, gravity) * t;

            // Interpret bounce also taking into factor the friction and bounciness of the surface
            if (nextPos.y <= groundLevel)
            {
                nextPos.y = groundLevel;
                if (currentVelocity.y < 0)
                {
                    float oldVy = currentVelocity.y;
                    currentVelocity.y = -oldVy * bounciness;

                    float changeInYVelocity = currentVelocity.y - oldVy;
                    float maxChangeInX = changeInYVelocity * friction;

                    currentVelocity.x = Mathf.MoveTowards(currentVelocity.x, 0, maxChangeInX);
                }
            }

            currentPos = nextPos;
        }

        lineRenderer.SetPositions(points);
    }
    
    public void ClearPath()
    {
        lineRenderer.positionCount = 0;
    }
}
