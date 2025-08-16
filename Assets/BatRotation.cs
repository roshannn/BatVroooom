using UnityEngine;

public class BatRotation : MonoBehaviour {
    public Transform batTransform;

    private void Awake() {
        batTransform = this.transform;
    }
    public void SetRotation(float value01) {
        Vector3 currentRot = batTransform.eulerAngles;
        float zRot = Mathf.Lerp(0, 90, value01);
        currentRot.z = zRot;
        batTransform.eulerAngles = currentRot;
    }

    float currRot = 0;
    [SerializeField]private float rotationSpeed = 2f;

    private void Update() {
        
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
