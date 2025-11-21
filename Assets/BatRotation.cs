using UnityEngine;
using WAS.EventBus;

public class BatRotation : MonoBehaviour {
    public Transform batTransform;
    public float minRot = 0f;
    public float maxRot = 90f;

    public float currRot;

    public bool isLocked = false;
    private void Awake() {
        batTransform = this.transform;
    }
    private void OnEnable() {
        GameEventBus.Subscribe<SetBatRotation>(SetRotation);
        GameEventBus.Subscribe<LockBatRotation>(SetRotationLock);
    }

    private void OnDisable() {
        GameEventBus.Unsubscribe<SetBatRotation>(SetRotation);
        GameEventBus.Unsubscribe<LockBatRotation>(SetRotationLock);
    }

    private void SetRotationLock(LockBatRotation data) {
        isLocked = data.isLocked;
    }

    private void SetRotation(SetBatRotation obj) {

        if(isLocked) { return; }
        obj.value = Mathf.Abs(obj.value - 1);
        SetRotation(obj.value);
    }
    public void SetRotation(float value01) {
        Vector3 currentRot = batTransform.eulerAngles;
        float zRot = Mathf.Lerp(0, 90, value01);
        currentRot.z = zRot;
        batTransform.eulerAngles = currentRot;
    }

    public float GetRotationFactor() {
        return Mathf.Max(0.5f,Mathf.InverseLerp(90, 0, currRot));
    }
    [SerializeField]private float rotationSpeed = 2f;

    private void Update() {
        if(isLocked) return;
        if (Input.GetKey(KeyCode.A)) {
            currRot += Time.deltaTime * rotationSpeed;
            currRot = Mathf.Clamp01(currRot);
            SetRotation(currRot);
        }
        if (Input.GetKey(KeyCode.D)) {
            currRot -= Time.deltaTime * rotationSpeed;
            currRot = Mathf.Clamp01(currRot);
            SetRotation(currRot);
        }
    }

}
