using UnityEngine;
using UnityEngine.EventSystems;
using WAS.EventBus;

public class RevButtonController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {


    private void Update() {
        if (Input.GetKey(KeyCode.Space)) {
            GameEventBus.Fire(new RevButtonPressed());
        }
        if (Input.GetKeyUp(KeyCode.Space)) {
            GameEventBus.Fire(new RevButtonReleased());
        }
    }

    public void OnPointerDown(PointerEventData eventData) {
        GameEventBus.Fire(new RevButtonPressed());
    }

    public void OnPointerUp(PointerEventData eventData) {
        GameEventBus.Fire(new RevButtonReleased());
    }
}

