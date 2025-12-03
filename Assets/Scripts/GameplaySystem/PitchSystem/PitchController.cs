using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WAS.EventBus;

public class PitchController : MonoBehaviour {
    [SerializeField]private Transform onSideWicket;
    [SerializeField]private Transform offSideWicket;

    [SerializeField] private Transform pitchTransform;
    [SerializeField] private BoxCollider2D pitchCollider;

    private void OnEnable() { 
        GameEventBus.Subscribe<GetBouncePos, Vector2>(GetBouncePos);

    }

    private void OnDisable() {
        GameEventBus.Unsubscribe<GetBouncePos, Vector2>(GetBouncePos);
    }

    Vector2 GetBouncePos(GetBouncePos data) {
        float t = Mathf.Clamp01(data.value01);
        float y = GetCircleYOnSurface(pitchCollider, data.ballCollider);
        Vector2 a = offSideWicket.position;
        Vector2 b = onSideWicket.position;
        a.y = y;
        b.y = y;
        return Vector2.Lerp(a, b, t);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.TryGetComponent<DeliveryController>(out DeliveryController deliveryController)) {
            deliveryController.Bounce();
        }
    }
    public float GetCircleYOnSurface(BoxCollider2D surface, CircleCollider2D circle) {
        // 1) Surface top (horizontal collider)
        float surfaceTopY = surface.bounds.max.y;

        // 2) World radius — CircleCollider2D scales only by X
        float worldRadius = circle.radius * circle.transform.lossyScale.x;

        // 3) Center offset (local offset → world offset)
        float worldCenterOffsetY = circle.offset.y * circle.transform.lossyScale.y;

        // 4) Final transform y
        return (surfaceTopY + worldRadius) - worldCenterOffsetY;
    }
}
