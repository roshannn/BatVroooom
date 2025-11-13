using UnityEngine;
using UnityEngine.UI;
using WAS.EventBus;
public class BatSlider : MonoBehaviour {
    public Slider slider;
    void Start() {
        slider.onValueChanged.RemoveAllListeners();
        slider.onValueChanged.AddListener(SetRotation);
        slider.value = 1f;
    }

    void SetRotation(float sliderVal) {
        GameEventBus.Fire(new SetBatRotation() { value = sliderVal });
    }
}
