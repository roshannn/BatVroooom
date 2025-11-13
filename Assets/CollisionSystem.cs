using UnityEditor;
using UnityEngine;

public class CollisionSystem : MonoBehaviour {
    private Collision2D coll;
    private BallController ball;
    Vector2 ballDir;
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.TryGetComponent<BallController>(out BallController ball)) {
            coll = collision;
            this.ball = ball;

            ballDir = ball.ballRb.linearVelocity.normalized;
            var magnitude = ball.ballRb.linearVelocity.magnitude * 2f;
            var changedBallDir = ballDir * magnitude;
            ball.ResetBall();
            
            ball.ballRb.linearVelocity = changedBallDir;
        }


    }


   

    //private void OnDrawGizmos() {
    //    if (coll != null && ball != null) {
    //        Vector2 contactPoint = coll.GetContact(0).point;
    //        Vector2 normal = coll.GetContact(0).normal.normalized;
    //        Vector2 normalDir = (normal - contactPoint).normalized;
    //        // Draw collision point
    //        Handles.DrawSolidDisc(contactPoint, Vector3.forward, 0.05f);

    //        // Draw normal
    //        Handles.color = Color.green;
    //        Handles.DrawAAPolyLine(contactPoint, contactPoint + normalDir);



    //        // Draw incoming direction
    //        Handles.color = Color.red;
    //        Handles.DrawAAPolyLine(contactPoint, contactPoint + ballDir);

    //        //// Draw reflected direction
    //        //Vector2 reflectionDirection = Vector2.Reflect(ballDirection, normal).normalized;
    //        //Handles.color = Color.blue;
    //        //Handles.DrawAAPolyLine(contactPoint, contactPoint + reflectionDirection);
    //    }
    //}

}
