using UnityEngine;
using UnityEngine.UI;
using WAS.EventBus;

public class FillBarController : MonoBehaviour
{
    [SerializeField]private Image fillBG;
    [SerializeField]private Image fillImage;

    private void OnEnable() {
        GameEventBus.Subscribe<UpdateRPM>(UpdateFill);
    }

    private void OnDisable() {
        GameEventBus.Subscribe<UpdateRPM>(UpdateFill);
        
    }

    public void UpdateFill(UpdateRPM data) {
        UpdateFill(data.currAmount, data.maxAmount);
    }
    public void UpdateFill(int maxAmount,int currAmount) {
        float value = (float)currAmount / (float)maxAmount;
        SetFill(value);
    }

    public void UpdateFill(float currAmount, float maxAmount) {
        float value = currAmount / maxAmount;
        SetFill(value);
    }

    public void SetFill(float value) {
        fillImage.fillAmount = value;
    }
}
