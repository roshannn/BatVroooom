using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WAS.EventBus;

public class BikeWheelieController : MonoBehaviour
{
	public BikeWheelieModel bikeData;

	public float currRPM;

    public Transform wheeliePivot;
    

    private bool isRevving = false;
    private bool isWheelie = false;
    private float currentWheelieAngle = 0f;
    private bool hasReachedMaxAngle = false;
    private Vector3 wheelieRotation {
        get {
            return wheeliePivot.transform.rotation.eulerAngles;
        }
        set {
            wheeliePivot.transform.rotation = Quaternion.Euler(value);
        }
    }

    private void OnEnable() {
        GameEventBus.Subscribe<RevButtonPressed>(OnRevPressed);
        GameEventBus.Subscribe<RevButtonReleased>(OnRevReleased);
        GameEventBus.Subscribe<WheelieTriggered>(TriggerWheelie);
    }

    private void OnDisable() {
        GameEventBus.Unsubscribe<RevButtonPressed>(OnRevPressed);
        GameEventBus.Unsubscribe<RevButtonReleased>(OnRevReleased);
        GameEventBus.Unsubscribe<WheelieTriggered>(TriggerWheelie);
    }

    private void Awake() {
		currRPM = 0;
    }
    private void Start() {
        IgnitionSwitchOn();
    }

    private void IgnitionSwitchOn() {
        currRPM = bikeData.minRpm;
    }

    private void Update() {
        HandleRevving();
        HandleWheelie();
    }

    private void HandleRevving() {
        if (isRevving) {
            currRPM = Mathf.MoveTowards(currRPM, bikeData.maxRpm, bikeData.rpmIncreaseRate * Time.deltaTime);
        } else {
            currRPM = Mathf.MoveTowards(currRPM, bikeData.minRpm, bikeData.rpmDecayRate * Time.deltaTime);
        }
        GameEventBus.Fire(new UpdateRPM() {minAmount = bikeData.minRpm, currAmount = currRPM, maxAmount = bikeData.maxRpm });
    }

    private void HandleWheelie()
    {
        if(!isWheelie) return;
        if (!hasReachedMaxAngle) {
            // Rotate up
            currentWheelieAngle += bikeData.wheelieRotationSpeed * Time.deltaTime;
            if (currentWheelieAngle >= bikeData.maxWheelieAngle) {
                currentWheelieAngle = bikeData.maxWheelieAngle;
                hasReachedMaxAngle = true;
                GameEventBus.Fire(new WheelieRPMChange() { revChangeSpeed = bikeData.wheelieSoundPitchData.wheeliePitchResetSpeed, revOffset = bikeData.wheelieSoundPitchData.wheeliePitchResetOffset });
            }
            SetWheelieAngle(currentWheelieAngle);
            return;
        } else {

            currentWheelieAngle -= bikeData.wheelieRotationDecay * Time.deltaTime;
            if (currentWheelieAngle < 0f)
            {
                currentWheelieAngle = 0f;
                hasReachedMaxAngle = false;
                GameEventBus.Fire(new WheelieTriggered() { isWheelie = false });
                canTriggerPitchOffset = true;
            }
            SetWheelieAngle(currentWheelieAngle);
        }
    }

    private void SetWheelieAngle(float angle) {
        Vector3 rot = wheelieRotation;
        rot.z = angle;
        wheelieRotation = rot;
    }

    private void OnRevPressed(RevButtonPressed data) {
        isRevving = true;
    }

    private void OnRevReleased(RevButtonReleased data) {
        isRevving = false;
    }

    private void TriggerWheelie(WheelieTriggered data) {
        isWheelie = data.isWheelie;
        TriggerPitchOffset();
    }

    bool canTriggerPitchOffset = true;
    private void TriggerPitchOffset() {
        if (isWheelie && canTriggerPitchOffset) {
            GameEventBus.Fire(new WheelieRPMChange() { revOffset = bikeData.wheelieSoundPitchData.wheeliePitchIncreaseOffset, revChangeSpeed = bikeData.wheelieSoundPitchData.wheeliePitchIncreaseSpeed });
            canTriggerPitchOffset = false;
        }
    }
}
