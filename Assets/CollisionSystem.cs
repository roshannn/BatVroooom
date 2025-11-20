using System;
using UnityEditor;
using UnityEngine;

public class CollisionSystem : MonoBehaviour {
    private Collision2D coll;
    private BallController ball;
    [SerializeField]private VehicleController vehicleController;
    [SerializeField]private BoxCollider2D batBladeCollider;
    
    Vector2 ballDir;
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.TryGetComponent<BallController>(out BallController ball)) {
            coll = collision;
            this.ball = ball;
            Debug.Log($"Old Speed: {ball.ballRb.linearVelocity.magnitude}| Bat Speed : {collision.otherRigidbody.linearVelocity}");
            // 1. Get the Ball's incoming direction
            Vector2 ballVelocity = collision.rigidbody.linearVelocity;
            Vector2 ballDir = ballVelocity.normalized;

            // 2. Get the Wall's Normal
            ContactPoint2D contactPoint = collision.GetContact(0);
            Vector2 bladeNormal = contactPoint.normal;

            // 3. Calculate Dot Product
            // This returns -1.0 (Head-on collision) to 0.0 (Glancing blow)
            float dotProduct = Vector2.Dot(ballDir, bladeNormal);

            // 4. Calculate Multiplier
            // Example Glancing: (|0| + 1) * 2 = 2x multiplier
            float multiplier = (Mathf.Abs(dotProduct)+0.5f) * 2f;

            multiplier *= vehicleController.wheelieSpeed * 2f / 100f;

            //Contact Point through bounding box where i get value from 1 at centre and 0.3 at extents 
            Vector2 localContactPoint = transform.InverseTransformPoint(contactPoint.point);
            float batBladeLength = batBladeCollider.size.y * 0.5f;
            float distFactor = (batBladeLength - Mathf.Abs(localContactPoint.y)) / batBladeLength;
            distFactor = Mathf.Clamp01(distFactor);
            float vibrationFactor = Mathf.Lerp(1f, 0.3f, distFactor);

            multiplier *= vibrationFactor;

            Debug.Log($"Local Contact Point: {localContactPoint} | Distance from Center: {distFactor:F2} | VibrationFactor : {vibrationFactor}");
            // 5. Apply new velocity
            // We multiply the CURRENT velocity by the multiplier
            ball.ballRb.linearVelocity *= multiplier;


            Debug.Log($"Dot: {dotProduct:F2} | Multiplier: {multiplier} | New Speed: {ball.ballRb.linearVelocity.magnitude}");
        }
    }


#if UNITY_EDITOR
    private void OnDrawGizmos() {
        //Handles.DrawSolidDisc(contactPoint, Vector3.forward, 0.05f);

        if (coll != null && ball != null) {
            Vector2 contactPoint = coll.GetContact(0).point;
            Vector2 normal = coll.GetContact(0).normal.normalized;
            // Draw collision point


            Handles.color = Color.blue;
            Handles.DrawSolidDisc(contactPoint, Vector3.forward, 0.05f);

            Handles.color = Color.blue;
            Handles.DrawSolidDisc(contactPoint, Vector3.forward, 0.05f);

            // Draw normal
            Handles.color = Color.green;
            Handles.DrawAAPolyLine(contactPoint, contactPoint - normal);


            Vector2 incomingDir = ballDir;
            incomingDir = (incomingDir - contactPoint).normalized;

            // Draw incoming direction
            Handles.color = Color.red;

            Handles.DrawAAPolyLine(contactPoint, contactPoint + incomingDir);

           
            //Debug.Log(Vector2.Dot(normalDir, incomingDir));

            //// Draw reflected direction
            //Vector2 reflectionDirection = Vector2.Reflect(ballDirection, normal).normalized;
            //Handles.color = Color.blue;
            //Handles.DrawAAPolyLine(contactPoint, contactPoint + reflectionDirection);
        }
    }
#endif

}
