using UnityEngine;
using UnityEngine.EventSystems;
using WAS.EventBus;

public class RevButtonController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
    

    public void OnPointerDown(PointerEventData eventData) {
        GameEventBus.Fire(new RevButtonPressed());
    }

    public void OnPointerUp(PointerEventData eventData) {
        GameEventBus.Fire(new RevButtonReleased());
    }
}

