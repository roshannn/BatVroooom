using System;
using UnityEngine;
using WAS.EventBus;

public class SoundRevController : MonoBehaviour {

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private BikeEngineSoundData bikeSoundData;

    private float wheeliePitchOffset;
    private float wheeliePitchOffsetChangeSpeed = 5f;
    [SerializeField]private float currPitchOffset = 0f;
    private void Awake() {
        audioSource ??= GetComponent<AudioSource>();
    }
    private void OnEnable() {
        GameEventBus.Subscribe<UpdateRPM>(ChangeRPM);
        GameEventBus.Subscribe<WheelieRPMChange>(OnWheelieRPMChange);
    }

    private void OnWheelieRPMChange(WheelieRPMChange data) {
        wheeliePitchOffset = data.revOffset;
        wheeliePitchOffsetChangeSpeed = data.revChangeSpeed;
    }

    private void OnDisable() {
        GameEventBus.Unsubscribe<UpdateRPM>(ChangeRPM);
        GameEventBus.Unsubscribe<WheelieRPMChange>(OnWheelieRPMChange);
        
    }
    private void ChangeRPM(UpdateRPM rpmData) {
        float lerp = Mathf.InverseLerp(rpmData.minAmount,rpmData.maxAmount,rpmData.currAmount);
        audioSource.pitch = Mathf.SmoothStep(bikeSoundData.minPitch, bikeSoundData.maxPitch, lerp) + currPitchOffset;
        audioSource.volume = Mathf.SmoothStep(bikeSoundData.minVolume, bikeSoundData.maxVolume, lerp);
    }

    private void Update() {
        float maxDelta = wheeliePitchOffsetChangeSpeed * Time.deltaTime;
        currPitchOffset = Mathf.MoveTowards(currPitchOffset, wheeliePitchOffset, maxDelta);
    }


}
