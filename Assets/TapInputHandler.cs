using UnityEngine;
using WAS.EventBus;

public class TapInputHandler : MonoBehaviour
{
    private bool isLocked = false;
    private bool isTapActive = false;
    private void OnEnable() {
        Input.multiTouchEnabled = true;
        GameEventBus.Subscribe<LockWheelieRecharge>(SetTapRecharge);
    }
    private void OnDisable() {
        GameEventBus.Unsubscribe<LockWheelieRecharge>(SetTapRecharge);

    }

    private void SetTapRecharge(LockWheelieRecharge data) {
        isLocked = data.isLocked;
    }
    void Update() {
#if UNITY_EDITOR || UNITY_STANDALONE
        HandleMouse();
#endif

#if UNITY_ANDROID || UNITY_IOS
        HandleTouch();
#endif
    }

    // ---------------- MOUSE ----------------
    void HandleMouse() {
        if (Input.GetMouseButtonDown(0)) {
            if(IsRightHalf(Input.mousePosition)){
                isTapActive = true;
            }
        }

        if (Input.GetMouseButton(0) && isTapActive) {
            if (!isLocked) {
                GameEventBus.Fire(new RevButtonPressed());
            }
        }

        if (Input.GetMouseButtonUp(0) && isTapActive) {
            isTapActive = false;
            GameEventBus.Fire(new RevButtonReleased());
        }
    }

    // ---------------- TOUCH ----------------
    // ---------------- TOUCH ----------------
    private int tapFingerId = -1;

    void HandleTouch() {
        if (Input.touchCount == 0) return;

        for (int i = 0; i < Input.touchCount; i++) {
            Touch t = Input.GetTouch(i);

            if (t.phase == TouchPhase.Began && !isTapActive) {
                if (IsRightHalf(t.position)) {
                    isTapActive = true;
                    tapFingerId = t.fingerId;
                }
            }

            if (isTapActive && t.fingerId == tapFingerId) {
                if (t.phase == TouchPhase.Moved || t.phase == TouchPhase.Stationary) {
                    if (!isLocked) {
                        GameEventBus.Fire(new RevButtonPressed());
                    }
                }

                if (t.phase == TouchPhase.Ended || t.phase == TouchPhase.Canceled) {
                    GameEventBus.Fire(new RevButtonReleased());
                    isTapActive = false;
                    tapFingerId = -1;
                }
            }
        }
    }

    bool IsRightHalf(Vector2 pos) {
        return pos.x > Screen.width * 0.5f;
    }
}
