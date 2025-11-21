using System;
using UnityEditor;
using UnityEngine;

public class CollisionSystem : MonoBehaviour {
    private Collision2D coll;
    private BallController ball;
    [SerializeField]private VehicleController vehicleController;
    [SerializeField]private BoxCollider2D batBladeCollider;
    [SerializeField]private BallController ballController;
    [SerializeField]private BatRotation batRotation;
    
    Vector2 ballDir;
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.TryGetComponent<BallController>(out BallController ball)) {
            coll = collision;
            this.ball = ball;
            Debug.Log($"Old Speed: {ball.ballRb.linearVelocity.magnitude}| Bat Speed : {collision.otherRigidbody.angularVelocity}");
            // 1. Get the Ball's incoming direction
            // Use relativeVelocity to get the impact vector before physics resolution
            Vector2 ballVelocity = ball.PrevFixedFrameDirection; 
            ballDir = ballVelocity.normalized;

            // 2. Get the Wall's Normal
            ContactPoint2D contactPoint = collision.GetContact(0);
            Vector2 bladeNormal = contactPoint.normal;

            // 3. Calculate Dot Product
            // This returns -1.0 (Head-on collision) to 0.0 (Glancing blow)
            float dotProduct = Vector2.Dot(ballDir, bladeNormal);

            // 4. Calculate Multiplier
            // Example Glancing: (|0| + 1) * 2 = 2x multiplier
            float dotOffset = ballController.GetCurrentDotOffset;
            float multiplier = (Mathf.Abs(dotProduct)+ dotOffset*batRotation.GetRotationFactor()) * 2f;
            Debug.Log($"Dot: {dotProduct:F2} | Multiplier: {multiplier}");

            float wheelieMultiplier = vehicleController.wheelieSpeed * ballController.GetWheelieMultiplier/ 100f; 
            Debug.Log($"Wheelie Speed: {vehicleController.wheelieSpeed:F2} | Multiplier: {wheelieMultiplier}");
            multiplier *= wheelieMultiplier;

            //Contact Point through bounding box where i get value from 1 at centre and 0.3 at extents 
            Vector2 localContactPoint = transform.InverseTransformPoint(contactPoint.point);
            float batBladeLength = batBladeCollider.size.y * 0.5f;
            float distanceFromCenter = Mathf.Abs(localContactPoint.y);
            float distFactor = distanceFromCenter / batBladeLength;
            distFactor = Mathf.Clamp01(distFactor);
            float vibrationFactor = Mathf.Lerp(1.2f, 0.6f, distFactor);
              
            multiplier *= vibrationFactor;

            Debug.Log($"Local Contact Point: {localContactPoint} | Distance Factor: {distFactor:F2} | Distance from Center: {distanceFromCenter:F2} | VibrationFactor : {vibrationFactor}");
            // 5. Apply new velocity
            // Calculate reflected direction
            Vector2 reflectedDir = Vector2.Reflect(ballDir, bladeNormal).normalized;

            // Apply multiplier to the reflected direction
            ball.ResetBall(BallState.BallShot);
            ball.ballRb.linearVelocity = reflectedDir * (multiplier * ballVelocity.magnitude);


            Debug.Log($"Final Multiplier: {multiplier} | New Speed: {ball.ballRb.linearVelocity.magnitude}");
        }
    }


#if UNITY_EDITOR
    private void OnDrawGizmos() {
        if (coll != null && ball != null) {
            Vector2 contactPoint = coll.GetContact(0).point;
            Vector2 normal = coll.GetContact(0).normal;

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
        }
    }
#endif

}
