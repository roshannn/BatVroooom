using System;
using UnityEngine;
using WAS.EventBus;

public class BallController : MonoBehaviour {
    [SerializeField] public Rigidbody2D ballRb;
    public Vector3 ballDirection => ballRb.linearVelocity.normalized;

    [SerializeField] private Transform onSideWicket;
    [SerializeField] private Transform offSideWicket;

    [SerializeField] private BoxCollider2D pitchCollider;

    public BallDataContainer ballDataContainer;
    private Vector2 startPos;
    private void Awake() {
        startPos = transform.position;
    }

    private void OnEnable() {
        GameEventBus.Subscribe<StartBowling>(LaunchBall);
    }
    private void OnDisable() {
        GameEventBus.Unsubscribe<StartBowling>(LaunchBall);
    }
    private void LaunchBall(StartBowling obj) {
        LaunchBall();
    }

    public void LaunchBall() {
        BallData currBallData = ballDataContainer.GetBallData(Utility.GetRandomEnumValue<BallType>());
        Debug.Log($"Ball type: {currBallData.ballType}");
        pitchCollider.sharedMaterial = currBallData.pitchMaterial;
        transform.position = startPos;
        Vector2 lengthPos = GetBouncePos(currBallData.length);

        float xDis = lengthPos.x - startPos.x;
        float yDis = lengthPos.y - startPos.y;
        float t = Mathf.Max(1e-4f, currBallData.time); 

        float g = Physics2D.gravity.y; // usually negative (~ -9.81)

        // Initial velocity components required to hit (xDis, yDis) in time t
        float vx = xDis / t;
        float vy = (yDis - 0.5f * g * t * t) / t; // note: g is negative, so minus here

        // Angle (radians). Use Atan2 for robustness.
        float angleRad = Mathf.Atan2(vy, vx);
        float angleDeg = angleRad * Mathf.Rad2Deg;
        Debug.Log($"θ = {angleDeg:F2}°");
        DebugBouncePos(lengthPos);

        ballRb.linearVelocity = new Vector2(vx, vy);

    }

    GameObject debug;
    private void DebugBouncePos(Vector2 lengthPos) {
        if(debug == null) {
            debug = new GameObject();
        }
        debug.transform.position = lengthPos;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.B)) {
            LaunchBall();
        }
        if (Input.GetKeyDown(KeyCode.R)) {
            ResetBall();
            ResetToInitialPosition();
        }
    }

    public void ResetBall() {
        ballRb.linearVelocity = Vector2.zero;
        ballRb.angularVelocity = 0;
    }

    private void ResetToInitialPosition() {
        transform.position = startPos;
    }

    public Vector2 GetBouncePos(float length01) {
        float t = Mathf.Clamp01(length01);
        Vector2 a = offSideWicket.position;
        a.y = -4.5f;
        Vector2 b = onSideWicket.position;
        b.y = -4.5f;

        // Interpolate along the pitch line so rotation/placement doesn’t matter
        return Vector2.Lerp(a, b, t);
    }

    public void ApplyLinearVelocity(Vector2 direction,float magnitude) {
        ResetBall();
        ballRb.linearVelocity = magnitude * direction;
    }
}

public enum BallType {
    Short, Medium, GoodLength
}