using UnityEditor;
using UnityEngine;

public class CollisionSystem : MonoBehaviour {


    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.TryGetComponent<BallController>(out BallController ball)) {
            var ContactPointData = collision.GetContact(0);
            var ReflectionDirection = GetCollisionDirection(ContactPointData.normal, ContactPointData.relativeVelocity);
            var ReflectedMagnitude = ContactPointData.relativeVelocity.magnitude * Vector2.Dot(ContactPointData.normal, ContactPointData.relativeVelocity);
            ball.ApplyLinearVelocity(ReflectionDirection, ReflectedMagnitude);
        }


    }


    Vector2 GetCollisionDirection(Vector2 normal,Vector2 movementDirection) {
        normal = normal.normalized;
        //Flip Direction before reflecting
        movementDirection = movementDirection.normalized;
        if (Vector2.Dot(normal, movementDirection) > 0) {
            movementDirection = -movementDirection;
        }
        Vector2 collisionDirection = Vector2.Reflect(movementDirection, normal).normalized;
        return collisionDirection;
    }
    //private void OnDrawGizmos() {
    //    if (coll != null && ball != null) {
    //        Vector2 contactPoint = coll.GetContact(0).point;
    //        Vector2 normal = coll.GetContact(0).normal.normalized;

    //        // Draw collision point
    //        Handles.DrawSolidDisc(contactPoint, Vector3.forward, 0.05f);

    //        // Draw normal
    //        Handles.color = Color.green;
    //        Handles.DrawAAPolyLine(contactPoint, contactPoint + normal);

    //        // Draw incoming direction
    //        Handles.color = Color.red;
    //        Handles.DrawAAPolyLine(contactPoint, contactPoint + ballDirection);

    //        // Draw reflected direction
    //        Vector2 reflectionDirection = Vector2.Reflect(ballDirection, normal).normalized;
    //        Handles.color = Color.blue;
    //        Handles.DrawAAPolyLine(contactPoint, contactPoint + reflectionDirection);
    //    }
    //}

}
