using UnityEngine;

public class BallController : MonoBehaviour
{
    public float hitForce = 10f;

    void OnCollisionEnter2D(Collision2D collision) {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();

            // Step 1: Get incoming velocity
            Vector2 incoming = rb.linearVelocity.normalized;

            // Step 2: Get bat's local X axis (right direction)
            Vector2 batForward = collision.collider.transform.right;

            // Step 3: Reflect the incoming vector based on bat's X-axis
            Vector2 reflected = Vector2.Reflect(incoming, batForward).normalized;

            // Step 4: Apply force in the reflected direction
            rb.linearVelocity = Vector2.zero; // stop current motion
            rb.AddForce(reflected * hitForce, ForceMode2D.Impulse);
    }
}
