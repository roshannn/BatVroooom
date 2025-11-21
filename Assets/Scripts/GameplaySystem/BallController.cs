using System;
using UnityEngine;
using WAS.EventBus;

public class BallController : MonoBehaviour {
    [SerializeField] public Rigidbody2D ballRb;
    public Vector3 ballDirection => ballRb.linearVelocity.normalized;
    public float ballMagnitude => ballRb.linearVelocity.magnitude;

    public float GetCurrentDotOffset => currBallData.ballData.dotProductIncrement;
    public float GetWheelieMultiplier => currBallData.ballData.wheelieSpeedMultiplier;

    [SerializeField] private Transform onSideWicket;
    [SerializeField] private Transform offSideWicket;

    [SerializeField] private BoxCollider2D pitchCollider;

    public BallDataContainer ballDataContainer;
    public BallDataHolder currBallData;

    public BallState currBallState;

    private Vector2 startPos;
    public Vector2 PrevFixedFrameDirection;
    private void Awake() {
        currBallState = BallState.BallIdle;
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
        currBallState = BallState.BallBowled;
        currBallData = ballDataContainer.GetBallData(Utility.GetRandomEnumValue<BallType>());
        Debug.Log($"Ball type: {currBallData.ballType}");
        pitchCollider.sharedMaterial = currBallData.ballData.pitchMaterial;
        transform.position = startPos;
        Vector2 lengthPos = GetBouncePos(currBallData.ballData.length);

        float xDis = lengthPos.x - startPos.x;
        float yDis = lengthPos.y - startPos.y;
        float t = Mathf.Max(1e-4f, currBallData.ballData.time); 

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
    private void FixedUpdate() {
        if(currBallState== BallState.BallBowled) {
            PrevFixedFrameDirection = ballRb.linearVelocity;
        }
    }
    public void ResetBall(BallState ballState = BallState.BallIdle) {
        currBallState = ballState;
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

public enum BallState {
    BallIdle,BallBowled,BallShot
}