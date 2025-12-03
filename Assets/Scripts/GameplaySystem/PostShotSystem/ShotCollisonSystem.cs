using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ShotCollisionSystem : MonoBehaviour {

    private Collision2D coll;
    private DeliveryController ball;
    private Vector2 ballDir;
    private float ballAngularVelocity;

    private void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.TryGetComponent<DeliveryController>(out DeliveryController ball)) {
            Debug.Log($"Old Speed: {ball.ballRb.linearVelocity.magnitude}| Bat Speed : {collision.otherRigidbody.angularVelocity}");
            coll = collision;
            this.ball = ball;
            ballAngularVelocity = ball.ballRb.angularVelocity * Mathf.Deg2Rad;
            float multiplier = 1f;
            ContactPoint2D contactPoint = collision.GetContact(0);
            ballDir = ball.PrevFixedFrameDirection.normalized;
            Vector2 bladeNormal = contactPoint.normal;
            Vector2 reflectedDir = Vector2.Reflect(ballDir, bladeNormal).normalized;
            float ballRadius = ball.colliderRadius;
            Vector2 tangent = new Vector2(bladeNormal.y, -bladeNormal.x).normalized;
            float ballSpin = ballRadius * ballAngularVelocity;
            float spinAmount = ballRadius * ballSpin;

            Vector2 correctedDir = (reflectedDir - tangent * spinAmount).normalized;
            ball.ballRb.linearVelocity = correctedDir * (multiplier * ball.ballRb.linearVelocity.magnitude);
        }
    }
#if UNITY_EDITOR
    private void OnDrawGizmos() {
        if (coll != null && ball != null) {
            Vector2 contactPoint = coll.GetContact(0).point;
            Vector2 normal = coll.GetContact(0).normal;

            // Compute tangent (90° rotated normal)
            Vector2 tangent = new Vector2(normal.y, -normal.x).normalized;
            
            // Draw collision point
            Handles.color = Color.yellow;
            Handles.DrawSolidDisc(contactPoint, Vector3.forward, 0.05f);

            // Draw normal
            Handles.color = Color.green;
            Handles.DrawAAPolyLine(contactPoint, contactPoint + normal);

            // Draw incoming direction (Red)
            // ballDir is the normalized velocity vector pointing INTO the wall.
            // Draw from (point - dir) to point to show it arriving.
            Handles.color = Color.red;
            Handles.DrawAAPolyLine(contactPoint - ballDir, contactPoint);

            // Draw reflected direction (Blue)
            Vector2 reflectedDir = Vector2.Reflect(ballDir, normal).normalized;
            Handles.color = Color.blue;
            Handles.DrawAAPolyLine(contactPoint, contactPoint + reflectedDir);

            // Draw tangent (Magenta)
            Handles.color = Color.magenta;
            Handles.DrawAAPolyLine(contactPoint, contactPoint + tangent);

            float ballRadius = ball.colliderRadius;

            float ballSpin = ballRadius * ballAngularVelocity;
            float spinAmount = ballRadius * ballSpin;
            Vector2 correctedDir = (reflectedDir - tangent * spinAmount).normalized;
            Handles.color = Color.cyan;
            Handles.DrawAAPolyLine(contactPoint, contactPoint + correctedDir);
        }
    }
#endif
}
