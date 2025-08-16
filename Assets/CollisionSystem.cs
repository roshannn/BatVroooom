using UnityEngine;

public class CollisionSystem : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.GetComponent<BallController>() != null) {
            // if bat hit decide what should happen.
            // more along the lines of adding bats normal velocity to the ball.
        }    
    }


}
