using UnityEngine;
using WAS.EventBus;

public class TimingInputHandler : MonoBehaviour
{
    private void OnEnable() {
        Input.multiTouchEnabled = true;

    }
    private void OnDisable() {

    }

    void Update() {
#if UNITY_EDITOR || UNITY_STANDALONE
        HandleMouse();
#endif

#if UNITY_ANDROID || UNITY_IOS
        HandleTouch();
#endif
    }

    void HandleMouse() {
        if (Input.GetMouseButtonDown(0)) {
            if(IsLeftHalf(Input.mousePosition)){
                Debug.Log("Left Side Tapped");
            }
        }
    }


    void HandleTouch() {
        if (Input.touchCount == 0) return;

        for (int i = 0; i < Input.touchCount; i++) {
            Touch t = Input.GetTouch(i);

            if (t.phase == TouchPhase.Began) {
                if (IsLeftHalf(t.position)) {
                    Debug.Log("Left Side Tapped");
                }
            }

            
        }
    }

    bool IsLeftHalf(Vector2 pos) {
        return pos.x < Screen.width * 0.5f;
    }
}
