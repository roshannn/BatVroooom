using UnityEngine;
using UnityEngine.Events;
using WAS.EventBus;

public class SwipeInput : MonoBehaviour {
    public float sensitivity = 0.0025f; // convert pixels → normalized input
    private bool swipeActive = false;
    private Vector2 lastPos;
    private float currValue = 1f;

    private bool isLocked = false;

    private void OnEnable() {
        GameEventBus.Subscribe<LockBatRotation>(SetSwipeLock);
    }
    private void OnDisable() {
        GameEventBus.Unsubscribe<LockBatRotation>(SetSwipeLock);

    }

    private void SetSwipeLock(LockBatRotation data) {
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
            if (IsLeftHalf(Input.mousePosition)) {
                swipeActive = true;
                lastPos = Input.mousePosition;
            }
            
        }

        if (Input.GetMouseButton(0) && swipeActive) {
            Vector2 current = Input.mousePosition;
            float deltaX = current.x - lastPos.x;
            if(!isLocked) {
                currValue += (deltaX * sensitivity);
                currValue = Mathf.Clamp01(currValue);
                GameEventBus.Fire(new SetBatRotation() { value = currValue });
            }

            lastPos = current;
        }

        if (Input.GetMouseButtonUp(0)) {
            swipeActive = false;
        }
    }

    // ---------------- TOUCH ----------------
    // ---------------- TOUCH ----------------
    private int swipeFingerId = -1;

    void HandleTouch() {
        if (Input.touchCount == 0) return;

        for (int i = 0; i < Input.touchCount; i++) {
            Touch t = Input.GetTouch(i);

            if (t.phase == TouchPhase.Began && !swipeActive) {
                if (IsLeftHalf(t.position)) {
                    swipeActive = true;
                    lastPos = t.position;
                    swipeFingerId = t.fingerId;
                }
            }

            if (swipeActive && t.fingerId == swipeFingerId) {
                if (t.phase == TouchPhase.Moved) {
                    Vector2 current = t.position;
                    float deltaX = current.x - lastPos.x;
                    if (!isLocked) {
                        currValue += (deltaX * sensitivity);
                        currValue = Mathf.Clamp01(currValue);
                        GameEventBus.Fire(new SetBatRotation() { value = currValue });
                    }
                    lastPos = current;
                }

                if (t.phase == TouchPhase.Ended || t.phase == TouchPhase.Canceled) {
                    swipeActive = false;
                    swipeFingerId = -1;
                }
            }
        }
    }

    bool IsLeftHalf(Vector2 pos) {
        return pos.x < Screen.width * 0.5f;
    }

   
}
