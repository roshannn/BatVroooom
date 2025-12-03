using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WAS.EventBus;

public class DeliveryController : MonoBehaviour {
    public Rigidbody2D ballRb;
    public CircleCollider2D ballCollider;

    public BallDataContainer ballDataContainer;

    public BallType currBallType;
    public BallDataHolder currBallData;
    public Vector2 PrevFixedFrameDirection;

    private Vector2 startPos;
    private DeliveryState deliveryState;

    [Header("<b>Debug Data</b>")]
    [SerializeField] private bool isDebug;
    [SerializeField] private BallType debugBallType;

    public float colliderRadius => ballCollider.radius * ballCollider.transform.lossyScale.x;

    private void Awake() {

        startPos = transform.position;
    }

    public void BeginDelivery() {
        deliveryState = DeliveryState.DeliveryActive;
        currBallType = isDebug ? debugBallType : Utility.GetRandomEnumValue<BallType>();
        currBallData = ballDataContainer.GetBallData(currBallType);
        Debug.Log($"Ball type: {currBallData.ballType}");
        Vector2 bouncePos = GameEventBus.Query<GetBouncePos, Vector2>(new GetBouncePos() { ballCollider = ballCollider, value01 = currBallData.ballData.length });
        Debug.Log($"Bounce Position : {bouncePos}");

        Vector2 launchVel = ComputeLaunchVelocity(startPos, bouncePos, currBallData);

        PredictPath(launchVel);
        LaunchBall(launchVel);
    }
    private void LaunchBall(Vector2 launchVel) {

        ResetToInitialPosition();
        ballRb.linearVelocity = launchVel;
    }
    private void PredictPath(Vector2 launchVel) {
        ResetToInitialPosition();

    }

    private Vector2 ComputeLaunchVelocity(Vector2 startPos, Vector2 bouncePos, BallDataHolder ballData) {
        Vector2 dis = new Vector2(bouncePos.x - startPos.x, bouncePos.y - startPos.y);
        float t = ballData.ballData.time;

        float g = Physics2D.gravity.y;

        float vx = dis.x / t;
        float vy = (dis.y - 0.5f * g * t * t) / t;

        return new Vector2(vx, vy);
    }

    private void FixedUpdate() {
        PrevFixedFrameDirection = ballRb.linearVelocity;
    }

    private void ResetToInitialPosition() {
        transform.position = startPos;
    }


    private void Update() {
        if (Input.GetKeyDown(KeyCode.Backspace)) {
            BeginDelivery();
        }
    }

    public void Bounce() {
        if (deliveryState != DeliveryState.DeliveryActive) {
            return;
        }
        deliveryState = DeliveryState.PostBounce;
        ballRb.linearVelocity = Vector2.zero;

        var ballMaterialBounciness = currBallData.ballData.ballPhysics.bounciness;
        var ballMaterialFriction = currBallData.ballData.ballPhysics.friction;
        var pitchMaterialBounciness = currBallData.ballData.pitchPhysics.bounciness;
        var pitchMaterialFriction = currBallData.ballData.pitchPhysics.friction;


        float e = Mathf.Max(ballMaterialBounciness, pitchMaterialBounciness);
        float mu = (ballMaterialFriction + pitchMaterialFriction) / 2;

        float oldVx = PrevFixedFrameDirection.x;
        float oldVy = PrevFixedFrameDirection.y;

        Vector2 newVelocity = new Vector2(oldVx * (1f - mu), -oldVy * e);

        ballRb.linearVelocity = newVelocity;
        ballRb.angularVelocity /= 2;
    }


    public enum DeliveryState {
        Idle, DeliveryActive, PostBounce
    }
}
