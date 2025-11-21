using System;
using UnityEngine;
using WAS.EventBus;

public class VehicleController : MonoBehaviour {

    public static VehicleState VehicleState;
    [SerializeField] private Transform pivot;

    [Header("Member RPM Stuff")]
    [SerializeField] private float currentRPM;
    [SerializeField] private RevButtonController revButton;
    [SerializeField] private FillBarController fillBar;

    //current turn data
    //come from model
    [Header("RPM Data")]
    [SerializeField] private float idleRPM = 1000;
    [SerializeField] private float maxRPM = 5000;
    [SerializeField] private float accelerationFactor = 200f;
    [SerializeField] private AnimationCurve accelerationCurve;


    [Header("Member Wheelie Stuff")]
    public float wheelieAngle;
    public float wheelieSpeed;
    public float currAngle;
    [Header("Wheelie Data")]
    [SerializeField] private float maxWheelieSpeed = 100f;
    [SerializeField] private float maxWheelieAngle = 45f;
    [SerializeField] private AnimationCurve wheelieAngleCurve;
    [SerializeField] private AnimationCurve wheelieSpeedCurve;
    [SerializeField] private float wheelieResetSpeed;
    [SerializeField] private float rpmResetSpeed;
    [SerializeField] private float idleAngle = 0f;
    
    private Rigidbody2D rb;

    private void Awake() {
        rb = pivot.GetComponent<Rigidbody2D>();
        UpdateRPM(idleRPM);
    }
    private void UpdateRPM(float rpm) {
        currentRPM = Mathf.Max(Mathf.Min(rpm, maxRPM), idleRPM);
        fillBar.UpdateFill(currentRPM, maxRPM);

    }
    private void OnEnable() {
        GameEventBus.Subscribe<RevButtonPressed>(RevEngine);
        GameEventBus.Subscribe<RevButtonReleased>(LaunchWheelie);
    }
    private void OnDisable() {
        GameEventBus.Unsubscribe<RevButtonPressed>(RevEngine);
        GameEventBus.Unsubscribe<RevButtonReleased>(LaunchWheelie);
    }
    private void RevEngine(RevButtonPressed obj) {
        VehicleState = VehicleState.StateRev;
        //Debug.Log(VehicleState);
    }

    private void LaunchWheelie(RevButtonReleased obj) {
        GameEventBus.Fire(new LockBatRotation() { isLocked= true });
        VehicleState = VehicleState.StateWheelie;
        SetWheelieSpeed();
        SetWheelieAngle();
        //Debug.Log(VehicleState);
    }
    private void SetWheelieSpeed() {
        float normalizedRPM = GetNormalizedRPM();
        wheelieSpeed = wheelieSpeedCurve.Evaluate(normalizedRPM) * maxWheelieSpeed;
    }

    private void SetWheelieAngle() {
        float normalizedRPM = GetNormalizedRPM();
        wheelieAngle = wheelieAngleCurve.Evaluate(normalizedRPM) * maxWheelieAngle;
    }



    private void FixedUpdate() {
        if (VehicleState == VehicleState.StateRev) {
            float calcRPM = currentRPM + (accelerationCurve.Evaluate(GetNormalizedRPM()) * accelerationFactor * Time.fixedDeltaTime);
            UpdateRPM(calcRPM);
        } else if (VehicleState == VehicleState.StateWheelie) {
            currAngle += wheelieSpeed * Time.fixedDeltaTime;
            currAngle = Mathf.Clamp(currAngle, idleAngle, wheelieAngle);
            rb.MoveRotation(currAngle);
            if (currAngle == wheelieAngle) {
                VehicleState = VehicleState.StateReset;

            }
        } else if (VehicleState == VehicleState.StateReset) {
            currAngle -= wheelieResetSpeed * Time.fixedDeltaTime;
            currAngle = Mathf.Clamp(currAngle, idleAngle, wheelieAngle);
            rb.MoveRotation(currAngle);
            

            float calcRPM = currentRPM - (rpmResetSpeed * Time.fixedDeltaTime);
            UpdateRPM(calcRPM);

            if (currAngle == idleAngle && currentRPM == idleRPM) {
                VehicleState = VehicleState.StateIdle;
                GameEventBus.Fire(new LockBatRotation() { isLocked = false });
                wheelieSpeed = 0f;
            }
        }
    }


    private float GetNormalizedRPM() {
        return (currentRPM - idleRPM) / (maxRPM - idleRPM);
    }

}


public enum VehicleState {
    StateIdle, StateRev, StateWheelie, StateReset
}