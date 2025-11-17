using System;
using UnityEditor;
using UnityEngine;

public class CollisionSystem : MonoBehaviour {
    private Collision2D coll;
    private BallController ball;
    [SerializeField]private VehicleController vehicleController;
    Vector2 ballDir;
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.TryGetComponent<BallController>(out BallController ball)) {
            coll = collision;
            this.ball = ball;

            ballDir = ball.ballDirection;
            ContactPoint2D contactPoint = collision.GetContact(0);
            Debug.Log("Magnitude before changing: " + ball.ballMagnitude);
            var magnitude = ball.ballMagnitude * GetMagnitudeFactor(GetContactNormal(contactPoint), ball.ballDirection.normalized);

            var changedBallDir = ballDir * magnitude;
            ball.ResetBall();

            ball.ballRb.linearVelocity = changedBallDir;
            Debug.Log("Magnitude after changing: " + ball.ballMagnitude);
        }
    }

    private Vector2 GetContactNormal(ContactPoint2D contactPoint) {
        return contactPoint.normal.normalized;
    }

    private float GetMagnitudeFactor(Vector2 normal,Vector2 ballDir) {
        float dotP = Vector2.Dot(normal,ballDir);
        //rounding
        Debug.Log($"Dot product of ball dir on contact and Normal is {dotP}");
        dotP = Mathf.Round(dotP * 100f) / 100f;
        float factor = (vehicleController.wheelieSpeed * 2f / 100f) * (BasedOnDotProd(dotP));

        Debug.Log($"Magnitude Increase factor: {factor}");


        return factor;
    }

    private float BasedOnDotProd(float dotP) {
        float abs = Mathf.Abs(dotP);
        return abs * 2;
    }

    //private float m


#if UNITY_EDITOR
    private void OnDrawGizmos() {
        if (coll != null && ball != null) {
            Vector2 contactPoint = coll.GetContact(0).point;
            Vector2 normal = coll.GetContact(0).normal.normalized;
            Vector2 normalDir = (normal - contactPoint).normalized;
            // Draw collision point
            Handles.DrawSolidDisc(contactPoint, Vector3.forward, 0.05f);

            // Draw normal
            Handles.color = Color.green;
            Handles.DrawAAPolyLine(contactPoint, contactPoint + normalDir);


            Vector2 incomingDir = ballDir.normalized;
            //incomingDir = incomingDir - contactPoint;

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
